using System;
using _Scripts.Data;
using UnityEngine;

namespace _Scripts {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private SlowEffectController effSlow;
        [SerializeField] private InvincibleEffectController effInvincible;
        public PlayerSubCtrl playerSubCtrl;
        
        public  PlayerData playerData;
        private PlayerSub[] _playerSubs;
        private int _numSubActivated;
        private int _numSubTotal;
        private float _radSub;

        private SpriteRenderer _spriteRenderer;
        private Vector3 _direction;
        private float _slowRate;
        private float _moveSpeed;
        private int _frameSpeed;
        private int _idlePointer;
        private int _movePointer;

        private float _hitRadius;
        private float _grazeRadius;
        private float _itemRadius;

        public float ItemRadius => _itemRadius;
        public float HitRadius => _hitRadius;
        public float GrazeRadius => _grazeRadius;

        private int _timer;
        private int _invincibleTimer;
        private int _invincibleTimerMax;

        public int InvincibleTimer {
            get => _invincibleTimer;
            set {
                _invincibleTimer = value;
                _invincibleTimerMax = value;
                effInvincible.gameObject.SetActive(true);
                effInvincible.radius = InvincibleEffectController.MaxRadius;
                effInvincible.speed = InvincibleEffectController.MaxSpeed;
            }
        }

        #region Animation
        
        [SerializeField] private Sprite[] _animPlayerIdle;
        [SerializeField] private Sprite[] _animPlayerLeft;
        [SerializeField] private Sprite[] _animPlayerRight;

        private void PlayAnim() {
            if (_timer % _frameSpeed == 0) {
                //get the direction
                int hor = (int)_direction.x;
                if (hor == 0) {
                    //Only when move pointer returned to zero can idle animation being played.
                    if (_movePointer == 0) {
                        _idlePointer++;
                        if (_idlePointer == 8) _idlePointer = 0;
                        _spriteRenderer.sprite = _animPlayerIdle[_idlePointer];
                    }
                    //If move pointer is not zero then make it naturally back to zero.
                    else {
                        _movePointer -= Math.Sign(_movePointer);
                        _spriteRenderer.sprite = _movePointer >= 0
                            ? _animPlayerRight[_movePointer]
                            : _animPlayerLeft[-_movePointer];
                    }
                }
                else {
                    _movePointer += hor;
                    if (_movePointer == 8) _movePointer = 4;
                    if (_movePointer == -8) _movePointer = -4;

                    _spriteRenderer.sprite =
                        _movePointer >= 0 ? _animPlayerRight[_movePointer] : _animPlayerLeft[-_movePointer];
                }
            }
        }
        
        #endregion

        #region Movement

        private Vector3 GetDirectionVectorNormalized() {
            var direction = Vector3.zero;
            if (Input.GetKey(KeyCode.LeftArrow)) direction.x -= 1;
            if (Input.GetKey(KeyCode.RightArrow)) direction.x += 1;
            if (Input.GetKey(KeyCode.DownArrow)) direction.y -= 1;
            if (Input.GetKey(KeyCode.UpArrow)) direction.y += 1;
            _direction = direction;
            
            var slowMultiplier = 
                (Input.GetKey(KeyCode.LeftShift)) ? _slowRate : 1f;

            return slowMultiplier * direction.normalized;
        }

        private void Movement() {
            var targetPos = _moveSpeed * Time.fixedDeltaTime * GetDirectionVectorNormalized()
                            + transform.position;
            
            if (targetPos.x > 3.9f) targetPos.x = 3.9f;
            if (targetPos.x < -3.9f) targetPos.x = -3.9f;
            if (targetPos.y > 4.4f) targetPos.y = 4.4f;
            if (targetPos.y < -4.2f) targetPos.y = -4.2f;
            
            transform.position = targetPos;
        }
        
        #endregion

        #region Fire
        
        private void Fire() {
            MainFire();
            playerSubCtrl.SubFire();
        }

        private void MainFire() {
            if (_timer % 2 == 0) {
                var pos = transform.position;
                var leftBullet = BulletManager.GetPlayerBulletWithType(PlayerBulletType.ReimuMain);
                leftBullet.transform.position = pos + 0.15f * Vector3.left + 0.5f * Vector3.up;
                var rightBullet = BulletManager.GetPlayerBulletWithType(PlayerBulletType.ReimuMain);
                rightBullet.transform.position = pos - 0.15f * Vector3.left + 0.5f * Vector3.up;
            }
        }

        #endregion

        public void GetHit() {
            Debug.Log("Hitted.");
        }

        public bool CheckInvincibility() => _invincibleTimer > 0;
        
        private void SetInvincibleEffect() {
            if (InvincibleTimer > 0) {
                _invincibleTimer--;
                effInvincible.speed
                    = (float)InvincibleTimer / _invincibleTimerMax
                      * InvincibleEffectController.MaxSpeed;
                effInvincible.radius
                    = (float)InvincibleTimer / _invincibleTimerMax
                      * InvincibleEffectController.MaxRadius;
            }

            if (InvincibleTimer == 0) {
                effInvincible.gameObject.SetActive(false);
            }
        }
        
        void Start() {
            _moveSpeed = 4f;
            _frameSpeed = 6;
            _slowRate = 0.6f;
            _itemRadius = 1f;
            _hitRadius = 0.03f;
            _grazeRadius = 0.5f;
            _timer = 0;
            _idlePointer = 0;
            _movePointer = 0;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            playerData = new PlayerData();
        }


        void FixedUpdate() {
            _timer++;
            SetInvincibleEffect();
            Movement();
            PlayAnim();
            if (Input.GetKey(KeyCode.Z)) Fire();
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, _hitRadius);
            Gizmos.DrawWireSphere(transform.position, _grazeRadius);
        }
    }
}
