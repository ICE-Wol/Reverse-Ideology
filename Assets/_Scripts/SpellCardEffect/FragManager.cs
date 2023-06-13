using System;
using UnityEngine;

public class FragManager : MonoBehaviour {
    public Vector2 pointNum = new Vector2(11, 11);
    public float edgeLength = 1f;
    
    public SpriteRenderer pointPrefab;
    public FragCtrl trianglePrefab;
    public Transform fragFatherObject;
    private FragCtrl[,] _frags;
    private SpriteRenderer[,] _points;
    private Vector3[,] _position;

    private int _timer = 0;
    private bool _breakFlag = false;

    private readonly int[] _randomOrder = {
        0, 14, 67, 2, 48, 50, 71, 56, 11, 82, 40, 33, 62, 96, 25, 1, 84, 57, 55, 69, 97, 21, 49, 5, 23, 72, 31, 73, 44,
        27,
        85, 13, 80, 16, 74, 61, 34, 18, 59, 0, 46, 87, 3, 76, 93, 89, 15, 6, 92, 43, 12, 39, 42, 70, 26, 95, 36, 99,
        28, 86, 68, 4, 9, 10, 88, 38, 81, 22, 75, 41, 7, 35, 51, 94, 29, 19, 37, 64, 8, 17, 58, 98, 60, 65, 24, 20, 90,
        53, 30, 79, 78, 63, 66, 45, 32, 52, 77, 54, 47, 83, 91
    };
    
    private void GenerateMarkPointForTest() {
        _points = new SpriteRenderer[11, 11];
        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                var pos = transform.position + edgeLength * i * Vector3.right + edgeLength * j * Vector3.up;
                _points[i, j] = Instantiate(pointPrefab, pos, Quaternion.Euler(0f, 0f, 0f));
            }
        }
    }
    
    private void InitFrag() {
        _frags = new FragCtrl[10, 10];
        _position = new Vector3[11, 11];

        for (int i = 0; i < 11; i++) {
            for (int j = 0; j < 11; j++) {
                _position[i, j] = transform.position + edgeLength * i * Vector3.right + edgeLength * j * Vector3.up;
            }
        }

        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                var pos = (_position[i, j] + _position[i + 1, j] + _position[i + 1, j + 1] + _position[i, j + 1]) / 4;
                _frags[i, j] = Instantiate(trianglePrefab, fragFatherObject);
                _frags[i, j].transform.position = pos;
                Vector2 basePoint = new Vector2(i / 10f, j / 10f);

                Vector2[] uv = new Vector2[4];

                uv[0] = basePoint;
                uv[2] = basePoint + Vector2.up / 10f;
                uv[1] = basePoint + Vector2.right / 10f;
                uv[3] = basePoint + Vector2.right / 10f + Vector2.up / 10f;

                _frags[i, j].gameObject.GetComponent<MeshFilter>().mesh.uv = uv;
            }
        }
    }

    public void EnableFrag() {
        fragFatherObject.gameObject.SetActive(true);
        _breakFlag = true;
    }

    public void DisableFrag() {
        _breakFlag = false;
        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                _frags[i, j].transform.position = _position[i, j];
                _frags[i, j].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        fragFatherObject.gameObject.SetActive(false);
        _timer = 0;
    }

    public void FloatFrag() {
        for (int i = 0; i <= 3; i++) {
            int num = _randomOrder[_timer + 25 * i];
            int x = num % 10;
            int y = num / 10;
            _frags[x, y].StartFloat();
        }
    }

    private void Start() {
        InitFrag();
        DisableFrag();
    }

    private void FixedUpdate() {
        if (_breakFlag) {
            _timer++;
            if (_timer < 26) {
                FloatFrag();
            } else if (_timer > 180) {
                DisableFrag();
            }
        }
    }
}
