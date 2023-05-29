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

    public enum PlayerBulletType {
        ReimuMain,
        Fin,
        Needle
    }
    public class BulletManager : MonoBehaviour {
        [SerializeField] private PlayerBullet playerBullet;
        [SerializeField] private Bullet enemyBullet;
        [SerializeField] private Sprite[] playerBulletSprites;
        [SerializeField] private Sprite[] bulletSprites;
        [SerializeField] private Material[] glowMaterial;
        
        private ObjectPool<PlayerBullet> _playerBulletPool;
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
            _playerBulletPool = new ObjectPool<PlayerBullet>(() => {
                return Instantiate(playerBullet);
            }, bullet => {
                bullet.gameObject.SetActive(true);
            }, bullet => {
                bullet.gameObject.SetActive(false);
                bullet.spriteRenderer.sprite = null;
                bullet.spriteRenderer.color = Color.white;
            }, bullet => {
                Destroy(bullet.gameObject);
            }, false, 50, 100);
            
            _bulletPool = new ObjectPool<Bullet>(() => {
                return Instantiate(enemyBullet);
            }, bullet => {
                bullet.gameObject.SetActive(true);
            }, bullet => {
                bullet.gameObject.SetActive(false);
                bullet.spriteRenderer.sprite = null;
                bullet.spriteRenderer.color = Color.white;
            }, bullet => {
                Destroy(bullet.gameObject);
            }, false, 300, 5000);
        }

        public static PlayerBullet GetPlayerBulletWithType(PlayerBulletType type) {
            var bullet = Manager._playerBulletPool.Get();
            bullet.type = type;
            bullet.spriteRenderer.sprite = GetPlayerBulletSprite(type);
            return bullet;
        }

        public static void ReleasePlayerBullet(PlayerBullet bullet) {
            Manager._playerBulletPool.Release(bullet);
        }

        private static Sprite GetPlayerBulletSprite(PlayerBulletType type) {
            return Manager.playerBulletSprites[(int)type];
        }

        public static Bullet GetBullet() {
            var b = Manager._bulletPool.Get();
            b.SetState(BulletStates.Spawning);
            return b;
        }

        public static void ReleaseBullet(Bullet target) {
            Manager._bulletPool.Release(target);
            target.Step = null;
        }

        public static Sprite GetBulletSprite(BulletType type) {
            return Manager.bulletSprites[(int)type];
        }

        public static Material GetBulletMaterial(bool isGlowing) {
            return isGlowing ? Manager.glowMaterial[1] : Manager.glowMaterial[0];
        }
    }
}
