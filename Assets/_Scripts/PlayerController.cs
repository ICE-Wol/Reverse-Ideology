using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private SlowEffectController effSlow;
        [SerializeField] private PlayerBullet playerBullet;
        [SerializeField] private PlayerSub playerSub;
        
        
        
        private PlayerSub[] _playerSubs;
        private int _numSubActivated;
        private float _radSub;

        private SpriteRenderer _spriteRenderer;
        private Vector2 _direction;
        private float _slowMultiplier;
        private float _slowRate;
        private float _moveSpeed;
        private int _frameSpeed;
        private int _idlePointer;
        private int _movePointer;

        private int _timer;
        private int _invincibleTimer;

        #region Animation
        
        private Sprite[] _animPlayerIdle;
        private Sprite[] _animPlayerLeft;
        private Sprite[] _animPlayerRight;

        private void GetAnim() {
            _animPlayerIdle = GameManager.manager.GetPlayerAnim(0, 0);
            _animPlayerLeft = GameManager.manager.GetPlayerAnim(0, 1);
            _animPlayerRight = GameManager.manager.GetPlayerAnim(0, 2);
        }
        
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

        public void SetSlow() {
            _slowMultiplier = _slowRate;
            _radSub = 0.5f;
            effSlow.SetSlow();
        }

        private void ResetSpeed() => _direction = Vector2.zero;

        private void ResetSlow() => _slowMultiplier = 1f;
        
        private void SetState() {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.y = Input.GetAxisRaw("Vertical");
            if(Input.GetKey(KeyCode.LeftShift))
                SetSlow();
            if(Input.GetKey(KeyCode.Z))
                Fire();
        }

        private void ResetState() {
            //_direction = Vector2.zero;
            _slowMultiplier = 1f;
            _radSub = 1f;
            //effSlow.SetNormal();
            //must be reset after the calculation or the slow effect wont function normally
        }

        private void Movement() {
            var targetPos = transform.position + _moveSpeed * _slowMultiplier * Time.fixedDeltaTime
                * (Vector3)_direction.normalized;
            //bound restriction
            if (targetPos.x > 3.9f) targetPos.x = 3.9f;
            if (targetPos.x < -3.9f) targetPos.x = -3.9f;
            if (targetPos.y > 4.4f) targetPos.y = 4.4f;
            if (targetPos.y < -4.2f) targetPos.y = -4.2f;
            transform.position = targetPos;
        }
        
        #endregion

        #region Fire
        
        public void Fire() {
            if (_timer % 2 == 0) {
                var bullet = BulletManager.Manager.PlayerBulletPool.Get();
                bullet.SetPlayerBulletType(0,0);
                bullet.transform.position = transform.position + 0.15f * Vector3.left + 0.5f * Vector3.up;
                //bullet.transform.rotation = Quaternion.Euler(0, 0, 90f);

                bullet = BulletManager.Manager.PlayerBulletPool.Get();
                bullet.SetPlayerBulletType(0,0);
                bullet.transform.position = transform.position - 0.15f * Vector3.left + 0.5f * Vector3.up;
                //bullet.transform.rotation = Quaternion.Euler(0, 0, 90f);
            }

            if (_timer % 6 == 0) {
                for (int i = 1; i <= _numSubActivated; i++) {
                    //(+ 90f - i * 360f / xx)to make "tail fin slap" 
                    //(_timer * 2f + i * 360f / _numSubActivated) to make normal circle
                    _playerSubs[i - 1].Fire((_timer * 2f + i * 360f / _numSubActivated) % 360f);
                }
            }
        }

        #endregion

        #region Subs
        
        private void GenerateSub() {
            _playerSubs = new PlayerSub[4];
            for (int i = 0; i <= 3; i++) {
                _playerSubs[i] = Instantiate(playerSub, transform.position, Quaternion.Euler(0f, 0f, 0f));
                _playerSubs[i].enabled = false;
            }
        }

        private void RefreshSub() {
            _numSubActivated = (int)GameManager.manager.playerData.Power / 100;
            for (int i = 1; i <= 4; i++) {
                _playerSubs[i - 1].enabled = (i <= _numSubActivated);
            }
        }

        private void FollowSub() {
            for (int i = 1; i <= _numSubActivated; i++) {
                var pos = transform.position;
                //x sin, y cos to make "tail fin slap"
                pos.x += _radSub * Mathf.Cos(Mathf.Deg2Rad * (_timer * 2f + i * 360f / _numSubActivated));
                pos.y += _radSub * Mathf.Sin(Mathf.Deg2Rad * (_timer * 2f + i * 360f / _numSubActivated));
                _playerSubs[i - 1].transform.position
                    = Calc.Approach(_playerSubs[i - 1].transform.position, pos, 8f * Vector3.one);
            }
        }
        
        #endregion

        public void GetHit() {
            Debug.Log("Hitted.");
        }

        public bool CheckInvincibility() => _invincibleTimer > 0;
        
        void Start() {
            _moveSpeed = 5f;
            _frameSpeed = 4;
            _slowMultiplier = 1f;
            _slowRate = 0.6f;
            _timer = 0;
            _idlePointer = 0;
            _movePointer = 0;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            GetAnim();
            GenerateSub();
            RefreshSub();
        }

        void FixedUpdate() {
            _timer++;
            if (_invincibleTimer > 0)
                _invincibleTimer--;
            SetState();
            Movement();
            PlayAnim();
            //the anims are relied on states(such as directions)
            FollowSub();
            ResetState();
        }
    }
}
