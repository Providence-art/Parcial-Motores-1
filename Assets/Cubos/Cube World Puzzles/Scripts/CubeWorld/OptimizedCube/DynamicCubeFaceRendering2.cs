using System.Collections.Generic;
using UnityEngine;
using HashGame.CubeWorld.Extensions;


namespace HashGame.CubeWorld.OptimizedCube
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(DynamicCubeNeighborData))]
    [RequireComponent(typeof(BoxCollider))]
    public class DynamicCubeFaceRendering2 : MonoBehaviour
    {
        #region variables
        #region const
        public const int CubeFaceNumber = 6;
        public const float SIZE = 0.5f;
        public static readonly Vector3[] _vertices = new Vector3[] {
        new Vector3(-SIZE, -SIZE, -SIZE),
        new Vector3(-SIZE, -SIZE, SIZE),
        new Vector3(-SIZE, SIZE, -SIZE),
        new Vector3(-SIZE, SIZE, SIZE),
        new Vector3(SIZE, -SIZE, -SIZE),
        new Vector3(SIZE, -SIZE, SIZE),
        new Vector3(SIZE, SIZE, -SIZE),
            new Vector3(SIZE, SIZE, SIZE),
        };
        public static readonly Vector3[] Vertices = new Vector3[] {
            _vertices[0], //0
            _vertices[4], //1
            _vertices[6], //2
            _vertices[2], //3
            _vertices[5], //4
            _vertices[7], //5
            _vertices[1], //6
            _vertices[3] ,//7
        };
        private static readonly int[][] _cubeTriangles = new int[CubeFaceNumber][]
        {
            new int[] { 5, 7, 4, 7, 6, 4 }, // forward
            new int[] { 2, 5, 1, 5, 4, 1 }, // right
            new int[] { 3, 2, 0, 2, 1, 0 }, // back
            new int[] { 7, 3, 6, 3, 0, 6 }, // left
            new int[] { 7, 5, 3, 5, 2, 3 }, // up
            new int[] { 0, 1, 6, 1, 4, 6 }  // down
        };
        public static readonly Vector2[] _UV = new Vector2[]
        {
            new Vector2(0, 0), // Vertex 0
            new Vector2(1, 0), // Vertex 1
            new Vector2(1, 1), // Vertex 2
            new Vector2(0, 1), // Vertex 3
            new Vector2(0, 0), // Vertex 4
            new Vector2(0, 1), // Vertex 5
            new Vector2(1, 0), // Vertex 6
            new Vector2(1, 1), // Vertex 7
        };
        #endregion
        protected bool[] includeFaceRender = new bool[CubeFaceNumber] { true, true, true, true, true, true };
        #region HideInInspector
        [HideInInspector] public Mesh mesh;
        [HideInInspector] public MeshRenderer meshRenderer;
        [HideInInspector] public MeshFilter meshFilter;
        [HideInInspector] public DynamicCubeNeighborData neighborData;
        [HideInInspector] public BoxCollider boxCollider;
        #endregion
        #region size
        public Vector3 RealSize { get => boxCollider.bounds.size; }
        public float sizeUp { get => RealSize.y / 2; }
        public float sizeDown { get => sizeUp; }
        public float sizeRight { get => RealSize.x / 2; }
        public float sizeLeft { get => sizeRight; }
        public float sizeForward { get => RealSize.z / 2; }
        public float sizeBack { get => sizeForward; }
        #endregion
        #region positions
        public Vector3 Position { get => transform.position; }
        public Vector3 UpCenter { get => Position + Vector3.up * sizeUp; }
        public Vector3 DownCenter { get => Position + Vector3.down * sizeDown; }
        public Vector3 RightCenter { get => Position + Vector3.right * sizeRight; }
        public Vector3 LeftCenter { get => Position + Vector3.left * sizeLeft; }
        public Vector3 ForwardCenter { get => Position + Vector3.forward * sizeForward; }
        public Vector3 BackCenter { get => Position + Vector3.back * sizeBack; }
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            neighborData = GetComponent<DynamicCubeNeighborData>();
            boxCollider = GetComponent<BoxCollider>();
        }
        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.green;
            for (int i = 0; i < Vertices.Length; i++)
                UnityEditor.Handles.Label(Vertices[i], i.ToString());
