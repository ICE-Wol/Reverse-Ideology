using UnityEngine;

namespace _Scripts {
      [ExecuteInEditMode]
      public class GameCamera : MonoBehaviour {
        private const float DefRatio = 1920f / 1080f;
        private const float CamSize = 4.8f;
        private float _screenRatio;

        // Use this for initialization
        void Start(){
            _screenRatio = Screen.width * 1.0f / Screen.height;
            GetComponent<Camera>().orthographicSize
                    = (_screenRatio < DefRatio) ?
                        CamSize * DefRatio / _screenRatio : CamSize;
            //u can draw a pic to understand this :
            //two rectangles expanding width or height
            //only need to change the size when the ratio became smaller.
        }
    }
}