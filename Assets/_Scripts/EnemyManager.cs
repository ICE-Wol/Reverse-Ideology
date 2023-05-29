using System.Collections.Generic;
using _Scripts.Fairies;
using UnityEngine;

namespace _Scripts {
    public class EnemyManager : MonoBehaviour
    {
        public List<FairyEnemyController> enemyList;
        public static EnemyManager Manager;

        public static void AddEnemy(FairyEnemyController ctrl) {
            Manager.enemyList.Add(ctrl);
        }
        private void Awake() {
            if (!Manager) {
                Manager = this;
            }
            else {
                Destroy(this.gameObject);
            }
        }
    }
}
