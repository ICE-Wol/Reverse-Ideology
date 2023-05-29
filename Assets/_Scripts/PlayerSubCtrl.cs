using Unity.VisualScripting;
using UnityEngine;
namespace _Scripts {
    public class PlayerSubCtrl : MonoBehaviour {
        [SerializeField] private PlayerSub playerSub;
        private PlayerSub[] _playerSubs;
        private int _numSubActivated;
        private int _numSubTotal;
        private float _radSub;

        private bool _isGenerated;
        private int _timer;

        public void GenerateSub(int maxPower) {
            // u can also create ten subs waiting for use.
            if (maxPower < 100) {
                DestroyAllSubs();
                return;
            }
            
            if (_playerSubs != null) {
                int nowSubTotal = maxPower / 100;
                if (_numSubTotal <= nowSubTotal) {
                    var array = new PlayerSub[nowSubTotal];
                    for (int i = 0; i < _numSubTotal; i++)
                        array[i] = _playerSubs[i];
                    for (int i =_numSubTotal; i < nowSubTotal; i++)
                        array[i] = Object.Instantiate(playerSub, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    _playerSubs.Free();
                    _playerSubs = array;
                }
                else {
                    var array = new PlayerSub[nowSubTotal];
                    for (int i = 0; i < nowSubTotal; i++)
                        array[i] = _playerSubs[i];
                    for (int i = nowSubTotal; i < _numSubTotal; i++)
                        Object.Destroy(_playerSubs[i].gameObject);
                    _playerSubs.Free();
                    _playerSubs = array;
                }

                _numSubTotal = nowSubTotal;
            }
            else {
                _numSubTotal = maxPower / 100;
                _playerSubs = new PlayerSub[_numSubTotal];
                for (int i = 0; i < _numSubTotal; i++) {
                    _playerSubs[i] = Object.Instantiate(playerSub, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    _playerSubs[i].gameObject.SetActive(false);
                }
            }

            _isGenerated = true;
        }

        private void DestroyAllSubs() {
            foreach (var sub in _playerSubs) {
                Destroy(sub.gameObject);
            }

            _playerSubs.Free();
        }

        public void RefreshSub(int currentPower) {
            _numSubActivated = (int)currentPower / 100;
            for (int i = 0; i < _numSubTotal; i++) {
                _playerSubs[i].gameObject.SetActive(i < _numSubActivated);
            }
        }

        private void UpdateSub() {
            if (!_isGenerated) return;
            _radSub = Input.GetKey(KeyCode.LeftShift) ?
                0.35f : (1f + 0.15f * Mathf.Sin(Mathf.Deg2Rad * _timer * 3f));
            for (int i = 0; i < _numSubActivated; i++) {
                var pos = transform.position;
                //x sin, y cos to make "tail fin slap"
                pos.x += _radSub * Mathf.Cos(Mathf.Deg2Rad * (_timer * 2f + i * 360f / _numSubActivated));
                pos.y += _radSub * Mathf.Sin(Mathf.Deg2Rad * (_timer * 2f + i * 360f / _numSubActivated));
                _playerSubs[i].transform.position
                    = _playerSubs[i].transform.position.
                        ApproachValue(pos, 8f * Vector3.one);
            }

            for (int i = _numSubActivated; i < _numSubTotal; i++) {
                _playerSubs[i].transform.position = transform.position;
            }
        }

        public void SubFire() {
            if (_timer % 6 == 0) {
                for (int i = 1; i <= _numSubActivated; i++) {
                    //(+ 90f - i * 360f / xx)to make "tail fin slap" 
                    //(_timer * 2f + i * 360f / _numSubActivated) to make normal circle
                    _playerSubs[i - 1].Fire((_timer * 2f + i * 360f / _numSubActivated) % 360f);
                }
            }
        }

        private void FixedUpdate() {
            UpdateSub();
            _timer++;
        }
    }
}