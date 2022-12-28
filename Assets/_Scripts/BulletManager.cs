using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts {
    public enum BulletType {
        JadeS,
        JadeR,
        Bacteria,
        Rice,
        Scale,
        StarS,
        Dart,
        Drop,
        Shot,
        Bubble,
        Ice,
        Spell1,
        Spell2,
        Spell3,
        Spell4,
        JadeM,
        Ellipse,
        StarM,
        Butterfly,
        Knife,
        Heart,
        Point,
        Sprinkle,
        Spark,
        Snows,
        SnowM,
        Bottle,
        Tube
    }
    public class BulletManager : MonoBehaviour {
        [SerializeField] private PlayerBullet playerBullet;
        [SerializeField] private Bullet enemyBullet;
        [SerializeField] private Sprite[] playerBulletSprites;
        [SerializeField] private Sprite[] bulletSprites;
        [SerializeField] private Material[] glowMaterial;
        
        public ObjectPool<PlayerBullet> PlayerBulletPool;
        private ObjectPool<Bullet> _bulletPool;
        public static BulletManager Manager;

        private void Awake() {
            if (!Manager) {
                Manager = this;
            }
            else {
                Destroy(this.gameObject);
            }
        }
        
        private void Start() {
            PlayerBulletPool = new ObjectPool<PlayerBullet>(() => {
                return Instantiate(playerBullet);
            }, bullet => {
                bullet.gameObject.SetActive(true);
            }, bullet => {
                bullet.gameObject.SetActive(false);
            }, bullet => {
                Destroy(bullet.gameObject);
            }, false, 50, 100);
            
            _bulletPool = new ObjectPool<Bullet>(() => {
                return Instantiate(enemyBullet);
            }, bullet => {
                bullet.gameObject.SetActive(true);
            }, bullet => {
                bullet.gameObject.SetActive(false);
            }, bullet => {
                Destroy(bullet.gameObject);
            }, false, 300, 5000);
        }

        public Sprite GetPlayerBulletSprite(int character, int type) {
            return playerBulletSprites[type];
        }

        public Bullet GetBullet() {
            var b = _bulletPool.Get();
            b.SetState(BulletStates.Spawning);
            return b;
        }

        public void ReleaseBullet(Bullet target) {
            _bulletPool.Release(target);
        }

        public Sprite GetBulletSprite(BulletType type) {
            return bulletSprites[(int)type];
        }

        public Material GetBulletMaterial(bool isGlowing) {
            return isGlowing ? glowMaterial[1] : glowMaterial[0];
        }
    }
}
