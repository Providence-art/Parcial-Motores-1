using UnityEngine;

namespace HashGame.CubeWorld.TerrainConstruction
{

    public class IKeyStep_Destroy : IKeyStep
    {
        public IKeyStep_Destroy(KeyController controller) { Controller = controller; }
        public KeySteps Step => KeySteps.Destroy;
        public KeyController Controller { get; }
        public void OnTriggerEnter(Collider other)
        {
        }
        public void OnStepFinish()
        {
        }

        public void OnStepStart()
        {
            GameObject.Destroy(Controller.gameObject);
        }

        public void OnUpdate()
        {

        }
    }
}