using UnityEngine;
using UnityEngine.Pool;

namespace _Scripts {
    public class ParticleManager : MonoBehaviour {
        [SerializeField] private Sprite[] particle00;
        [SerializeField] private Sprite[] player01Shoot00;
        [SerializeField] private Sprite[] player01Shoot01;
        
        [SerializeField] private Particle particle;
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
            }, bullet => {
                bullet.gameObject.SetActive(true);
            }, bullet => {
                bullet.gameObject.SetActive(false);
            }, bullet => {
                Destroy(bullet.gameObject);
            }, false, 50, 200);
        }
        
        public void ReleaseParticle(Particle target) {
            _particlePool.Release(target);   
        }

        public Particle GetParticle(int character, int ord) {
            var p = _particlePool.Get();
            p.SetType(character, ord);
            return p;
        }
        
        public Particle GetParticle(Sprite sprite) {
            var p = _particlePool.Get();
            p.SetType(sprite);
            return p;
        }

        public Sprite[] GetParticleAnim(int character, int ord) {
            switch (character) {
                default:
                case 0:
                    switch (ord) {
                        default:
                        case 0:
                            //0 0 can define sprite by yourself.
                            return particle00;
                            break;
                    }
                    break;
                
                case 1:
                    switch (ord) {
                    default:
                        return player01Shoot00;
                    case 1:
                        return player01Shoot01;
                }
            }
        }
    }
}
