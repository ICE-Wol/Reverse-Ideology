using UnityEngine;

namespace _Scripts {
    public class CrystalWholeController : MonoBehaviour {
        [SerializeField] private CrystalPieceController piece;
        public CrystalPieceController[] pieces;
        public StarRingController[] rings;
        public float radius = 0.3f;
        public ItemType type;
        public bool isCollected = false;

        public float speed = -2f;
        public float scale = 0f;
        public Vector3 direction = Vector3.down;
        private int _timer;

        void Start() {
            pieces = new CrystalPieceController[5];
            for (int i = 0; i < 5; i++) {
                pieces[i] = Instantiate(piece, this.transform);
                pieces[i].transform.rotation = Quaternion.Euler(0, 0, i * 72f + 90f);
                pieces[i].transform.localPosition = radius * (i * 72f).Deg2Dir();
                pieces[i].type = type - 2;
                pieces[i].isSingle = false;
            }
        }

        private bool CheckPiecesExistence() {
            for (int i = 0; i < 5; i++) {
                if (pieces[i] != null) return true;
            }

            return false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isCollected) {
                speed = speed.ApproachValue(0.5f, 64f);
                transform.position += Time.fixedDeltaTime * speed * direction;

                scale = scale.ApproachValue(0.6f, 32f);
                transform.localScale = scale * Vector3.one;
            }

            if (isCollected && !CheckPiecesExistence()) {
                DestroyImmediate(gameObject);
            }

            if (!isCollected) {
                for (int i = 0; i < 5; i++) {
                    pieces[i].transform.rotation
                        = Quaternion.Euler(0,0,i * 72f + 90f + _timer / 6f);
                    pieces[i].transform.localPosition
                        = radius * (Mathf.Cos(Mathf.Deg2Rad * _timer / 6f) / 8f + 1f)
                                 * (i * 72f + _timer / 6f).Deg2Dir();
                }
                
                if (GameManager.GetPlayerDistance(transform.position) < radius || 
                    GameManager.Player.transform.position.y >= 2.5f) {
                    isCollected = true;
                    foreach (var p in pieces) {
                        p.isCollected = true;
                    }
                    foreach (var ring in rings) {
                        ring.isTriggered = true;
                    }
                    
                    if (type == ItemType.Bomb) {
                        GameManager.Player.playerData.Bomb += 1;
                    } else if (type == ItemType.Life) {
                        GameManager.Player.playerData.Life += 1;
                    }
                
                    PlayerStatusManager.Manager.RefreshSlot();
                }
            }

            _timer++;
        }
    }
}
