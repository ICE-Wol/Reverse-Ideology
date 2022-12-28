using System;
using _Scripts.Data;
using TMPro;
using UnityEngine;

namespace _Scripts {
    public class GameManager : MonoBehaviour {
        public static GameManager manager;
        public PlayerData playerData;
        public PlayerController player;

        public TMP_Text playerDataText;

        private void Awake() {
            if (manager == null) {
                manager = this;
            }
            else {
                Destroy(this.gameObject);
            }
            
            playerData = new PlayerData();
            RefreshPlayerData();
        }

        public void RefreshPlayerData() {
            playerDataText.text = "\nPlayer:" + playerData.Life +
                                  "\nBomb:" + playerData.Bomb +
                                  "\nPower:" + playerData.Power;
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
