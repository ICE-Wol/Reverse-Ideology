using System;
using UnityEngine;

namespace _Scripts {
    [Serializable]
    public class StepLinear : BulletStepBehaviour {
        public bool isUniformSpeed;
        public int startTime;
        public int endTime;
        public float startSpeed;
        public float endSpeed;

        public StepLinear() {
            type = StepType.Linear;
            isUniformSpeed = true;
            timer = 0;
            startTime = 0;
            endTime = 0;
            startSpeed = 0;
            endSpeed = 0;
        }

        public override void StepBehaviour(Bullet bullet) {
            if (!isUniformSpeed) {
                if (endTime <= startTime) {
                    Debug.Log("Invalid input! endTime should always bigger than startTime");
                    return;
                }

                if (timer >= startTime && timer <= endTime) {
                    float t = (float)(timer - startTime) / (endTime - startTime);
                    speed = Mathf.SmoothStep(startSpeed, endSpeed, t);
                }
            }
            timer++;
            bullet.transform.position += speed * Time.fixedDeltaTime * direction;
        }
    }
}