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
        [SerializeField] private CrystalPieceController crystalPiece;
        [SerializeField] private CrystalWholeController crystalWhole;
        
        private void Awake() {
            if (!Manager) {
                Manager = this;
            }
            else {
                Destroy(this.gameObject);
            }
        }
        
        public static Sprite GetItemSprite(ItemType type) {
            return Manager.itemSprites[(int)type];
        }

        public static void GetItemToPosition(ItemType type,Vector3 pos) {
            if (type <= ItemType.Full) {
                var i = Manager._itemPool.Get();
                i.Init(type);
                i.transform.position = pos;
            }
            else {
                if (type == ItemType.BombFrag) {
                    var p = Instantiate(Manager.crystalPiece, pos, 
                        Quaternion.Euler(0f,0f,0f));
                    p.type = ItemType.BombFrag;
                } else if (type == ItemType.LifeFrag) {
                    var p = Instantiate(Manager.crystalPiece, pos, 
                        Quaternion.Euler(0f,0f,0f));
                    p.type = ItemType.LifeFrag;
                } else if (type == ItemType.Bomb) {
                    var p = Instantiate(Manager.crystalWhole, pos, 
                        Quaternion.Euler(0f,0f,0f));
                    p.type = ItemType.Bomb;
                } else if (type == ItemType.Life) {
                    var p = Instantiate(Manager.crystalWhole, pos, 
                        Quaternion.Euler(0f,0f,0f));
                    p.type = ItemType.Life;
                }
            }
        }

        public static void ReleaseItem(Item item) {
            Manager._itemPool.Release(item);
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
