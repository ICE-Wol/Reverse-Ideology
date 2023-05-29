using System;
using _Scripts.BCtrl;
using UnityEngine;

namespace _Scripts.Fairies {
    public class St1SniperFairy : FairyEnemyController {
        
        private void Start() {
            Initialize();
            
        }

        private void FixedUpdate() { 
            PlayAnim();
            Movement();
            RegularShoot();
            timer++;
        }

        protected override void Movement() {
            transform.position = Vector3.zero + 2f * (Vector3)Calc.Deg2Dir(timer);
        }

        protected override void RegularShoot() {
            if ((timer >= startFrame) && ((timer - startFrame) % frameCycle) == 0) {
                var c = Instantiate(mainController, transform.position, transform.rotation);
                c.initDeg = (Vector2.SignedAngle(Vector2.right, c.Snip())) - 180f;
                c.Activate();
                ItemManager.GetItemToPosition(ItemType.Full, transform.position);
            }
        }

        protected override void DeadShoot() { }
    }
}