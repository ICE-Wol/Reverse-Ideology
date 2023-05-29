using System;
using _Scripts.BCtrl;
using _Scripts.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Fairies {
    public class St1OpeningFairy : FairyEnemyController {
        public Vector3 dir;
        public float spd;
        public BulletData mainData;
        public BulletData deadData;
        public StepLinear mainStep;
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
            transform.position += spd * Time.fixedDeltaTime * dir;
        }

        protected override void RegularShoot() {
            if ((timer >= startFrame) && ((timer - startFrame) % frameCycle) == 0) {
                mainStep.type = StepType.Linear;
                var p = BulletManager.GetBullet();
                p.transform.position = transform.position;
                mainStep.direction = p.Snip();
                p.Data = mainData;
                p.Step = mainStep.Clone();
            }
        }

        protected override void DeadShoot() { }
    }
}