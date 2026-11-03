namespace HashGame.CubeWorld.OptimizedCube
{
    public class Spawner_Idle : ISpawnerBase
    {
        public Spawner_Idle(Spawner controller) : base(SpawnStates.Idle, controller)
        {
        }

        public override void Generate()
        {
            if (!Controller.CanGenerate()) return;
            ChangeState(SpawnStates.Generate);
        }

        public override void OnStepFinish()
        {
            if (Settings.useColors)
            {
                Controller.obstacle.setColor(Settings.onCoolDownColor);
            }
        }

        public override void OnStepStart()
        {
            if (Settings.useColors)
            {
                Controller.obstacle.setColor(Settings.onReadyColor);
            }
        }
    }
}