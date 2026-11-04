using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class AutoWalk_Unloading : IAutoWalkBase
    {
        public AutoWalk_Unloading(AutoWalk controller) : base(AutoWalkStates.Unloading, controller) { }

        public override void ObjectIsStanding(GameObject obj)
        {
        }

        public override void onEnd()
        {
        }

        public override void onFixUpdate()
        {
        }

        public override void onStart()
        {
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null) Controller.obstacle.terrainManager.PlaySFX(State);
            if (Controller.buffer.Hero)
            {
                Controller.buffer.Hero.AutowalkIsOver();
                return;
            }
            ChangeState(AutoWalkStates.Idle);
        }

        public override void OnTriggerExit(Collider other)
        {
            if (other == null) return;
            if (other.gameObject == null) return;
            if (other.GetComponent<HeroController>() == null) return;
            ChangeState(AutoWalkStates.Idle);
        }

        public override void onUpdate()
        {
        }
    }
}