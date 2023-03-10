using System;
using UnityEngine;

namespace _Scripts {
    public class CircularHealthBar : MonoBehaviour {
        [SerializeField] private float radius;
        [SerializeField] private int curPercent;
        [SerializeField] private int maxPercent;
        private Vector3[] _defaultPoints;
        private LineRenderer _line;

        private void Start() {
            _line = GetComponent<LineRenderer>();
            GenerateLine();
        }

        private void GenerateLine() {
            _defaultPoints = new Vector3[maxPercent + 1];
            for (int i = 0; i <= maxPercent; i++) {
                var degree = 360f / maxPercent * i + 90f;
                _defaultPoints[i] = radius * Calc.Deg2Dir(degree);
            }

            _line.positionCount = maxPercent + 1;
            _line.SetPositions(_defaultPoints);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current">current HP in %</param>
        public void RefreshLine(int current) {
            curPercent = current;
            _line.positionCount = curPercent + 1;
        }

        //test
        /*private void OnDrawGizmos() {
            foreach (var point in _defaultPoints) {
                Gizmos.DrawSphere(point, 0.1f);
            }
        }*/
    }
}

/*private MeshFilter _filter;
private Mesh _mesh;
private Vector3[] _defaultVertices;
private Vector3[] _vertices;
private Vector2[] _uv;
private int[] _triangles;*/

/*private void RefreshMesh() {
    for (int i = 0; i < maxPercent - curPercent; i++) {
        var tmp = maxPercent - curPercent;
        _vertices[i * 2] = _defaultVertices[tmp * 2];
        _vertices[i * 2 + 1] = _defaultVertices[tmp * 2 + 1];
    }
    
    for (int i = maxPercent - curPercent; i <= maxPercent; i++) {
        _vertices[i * 2] = _defaultVertices[i * 2];
        _vertices[i * 2 + 1] = _defaultVertices[i * 2 + 1];
    }
    
    _mesh.vertices = _vertices;
    _mesh.uv = _uv;
    _filter.mesh = _mesh;
}*/

/*private void GenerateMesh() {
    _defaultVertices = new Vector3[(maxPercent + 1) * 2];
    _vertices = new Vector3[(maxPercent + 1) * 2];
    _uv = new Vector2[(maxPercent + 1) * 2];
    _triangles = new int[maxPercent * 2 * 3];
    
    for (int i = 0; i <= maxPercent; i++) {
        var degree = - 360f / maxPercent * i + 90f;
        _defaultVertices[i * 2] = innerRadius * Calc.Degree2Direction(degree);
        _defaultVertices[i * 2 + 1] = outerRadius * Calc.Degree2Direction(degree);
        
        _vertices[i * 2] = _defaultVertices[i * 2];
        _vertices[i * 2 + 1] = _defaultVertices[i * 2 + 1];
        
        _uv[i * 2] = (float)i / maxPercent * Vector2.right;
        _uv[i * 2 + 1] = _uv[i * 2] + Vector2.up;
        
        if(i == maxPercent) continue;
        // the last two points dont have triangle sets.
        
        _triangles[i * 6] = i * 2;
        _triangles[i * 6 + 1] = (i + 1) * 2 + 1;
        _triangles[i * 6 + 2] = i * 2 + 1;

        _triangles[i * 6 + 3] = i * 2;
        _triangles[i * 6 + 4] = (i + 1) * 2;
        _triangles[i * 6 + 5] = (i + 1) * 2 + 1;
    }
    
    _mesh.vertices = _vertices;
    _mesh.uv = _uv;
    _mesh.triangles = _triangles;
    _filter.mesh = _mesh;
}*/
