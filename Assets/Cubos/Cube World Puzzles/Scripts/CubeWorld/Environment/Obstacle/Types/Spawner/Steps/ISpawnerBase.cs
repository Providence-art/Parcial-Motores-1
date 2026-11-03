namespace HashGame.CubeWorld.OptimizedCube
{
    public abstract class ISpawnerBase : ISpawner
    {
        public ISpawnerBase(SpawnStates state, Spawner controller)
        {
            this.State = state;
            this.Controller = controller;
        }
        public SpawnStates State { get; }

        public Spawner Controller { get; }
        public Spawner.SpawnerBuffer buffer => Controller.buffer;
        public SpawnerSettings Settings => Controller.Settings;
        public abstract void OnStepFinish();
        public abstract void OnStepStart();
        public abstract void Generate();
        public void ChangeState(SpawnStates state, bool force = false) => Controller.ChangeState(state, force);

    }
}