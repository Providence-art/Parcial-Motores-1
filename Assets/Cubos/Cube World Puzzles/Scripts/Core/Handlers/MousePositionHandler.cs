#if UNITY_EDITOR
namespace HashGame.CubeWorld.EditorHandler
{
    using UnityEditor;
    using UnityEngine;

    public class MousePositionHandler
    {
        public double LastClickTime { get { return _lastClickTime; } }
        private Vector3[] _positions;
        private double _lastClickTime;
        public float Y { get; protected set; }
        public MousePositionHandler(int size)
        {
            _positions = new Vector3[size];
        }
        public Vector3 getMousePosition()
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            float drawPlaneHeight = 0;
            float distanceToDrawPlane = (drawPlaneHeight - mouseRay.origin.y) / mouseRay.direction.y;
            var result = mouseRay.GetPoint(distanceToDrawPlane);
            Y = result.y;
            return result;
        }
        public Vector3 getPointInRegion(Vector3 input)
        {
            return new Vector3(input.x, Y, input.z);
        }
        public void AddPosition(Vector3 position)
        {
            int i = _positions.Length;
            while (--i > 0)
            {
                _positions[i] = _positions[i - 1];
            }
            _lastClickTime = EditorApplication.timeSinceStartup;
            _positions[0] = position;
        }
        public Vector3 getPosition(int index)
        {
            if (index >= 0 && index < _positions.Length)
            {
                return _positions[index];
            }
            return _positions[0];
        }
        public Vector3 getLastPosition()
        {
            return _positions[0];
        }
    }
}
#endif