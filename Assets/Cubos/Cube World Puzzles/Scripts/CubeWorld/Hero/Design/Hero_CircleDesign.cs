using HashGame.CubeWorld.Extensions;
using UnityEngine;
namespace HashGame.CubeWorld.HeroManager
{
    [RequireComponent(typeof(HeroController))]
    public class Hero_CircleDesign : MonoBehaviour
    {
        #region variable
        #region const
        private const float D = 360f;
        public GameObject CubeWheel => heroController.CubeWheel;
        #endregion
        [Min(1)]
        public int CircleCubesCount = 1;
        public bool destroyChilds = true;
        #region HideInInspector
        [HideInInspector] public HeroController heroController;
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            heroController = GetComponent<HeroController>();
        }
        private void Start()
        {
            CreateCircles();
        }
        #endregion
        #region gizmo
        public void OnDrawGizmosSelected()
        {

        }
        #endregion
        #region functions
        public float getCenterToSurface()
        {
            return Mathf.Min(Mathf.Min(heroController.CenterToUpDownSize, heroController.CenterToForwardBackSize), heroController.CenterToRightLeftSize);
        }
        #region create
        public void CreateCircles()
        {
            if (destroyChilds) BasicExtensions.DeleteChildren(CubeWheel.transform);
            float distance = getCenterToSurface();
            float theta = D / CircleCubesCount;
            float t = 0.0f;
            Vector3 scale = heroController.HeroSize / (CircleCubesCount);
            float newCubeCenterToSurface = distance / CircleCubesCount;
            for (int i = 0; i < CircleCubesCount; i++)
            {
                Vector3 position = transform.position + offset(t, distance - newCubeCenterToSurface, 0.0f);
                GameObject cube = createCube(position);
                cube.transform.localScale = scale;
                t += theta;
            }
        }
        protected Vector3 offset(float tetha, float distance, float height)
        {
            float x = Mathf.Cos(tetha * Mathf.Deg2Rad) * distance;
            float z = Mathf.Sin(tetha * Mathf.Deg2Rad) * distance;
            return new Vector3(x, height, z);
        }
        protected GameObject createCube(Vector3 position)
        {
            var node = GameObject.CreatePrimitive(PrimitiveType.Cube);
            node.transform.parent = CubeWheel.transform;
            node.transform.position = position;
            node.transform.LookAt(CubeWheel.transform.position);
            var col = node.GetComponent<Collider>();
            if (col)
            {
                BasicExtensions.DestroyObject(col);
            }
            return node;
        }
        #endregion
        #endregion
    }
}