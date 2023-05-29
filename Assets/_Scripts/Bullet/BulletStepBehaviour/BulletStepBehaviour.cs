using System;
using UnityEngine;

namespace _Scripts {
    [Serializable]
    public abstract class BulletStepBehaviour {
        [HideInInspector] public StepType type;
        [HideInInspector] public int timer;
        
        public float speed;
        public float rotation;
        public Vector3 direction;
        public abstract void StepBehaviour(Bullet bullet);
        public BulletStepBehaviour Clone(){
            return MemberwiseClone() as BulletStepBehaviour;
        }
    }
}