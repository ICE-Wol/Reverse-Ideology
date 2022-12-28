using System;
using _Scripts.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace _Scripts {
    public class EnemyController : MonoBehaviour {
        public BulletController tempController;
        public CircularHealthBar tempHealthBar;
        
        [SerializeField] private Sprite[] animFairy;
        private SpriteRenderer _spriteRenderer;
        private int _timer;
        private int _frameSpeed;
        private int _idlePointer;
        private int _movePointer;
        private Vector3 _prePosition;
        private Vector2 _direction;

        public int Health { private set; get; }
        public int MaxHealth { private set; get; }
        public float Radius { private set; get; }

        public void TakeDamage(int dam) {
            Health -= dam;
            tempHealthBar.RefreshLine(100 * Health / MaxHealth);
            if(Health <= 0) Break();
        }

        private void Break() {
            EnemyManager.Manager.enemyList.Remove(this);
            //TODO: create some particles here.
        }
        
        private void PlayAnim() {
            if (_timer % _frameSpeed == 0) {
                //get the direction
                float hor = _direction.x;
                if (Calc.Equal(hor, 0f)) {
                    //no horizontal movement.
                    if (_movePointer == 0) {
                        _idlePointer++;
                        if (_idlePointer == 4) _idlePointer = 0;
                        _spriteRenderer.sprite = animFairy[_idlePointer];
                    }
                    //no horizontal movement but have side animation
                    else {
                        _movePointer -= Math.Sign(_movePointer);
                        _spriteRenderer.sprite = animFairy[4 + Math.Abs(_movePointer)];
                        _spriteRenderer.flipX = (_movePointer < 0);
                    }
                }
                else {
                    _movePointer += (int)Mathf.Sign(hor);
                    if (Math.Abs(_movePointer) == 8) 
                        _movePointer -= 4 * Math.Sign(_movePointer);
                    _spriteRenderer.sprite = animFairy[4 + Math.Abs(_movePointer)];
                    _spriteRenderer.flipX = (_movePointer < 0);
                }
            }
        }

        private BulletData _tempBullet;
        void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _timer = 0;
            _frameSpeed = 4;
            _movePointer = 0;
            Health = 500;
            MaxHealth = 500;
            Radius = 0.3f;
            _prePosition = transform.position;

            _tempBullet = new BulletData();
            _tempBullet.color = Color.cyan;
            _tempBullet.type = BulletType.Scale;
            _tempBullet.speed = 3f;

            //temp controller = flower bullet set
            /*BulletData data = new BulletData();
            data.type = BulletType.Point;
            data.color = Color.white;
            
            BulletData data2 = new BulletData();
            data2.type = BulletType.Rice;
            data2.color = Color.blue;
            var c = Instantiate(tempController, transform.position, Quaternion.Euler(0, 0, 0));
            (c as FlowerBulletSet).Activate(18 * 5,6,2,2f,90f,2f,data,data2);
            
            var d = Instantiate(tempController, transform.position, Quaternion.Euler(0, 0, 0));
            (d as FlowerBulletSet).Activate(18 * 5,6,-2,2f,90f,2f,data,data2);*/
        }

        void FixedUpdate() {

            //transform.position = Vector3.zero + Vector3.right * Mathf.Sin(Mathf.Deg2Rad * _timer / 5f);
            _direction = (transform.position - _prePosition).normalized;
            PlayAnim();
            _prePosition = transform.position;

            //attack here.
            //temp controller: BulletController; 
            /*if (_timer % 6 == 0) {
                _tempBullet.color = Color.cyan;
                var c = Instantiate(tempController, transform.position,Quaternion.Euler(0f,0f,0f));
                c.Activate(10, 180f * Mathf.Sin(_timer*Mathf.Deg2Rad) + _timer * 4, 7f,0.5f, _tempBullet);
                _tempBullet.color = Color.blue;
                var d = Instantiate(tempController, transform.position,Quaternion.Euler(0f,0f,0f));
                d.Activate(10, 180f * Mathf.Sin(_timer*Mathf.Deg2Rad) + 180f + _timer * 4, 7f,0.5f, _tempBullet);
            }

            if (_timer % 30 == 0) {
                BulletData data = new BulletData();
                data.type = BulletType.JadeR;
                data.color = Color.green;
                data.speed = 4f;
                var c = Instantiate(tempController, transform.position,Quaternion.Euler(0f,0f,0f));
                c.Activate(48, _timer,data);
            }*/

            if (_timer % 120 == 0) {
                //ItemManager.Manager.GetItemToPosition(ItemType.LifeFrag, transform.position);    
            }

            _timer++;
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }
}
