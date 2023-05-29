using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts {
    public class WaterBombController : MonoBehaviour {
        private SpriteRenderer[] _spriteLayers;
        public Sprite waterBombSprite;
        public Material waterBombMaterial;
        
        public int order;
        private int _timer;
        private bool _isExploded;
        private float _maxScale;
        private float _maxRotateRadius;
        private float _currentRotateRadius;
        private float[] _layerRotateRadius;
        private int[] _layerRotateOffset;
        
        public float random;
        private int _propHueID;
        private int _propSatID;

        private Color color {
            set {
                float H = 0, S = 0, V = 0;
                Color.RGBToHSV(value, out H, out S, out V);
                for (int i = 0; i <= 3; i++) {
                    _spriteLayers[i].material.SetFloat(_propHueID, H);
                    _spriteLayers[i].material.SetFloat(_propSatID, S);
                }
            }
        }

        private void Awake() {
            _propHueID = Shader.PropertyToID("_Hue");
            _propSatID = Shader.PropertyToID("_Saturation");
        }
        private void Start() {
            _spriteLayers = new SpriteRenderer[4];
            _layerRotateRadius = new float[4];
            _layerRotateOffset = new int[4];
            for (int i = 0; i <= 3; i++) {
                if (i == 0) {
                    _spriteLayers[i] = new GameObject().AddComponent<SpriteRenderer>();
                    _spriteLayers[i].transform.SetParent(transform);
                    _spriteLayers[i].transform.localPosition = Vector3.zero;
                    _spriteLayers[i].sprite = waterBombSprite;
                    _spriteLayers[i].material = waterBombMaterial;
                }else {
                    _spriteLayers[i] = Instantiate(_spriteLayers[0],transform);
                }
                _spriteLayers[i].color = _spriteLayers[i].color.SetAlpha(Random.Range(0.3f, 0.4f));
                _layerRotateRadius[i] = Random.Range(0, 0.2f);
                _layerRotateOffset[i] = Random.Range(0, 360);
            }

            _timer = 0;//Random.Range(0,100);
            _maxScale = Random.Range(0.75f, 0.85f);
            _maxRotateRadius = Random.Range(2.3f, 2.7f);
            _currentRotateRadius = 0f;
            color = Color.blue;
            random = Random.Range(0.75f, 0.85f);

            transform.localScale = Vector3.zero;
        }

        private void Explode() {
            if (Input.anyKeyDown) {
                _isExploded = true;
                for (int n = 0; n <= 3; n++) {
                    for (int i = 1; i <= 36; i++) {
                        var pBreak =
                            ParticleManager.GetParticleToPosition(ParticleType.WaterBombBreak, transform.position);
                    }

                    var randOffset = new Vector3(Random.value - 0.5f, Random.value - 0.5f, 0f);
                    var pRing = ParticleManager.GetParticleToPosition(ParticleType.ParticleRing,
                        transform.position + 2f * randOffset);
                }
            }
        }

        private void LayerSpriteCalculation() {
            for (int i = 0; i <= 3; i++) {
                float selfRadius = 1f;
                bool isClockwise = true;
                switch (i) {
                    case 3:
                        selfRadius = ((1f - (Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * _timer * 10f))) * 0.3f) + 0.7f);
                        break;
                    case 2:
                        selfRadius = 0.7f * ((1f - (Mathf.Abs(Mathf.Sin(-Mathf.Deg2Rad * _timer * 10f))) * 0.3f) + 0.9f);
                        isClockwise = false;
                        break;
                    case 1:
                        selfRadius = ((1f - (Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * _timer * 10f))) * 0.3f));
                        break;
                    case 0:
                        selfRadius = 1.2f * ((1f - (Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad * _timer * 10f))) * 0.3f) + 0.7f);
                        isClockwise = false;
                        break;
                }
                
                int heartAngle = (_timer + _layerRotateOffset[i]) * (isClockwise ? 1 : -1);
                float radiusSinMultiplier = Mathf.Sin(Mathf.Deg2Rad * heartAngle * 6);
                Vector3 circleVector = (3 * heartAngle).Deg2Dir3();
                _spriteLayers[i].transform.localScale = selfRadius * Vector3.one;
                _spriteLayers[i].transform.localRotation = Quaternion.Euler(0, 0, heartAngle);
                _spriteLayers[i].transform.localPosition = radiusSinMultiplier * _layerRotateRadius[i] * circleVector;
            }
        }
    

        private void FixedUpdate() {
            if (!_isExploded) {
                LayerSpriteCalculation();
                transform.localScale =
                    transform.localScale.ApproachValue(_maxScale * Vector3.one, 16f * Vector3.one);
                _currentRotateRadius.ApproachRef(_maxRotateRadius, 32f);
                transform.position = GameManager.Player.transform.position +
                                     _currentRotateRadius * (_timer * 3f + order * 360f / 8f).Deg2Dir3();
            }

            if (_isExploded) {
                for (int i = 0; i <= 3; i++) {
                    transform.localScale = transform.localScale.ApproachValue
                        (((i == 3) ? 4f : 1f) * Vector3.one, 16f * Vector3.one);
                    _spriteLayers[i].color = _spriteLayers[i].color.Fade(12f);
                }
            }

            //if(!_isExploded) Explode();

            _timer++;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}
