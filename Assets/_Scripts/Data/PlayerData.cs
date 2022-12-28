using UnityEngine;

namespace _Scripts.Data {
    public class PlayerData {
        public int Type;
        public int Life;
        public int LifeFrag;
        public int Bomb;
        public int BombFrag;
        public int Power;
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
            Power = 400;
            Graze = 0;
            MaxPoint = 1000;
            Score = 0;
            MaxScore = 0;
        }
    }
}
