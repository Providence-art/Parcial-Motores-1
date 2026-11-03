using UnityEngine;

namespace HashGame.CubeWorld.TerrainConstruction
{
    public class IKeyStep_TransferToTheCustomer : IKeyStep
    {
        public IKeyStep_TransferToTheCustomer(KeyController controller) { Controller = controller; }
        public KeySteps Step => KeySteps.TransferToTheCustomer;
        public KeyController Controller { get; }
        public KeyController.TransferToTheCustomerData data { get => Controller.transferToTheCustomerData; }
        private bool isLock;
        private Vector3 velocity = Vector3.zero;
        private float deltaX;
        public void OnTriggerEnter(Collider other)
        {
            if (other == null || other.gameObject == null) return;
            if (other.gameObject == (data.owner.gameObject))
            {
                onHit();
            }
        }
        public void OnStepFinish()
        {
        }

        public void OnStepStart()
        {
            velocity = Vector3.zero;
            deltaX = Mathf.Min(data.owner.sizeForward, data.owner.sizeRight) / 2.0f;
        }

        public void OnUpdate()
        {
            if (isLock) return;
            Controller.transform.position = Vector3.SmoothDamp(Controller.transform.position, data.owner.transform.position, ref velocity, data.smoothTime);
            if (Vector3.Distance(Controller.transform.position, data.owner.transform.position) < deltaX)
            {
                onHit();
            }
        }
        private void onHit()
        {
            data.owner.KeyRecive(Controller);
            Controller.KeyStepChange(KeySteps.Destroy);
            isLock = true;
        }
    }
}