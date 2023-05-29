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
        public float radius = 0.15f;
        public bool isSingle;
        public bool isCollected = false;
        public bool haveReleasedParticles = false;

        public float speed = -2f;
        public float scale = 0f;
        public Vector3 direction = Vector3.down;

        /// <summary>
        /// Change the type of the piece.
        /// </summary>
        public void SetSprite(ItemType type) {
            if(type == ItemType.BombFrag || type == ItemType.Bomb) {
                piece.sprite = crystals[0];
                glow.sprite = glows[0];
            }
            else if (type == ItemType.LifeFrag || type == ItemType.Life) {
                piece.sprite = crystals[1];
                glow.sprite = glows[1];
            }
        }

        private void Start() {
            SetSprite(type);
            if (isSingle) {
                //saving the actual reference of the ring object instead of prefab reference.
                ring = Instantiate(ring, this.transform);
                transform.localScale = 0.8f * Vector3.one;
            }

            haveReleasedParticles = false;
        }

        private void Update() {
            if (isSingle && !isCollected) {
                speed = speed.ApproachValue(0.5f, 64f);
                transform.position += Time.fixedDeltaTime * speed * direction;
            
                scale = scale.ApproachValue(0.6f, 32f);
                transform.localScale = scale * Vector3.one;
            }
            
            var dis = GameManager.GetPlayerDistance(transform.position);
            if (isSingle && !isCollected) {
                if (dis < radius || GameManager.Player.transform.position.y >= 2.5f) {
                    isCollected = true;
                    ring.isTriggered = true;

                    if (type == ItemType.BombFrag) {
                        GameManager.Player.playerData.BombFrag += 1;
                    } else if (type == ItemType.LifeFrag) {
                        GameManager.Player.playerData.LifeFrag += 1;
                    }
                    
                    PlayerStatusManager.Manager.RefreshSlot();
                    
                    for (int i = 0; i <= 3; i++) {
                        for (float j = -0.5f * i; j < 0.5f * i + 0.01f; j++) {
                            //print("par");
                            var p = ParticleManager.GetParticle(ParticleType.ParticleStar);
                            if(type == ItemType.BombFrag || type == ItemType.Bomb)
                                p.SetDirectColor(Color.green);
                            else if(type == ItemType.LifeFrag || type == ItemType.Life)
                                p.SetDirectColor(Color.magenta);
                            p.transform.position = transform.position + 0.05f * (i - 3) * Vector3.up + 0.05f * j * Vector3.right;
                            if(i == 3) continue;
                            p = ParticleManager.GetParticle(ParticleType.ParticleStar);
                            if(type == ItemType.BombFrag || type == ItemType.Bomb)
                                p.SetDirectColor(Color.green);
                            else if (type == ItemType.LifeFrag || type == ItemType.Life) {
                                p.SetDirectColor(Color.magenta);
                            }

                            p.transform.position = transform.position - 0.05f * (i - 3) * Vector3.up + 0.05f * j * Vector3.right;
                        }
                    }
                    
                    for (int i = 0; i <= 3; i++) {
                        for (float j = -0.5f * i; j < 0.5f * i + 0.01f; j++) {
                            var p = ParticleManager.GetParticle(ParticleType.ParticleStar);
                            p.transform.position = transform.position + 0.05f * (i - 3) * Vector3.up + 0.05f * j * Vector3.right;
                            p.SetDirectColor(Color.white);
                            if(i == 3) continue;
                            p = ParticleManager.GetParticle(ParticleType.ParticleStar);
                            p.transform.position = transform.position - 0.05f * (i - 3) * Vector3.up + 0.05f * j * Vector3.right;
                            p.SetDirectColor(Color.white);
                        }
                    }
                }
            }

            if (!isSingle && isCollected && !haveReleasedParticles) {
                for (int i = 0; i <= 3; i++) {
                    for (float j = -0.5f * i; j < 0.5f * i + 0.01f; j++) {
                        var p = ParticleManager.GetParticle(ParticleType.ParticleStar);
                        if(type == ItemType.BombFrag || type == ItemType.Bomb)
                            p.SetDirectColor(Color.green);
                        else if(type == ItemType.LifeFrag || type == ItemType.Life)
                            p.SetDirectColor(Color.magenta);
                        p.transform.position = transform.position + 0.05f * (i - 3) * Vector3.up + 0.05f * j * Vector3.right;
                        if(i == 3) continue;
                        p = ParticleManager.GetParticle(ParticleType.ParticleStar);
                        if (type == ItemType.BombFrag || type == ItemType.Bomb) {
                            p.SetDirectColor(Color.green);
                        }
                        else if (type == ItemType.LifeFrag || type == ItemType.Life) {
                            p.SetDirectColor(Color.magenta);
                        }

                        p.transform.position = transform.position - 0.05f * (i - 3) * Vector3.up + 0.05f * j * Vector3.right;
                    }
                }
                    
                for (int i = 0; i <= 3; i++) {
                    for (float j = -0.5f * i; j < 0.5f * i + 0.01f; j++) {
                        var p = ParticleManager.GetParticle(ParticleType.ParticleStar);
                        p.transform.position = transform.position + 0.05f * (i - 3) * Vector3.up + 0.05f * j * Vector3.right;
                        p.SetDirectColor(Color.white);
                        if(i == 3) continue;
                        p = ParticleManager.GetParticle(ParticleType.ParticleStar);
                        p.transform.position = transform.position - 0.05f * (i - 3) * Vector3.up + 0.05f * j * Vector3.right;
                        p.SetDirectColor(Color.white);
                    }
                }

                haveReleasedParticles = true;
            }
   
            if (isCollected) {
                piece.color = piece.color.Fade(8f);
                glow.color = glow.color.Fade(16f);

                if (glow.color.a <= 0.01f) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
