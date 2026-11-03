using UnityEngine;

namespace HashGame.CubeWorld.TerrainConstruction
{
    public enum KeySteps : int
    {
        Idle = 0,
        Follower,
        TransferToTheCustomer,
        Destroy
    }
    public interface IKeyStep
    {
        public KeySteps Step { get; }
        public KeyController Controller { get; }
        public void OnStepStart();
        public void OnStepFinish();
        public void OnUpdate();
        public void OnTriggerEnter(Collider other);
    }
}