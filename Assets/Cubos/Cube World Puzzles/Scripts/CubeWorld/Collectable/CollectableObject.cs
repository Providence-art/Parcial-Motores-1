using HashGame.CubeWorld.HeroManager;
using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;
namespace HashGame.CubeWorld.TerrainConstruction
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class CollectableObject : MonoBehaviour
    {
        #region variable
        public const string PREFAB = "Prefabs/Collectable";
        [HideInInspector] public BoxCollider boxCollider;
        [HideInInspector] public Rigidbody rb;
        #endregion
        #region Functions
        private void OnValidate()
        {
            boxCollider = GetComponent<BoxCollider>();
            rb = GetComponent<Rigidbody>();
        }
        #endregion
        #region functions
        public bool StayOnGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider != null)
                {
                    GameObject cube = hit.collider.gameObject;
                    if (cube == null) return false;
                    return StandOnCube(cube.GetComponent<Obstacle>());
                }
            }
            return false;
        }
        public bool StandOnCube(Obstacle obstacle)
        {
            if (obstacle == null) return false;
            transform.position = obstacle.Position + Vector3.up * (obstacle.sizeUp+boxCollider.bounds.size.y);
            return true;
        }
        #endregion
        #region struct
        [System.Serializable]
        public struct CollectableData
        {
            public float rotationSpeed;
        }
        [System.Serializable]
        public struct HeroInventoryStoreData
        {

            public float orbitSpeed;
            public float smoothTime;
            [Range(0f, .5f)]
            public float orbitHeightCoefficient;
            [Range(1.0f, 2.0f)]
            public float orbitRadiusCoefficient;
            //
            public float oscillationFrequency { get; set; }
            public float oscillationAmplitude { get; set; }
            public HeroController owner { get; set; }
        }
        [System.Serializable]
        public struct TransferToTheCustomerData
        {
            public Door owner { get; set; }
            public float smoothTime;
        }
        #endregion
    }
}