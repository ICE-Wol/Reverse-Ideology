using _Scripts;
using UnityEngine;

namespace _Scripts {
    public class PlayerSub : MonoBehaviour {
        [SerializeField] private SpriteRenderer shade;
        private int _timer;
        private PlayerBulletType _type;
        void Start() {
            _timer = 0;
            _type = PlayerBulletType.Needle;
        }

        public void Fire(float direction) {
            var pos = transform.position;
            var leftBullet = BulletManager.GetPlayerBulletWithType(_type);
            leftBullet.transform.position = pos + Vector3.up + 0.08f * Vector3.left;

            var rightBullet = BulletManager.GetPlayerBulletWithType(_type);
            rightBullet.transform.position = pos + Vector3.up - 0.08f * Vector3.left;
        }
        
        void FixedUpdate() {
            transform.rotation = Quaternion.Euler(0f,0f,_timer * 3f);
            shade.transform.rotation = Quaternion.Euler(0f,0f,-_timer * 3f);
            _timer++;
        }
    }
}
