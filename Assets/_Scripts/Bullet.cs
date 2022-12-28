using System;
using System.Collections.Generic;
using _Scripts.Data;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts {
    public enum BulletStates {
        Inactivated,
        Spawning,
        Activated,
    }
    
    public class Bullet : MonoBehaviour {
        public SpriteRenderer spriteRenderer;
        public BulletController parent;
        public int order;

        private int _timer;
        private BulletStates _state;

        public int startTime;
        public BulletType type;
        public Color color;
        public float alpha;
        public Vector3 scale;
        public float radius;
        public bool isGlowing;
        
        public Vector3 direction;
        public float speed;
        public float rotation;

        /// <summary>
        /// The tag showing whether the bullet is moving by itself.
        /// </summary>
        public bool isAuto;


        private int _propHueID;
        private int _propSatID;
        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            _propHueID = Shader.PropertyToID("_Hue");
            _propSatID = Shader.PropertyToID("_Saturation");
        }

        public void SetParent(BulletController controller, int ord) {
            parent = controller;
            order = ord;
        }

        public void SetData(BulletData data) {
            type = data.type;
            color = data.color;
            isGlowing = data.isGlowing;
            scale = data.scale;

            alpha = 1;
            
            isAuto = data.isAuto;
            if (isAuto) {
                speed = data.speed;
                direction = data.direction;
                rotation = data.rotation;
            }
            
            
            spriteRenderer.sprite
                = BulletManager.Manager.GetBulletSprite(BulletType.Point);
            spriteRenderer.material
                = BulletManager.Manager.GetBulletMaterial(isGlowing);

            float H = 0, S = 0, V = 0;
            Color.RGBToHSV(color,out H,out S,out V);
            spriteRenderer.material.SetFloat(_propHueID, H);
            spriteRenderer.material.SetFloat(_propSatID, S);
        }

        public bool CheckCollision(GameObject target, float r) {
            var d2 = (transform.position - target.transform.position).sqrMagnitude;
            return (radius * radius + r * r > d2);
        }

        public void CheckBound() {
            var pos = transform.position;
            if (Mathf.Abs(pos.x) > 6f || Mathf.Abs(pos.y) > 6f) {
                Release();
            }
        }

        public void Release() {
            _state = BulletStates.Inactivated;
            if (parent) {
                parent.ReleaseBulletInArray(order);
                parent = null;
                order = 0;
            }
            BulletManager.Manager.ReleaseBullet(this);
        }

        /*private void Generate(int mode) {
            switch (mode) {
                default:
                    break;
                case 1:
                    if (_timer % 10 == 0) {
                        var p1 = BulletManager.Manager.BulletPool.Get();
                        p1.SetData(new BulletData());
                        p1.transform.position = this.transform.position;
                        p1.SetMovement(0, new float[] { _rotation + 30f, (1.5f + Mathf.Sin(Mathf.Deg2Rad*2f*_timer))/2f });

                        var p2 = BulletManager.Manager.BulletPool.Get();
                        p2.SetData(new BulletData());
                        p2.transform.position = this.transform.position;
                        p2.SetMovement(0, new float[] { _rotation - 30f, (1.5f + Mathf.Sin(Mathf.Deg2Rad*2f*_timer))/2f });
                    }

                    break;
            }
        }*/
        private float _fogScale;
        private float _fogAlpha;
        
        public BulletStates GetState() => _state;
        public void SetState(BulletStates state) {
            _state = state;
            switch (state) {
                case BulletStates.Spawning:
                    _fogScale = 4f;
                    _fogAlpha = 0f;
                    break;
                case BulletStates.Activated:
                    transform.localScale = scale;
                    spriteRenderer.sprite
                        = BulletManager.Manager.GetBulletSprite(type);
                    var c = spriteRenderer.color;
                    c.a = alpha;
                    spriteRenderer.color = c;
                    break;
            }
        }

        private void FixedUpdate() {
            switch (_state) {
                case BulletStates.Spawning:
                    _fogScale = Calc.Approach(_fogScale, 1.2f, 4f);
                    _fogAlpha = Calc.Approach(_fogAlpha, 0.8f, 16f);

                    transform.localScale = _fogScale * Vector3.one;
                    var c = spriteRenderer.color;
                    c.a = _fogAlpha;
                    spriteRenderer.color = c;

                    if (Calc.Equal(_fogScale, 1.2f, 0.1f))
                        SetState(BulletStates.Activated);
                    break;
                case BulletStates.Activated:
                    if (isAuto) {
                        CheckCollision(GameManager.manager.player.gameObject, 0f);
                        transform.position += Time.fixedDeltaTime * speed * direction;
                        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
                    }
                    
                    CheckBound();
                    _timer++;
                    break;
            }
        }
    }
}
