using UnityEngine;
using UnityEngine.Serialization;

namespace ToolboxEngine
{
    public class MoveAround2D : MonoBehaviour
    {
        //[SerializeField]
        //[FormerlySerializedAs("radius")]    //Keep serialized value after variable name changed
        public float _radius = 1f;

        public float speed = 90f;

        private float _angle = 0f;

        private Vector3 _center = Vector3.zero;

        public Vector3 GetCenter()
        {
            return _center;
        }

        // Start is called before the first frame update
        void Start()
        {
            _center = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            _angle += speed * Time.deltaTime;
            _angle %= 360;

            float radAngle = _angle * Mathf.Deg2Rad;
            Vector3 newPosition = transform.position;
            newPosition.x = _center.x + _radius * Mathf.Cos(radAngle);
            newPosition.y = _center.y + _radius * Mathf.Sin(radAngle);
            transform.position = newPosition;
        }

#if UNITY_EDITOR    //Use debug tools only in editor mode, will not show in build 

        //[Header("Debug")]
        public bool guiDebug = false;
        public int guiFontSize = 18;
        public Color guiDebugTextColor = Color.white;
        private GUIStyle _debugTextStyle = null;

        public bool gizmosDebug = false;
        public float gizmosSize = 0.1f;
        public Color gizmosCenterColor = Color.green;
        public Color gizmosPositionColor = Color.blue;
        public Color gizmosLineColor = Color.red;

        
        private void OnGUI()
        {
            if (!guiDebug) return;

            if (null == _debugTextStyle)
            {
                _debugTextStyle = new GUIStyle();
            }
            _debugTextStyle.fontSize = guiFontSize;
            _debugTextStyle.normal.textColor = guiDebugTextColor;

            GUILayout.BeginVertical();
            GUILayout.Label("Radius = " + _radius, _debugTextStyle);
            GUILayout.Label("Speed = " + speed, _debugTextStyle);
            GUILayout.Label("Angle = " + _angle, _debugTextStyle);
            GUILayout.EndVertical();
        }
#endif
    }
}
