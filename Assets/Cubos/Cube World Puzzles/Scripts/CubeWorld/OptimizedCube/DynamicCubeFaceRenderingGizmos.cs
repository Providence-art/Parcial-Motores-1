#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
namespace HashGame.CubeWorld.OptimizedCube
{
    public class DynamicCubeFaceRenderingGizmos : MonoBehaviour
    {
        #region variables
        #region HideInInspector
        [HideInInspector][SerializeField] private DynamicCubeFaceRendering _target;
        public DynamicCubeFaceRendering Target
        {
            get
            {
                if (_target == null) _target = GetComponent<DynamicCubeFaceRendering>();
                return _target;
            }
        }
        #endregion
        public bool namingFaces = false;
        public bool namingVertices = false;
        [Min(0.0f)]
        public float Multiply = 0.0f;
        public Color LableColor = Color.green;
        #endregion
        private void OnDrawGizmosSelected()
        {
            showNamingFaces();
            showNamingVertices();
        }
        #region namingVertices
        private void showNamingVertices()
        {
            if (!namingVertices) return;
            Handles.color = LableColor;
            for (int i = 0; i < DynamicCubeFaceRendering._vertices.Length; i++)
            {
                Handles.Label(DynamicCubeFaceRendering._vertices[i], i.ToString());
            }

            //Handles.Label(DynamicCubeFaceRendering._Vertices[0], i++.ToString());
            //Handles.Label(DynamicCubeFaceRendering._Vertices[4], i++.ToString());
            //Handles.Label(DynamicCubeFaceRendering._Vertices[6], i++.ToString());
            //Handles.Label(DynamicCubeFaceRendering._Vertices[2], i++.ToString());
            //Handles.Label(DynamicCubeFaceRendering._Vertices[5], i++.ToString());
            //Handles.Label(DynamicCubeFaceRendering._Vertices[7], i++.ToString());
            //Handles.Label(DynamicCubeFaceRendering._Vertices[1], i++.ToString());
            //Handles.Label(DynamicCubeFaceRendering._Vertices[3], i++.ToString());
        }
        #endregion
        #region namingFaces
        private void showNamingFaces()
        {
            if (!namingFaces || !Target) return;
            Gizmos.color = Color.green;
            drawNamingFaces(Target.UpCenter, Target.UpCenter + Vector3.up * Multiply, "up");
            drawNamingFaces(Target.DownCenter, Target.DownCenter + Vector3.down * Multiply, "down");
            Gizmos.color = Color.red;
            drawNamingFaces(Target.RightCenter, Target.RightCenter + Vector3.right * Multiply, "right");
            drawNamingFaces(Target.LeftCenter, Target.LeftCenter + Vector3.left * Multiply, "left");
            Gizmos.color = Color.blue;
            drawNamingFaces(Target.ForwardCenter, Target.ForwardCenter + Vector3.forward * Multiply, "forward");
            drawNamingFaces(Target.BackCenter, Target.BackCenter + Vector3.back * Multiply, "back");
        }
        private void drawNamingFaces(Vector3 p1, Vector3 p2, string name)
        {
            Gizmos.DrawLine(p1, p2);
            if (!string.IsNullOrEmpty(name))
            {
                Handles.Label(p2, name);
            }
        }
        #endregion
    }
}
#endif