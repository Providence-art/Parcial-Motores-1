using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    [RequireComponent(typeof(Door))]
    public abstract class DoorTypeBase : MonoBehaviour
    {
        #region variable
        [HideInInspector] public Door _target;
        [HideInInspector]
        public Door Target
        {
            get
            {
                if (_target == null)
                {
                    _target = GetComponent<Door>();
                }
                return _target;
            }
        }
        [HideInInspector] public BoxCollider boxCollider => Target.boxCollider;
        public DoorType doorType => Target.doorType;
        #endregion
    }
}