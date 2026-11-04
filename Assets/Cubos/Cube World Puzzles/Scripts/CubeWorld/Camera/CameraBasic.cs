using HashGame.CubeWorld.HeroManager;
using UnityEngine;
namespace HashGame.CubeWorld.CameraSystem
{
    public abstract class CameraBasic : MonoBehaviour
    {
        #region variable
        #region abstract
        public abstract bool IncludeAxisMapping { get; }
        #endregion
        public float distance = 10f;
        public float height = 8f;
        public HeroController target;
        #endregion
        private void Start()
        {
            targetInit();
        }
        protected void targetInit()
        {
            if (target == null)
            {
                var heros = GameObject.FindObjectsByType<HeroController>(FindObjectsSortMode.None);
                if (heros == null) return;
                foreach (var hero in heros)
                {
                    if (hero == null) continue;
                    if (hero.IsHuman)
                    {
                        target = hero;
                        break;
                    }
                }
            }
            if (target != null)
            {
                target.SetCamera(this);
            }
        }
        public abstract void ChangeRotation(Vector3 motionAxis);
        protected Vector3 offset(float rotationAngle) => offset(rotationAngle, distance, height);
        protected Vector3 offset(float tetha, float distance, float height)
        {
            float x = Mathf.Cos(tetha * Mathf.Deg2Rad) * distance;
            float z = Mathf.Sin(tetha * Mathf.Deg2Rad) * distance;
            return new Vector3(x, height, z);
        }
    }
}