using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Data {
    public class BulletData {
        public BulletType type;
        public Color color;
        public Vector3 scale;
        public float radius;
        public bool isGlowing;
        public bool isAuto;

        public Vector3 direction;
        public float rotation;
        public float speed;

        public BulletData() {
            type = BulletType.JadeS;
            color = Color.gray;
            radius = 0.1f;
            scale = Vector3.one;
            isGlowing = false;
            isAuto = false;
        }
    }

}