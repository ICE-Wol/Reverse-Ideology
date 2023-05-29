using System;
using _Scripts;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace _Scripts {
    public class StarRingController : MonoBehaviour
    {
        public Particle[] particles;
        public Particle[] glows;
        public Sprite sprite;
        public ItemType type;
        public bool isTriggered;
        
        /// <summary>
        /// 4 or 5,depending on pieces or full
        /// </summary>
        public int num = 4;
        public float radius = 0.5f;

        /// <summary>
        /// 0 - 2, 1 x = -45 |||| 2 x = 45
        /// </summary>
        public int rotation;

        private Vector3[] _positions;
        private int _timer;

        private void Start() {
            isTriggered = false;
            _timer = 0;
            _positions = new Vector3[num * 4];
            particles = new Particle[num * 4];
            for (int i = 0; i < num * 4; i++) {
                particles[i] = ParticleManager.GetParticle(sprite);
                particles[i].transform.SetParent(this.transform);
                particles[i].transform.localPosition = Vector3.zero;
                particles[i].transform.localScale = Vector3.one / 2f;

                if (rotation != 0) {
                    particles[i].SetDirectColor(Color.yellow.SetAlpha(0.5f));
                }
            }

            glows = new Particle[num * 4];
            for (int i = 0; i < num * 4; i++) {
                glows[i] = ParticleManager.GetParticle(sprite);
                glows[i].transform.SetParent(this.transform);
                if (i % 2 == 0) {
                    if(type == ItemType.LifeFrag || type == ItemType.Life)
                        glows[i].SetDirectColor(Color.magenta);
                    if(type == ItemType.BombFrag || type == ItemType.Bomb)
                        glows[i].SetDirectColor(Color.green);
                    glows[i].transform.localScale = new Vector3(1.3f, 0.5f, 0.5f);
                }
                else {
                    glows[i].transform.localScale = Vector3.one / 2f;
                }

                glows[i].spriteRenderer.material = BulletManager.GetBulletMaterial(true);
                particles[i].transform.localPosition = Vector3.zero;
            }
        }

        private void SelfRelease() {
            //TODO: make this thing to the ui panel. 
            for (int i = 0; i < num * 4; i++) {
                ParticleManager.ReleaseParticle(glows[i]);
                ParticleManager.ReleaseParticle(particles[i]);
            }
            Destroy(gameObject);
        }

        private void FixedUpdate() {
            if (!isTriggered) {
                var rad1 = (Mathf.Sin(Mathf.Deg2Rad * _timer * 5f) / 10f + radius);
                var rad2 = (Mathf.Cos(Mathf.Deg2Rad * _timer) / 10f + radius);
                for (int i = 0; i < num * 4; i++) {
                    _positions[i] = rad1 * (Vector3)(360f * i / (num * 4) + _timer).Deg2Dir();
                    if (i % 2 != 0) {
                        _positions[i] = rad2 * (Vector3)(360f * i / (num * 4) + _timer).Deg2Dir();
                    }

                    var pos = _positions[i];
                    var p = particles[i];
                    p.transform.localRotation = Quaternion.Euler(0, 0, 360f * i / (num * 4) + _timer);
                    p.transform.localPosition = pos; //Calc.Approach(p.transform.position, pos, 32f * Vector3.one);

                    p = glows[i];
                    var deg = Vector2.SignedAngle(Vector2.right, p.transform.localPosition);
                    p.transform.localRotation = Quaternion.Euler(0, 0, deg);
                    if (i % 4 == 0) {
                        if (num == 4) {
                            p.transform.localScale
                                = new Vector3((Mathf.Sin(Mathf.Deg2Rad * _timer * 3f) / 2 + 2.2f), 0.5f, 1);
                        }
                        else if (num == 5) {
                            p.transform.localScale
                                = new Vector3((Mathf.Sin(Mathf.Deg2Rad * _timer * 3f) / 2 + 4.0f), 0.5f, 1);
                        }
                    }

                    p.transform.localPosition = pos; //Calc.Approach(p.transform.position, pos, 32f * Vector3.one);

                }

                if (rotation == 0) {
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else if (rotation == 1) {
                    transform.localRotation =
                        Quaternion.Euler(90 * Mathf.Sin(_timer / 4f * Mathf.Deg2Rad), _timer / 2f, 90);
                }
                else {
                    transform.localRotation =
                        Quaternion.Euler(90 * Mathf.Cos(_timer / 4f * Mathf.Deg2Rad), _timer / 2f, 90);
                }

                if (rotation != 0) {
                    for (int i = 0; i < num * 4; i++) {
                        glows[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                        glows[i].transform.localScale = Vector3.one / 2f;
                        if (i % 4 == 0) {
                            glows[i].transform.localScale = Vector3.one;
                        }

                        particles[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                        particles[i].transform.localScale = Vector3.one / 2f;
                        if (i % 4 == 0) {
                            particles[i].transform.localScale = Vector3.one;
                        }
                    }
                }
            }
            else {
                for (int i = 0; i < num * 4; i++) {
                    particles[i].transform.localPosition = particles[i].transform.localPosition.ApproachValue(Vector3.zero, 8f * Vector3.one);
                    glows[i].transform.localPosition = particles[i].transform.localPosition.ApproachValue(Vector3.zero, 16f * Vector3.one);
                }
                
                //destroy condition
                if (particles[0].transform.localPosition.Equal(Vector3.zero, 0.01f)) {
                    SelfRelease();
                }
            }
            
            _timer++;
        }
    }
}
