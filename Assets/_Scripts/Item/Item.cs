using System;
using _Scripts.Interface;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts {
    public class Item : MonoBehaviour, ICollectable {
        public SpriteRenderer spriteRenderer;
        public ItemType type;
        public Vector3 direction = Vector3.down;
        public float rotation;
        public float speed;
        public float scale;

        private int _timer;

        private void Start() {
            CollectRadius = 0.1f;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Init(ItemType type) {
            IsCollected = false;
            _timer = 0;
            speed = -1.5f;
            rotation = 1080f;
            if (Random.Range(0, 1f) < 0.5f) {
                rotation *= -1;
            }
            scale = 0;
            this.type = type;
            spriteRenderer.sprite = ItemManager.GetItemSprite(type);
            spriteRenderer.color = (type == ItemType.Faith) ? 
                Color.green.SetAlpha(0.6f) : Color.white;
        }

        void FixedUpdate() {
            speed = speed.ApproachValue(1.5f, 32f);
            rotation = rotation.ApproachValue(0f, 32f);
            scale = scale.ApproachValue(1f, 32f);
            
            transform.localScale = scale * Vector3.one;
            transform.rotation = Quaternion.Euler(0,0,rotation);
            
            if(!IsCollected)
                transform.position += Time.fixedDeltaTime * speed * direction;
            
            if (transform.position.y <= -4.8f) {
                ItemManager.ReleaseItem(this);
            }

            if (!IsCollected) {
                IsCollected = CheckCollect();
            }
            else {
                CollectBehaviour();
            }

            _timer++;
        }

        public bool IsCollected { get; set; }
        public float CollectRadius { get; set; }

        public bool CheckCollect() {
            var distance 
                = Vector2.Distance(transform.position, 
                    GameManager.Player.transform.position);
            var targetRadius = GameManager.Player.ItemRadius;
            return (distance <= CollectRadius + targetRadius) || 
                   (GameManager.Player.transform.position.y >= 2.5f);
        }

        public void TriggerEffect() {
            var data = GameManager.Player.playerData;
            switch (type) {
                case ItemType.Power:
                    data.Power += 10;
                    GameManager.Manager.PowerText.text
                        = GameManager.NumToPowerText(data.Power, data.MaxPower);

                    break;
                case ItemType.Point:
                    data.Point++;
                    //TODO: differ from get height.
                    data.Score += data.MaxPoint;
                    GameManager.Manager.CurScoreText.text
                        = GameManager.NumToCommaStr(data.Score);
                    break;
                case ItemType.Gold:
                    data.Gold++;
                    break;
                case ItemType.Full:
                    data.Power = data.MaxPower;
                    break;
            }
        }
        public void CollectBehaviour() {
            var targetPosition = GameManager.Player.transform.position;
            transform.position = transform.position.ApproachValue(targetPosition, 8f * Vector3.one, 0.1f);
            if (transform.position.Equal(targetPosition, 0.1f)) {
                TriggerEffect();
                ItemManager.ReleaseItem(this);
            }
        }
    }
}
