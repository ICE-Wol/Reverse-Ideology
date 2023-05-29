using UnityEngine;

namespace _Scripts.Interface {
    public interface ICollectable {
        public bool IsCollected {
            set;
            get;
        }
        public float CollectRadius {
            set;
            get;
        }

        public bool CheckCollect();
        public void CollectBehaviour();
        public void TriggerEffect();
    }
}