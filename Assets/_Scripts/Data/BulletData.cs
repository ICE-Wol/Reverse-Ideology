using System;
using _Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Data {
    [Serializable]
    public class BulletData {
        public BulletType type;
        public Color color;
        public Vector3 scale;
        public float radius;
        public bool isGlowing;
        public bool moveWhenSpawning;
        public BulletData Clone(){
            return MemberwiseClone() as BulletData;
        }
        public BulletData() {
            type = BulletType.JadeS;
            color = Color.gray;
            radius = 0.1f;
            scale = Vector3.one;
            isGlowing = false;
        }
    }

}