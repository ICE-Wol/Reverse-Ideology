using System;
using UnityEngine;

namespace _Scripts {
    public class CrystalPieceController : MonoBehaviour {
        public SpriteRenderer piece;
        public SpriteRenderer glow;
        
        public Sprite[] crystals;
        public Sprite[] glows;
        public StarRingController ring;

        public ItemType type;
        public bool isSingle;
        public bool isCollected;

        /// <summary>
        /// Change the type of the piece.
        /// </summary>
        public void SetSprite(ItemType type) {
            piece.sprite = crystals[((int)type - 5)/2];
            glow.sprite = glows[((int)type - 5)/2];
        }

        private void Start() {
            SetSprite(type);
            if (isSingle) {
                Instantiate(ring, this.transform);
                transform.localScale = 0.8f * Vector3.one;
            }
        }
    }
}
