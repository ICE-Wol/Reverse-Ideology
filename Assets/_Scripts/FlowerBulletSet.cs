using System;
using _Scripts.Data;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace _Scripts {
    public class FlowerBulletSet : BulletController {
        private Bullet[,] _bullets;
        private int _intervalTime;
        private int _currentNum;
        private int _petalNum;
        private BulletData _heartData;
        private BulletData _petalData;
        private void Start() {
            _bullets = new Bullet[ways, _petalNum + 1];
        }

        public new void Activate(int ways, int petalNum, int intervalTime,
            float genRadius, float initDeg, float intervalDeg,
            BulletData heartData, BulletData petalData) {
            this.genRadius = genRadius;
            this.ways = ways;
            this.initDeg = initDeg;
            this.intervalDeg = intervalDeg;
            _petalNum = petalNum;
            _intervalTime = intervalTime;
            _petalData = petalData;
            _heartData = heartData;
            isActivated = true;
            
        }

        public new void GenerateBullets() {
            if (timer % _intervalTime == 0) {
                var heart = BulletManager.Manager.GetBullet();
                heart.SetData(_heartData);
                heart.startTime = timer;
                
                //fix
                if (_intervalTime < 0) timer *= -1;
                var rad = Mathf.Cos(Mathf.Deg2Rad * timer * 6) + 0.3f;
                Vector3 posHeart = transform.position 
                                   + rad * genRadius * (Vector3)Calc.Deg2Dir(timer + initDeg);
                heart.transform.position = posHeart;

                _bullets[_currentNum, 0] = heart;
                
                for (int i = 1; i <= _petalNum; i++) {
                    var petal = BulletManager.Manager.GetBullet();
                    if (rad < 0) {
                        _petalData.type = BulletType.Scale;
                        _petalData.color = Color.green;
                    }
                    else {
                        _petalData.type = BulletType.Rice;
                        _petalData.color = Color.blue;
                    }
                    petal.SetData(_petalData);
                    var posPetal = 0.2f * Calc.Deg2Dir(timer + i * 360f / _petalNum);
                    petal.transform.position = posHeart + (Vector3)posPetal;
                    petal.rotation = timer + i * 360f / _petalNum;
                    petal.transform.rotation = Quaternion.Euler(0, 0, timer + i * 360f / _petalNum);
                    _bullets[_currentNum, i] = petal;
                }
                //fix
                if (_intervalTime < 0) timer *= -1;
                _currentNum++;
            }
        }

        private void FixedUpdate() {
            if (isActivated && _currentNum < ways) {
                GenerateBullets();
                timer++;
            }

            if (_currentNum == ways) {
                for (var i = 0; i < ways; i++) {
                    float offset = Random.Range(-5f, 5f);
                    _bullets[i, 0].direction =
                        (float)_intervalTime / Mathf.Abs(_intervalTime) * Calc.Deg2Dir((float)_bullets[i, 0].startTime + offset);
                    _bullets[i, 0].isAuto = true;
                    for (var j = 1; j <= _petalNum; j++) {
                        _bullets[i, j].direction = Calc.Deg2Dir(_bullets[i, j].rotation);
                        _bullets[i, j].isAuto = true;
                    }
                }
                _currentNum++;
            }

            if (_currentNum > ways) {
                for (var i = 0; i < ways; i++) {
                    _bullets[i, 0].speed = Calc.Approach(_bullets[i, 0].speed, 2f, 32f);
                    for (var j = 1; j <= _petalNum; j++) {
                        _bullets[i, j].speed = Calc.Approach(_bullets[i, j].speed, 3f, 32f);
                    }
                }
            }
            
        }
    }
}
