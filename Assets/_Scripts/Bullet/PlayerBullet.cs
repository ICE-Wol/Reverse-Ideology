using _Scripts.Fairies;
using UnityEngine;

namespace _Scripts {
    public class PlayerBullet : MonoBehaviour {
        public SpriteRenderer spriteRenderer;
        private FairyEnemyController _nearestFairyEnemy;
        private PlayerBulletType _type;

        public PlayerBulletType type {
            set {
                _type = value;
                Init();
                switch (value) {
                    case PlayerBulletType.Fin:
                        _speed = 4f;
                        spriteRenderer.color = spriteRenderer.color.SetAlpha(1f);
                        break;
                    case PlayerBulletType.Needle:
                        color = Color.blue;
                        spriteRenderer.color = spriteRenderer.color.SetAlpha(0.5f);
                        _speed = 25f;
                        _direction = 90f;
                        transform.rotation = Quaternion.Euler(0f, 0f, _direction);
                        break;
                    case PlayerBulletType.ReimuMain:
                        color = Color.blue;
                        spriteRenderer.color = spriteRenderer.color.SetAlpha(0.5f);
                        _speed = 25f;
                        _direction = 90f;
                        transform.rotation = Quaternion.Euler(0f, 0f, _direction);
                        break;
                    default:
                        break;
                }
            }
            get {
                return _type;
            }
        }
        private float _speed;
        private float _radius;
        private float _direction;
        private float _targetDirection;
        private float _targetDirPrevious;
        private int _damage;
        private int _timer;
        
        private int _propHueID;
        private int _propSatID;
        
        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            _propHueID = Shader.PropertyToID("_Hue");
            _propSatID = Shader.PropertyToID("_Saturation");
        }

        public Color color {
            set {
                float H = 0, S = 0, V = 0;
                Color.RGBToHSV(value, out H, out S, out V);
                spriteRenderer.material.SetFloat(_propHueID, H);
                spriteRenderer.material.SetFloat(_propSatID, S);
            }
            get => spriteRenderer.color;
        }

        private void Init() {
            _timer = 0;
            _radius = 0.06f;
            _damage = 1;
        }

        public void SetDirection(float direction) {
            _direction = direction;
        }

        void Movement() {
            switch(_type) {
                case PlayerBulletType.Fin:
                    //if no signed the bullet would like wave
                    //homing bullet
                    if (_timer is < 300 and > 0/* && _timer % 10 == 0*/ && _nearestFairyEnemy != null) {
                        _targetDirection = Vector2.SignedAngle(Vector2.right,
                                _nearestFairyEnemy.transform.position - transform.position);
                    }

                    if (_timer > 0 && _nearestFairyEnemy != null) {
                        //TODO: maybe have better ways.
                        var tmp = _targetDirection;
                        if (_targetDirection - _direction > 180f) {
                            _targetDirection -= 360f;
                        }

                        if (_direction - _targetDirection > 180f) {
                            _targetDirection += 360f;
                        }

                        //_direction = Calc.Approach(_direction, _targetDirection, 16f, 5f);
                        if (Mathf.Abs(_direction - _targetDirection) <= 5f) _direction = _targetDirection; 
                        else if (_direction >= _targetDirection) _direction -= 5f;
                        else _direction += 5f;
                    }
                    break;
            }
            
            transform.position += _speed * Time.fixedDeltaTime * (Vector3)_direction.Deg2Dir();
            transform.rotation = Quaternion.Euler(0f, 0f, _direction);
            if (transform.position.y > 5f) {
                BulletManager.ReleasePlayerBullet(this);
            }
        }

        void MakeParticle() {
            ParticleType parType;
            switch(_type)
            {
                default:
                    parType = ParticleType.ReimuMainBreak;
                    /*transform.position += _speed * Time.fixedDeltaTime * Vector3.up;
                    transform.rotation = Quaternion.Euler(0f,0f,90f);
                    if (transform.position.y >= 10f) 
                        BulletManager.Manager.PlayerBulletPool.Release(this);*/
                    break;
                
                case PlayerBulletType.Needle:
                    parType = ParticleType.NeedleBreak;
                    break;

                case PlayerBulletType.Fin:
                    parType = ParticleType.FinBreak;
                    /*var tar = Vector2.SignedAngle(Vector2.right,_nearestFairyEnemy.transform.position - transform.position);
                    if(_timer < 300f) _direction = Calc.Approach(_direction, tar, 4f);
                    transform.position += _speed * Time.fixedDeltaTime * (Vector3)Calc.Deg2Dir(_direction);
                    transform.rotation = Quaternion.Euler(0f,0f,_direction);
                    if(_timer > 3000f)
                        BulletManager.Manager.PlayerBulletPool.Release(this);*/
                    break;
            }

            var p = ParticleManager.GetParticle(parType);
            p.SetDirection(_direction);
            p.transform.position = transform.position;
        }

        void CheckHit() {
            float minDis = 1000f;
            if (EnemyManager.Manager.enemyList.Count > 0) {
                foreach (var enemy in EnemyManager.Manager.enemyList) {
                    var dis = ((Vector2)(enemy.transform.position - transform.position)).magnitude;
                    if (dis < minDis) {
                        minDis = dis;
                        _nearestFairyEnemy = enemy;
                    }
                }
            }

            //single hit bullet
            if (_nearestFairyEnemy != null && _radius + _nearestFairyEnemy.radius >= minDis) {
                _nearestFairyEnemy.TakeDamage(_damage);
                MakeParticle();
                BulletManager.ReleasePlayerBullet(this);
            }
        }

        // Update is called once per frame
        void FixedUpdate() {
            CheckHit();
            Movement();
            _timer++;
        }
    }
}
