using UnityEngine;
using HashGame.CubeWorld.HeroManager;
namespace HashGame.CubeWorld.TerrainConstruction
{
    public class IKeyStep_Follower : IKeyStep
    {
        public IKeyStep_Follower(KeyController controller) { Controller = controller; }
        public KeySteps Step => KeySteps.Follower;
        public KeyController Controller { get; }
        //public KeyController.KeyData data { get => Controller.keyData; }
        public KeyController.HeroInventoryStoreData inventoryStoreData { get => Controller.heroInventoryStoreData; }
        public HeroController Owner { get => inventoryStoreData.owner; }
        public Transform transform => Controller.transform;
        private Vector3 velocity = Vector3.zero;
        private float realOrbitRadius;
        public float orbitRadius { get => inventoryStoreData.orbitRadiusCoefficient * realOrbitRadius; }
        public float orbitHeight { get => Owner.UpDownSize * inventoryStoreData.orbitHeightCoefficient; }
        private float theta;
        private bool isLock;
        public void Update()
        {
            if (Owner == null) return;
            float x = Mathf.Cos(theta) * orbitRadius;
            float z = Mathf.Sin(theta) * orbitRadius;
            float y = Mathf.Sin(theta) * orbitHeight;
            float oscillation = Mathf.Sin(Time.time * inventoryStoreData.oscillationFrequency) * inventoryStoreData.oscillationAmplitude;
            float currentRadius = orbitRadius + oscillation;
            //
            Vector3 targetPosition = Owner.Position + new Vector3(x, y, z);// * currentRadius;//Vector3.forward * Mathf.Max(Owner.HeroSize.x, Owner.HeroSize.z);
            theta += Time.deltaTime * inventoryStoreData.orbitSpeed;
            if (theta > 2 * Mathf.PI)
            {
                theta = -2 * Mathf.PI;
            }
            //
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, inventoryStoreData.smoothTime);
        }
        public void OnStepFinish()
        {
        }

        public void OnStepStart()
        {
            isLock = false;
            velocity = Vector3.zero;
            if (Owner == null)
            {
                isLock = true;
                Controller.KeyStepChange(KeySteps.Idle);
                return;
            }
            initOrbitVariables();
        }
        private void initOrbitVariables()
        {
            realOrbitRadius = Mathf.Max(Owner.ForwardBackSize, Owner.RightLeftSize);
            theta = Random.Range(-2 * Mathf.PI, 2 * Mathf.PI);
        }
        public void OnUpdate()
        {
            if (isLock) return;
            Update();
        }
        public void OnTriggerEnter(Collider other)
        {

        }
    }
}