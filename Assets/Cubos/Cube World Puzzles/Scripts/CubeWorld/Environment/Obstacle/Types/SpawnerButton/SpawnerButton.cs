using HashGame.CubeWorld.Extensions;
using HashGame.CubeWorld.HeroManager;
using UnityEngine;
using UnityEngine.Events;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class SpawnerButton : ObstacleTypesBase
    {
        #region enum
        public enum ActiveHeroType : int
        {
            Human,
            Cpu,
            All,
        }
        #endregion
        #region variable
        public Spawner Target;
        public ActiveHeroType activeHeroType = ActiveHeroType.Human;
        public SpawnerButtonEvents events = new SpawnerButtonEvents();
        public override ObstacleType MyObstacleType => ObstacleType.SpawnerButton;
        #endregion
        #region functions
        public override void ObjectIsStanding(GameObject obj)
        {
            if (Target == null || obj == null) return;
            HeroController hero = obj.GetComponent<HeroController>();
            if (hero == null) return;
            switch (activeHeroType)
            {
                case ActiveHeroType.Human:
                    if (!hero.IsHuman) return;
                    break;
                case ActiveHeroType.Cpu:
                    if (hero.IsHuman) return;
                    break;
            }
            Target.TryGenerate();
            events.onPressingInvoke();
        }
        public void OnTargetSet()
        {

        }

        public override void OnDestroyClass()
        {
        }
        #endregion
        #region struct
        [System.Serializable]
        public class SpawnerButtonEvents : StepsEventStructBase
        {
            public UnityEvent onPressing;
            public void onPressingInvoke() => invoke(ref onPressing);
        }
        #endregion
    }
}
