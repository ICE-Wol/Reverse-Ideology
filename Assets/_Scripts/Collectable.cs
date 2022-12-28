using UnityEngine;

namespace _Scripts {
    public interface ICollectable {
        public float CollectRadius {
            set;
            get;
        }

        public void Collect();
    }
}