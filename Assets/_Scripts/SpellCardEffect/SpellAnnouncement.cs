using System;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace _Scripts {
    public class SpellAnnouncement : MonoBehaviour {
        public Image bossPortraitImage;
        public RectTransform bossPortraitObject;
        public RectTransform spellNameObject;
        public SpellBannerCtrl spellBannerCtrl;
        public MeshRenderer spellBgFull;
        public FragManager spellBgFrag;

        public Vector2 pStart;
        public Vector2 pStay;
        public Vector2 pEnd;

        public Vector2 nStart;
        public Vector2 nStay;
        public Vector2 nEnd;
        public Vector2 nScale;

        public bool isAnnouncing;
        public bool isBreaking;
        public int midTimerFlag;
        public int endTimerFlag;

        private float _alpha;
        private int _timer;
        
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        public void ModifySpellCardBackgroundAlpha(float value)
        {
            spellBgFull.material.SetFloat(Alpha, value);
        }

        public void ResetAnnounce() {
            _timer = 0;
            isAnnouncing = false;
            isBreaking = false;
            
            spellNameObject.anchoredPosition = nStart;
            spellNameObject.localScale = nScale;
            bossPortraitObject.anchoredPosition = pStart;
            ModifySpellCardBackgroundAlpha(0f);
            _alpha = 0;

            spellBannerCtrl.enabled = false;
        }

        public void StartAnnouncing() {
            isAnnouncing = true;
            spellBannerCtrl.enabled = true;
            spellBannerCtrl.BannerAppear();
            spellBgFull.gameObject.SetActive(true);
        }

        public void SpellBreak() {
            isBreaking = true;
            spellBgFull.gameObject.SetActive(false);
            spellBgFrag.EnableFrag();
        }
        
        private void Start() {
            ResetAnnounce();   
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                StartAnnouncing();
            }

            if (Input.GetMouseButtonDown(1)) {
                SpellBreak();
            }
        }

        private void FixedUpdate() {
            if (isAnnouncing) {
                if (_timer <= midTimerFlag) {
                    bossPortraitObject.anchoredPosition =
                        bossPortraitObject.anchoredPosition.ApproachValue(pStay, 16f * Vector2.one);
                    spellNameObject.anchoredPosition =
                        spellNameObject.anchoredPosition.ApproachValue(nStay, 16f * Vector2.one);
                    if (_timer == midTimerFlag) {
                        spellBannerCtrl.BannerDisappear();
                    }
                } else if (_timer <= endTimerFlag) {
                    bossPortraitObject.anchoredPosition =
                        bossPortraitObject.anchoredPosition.ApproachValue(pEnd, 16f * Vector2.one);
                    spellNameObject.anchoredPosition =
                        spellNameObject.anchoredPosition.ApproachValue(nEnd, 16f * Vector2.one);
                    spellNameObject.localScale =
                        spellNameObject.localScale.ApproachValue(Vector2.one, 16f * Vector3.one);
                    
                } else {
                    spellBannerCtrl.enabled = false;
                }

                if (isBreaking) {
                    spellNameObject.anchoredPosition =
                        spellNameObject.anchoredPosition.ApproachValue(nEnd + 300f * Vector2.up, 16f * Vector2.one);
                }

                _alpha.ApproachRef(1f, 16f);
                ModifySpellCardBackgroundAlpha(_alpha);

                _timer++;
            }
        }
    }
}
