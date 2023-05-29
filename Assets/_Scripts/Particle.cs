using UnityEngine;

namespace _Scripts {
    public class Particle : MonoBehaviour {
        public SpriteRenderer spriteRenderer;
        private Sprite[] _anim;
        private int _spritePointer;
        private int _timer;
        
        private ParticleType _type;
        private int _frameSpeed;
        private float _direction;
        private float _rotation;
        private float _speed;
        private Vector3 _scale;
        
        /// <summary>
        /// Particle Maple
        /// </summary>
        private float _randomBase;

        /// <summary>
        /// particle star
        /// </summary>
        private float _randX;
        private float _randY;

        /// <summary>
        /// Whether the particle sprite will be played repeatedly.
        /// </summary>
        private bool _isLoop;
        
        /// <summary>
        /// Whether the particle sprite has animation.
        /// </summary>
        private bool _isAnimated;

        public void SetType(Sprite sprite) {
            _isLoop = true;
            _type = ParticleType.SelfDefine;
            _spritePointer = 0;
            _timer = 0;
            transform.localScale = Vector3.one;
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = spriteRenderer.color.SetAlpha(1f);
        }

        private Color _color;
        private int _propHueID;
        private int _propSatID;
        public Color color {
            set {
                _color = value;
                float H, S, V = 0;
                Color.RGBToHSV(value, out H, out S, out V);
                spriteRenderer.material.SetFloat(_propHueID, H);
                spriteRenderer.material.SetFloat(_propSatID, S);
            }
            get => _color;
        }

        public void SetDirectColor(Color c) {
            spriteRenderer.color = c;
        }
        
        public void SetDirection(float direction) {
            _direction = direction;
        }

        public void SetSprite(Sprite sprite) {
            spriteRenderer.sprite = sprite;
        }

        public void SetAnimation(Sprite[] sprites) {
            _anim = sprites;
        }
        
        void Awake() {
            //if put this into start() will cause some problem
            //the initial color will not be convert to the material properly
            //and the particle will sometimes be red
            _timer = 0;
            _propHueID = Shader.PropertyToID("_Hue");
            _propSatID = Shader.PropertyToID("_Saturation");
            //spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void SetType(ParticleType type) {
            _timer = 0;
            _spritePointer = 0;
            _direction = 0;
            _rotation = 0;
            _isAnimated = false;
            _type = type;
            switch (type) {
                case ParticleType.BulletErased:
                    _speed = 0;
                    _frameSpeed = 2;
                    _isAnimated = true;
                    _isLoop = false;
                    _rotation = Random.Range(0, 360);
                    spriteRenderer.color = spriteRenderer.color.SetAlpha(0.5f);
                    transform.localScale = 0.5f * Vector3.one;
                    _scale = Random.Range(0.8f, 1.2f) * Vector3.one;
                    transform.rotation = Quaternion.Euler(0, 0, _rotation);
                    break;
                case ParticleType.ReimuMainBreak:
                    //spriteRenderer.color = Color.white;
                    color = Color.blue;
                    _speed = 4f;
                    _frameSpeed = 4;
                    _isAnimated = true;
                    _isLoop = false;
                    break;
                case ParticleType.FinBreak:
                    spriteRenderer.color = Color.white;
                    spriteRenderer.color = spriteRenderer.color.SetAlpha(0.6f);
                    _speed = 1f;
                    _frameSpeed = 3;
                    _isAnimated = true;
                    _isLoop = false;
                    break;
                case ParticleType.NeedleBreak:
                    spriteRenderer.color = Color.cyan;
                    spriteRenderer.color = spriteRenderer.color.SetAlpha(1f);
                    transform.rotation = Quaternion.Euler(0, 0, 90f);
                    _speed = 1f;
                    _isAnimated = false;
                    break;
                case ParticleType.WaterBombBreak:
                    color = Color.cyan;
                    _speed = Random.Range(5f,8f);
                    _direction = Random.Range(-360f, 360f);
                    if (_direction < 0f) {
                        _speed -= 3f;
                        color = Color.blue;
                    }
                    _isAnimated = false;
                    break;
                case ParticleType.ParticleStar:
                    if (spriteRenderer.color == Color.white)
                        _speed = Random.Range(2f, 3f);
                    else _speed = Random.Range(4f, 5f);
                    _randX = Random.Range(0.25f, 1f);
                    _randY = Random.Range(0.25f, 1f);
                    _rotation = Random.Range(0, 360f);
                    _direction = _rotation;
                    break;
                case ParticleType.ParticleMaple:
                    spriteRenderer.color = spriteRenderer.color.SetAlpha(0.6f);
                    _randomBase = Random.Range(0, 360f);
                    _speed = Random.Range(2f, 5f);
                    _rotation = Random.Range(0f, 360f);
                    _direction = _rotation;
                    break;
                case ParticleType.ParticleRing:
                    color = Color.cyan;
                    _randomBase = Random.Range(0.5f, 1.5f);
                    transform.localScale = _randomBase * Vector3.one;
                    break;
                case ParticleType.FairyBreakStream:
                    _rotation = Random.Range(0, 360f);
                    _spritePointer = 0;
                    _frameSpeed = 3;
                    _isLoop = false;
                    _isAnimated = true;
                    break;
            }
        }
        
