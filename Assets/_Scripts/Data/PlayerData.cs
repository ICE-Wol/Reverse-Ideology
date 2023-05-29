namespace _Scripts.Data {
    public class PlayerData {
        public int Type;
        public int Life;
        public int Bomb;

        private int _lifeFrag;

        public int LifeFrag {
            set {
                if (value == 5) {
                    Life += 1;
                    _lifeFrag = 0;
                    return;
                }
                else {
                    _lifeFrag = value;
                }
            }
            get => _lifeFrag;
        }
        
        private int _bombFrag;

        public int BombFrag {
            set {
                if (value == 5) {
                    Bomb += 1;
                    _bombFrag = 0;
                    return;
                }
                else {
                    _bombFrag = value;
                }
            }

            get => _bombFrag;
        }
        
        private int _power;

        public int Power {
            set {
                if (_power == 400) {
                    Point += 1;
                    return;
                }
                _power = value;
                if (Power % 100 == 0)
                    GameManager.Player.playerSubCtrl.RefreshSub(value);
            }
            get => _power;
        }

        private int _maxPower;
        public int MaxPower {
            set {
                _maxPower = value;
                GameManager.Player.playerSubCtrl.GenerateSub(value);
            }
            get {
                return _maxPower;
            }
        }
        public int Point;
        public int Gold;
        public int Graze;
        public int MaxPoint;
        public int Score;
        public int MaxScore;

        public PlayerData() {
            Type = 0;
            Life = 2;
            LifeFrag = 0;
            Bomb = 3;
            BombFrag = 0;
            
            MaxPower = 400;//Generate sub before refresh.
            Power = 100;
            
            Graze = 0;
            MaxPoint = 10000;
            Score = 0;
            MaxScore = 0;
        }
    }
}
