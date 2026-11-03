using HashGame.CubeWorld.Extensions;
using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;
namespace HashGame.CubeWorld.CameraSystem
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class RenderDistanceController : MonoBehaviour
    {
        [HideInInspector] public SphereCollider sphereCollider;
        [HideInInspector] public Rigidbody rb;
        private void OnValidate()
        {
            rb = GetComponent<Rigidbody>();
            sphereCollider = GetComponent<SphereCollider>();
        }
        private void Start()
        {
            checkRequirements();
            rb.isKinematic = true;
            sphereCollider.isTrigger = true;
        }
        private void OnTriggerEnter(Collider other) => DisplayeAllFaces(other.gameObject, true);

        private void OnTriggerExit(Collider other) => DisplayeAllFaces(other.gameObject, false);
        private void DisplayeAllFaces(GameObject node, bool value)
        {
            if (node == null) return;
            Obstacle obstacle = node.GetComponent<Obstacle>();
            if (obstacle)
            {
                obstacle.TryDisplayeAllFaces(value);
                return;
            }
            Door door = node.GetComponent<Door>();
            if (door)
            {
                door.DisplayeAllFaces(value);
            }
        }
        private void checkRequirements()
        {
            TerrainManager terrainManager = FindAnyObjectByType<TerrainManager>();
            if (!terrainManager) return;
            if (!terrainManager.localSettings.limitedVisibility)
            {
                BasicExtensions.DestroyObject(this);
            }
        }
    }
}