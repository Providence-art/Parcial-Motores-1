using UnityEngine;

namespace HashGame.CubeWorld.OptimizedCube
{
    public enum SpawnStates : int
    {
        Idle = 0,
        Generate,
        CoolDown
    }
    public interface ISpawner
    {
        public SpawnStates State { get; }
        public Spawner Controller { get; }
    }
}