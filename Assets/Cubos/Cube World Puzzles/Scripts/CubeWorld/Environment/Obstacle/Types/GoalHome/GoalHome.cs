using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class GoalHome : ObstacleTypesBase
    {
        #region variable
        public override ObstacleType MyObstacleType => ObstacleType.GoalHome;
        #endregion
        #region Functions
        #endregion
        #region fucntions
        public override void ObjectIsStanding(GameObject obj)
        {
            if (obj == null) return;
            HeroController hero = obj.GetComponent<HeroController>();
            if (!hero || obstacle.terrainManager == null) return;
            obstacle.terrainManager.PlaySFX(MyObstacleType);
            obstacle.terrainManager.CheckGoalHomes();
        }
        public bool IsHeroHost()
        {
            if (!obstacle.neighborData.Radiation(CubeFace.Up, out var hit)) return false;
            if (hit.collider == null) return false;
            if (hit.collider.GetComponent<HeroController>() == null) return false;
            return true;
        }
        public override void OnDestroyClass()
        {
        }
        #endregion
    }
}