using HashGame.CubeWorld.Extensions;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    [RequireComponent(typeof(Obstacle))]
    public abstract class ObstacleTypesBase : MonoBehaviour
    {
        #region variable
        public const float MiniaturizedBoxColliderScale = .95f;
        [HideInInspector] public Obstacle _obstacle;
        public Obstacle obstacle
        {
            get { if (_obstacle == null) _obstacle = GetComponent<Obstacle>(); return _obstacle; }
            set { _obstacle = value; }
        }
        public BoxCollider boxCollider => obstacle.boxCollider;
        public abstract ObstacleType MyObstacleType { get; }
        public ObstacleType obstacleType => obstacle.obstacleType;
        #endregion
        private void Start()
        {
            if (obstacleType != MyObstacleType) BasicExtensions.DestroyObject(this);
        }
        public abstract void ObjectIsStanding(GameObject obj);
        public abstract void OnDestroyClass();
    }
}