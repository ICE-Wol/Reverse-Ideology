
using UnityEngine;

namespace _Scripts {
    public class Particle : MonoBehaviour {
        public SpriteRenderer spriteRenderer;
        private Sprite[] _anim;
        private int _frameSpeed;
        private int _spritePointer;
        private int _timer;
        private int _type;
        private float _direction;
        
        /// <summary>
        /// Whether the particle sprite will be played repeatedly.
        /// </summary>
        private bool _isLoop;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="character">0: dont belong to any characters.</param>
        /// <param name="order"></param>
        public void SetType(int character, int order) {
            _type = character * 10 + order;
            _spritePointer = 0;
            _timer = 0;
            transform.localScale = Vector3.one;
            spriteRenderer.sprite = null;
            spriteRenderer.color = Calc.SetAlpha(spriteRenderer.color, 1f);
            _anim = ParticleManager.Manager.GetParticleAnim(character, order);
        }
        
        public void SetType(Sprite sprite) {
            _isLoop = true;
            _type = 0;
            _spritePointer = 0;
            _timer = 0;
            transform.localScale = Vector3.one;
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = Calc.SetAlpha(spriteRenderer.color, 1f);
        }

        public void SetColor(Color color) {
            spriteRenderer.color = color;
        }
        
        public void SetDirection(float direction) {
            _direction = direction;
        }
        
        void Start() {
            _timer = 0;
            _frameSpeed = 10;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        void FixedUpdate() {
            if (_type == 10 || _type == 11) {
                transform.localScale += Time.fixedDeltaTime * 3f * Vector3.one;
                transform.position += Time.fixedDeltaTime * 3f * (Vector3)Calc.Deg2Dir(_direction);
                transform.rotation = Quaternion.Euler(0f, 0f, _direction);

                spriteRenderer.color = Calc.Fade(spriteRenderer.color, 5f);
            }
            
            //_type = 0, single image sprite to self identify.
            if (_timer % _frameSpeed == 0 && _type != 0) { 
                if (_spritePointer >= _anim.Length) {
                    if (_isLoop) {
                        _spritePointer = 0;
                        spriteRenderer.sprite = _anim[_spritePointer];
                    } else {
                        ParticleManager.Manager.ReleaseParticle(this);
                        //Note: this update will still be going shortly after the release function.
                        return;
                    }
                }
                else spriteRenderer.sprite = _anim[_spritePointer];
                _spritePointer++;
            }
            _timer++;
        }
    }
}
