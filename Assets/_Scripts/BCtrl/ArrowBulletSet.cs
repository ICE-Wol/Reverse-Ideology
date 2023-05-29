using UnityEngine;

namespace _Scripts.BCtrl {
    public class ArrowBulletSet : BulletController {
        public float maxSpeed;
        public float decaySpeed;
        public override void GenerateBullets() {
            for (int i = 0; i < ways; i++) {
                var p = BulletManager.GetBullet();
                mainStepBehaviour.rotation = initDeg + intervalDeg * (i - ways/2);
                mainStepBehaviour.direction = mainStepBehaviour.rotation.Deg2Dir();
                //Lineral decay
                var step = (mainStepBehaviour as StepLinear);
                step.type = StepType.Linear;
                step.isUniformSpeed = false;
                step.startSpeed = maxSpeed - Mathf.Abs(i - ways / 2) * decaySpeed;
                step.endSpeed = step.startSpeed - 8f;
                step.endTime = 120;
                p.Step = step.Clone();
                p.Data = mainBulletData;
                p.SetParent(this, i);
                p.transform.position = transform.position + genRadius * mainStepBehaviour.direction;
            }
        }

        private new void FixedUpdate() {
            base.FixedUpdate();
        }
    }
}