using _Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class FragCtrl : MonoBehaviour {
    private bool _breakFlag;

    private Vector3 _target;
    private Vector3 _rot;

    private float _speed;
    private float _finalSpeed;
    public void StartFloat() {
        _breakFlag = true;
        _target = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
        _rot = Vector3.zero;
        _speed = 0f;
        _finalSpeed = Random.Range(0.25f, 0.4f);
    }

    private void FixedUpdate() {
        if (_breakFlag) {
            _speed.ApproachRef(_finalSpeed, 48f);
            _rot.ApproachRef( _target, 64f * Vector3.one);
            
            transform.position += _speed * Vector3.up;
            transform.rotation = Quaternion.Euler(_rot);
        }
    }
}