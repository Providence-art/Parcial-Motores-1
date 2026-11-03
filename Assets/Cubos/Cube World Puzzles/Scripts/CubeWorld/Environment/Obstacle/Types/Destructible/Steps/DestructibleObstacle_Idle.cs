using HashGame.CubeWorld.HeroManager;
using UnityEngine;
using static HashGame.CubeWorld.OptimizedCube.DestructibleObstacle;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class DestructibleObstacle_Idle : IDestructibleObstacleBase
    {
        public DestructibleObstacle_Idle(DestructibleObstacle controller) : base(DestructibleStates.Idle, controller) { }
        private bool readyChangeState;
        private Color color;
        private DestructibleLocalSettings settings => Controller.localSettings;

        private float r, g, b, a;
        public override void ObjectIsStanding(GameObject obj)
        {
            if (readyChangeState || obj == null) return;
            HeroController hero = obj.GetComponent<HeroController>();
            if (!hero) return;
            Controller.buffer.touchCount++;
            if (Controller.Settings.SwitchingBetweenColors)
            {
                color.r += r;
                color.g += g;
                color.b += b;
                color.a += a;

                Controller.obstacle.setColor(color);
            }
            events.onHeroDetectionInvoke();
            readyChangeState = Controller.isTouchingOverflow();
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null) Controller.obstacle.terrainManager.PlaySFX(State);
        }

        public override void onStart()
        {
            if (settings.MaxTouchCount < 1)
            {
                ChangeState(DestructibleStates.Destroy, true);
                return;
            }
            readyChangeState = false;
            if (Controller.Settings.SwitchingBetweenColors)
            {
                r = (Controller.Settings.colorSafe.r - Controller.Settings.colorDangerous.r) / settings.MaxTouchCount;
                g = (Controller.Settings.colorSafe.g - Controller.Settings.colorDangerous.g) / settings.MaxTouchCount;
                b = (Controller.Settings.colorSafe.b - Controller.Settings.colorDangerous.b) / settings.MaxTouchCount;
                a = (Controller.Settings.colorSafe.a - Controller.Settings.colorDangerous.a) / settings.MaxTouchCount;
                Controller.obstacle.setColor(color = Controller.Settings.colorDangerous);
            }
        }

        public override void onFixUpdate()
        {
            if (!readyChangeState) return;
            if (Controller.obstacle.neighborData.Radiation(CubeFace.Up, out var hit)
                && hit.collider != null
                && hit.collider.GetComponent<HeroController>() != null)
                return;
            ChangeState(DestructibleStates.Destroy);
        }
    }
}