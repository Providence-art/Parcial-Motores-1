using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public class Spawner_CoolDown : ISpawnerBase
    {
        public Spawner_CoolDown(Spawner controller) : base(SpawnStates.CoolDown, controller) { }
        private float _t;
        private bool coolDownComplete;
        public override void Generate()
        {
        }

        public override void OnStepFinish()
        {
            if (Controller.obstacle != null && Controller.obstacle.terrainManager != null) Controller.obstacle.terrainManager.PlaySFX(State);
        }

        public override void OnStepStart()
        {
            coolDownComplete = false;
            _t = 0;
        }
        public void onFixUpdate()
        {
            if (coolDownComplete)
            {
                if (Controller.CanGenerate()) ChangeState(SpawnStates.Idle);
            }
            else //if (!coolDownComplete)
            {
                if (_t > Settings.coolDownTime)
                {
                    coolDownComplete = true;
                }
                _t += Time.deltaTime;
            }

        }
    }
}