        void FixedUpdate() {
            //_type = 0, single image sprite to self identify.
            if (_isAnimated && (_timer % _frameSpeed == 0)) { 
                if (_spritePointer >= _anim.Length) {
                    if (_isLoop) {
                        _spritePointer = 0;
                        spriteRenderer.sprite = _anim[_spritePointer];
                    } else {
                        ParticleManager.ReleaseParticle(this);
                        //Note: this update will still be going shortly after the release function.
                        return;
                    }
                }
                else spriteRenderer.sprite = _anim[_spritePointer];
                _spritePointer++;
            }
            
            switch (_type) {
                case ParticleType.BulletErased:
                    spriteRenderer.color = spriteRenderer.color.Fade(16f);
                    transform.localScale = transform.localScale.ApproachValue(_scale, 16f * Vector3.one);
                    break;
                
                case ParticleType.ReimuMainBreak:
                    //Main Shoot always straight to top
                    transform.position += _speed * Time.fixedDeltaTime * (Vector3)_direction.Deg2Dir();
                    transform.rotation = Quaternion.Euler(0, 0, _direction);
                    transform.localScale =
                        transform.localScale.ApproachValue(2f * Vector3.one, 16f * Vector3.one);
                    spriteRenderer.color = spriteRenderer.color.Fade(6f);
                    break;
                
                case ParticleType.FinBreak:
                    transform.position += _speed * Time.fixedDeltaTime * (Vector3)_direction.Deg2Dir();
                    transform.rotation = Quaternion.Euler(0, 0, _direction);
                    break;
                
                case ParticleType.NeedleBreak:
                    transform.position += _speed * Time.fixedDeltaTime * (Vector3)_direction.Deg2Dir();
                    transform.rotation = Quaternion.Euler(0, 0, _direction);
                    /*transform.localScale =
                        Calc.Approach(transform.localScale, new Vector3(0f, 1.5f, 0f), 16f * Vector3.one);*/
                    transform.localScale =
                        transform.localScale.ApproachValue(new Vector3(1.5f, 0f, 0f), 8f * Vector3.one);
                    if (transform.localScale.x.Equal(0f, 0.01f)) {
                        ParticleManager.ReleaseParticle(this);
                    }
                    break;
                
                case ParticleType.WaterBombBreak:
                    var approachSpeedMultiplier = ((_direction > 0f) ? 1f : 2f);
                    _speed.ApproachRef(0f, 16f * approachSpeedMultiplier);
                    transform.position += _speed * Time.fixedDeltaTime * _direction.Deg2Dir3();
                    transform.localScale =
                        transform.localScale.ApproachValue(Vector3.zero, 16f * approachSpeedMultiplier * Vector3.one);
                    spriteRenderer.color = spriteRenderer.color.Fade(8f);
                    if (spriteRenderer.color.a.Equal(0f, 0.01f)) {
                        ParticleManager.ReleaseParticle(this);
                    }
                    break;
                
                case ParticleType.ParticleMaple:
                    _speed = _speed.ApproachValue(0, 16f);
                    transform.position += _speed * Time.fixedDeltaTime * (Vector3)_direction.Deg2Dir();
                    transform.rotation = Quaternion.Euler(0, 0, _rotation + _timer * _rotation / 360f);
                    var xScale = Mathf.Sin((_timer + _randomBase) * 3f * Mathf.Deg2Rad);
                    //transform.localScale = Calc.Approach(transform.localScale, Vector3.zero, 16f * Vector3.one);
                    transform.localScale = new Vector3(xScale, transform.localScale.y, 1f);
                    spriteRenderer.color = spriteRenderer.color.Fade(16f);
                    if (_speed.Equal(0f, 0.05f)) {
                        ParticleManager.ReleaseParticle(this);
                    }
                    break;
                
                case ParticleType.ParticleStar:
                    if (_speed.Equal(0f, 0.05f)) {
                        transform.position = transform.position.ApproachValue(GameManager.Player.transform.position,
                            _randX * 32f * Vector3.right + _randY * 32f * Vector3.up);
                        transform.localScale =
                            transform.localScale.ApproachValue(0.5f * Vector3.zero, 64f * Vector3.one);
                        if(transform.localScale.x.Equal(0, 0.05f))
                            ParticleManager.ReleaseParticle(this);
                    } else {
                        _speed = _speed.ApproachValue(0, 16f);
                        spriteRenderer.color = Calc.SetAlpha(spriteRenderer.color, 0.6f);
                        transform.position += _speed * Time.fixedDeltaTime * (Vector3)_direction.Deg2Dir();
                        transform.rotation = Quaternion.Euler(0, 0, _rotation);
                        transform.localScale =
                            transform.localScale.ApproachValue(0.3f * Vector3.one, 16f * Vector3.one);
                    }
                    break;
                
                case ParticleType.ParticleRing:
                    transform.localScale =
                        transform.localScale.ApproachValue((0.5f + _randomBase) * Vector3.one, 10f * Vector3.one);
                    if (transform.localScale.x > 0.5f) {
                        spriteRenderer.color = spriteRenderer.color.Fade(10f);
                    }
                    if (spriteRenderer.color.a.Equal(0f, 0.01f)) {
                        ParticleManager.ReleaseParticle(this);
                    }

                    break;
                
                case ParticleType.FairyBreakStream:
                    spriteRenderer.color = spriteRenderer.color.Fade(12f);
                    if (spriteRenderer.color.a.Equal(0f, 0.01f)) {
                        ParticleManager.ReleaseParticle(this);
                    }
                    break;
            }
            _timer++;
        }
    }
}
