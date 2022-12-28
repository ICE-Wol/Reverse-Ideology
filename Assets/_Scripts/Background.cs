using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts {
    public class Background : MonoBehaviour {
        private MeshRenderer _meshRenderer;
        private Material _material;
        private float _alpha;
        private Color _color;
        void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();
            _material = _meshRenderer.material;
            //_alpha = _material.color.a;
            //_color = _material.shader.GetPropertyDefaultVectorValue("_TintColor");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            _material.mainTextureOffset -= Vector2.up * Time.fixedDeltaTime;
            //var a = 0.8f * _alpha + 0.2f * Mathf.Sin(Time.time * Mathf.Deg2Rad);
            //_material.shader.Set( new Color(_color.r, _color.g, _color.b, a));
        }
    }
}
