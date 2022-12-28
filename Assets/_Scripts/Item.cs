using System;
using UnityEngine;

namespace _Scripts {
    public class Item : MonoBehaviour {
        public SpriteRenderer spriteRenderer;
        public ItemType type;
        public float speed = 1f;
        public Vector3 direction = Vector3.down;

        private int _timer;
        private float _radius;

        private void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(ItemType type) {
            _timer = 0;
            this.type = type;
            spriteRenderer.sprite = ItemManager.Manager.GetItemSprite(type);
            spriteRenderer.color = (type == ItemType.Faith) ? 
                Calc.SetAlpha(Color.green, 0.6f) : Color.white;
        }

        void FixedUpdate() {
            transform.position += Time.fixedDeltaTime * speed * direction;
            if (transform.position.y <= -4.8f) {
                ItemManager.Manager.ReleaseItem(this);
            }
            _timer++;
        }
    }
}
