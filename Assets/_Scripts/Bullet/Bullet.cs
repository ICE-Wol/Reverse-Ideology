using System;
using _Scripts.Data;
using UnityEditor;
using UnityEngine;

namespace _Scripts {
    public enum BulletStates {
        Inactivated,
        Spawning,
        Activated,
        Destroying,
    }

    public enum StepType {
        None,
        Linear,
    }

    public enum DestroyType {
        None,
        Change,
    }
    
    public class Bullet : MonoBehaviour {
        public SpriteRenderer spriteRenderer;
        public BCtrl.BulletController parent;
        public int order;

        private int _timer;
        private BulletStates _state;

        private int _propHueID;
        private int _propSatID;
        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            _propHueID = Shader.PropertyToID("_Hue");
            _propSatID = Shader.PropertyToID("_Saturation");
        }

        public void SetParent(BCtrl.BulletController controller, int ord) {
            parent = controller;
            order = ord;
        }

        private BulletData _data;
        public BulletData Data {
            get => _data;
            set {
                _data = value;
                
                spriteRenderer.sprite
                    = BulletManager.GetBulletSprite(_data.type);
                spriteRenderer.material
                    = BulletManager.GetBulletMaterial(_data.isGlowing);
                float H = 0, S = 0, V = 0;
                Color.RGBToHSV(_data.color, out H, out S, out V);
                spriteRenderer.material.SetFloat(_propHueID, H);
                spriteRenderer.material.SetFloat(_propSatID, S);
            }
        }

        public BulletStepBehaviour Step {
            set;
            get;
        }

        public bool CheckPlayerCollision() {
            var sqrDis = ((Vector2)(transform.position - GameManager.Player.transform.position)).sqrMagnitude;
            var rad = GameManager.Player.HitRadius;
            return sqrDis < rad * rad + Data.radius * Data.radius;
        }

        private bool _isGrazed = false;
        public bool CheckPlayerGraze() {
            var sqrDis = ((Vector2)(transform.position - GameManager.Player.transform.position)).sqrMagnitude;
            var rad = GameManager.Player.GrazeRadius;
            return sqrDis < rad * rad + Data.radius * Data.radius;
        }
        
        public Vector3 Snip() =>
            (GameManager.Player.transform.position - transform.position).normalized;

        public void CheckBound() {
            var pos = transform.position;
            if (Mathf.Abs(pos.x) > 6f || Mathf.Abs(pos.y) > 6f) {
                Release();
            }
        }

        public void Release() {
            _state = BulletStates.Inactivated;
            _timer = 0;
            BulletManager.ReleaseBullet(this);
        }

        private float _fogScale;
        private float _fogAlpha;
        
        public BulletStates GetState() => _state;
        public void SetState(BulletStates state) {
            _state = state;
            switch (state) {
                case BulletStates.Spawning:
                    spriteRenderer.color = spriteRenderer.color.SetAlpha(0f);
                    spriteRenderer.sprite
                        = BulletManager.GetBulletSprite(BulletType.Point);
                    _fogScale = 4f;
                    _fogAlpha = 0f;
                    _isGrazed = false;
                    break;
                case BulletStates.Activated:
                    transform.localScale = _data.scale;
                    spriteRenderer.sprite
                        = BulletManager.GetBulletSprite(_data.type);
                    var c = spriteRenderer.color;
                    c.a = _data.color.a;
                    spriteRenderer.color = c;
                    break;
                case BulletStates.Destroying:
                    var p = ParticleManager.GetParticle(ParticleType.BulletErased);
                    p.SetDirectColor(Data.color);
                    p.transform.position = transform.position;
                    _fogScale = 1f;
                    _fogAlpha = 1f;
                    break;
            }
        }

        private void FixedUpdate() {
            //bullet life time up_limit
            //if (_timer >= 200 && _state != BulletStates.Destroying) {
            //    SetState(BulletStates.Destroying);
            //}
            switch (_state) {
                case BulletStates.Spawning:
                    transform.rotation = Quaternion.Euler(0,0,Step.rotation);
                    if(Data.moveWhenSpawning) Step.StepBehaviour(this);
                    
                    _fogScale.ApproachRef(1.2f, 4f);
                    _fogAlpha.ApproachRef(0.8f, 16f);

                    transform.localScale = _fogScale * Vector3.one;
                    var c = spriteRenderer.color;
                    c.a = _fogAlpha;
                    spriteRenderer.color = c;

                    if (_fogScale.Equal(1.2f, 0.1f))
                        SetState(BulletStates.Activated);
                    break;
                case BulletStates.Activated:
                    transform.rotation = Quaternion.Euler(0,0,Step.rotation);
                    Step.StepBehaviour(this);
                    CheckBound();
                    if (!_isGrazed) {
                        _isGrazed = CheckPlayerGraze();
                        if (_isGrazed) {
                            var g = GameManager.Player.playerData.Graze++;
                            GameManager.Manager.GrazeText.text = g.ToString();
                        }
                    }
                    if (CheckPlayerCollision()) {
                        SetState(BulletStates.Destroying);
                        if (!GameManager.Player.CheckInvincibility()) {
                            GameManager.Player.GetHit();
                        }
                    }
                    _timer++;
                    break;
                case BulletStates.Destroying:
                    _fogScale = _fogScale.ApproachValue(3f, 18f);
                    _fogAlpha = _fogAlpha.ApproachValue(0f, 8f);
                    transform.localScale = _fogScale * Vector3.one;
                    spriteRenderer.color = spriteRenderer.color.SetAlpha(_fogAlpha);
                    
                    if (_fogAlpha.Equal(0, 0.01f))
                        Release();
                    break;
                
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, _data.radius);
        }
    }
}


/*private void Generate(int mode) {
    switch (mode) {
        default:
            break;
        case 1:
            if (_timer % 10 == 0) {
                var p1 = BulletManager.Manager.BulletPool.Get();
                p1.SetData(new BulletData());
                p1.transform.position = this.transform.position;
                p1.SetMovement(0, new float[] { _rotation + 30f, (1.5f + Mathf.Sin(Mathf.Deg2Rad*2f*_timer))/2f });

                var p2 = BulletManager.Manager.BulletPool.Get();
                p2.SetData(new BulletData());
                p2.transform.position = this.transform.position;
                p2.SetMovement(0, new float[] { _rotation - 30f, (1.5f + Mathf.Sin(Mathf.Deg2Rad*2f*_timer))/2f });
            }

            break;
    }
}*/