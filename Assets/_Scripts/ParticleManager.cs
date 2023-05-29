using System;
using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts {
    public enum ParticleType {
        SelfDefine,
        BulletErased,
        ReimuMainBreak,
        FinBreak,
        NeedleBreak,
        WaterBombBreak,
        ParticleMaple,
        ParticleStar,
        ParticleRing,
        FairyBreakStream,
    }
    public class ParticleManager : MonoBehaviour {
        public FairyBreakEffectController fairyBreakEffect;

        [SerializeField] private Sprite[] bulletErased;
        [SerializeField] private Sprite[] reimuMainShoot;
        [SerializeField] private Sprite[] reimuHighSpeedShoot;
        [SerializeField] private Sprite[] fairyBreakStream;
        [SerializeField] private Particle particle;
        [SerializeField] private Sprite[] singleParticleSprites;
        [SerializeField] private Material[] singleParticleMaterials;
        private ObjectPool<Particle> _particlePool;
        public static ParticleManager Manager;

        private void Awake() {
            if (!Manager) {
                Manager = this;
            }
            else {
                Destroy(this.gameObject);
            }
            
            //TODO: put this in start will cause some priory level problem leading to some undefined problems,fix them later.
            //check Hatagora's lib for some ref. 
            _particlePool = new ObjectPool<Particle>(() => {
                return Instantiate(particle); 
            }, p => {
                p.gameObject.SetActive(true);
            }, p => {
                p.gameObject.SetActive(false);
                p.spriteRenderer.sprite = null;
                p.spriteRenderer.color = Color.white;
                p.transform.localScale = Vector3.one;
            }, p => {
                Destroy(p.gameObject);
            }, false, 50, 200);
        }
        
        public static void ReleaseParticle(Particle target) {
            Manager._particlePool.Release(target);
        }

        public static Particle GetParticle(Sprite sprite) {
            var p = Manager._particlePool.Get();
            p.SetType(sprite);
            return p;
        }

        public static Particle GetParticleToPosition(ParticleType type, Vector3 pos) {
            var p = GetParticle(type);
            p.transform.position = pos;
            return p;
        }

        public static Particle GetParticle(ParticleType type) {
            var p = Manager._particlePool.Get();
            switch (type) {
                case ParticleType.BulletErased:
                    p.SetAnimation(Manager.bulletErased);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[1];
                    break;
                case ParticleType.ReimuMainBreak:
                    p.SetAnimation(Manager.reimuMainShoot);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[1];
                    break;
                case ParticleType.FinBreak:
                    p.SetAnimation(Manager.reimuHighSpeedShoot);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[0];
                    break;
                case ParticleType.NeedleBreak:
                    p.SetSprite(Manager.singleParticleSprites[2]);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[0];
                    break;
                case ParticleType.WaterBombBreak:
                    p.SetSprite(Manager.singleParticleSprites[3]);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[1];
                    break;
                case ParticleType.ParticleStar:
                    p.SetSprite(Manager.singleParticleSprites[0]);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[0];
                    break;
                case ParticleType.ParticleMaple:
                    p.SetSprite(Manager.singleParticleSprites[1]);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[1];
                    break;
                case ParticleType.ParticleRing:
                    p.SetSprite(Manager.singleParticleSprites[4]);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[1];
                    break;
                case ParticleType.FairyBreakStream:
                    p.SetSprite(Manager.fairyBreakStream[0]);
                    p.SetAnimation(Manager.fairyBreakStream);
                    p.spriteRenderer.material = Manager.singleParticleMaterials[1];
                    break;
            }
            p.SetType(type);
            return p;
        }
    }
}
