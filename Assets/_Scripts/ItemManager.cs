using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts {
    public enum ItemType{
        Faith,
        Power,
        Point,
        Gold,
        Full,
        BombFrag,
        LifeFrag,
        Bomb,
        Life,
        Card,
    }
    public class ItemManager : MonoBehaviour {
        public static ItemManager Manager;
        private ObjectPool<Item> _itemPool;
        [SerializeField] private Item item;
        [SerializeField] private Sprite[] itemSprites;
        
        private void Awake() {
            if (!Manager) {
                Manager = this;
            }
            else {
                Destroy(this.gameObject);
            }
        }
        
        public Sprite GetItemSprite(ItemType type) {
            return itemSprites[(int)type];
        }

        public void GetItemToPosition(ItemType type,Vector3 pos) {
            var i = _itemPool.Get();
            i.Init(type);
            i.transform.position = pos;
        }

        public void ReleaseItem(Item item) {
            _itemPool.Release(item);
        }
        void Start() {
            _itemPool = new ObjectPool<Item>(() => {
                return Instantiate(item);
            }, bullet => {
                bullet.gameObject.SetActive(true);
            }, bullet => {
                bullet.gameObject.SetActive(false);
            }, bullet => {
                Destroy(bullet.gameObject);
            }, false, 50, 200);
        }
    }
}
