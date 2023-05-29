using System;
using System.Collections.Generic;
using _Scripts.Data;
using TMPro;
using UnityEngine;

namespace _Scripts {
    public class GameManager : MonoBehaviour {
        public static GameManager Manager;
        public static PlayerController Player;
        public TMP_Text HiScoreText;
        public TMP_Text CurScoreText;
        public TMP_Text PowerText;
        public TMP_Text MaxPointText;
        public TMP_Text GrazeText;

        public static string NumToCommaStr(int num) {
            if (num == 0) return "0";
            Stack<int> numStack = new Stack<int>();
            while (num != 0) {
                numStack.Push(num % 1000);
                num /= 1000;
            }
            int max = numStack.Count;

            // Use StringBuilder for concatenation in tight loops.
            var sb = new System.Text.StringBuilder();
            for (int i = 1; i <= max; i++) {
                var top = numStack.Pop();
                if (i != 1) {
                    if (top < 100) {
                        sb.Append("0");
                    }

                    if (top < 10) {
                        sb.Append("0");
                    }
                }
                sb.Append(top.ToString());
                if (i != max) sb.Append(",");
            }

            return sb.ToString();
        }

        public static string NumToPowerText(int num, int maxNum) {
            var sb = new System.Text.StringBuilder();
            sb.Append("<color=#EECBAD>");
            sb.Append(num / 100);
            sb.Append(".<size=25>");
            var t = (num % 100);
            if (t < 10) sb.Append("0");
            sb.Append(t);
            sb.Append("</size>/");
            sb.Append(maxNum / 100);
            sb.Append(".<size=25>");
            t = (maxNum % 100);
            if (t < 10) sb.Append("0");
            sb.Append(t);
            sb.Append("</size></color>");
            return sb.ToString();
        }

        [SerializeField] private PlayerController player;

        private void Awake() {
            if (Manager == null) {
                Manager = this;
            }
            else {
                Destroy(this.gameObject);
            }

            Player = player;
        }

        public static float GetPlayerDistance(Vector3 startPos) {
            return Vector2.Distance(startPos, Player.transform.position);
        }

        [SerializeField] private Sprite[] player00Idle;
        [SerializeField] private Sprite[] player00Left;
        [SerializeField] private Sprite[] player00Right;

        public Sprite[] GetPlayerAnim(int ord, int dir) {
            switch (ord) {
                default:
                    switch (dir) {
                        case 0:
                            return player00Idle;
                        case 1:
                            return player00Left;
                        case 2:
                            return player00Right;
                    }
                    break;
            }
            return null;
        }
    }
}
