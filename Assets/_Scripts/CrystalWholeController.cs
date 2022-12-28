using UnityEngine;

namespace _Scripts {
    public class CrystalWholeController : MonoBehaviour {
        [SerializeField] private CrystalPieceController piece;
        public CrystalPieceController[] pieces;
        public StarRingController[] rings;
        public float radius = 0.3f;
        public ItemType type;
        public bool isCollected = false;

        private int _timer;

        void Start() {
            transform.localScale = 0.6f * Vector3.one;
            pieces = new CrystalPieceController[5];
            for (int i = 0; i < 5; i++) {
                pieces[i] = Instantiate(piece, this.transform);
                pieces[i].transform.rotation = Quaternion.Euler(0, 0, i * 72f + 90f);
                pieces[i].transform.localPosition = radius * Calc.Deg2Dir(i * 72f);
                pieces[i].type = type - 1;
                pieces[i].isSingle = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < 5; i++) {
                pieces[i].transform.rotation
                    = Quaternion.Euler(0,0,i * 72f + 90f + _timer / 6f);
                pieces[i].transform.localPosition
                    = radius * (Mathf.Cos(Mathf.Deg2Rad * _timer / 6f) / 8f + 1f)
                             * Calc.Deg2Dir(i * 72f + _timer / 6f);
            }

            _timer++;
        }
    }
}
