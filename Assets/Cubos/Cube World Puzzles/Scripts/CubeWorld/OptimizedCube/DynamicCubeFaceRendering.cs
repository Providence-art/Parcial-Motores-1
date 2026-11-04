using System.Collections.Generic;
using UnityEngine;
using HashGame.CubeWorld.Extensions;

namespace HashGame.CubeWorld.OptimizedCube
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(DynamicCubeNeighborData))]
    [RequireComponent(typeof(BoxCollider))]
    //[RequireComponent(typeof(DynamicCubeFaceRenderingGizmos))]
    public class DynamicCubeFaceRendering : MonoBehaviour
    {
        #region variables
        #region const
        public readonly int CubeFaceNumber = System.Enum.GetValues(typeof(CubeFace)).Length;
        public const float SIZE = 0.5f;
        public static readonly Vector3[] _vertices = new Vector3[] {
            new Vector3(-SIZE, -SIZE, -SIZE),//0
            new Vector3(-SIZE, -SIZE, SIZE),//1
            new Vector3(-SIZE, SIZE, -SIZE),//2
            new Vector3(-SIZE, SIZE, SIZE),//3
            new Vector3(SIZE, -SIZE, -SIZE),//4
            new Vector3(SIZE, -SIZE, SIZE),//5
            new Vector3(SIZE, SIZE, -SIZE),//6
            new Vector3(SIZE, SIZE, SIZE),//7
        };
        public static readonly Vector3[] _Vertices = new Vector3[] {
            // forward
            _vertices[5],
            _vertices[7],
            _vertices[3],
            _vertices[1],
            //right
            _vertices[4],
            _vertices[6],
            //back
            _vertices[0],
            _vertices[2],
            //left
            //up
            _vertices[2],
            _vertices[3],
            //down
            _vertices[5],
            _vertices[1],
        };
        public static readonly int[][] _Triangles = new int[][]
        {
            new int[] {
                0,1,2,
                0,2,3,
            }, // forward
            
            new int[] {
                4,5,1,
                4,1,0,
            }, // right
            
            new int[] {
                6,7,5,
                6,5,4,
            }, // back
            
            new int[] {
                3,2,7,
                3,7,6,
            }, // left
            
            new int[] {
                1,5,8,
                1,8,9,
            }, // up

            new int[] {
                6,4,10,
                6,10,11,
            }, // down
        };
        public static readonly Vector2[] _UV = new Vector2[]
        {
            //Forward
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 0),
            //Right
            new Vector2(0, 0),
            new Vector2(1, 0),
            //new Vector2(1, 1),
            //new Vector2(0, 1),
            //back
            new Vector2(0, 1),
            new Vector2(1, 1),
            //left
            //new Vector2(0, 0),
            //new Vector2(1,0),
            //up
            new Vector2(0, 0),
            new Vector2(0,1),
            //down
            new Vector2(1,0),
            new Vector2(1, 1),
        };
        #endregion
        protected bool[] includeFaceRender = new bool[] { true, true, true, true, true, true };
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
        [SerializeField] private Material _material;
        public Material material
        {
            get
            {
                if (_material == null) _material = new Material(Shader.Find("Unlit/Color"));
                        return _material;   
            }
        }
        #endregion
        #region Functions
        private void OnValidate()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshFilter = GetComponent<MeshFilter>();
            neighborData = GetComponent<DynamicCubeNeighborData>();
            boxCollider = GetComponent<BoxCollider>();
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
                if (i >= _Triangles.Length) break;
                if (includeFaces[i])
                {
                    triangles.AddArrayToList<int>(_Triangles[i]);
                }
            }
            return triangles.ToArray();
        }
        public void CreateCube() => CreateCube(material);
        public void CreateCube(Material mat) => CreateCube(mat, _Vertices);
        public void CreateCube(Vector3[] __vertices) => CreateCube(material, __vertices);
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
            Bounds bounds = meshFilter.sharedMesh.bounds;
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;
            //meshRenderer.material = mat;
        }
        public void UpdateMesh(Vector3[] __vertices, bool updateCenter = true) => UpdateMesh(__vertices, updateCenter, _UV);
        public void UpdateMesh(Vector3[] __vertices, bool updateCenter, Vector2[] uv)
        {
            //TODO: alot,...
            mesh = new Mesh();
            mesh.name = "OptimizedCube";
            //Vector3[] __vertices = new Vector3[Vertices.Length]; 
            //Array.Copy(Vertices, __vertices, Vertices.Length);
            if (updateCenter)
            {
                Vector3 center = Vector3.zero;
                for (int i = 0; i < __vertices.Length; i++)
                {
                    center += __vertices[i];
                }
                center /= __vertices.Length;
                for (int i = 0; i < __vertices.Length; i++)
                {
                    __vertices[i] -= center;
                }
            }

            mesh.vertices = __vertices;
            mesh.triangles = getCubeTriangles(includeFaceRender);
            mesh.uv = uv;

            //mesh.Optimize();
            mesh.RecalculateNormals();
            if (meshFilter == null) meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            SetMaterial(material);

            Bounds bounds = meshFilter.sharedMesh.bounds;
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;
        }
        #endregion
        #region ScaledVertices
        public static Vector3[] ScaledVertices_Up(float scale = 1.0f) => new Vector3[] {
            _Vertices[0],
            _Vertices[1]+ Vector3.up * scale,
            _Vertices[2]+ Vector3.up * scale,
            _Vertices[3],
            //right
            _Vertices[4],
            _Vertices[5]+ Vector3.up * scale,
            //back
            _Vertices[6],
            _Vertices[7]+ Vector3.up * scale,
            //left
            //up
            _Vertices[8]+ Vector3.up * scale,
            _Vertices[9]+ Vector3.up * scale,
            //down
            _Vertices[10],
            _Vertices[11],
        };
        public static Vector3[] ScaledVertices_Right(float scale = 1.0f) => new Vector3[] {
            _Vertices[0],
                _Vertices[1]+scale*Vector3.right,
                _Vertices[2]+scale*Vector3.right,
                _Vertices[3],
                _Vertices[4]+scale*Vector3.right,
                _Vertices[5]+scale*Vector3.right,
                _Vertices[6],
                _Vertices[7],
        };
        public static Vector3[] ScaledVertices_Forward(float scale = 1.0f) => new Vector3[] {
                _Vertices[0],
                _Vertices[1],
                _Vertices[2],
                _Vertices[3],
                _Vertices[4]+scale*Vector3.forward,
                _Vertices[5]+scale*Vector3.forward,
                _Vertices[6]+scale*Vector3.forward,
                _Vertices[7]+scale*Vector3.forward,
        };
        #endregion
        #region material & color
        public void SetMaterial(Material mat)
        {
            _material = mat;
            meshRenderer.material = mat;
        }
        public void setColor(Color color)
        {
            meshRenderer.material.color = color;
        }
        #endregion
        public Vector3[] Vertices(float scaleX, float scaleY, float scaleZ) => new Vector3[] {
            _vertices[5]+ Vector3.right * scaleX + Vector3.down * scaleY + Vector3.forward * scaleZ,
            _vertices[7]+ Vector3.right * scaleX + Vector3.up * scaleY + Vector3.forward * scaleZ,
            _vertices[3]+ Vector3.left * scaleX + Vector3.up * scaleY + Vector3.forward * scaleZ,
            _vertices[1]+ Vector3.left * scaleX + Vector3.down * scaleY + Vector3.forward * scaleZ,
            //right
            _vertices[4]+ Vector3.right * scaleX + Vector3.down * scaleY + Vector3.back * scaleZ,
            _vertices[6]+ Vector3.right * scaleX + Vector3.up * scaleY + Vector3.back * scaleZ,
            //back
            _vertices[0]+ Vector3.left * scaleX + Vector3.down * scaleY + Vector3.back * scaleZ,
            _vertices[2]+ Vector3.left * scaleX + Vector3.up * scaleY + Vector3.back * scaleZ,
            //left
            //up
            _vertices[2]+ Vector3.left * scaleX + Vector3.up * scaleY + Vector3.back * scaleZ,
            _vertices[3]+ Vector3.left * scaleX + Vector3.up * scaleY + Vector3.forward * scaleZ,
            //down
            _vertices[5]+ Vector3.right * scaleX + Vector3.down * scaleY + Vector3.forward * scaleZ,
            _vertices[1]+ Vector3.left * scaleX + Vector3.down * scaleY + Vector3.forward * scaleZ,
        };
    }
}