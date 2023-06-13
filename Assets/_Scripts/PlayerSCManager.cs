using UnityEngine;

namespace _Scripts {
    public class PlayerSCManager : MonoBehaviour {
        [SerializeField] private WaterBombController waterBomb;
        private WaterBombController[] _waterBombs;
        private float[] _bombRadius;

        private float _timer;
        //private float _radius;
        private bool _trigger;
        //private int _cnt;
        private void InitSpellCard() {
            _timer = 0;
            //_radius = 0;
            //_cnt = 0;
            _waterBombs = new WaterBombController[8];
            _bombRadius = new float[8];
            for (int i = 0; i < 8; i++) {
                _waterBombs[i] = Instantiate(waterBomb,transform);
                _waterBombs[i].transform.localScale = Vector3.zero;
                _waterBombs[i].order = i;
                _bombRadius[i] = 0f;
            }
        }

        private void Start() {
            InitSpellCard();
        }

        private void FixedUpdate() {
            _timer++;
            /*if (!_trigger) {
                if (Input.anyKeyDown) {
                    _trigger = true;
                    InitSpellCard();
                }
            }
            else {

                _radius.ApproachRef(3f, 64f);
                for (int i = 0; i < 8; i++) {
                    _waterBombs[i].transform.localScale =
                        _waterBombs[i].transform.localScale.ApproachValue(_waterBombs[i].random * Vector3.one, 32f * Vector3.one);
                    _waterBombs[i].transform.localPosition = _radius * (360f / 8f * i + _timer * 3f).Deg2Dir3();
                }

                _timer++;
            }*/
        }

        private bool SpellCardValidityCheck() {
            return GameManager.Player.playerData.Bomb > 0;
        }
    }
}