#endif
        }
        #endregion
        #region faces
        public float getSize(CubeFace face)
        {
            switch (face)
            {
                case CubeFace.Up: return sizeUp;
                case CubeFace.Down: return sizeDown;
                case CubeFace.Left: return sizeLeft;
                case CubeFace.Right: return sizeRight;
                case CubeFace.Forward: return sizeForward;
                case CubeFace.Back: return sizeBack;
                default: return SIZE;
            }
        }
        public static Vector3 getDirection(CubeFace face) => face.ToVector3();
        #endregion
        #region update mesh face
        public void DisplayeAllFaces(bool enable)
        {
            for (int i = 0; i < includeFaceRender.Length; i++) includeFaceRender[i] = enable;
            UpdateIncludeFaces();
        }
        public void DisplayFaces(CubeFace[] faces, bool enable)
        {
            foreach (var face in faces)
            {
                includeFaceRender[(int)face] = enable;
            }
            UpdateIncludeFaces();
        }
        public void DisplayFace(CubeFace face, bool enable, bool forceUpdate = true)
        {
            includeFaceRender[(int)face] = enable;
            if (forceUpdate) UpdateIncludeFaces();
        }
        public void ToggleFace(CubeFace face)
        {
            includeFaceRender[(int)face] = !includeFaceRender[(int)face];
            UpdateIncludeFaces();
        }
        public void ToggleFaces()
        {
            for (int i = 0; i < includeFaceRender.Length; i++)
            {
                includeFaceRender[i] = !includeFaceRender[i];
            }
        }
        public void UpdateIncludeFaces()
        {
            //mesh.triangles = getCubeTriangles(includeFaceRender);
            //meshmesh.RecalculateNormals();
            //meshFilter.mesh.triangles = getCubeTriangles(includeFaceRender);
            //meshFilter.mesh.RecalculateNormals();
            if (meshFilter == null)
            {
                meshFilter = GetComponent<MeshFilter>();
            }
            if (meshFilter == null)
            {
                return;
            }
            meshFilter.sharedMesh.triangles = getCubeTriangles(includeFaceRender);
            meshFilter.sharedMesh.RecalculateNormals();
        }
        #endregion
        #region create cube mesh
        private int[] getCubeTriangles(bool[] includeFaces)
        {
            if (includeFaces == null) return new int[0];
            List<int> triangles = new List<int>();
            for (int i = 0; i < includeFaces.Length; i++)
            {
                if (includeFaces[i])
                {
                    triangles.AddArrayToList<int>(_cubeTriangles[i]);
                }
            }
            return triangles.ToArray();
        }
        public void CreateCube() => CreateCube(new Material(Shader.Find("Unlit/Color")));
        public void CreateCube(Material mat) => CreateCube(mat, Vertices);
        public void CreateCube(Material mat, Vector3[] __vertices)
        {
            transform.localScale = Vector3.one;
            mesh = new Mesh();
            mesh.name = "OptimizedCube";
            //Vector3[] __vertices = new Vector3[Vertices.Length]; 
            //Array.Copy(Vertices, __vertices, Vertices.Length);
            mesh.vertices = __vertices;

            mesh.triangles = getCubeTriangles(includeFaceRender);
            mesh.uv = _UV;
            mesh.RecalculateNormals();
            if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            SetMaterial(mat);
            //meshRenderer.material = mat;
        }
        public void UpdateMesh(Vector3[] __vertices, bool updateCenter = true) => UpdateMesh(__vertices, updateCenter, _UV);
        public void UpdateMesh(Vector3[] __vertices, bool updateCenter, Vector2[] uv)
        {
            mesh = new Mesh();
            mesh.name = "OptimizedCube";
            //Vector3[] __vertices = new Vector3[Vertices.Length]; 
            //Array.Copy(Vertices, __vertices, Vertices.Length);
            if (updateCenter)
            {
                Vector3 center = Vector3.zero;
                foreach (Vector3 vertex in __vertices)
                {
                    center += vertex;
                }
                center /= __vertices.Length;
                for (int i = 0; i < __vertices.Length; i++)
                {
                    __vertices[i] -= center;
                }
            }

            mesh.vertices = __vertices;

            mesh.triangles = getCubeTriangles(includeFaceRender);
            mesh.uv = _UV;
            mesh.Optimize();
            mesh.RecalculateNormals();
            if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            Bounds bounds = meshFilter.sharedMesh.bounds;
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;
        }
        #endregion
        #region ScaledVertices
        public static Vector3[] ScaledVertices_Up(float scale = 1.0f) => new Vector3[] {
                Vertices[0],
                Vertices[1],
                Vertices[2]+scale*Vector3.up,
                Vertices[3]+scale*Vector3.up,
                Vertices[4],
                Vertices[5]+scale*Vector3.up,
                Vertices[6],
                Vertices[7]+scale*Vector3.up,
        };
        public static Vector3[] ScaledVertices_Right(float scale = 1.0f) => new Vector3[] {
                Vertices[0],
                Vertices[1]+scale*Vector3.right,
                Vertices[2]+scale*Vector3.right,
                Vertices[3],
                Vertices[4]+scale*Vector3.right,
                Vertices[5]+scale*Vector3.right,
                Vertices[6],
                Vertices[7],
        };
        public static Vector3[] ScaledVertices_Forward(float scale = 1.0f) => new Vector3[] {
                Vertices[0],
                Vertices[1],
                Vertices[2],
                Vertices[3],
                Vertices[4]+scale*Vector3.forward,
                Vertices[5]+scale*Vector3.forward,
                Vertices[6]+scale*Vector3.forward,
                Vertices[7]+scale*Vector3.forward,
        };
        #endregion
        #region material & color
        public void SetMaterial(Material mat)
        {
            meshRenderer.material = mat;
        }
        public void setColor(Color color)
        {
            meshRenderer.material.color = color;
        }
        #endregion
    }
}