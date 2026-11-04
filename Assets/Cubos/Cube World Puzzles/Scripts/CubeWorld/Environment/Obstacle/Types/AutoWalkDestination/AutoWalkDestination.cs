using HashGame.CubeWorld.Extensions;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class AutoWalkDestination : ObstacleTypesBase

    {
        public override ObstacleType MyObstacleType => ObstacleType.AutoWalkDestination;
        public override void ObjectIsStanding(GameObject obj)
        {
        }
        private void Awake()
        {
            if (obstacleType != MyObstacleType)
            {
                BasicExtensions.DestroyObject(this);
                return;
            }
            obstacle.boxCollider.enabled = false;
            obstacle.meshRenderer.enabled = false;
        }
        public override void OnDestroyClass()
        {
        }
    }
}