using UnityEngine;

namespace _Scripts {
    public class RingEffectController : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private Vector3[] _pos;
        
        /// <summary>
        /// The num of edges of the polygon to approach the circle.
        /// </summary>
        public int fragment;
        
        /// <summary>
        /// Radius of the circle.
        /// </summary>
        public float radius;
        
        /// <summary>
        /// Rotation speed(frame) of the circle.
        /// </summary>
        public float frameSpeed;
        //private bool _isActivated;
        private int _timer;
        private static readonly int MainTex = Shader.PropertyToID("_MainTex");

        void Start() {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.loop = true;
            _lineRenderer.positionCount = fragment;
            _lineRenderer.textureMode = LineTextureMode.Tile;
            //_lineRenderer.material.SetTextureScale(,new Vector2(8f,1f));
            //_lineRenderer.material.SetTextureScale(MainTex, new Vector2(1, 0.5f));
            _pos = new Vector3[fragment];
        }
        
        void Refresh() {
            for (int i = 0; i < fragment; i++) {
                _pos[i] = radius * Calc.Deg2Dir(_timer * frameSpeed + 360f / fragment * i);
            }
            _lineRenderer.SetPositions(_pos);
            _timer++;
        }
        void FixedUpdate()
        {
            Refresh();
        }
    }
}
