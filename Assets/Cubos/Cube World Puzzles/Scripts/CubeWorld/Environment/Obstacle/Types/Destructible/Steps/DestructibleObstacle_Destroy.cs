using HashGame.CubeWorld.Extensions;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class DestructibleObstacle_Destroy : IDestructibleObstacleBase
    {
        public DestructibleObstacle_Destroy(DestructibleObstacle controller) : base(DestructibleStates.Destroy, controller) { }
        private float t;
        public override void ObjectIsStanding(GameObject obj) { }

        public override void onStart()
        {
            t = 0.0f;
        }

        public override void onFixUpdate()
        {
            if (t > Controller.localSettings.DestroyTime)
            {
                destroy();
                return;
            }
            t += Time.deltaTime;
        }
        private void destroy()
        {
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null) Controller.obstacle.terrainManager.PlaySFX(State);
            events.onDestroyInvoke();
            BasicExtensions.DestroyObject(Controller.gameObject);
        }
    }
}
