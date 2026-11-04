using HashGame.CubeWorld.HeroManager;
using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;

namespace HashGame.CubeWorld.TerrainConstruction
{
    public class KeyController : CollectableObject
    {
        #region variable
        public const string NAME = "Key";
        public const string Prefab = PREFAB + "/Key/" + NAME;
        #region readonly
        public Vector3 Position { get => transform.position; }
        #endregion
        public StepsEventStruct2Phase events = new StepsEventStruct2Phase();
        public CollectableData keyData = new CollectableData()
        {
            rotationSpeed = 50.0f,
        };
        public HeroInventoryStoreData heroInventoryStoreData = new HeroInventoryStoreData()
        {
            orbitSpeed = 1.0f,
            smoothTime = 0.3f,
            orbitHeightCoefficient = .2f,
            orbitRadiusCoefficient = 1.5f,
            //
            oscillationFrequency = 1.0f,
            oscillationAmplitude = 1.0f,
        };
        public TransferToTheCustomerData transferToTheCustomerData = new TransferToTheCustomerData()
        {
            smoothTime = 0.3f,
        };
        #region hide
        //[HideInInspector]
        public KeySteps currentStep { get; protected set; }
        [HideInInspector] public IKeyStep[] keySteps;
        #endregion
        #endregion
        #region Functions
        private void Awake()
        {
            boxCollider.isTrigger = true;
            rb.isKinematic = true;
            //
            keySteps = new IKeyStep[System.Enum.GetValues(typeof(KeySteps)).Length];
            keySteps[(int)KeySteps.Idle] = new IKeyStep_Idle(this);
            keySteps[(int)KeySteps.Follower] = new IKeyStep_Follower(this);
            keySteps[(int)KeySteps.TransferToTheCustomer] = new IKeyStep_TransferToTheCustomer(this);
            keySteps[(int)KeySteps.Destroy] = new IKeyStep_Destroy(this);

            KeyStepChange(KeySteps.Idle, true);
        }
        private void Update()
        {
            if (keySteps[(int)currentStep] != null) keySteps[(int)currentStep].OnUpdate();
            if (this != null) this.transform.Rotate(0, keyData.rotationSpeed * Time.deltaTime, 0);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (keySteps[(int)currentStep] != null) keySteps[(int)currentStep].OnTriggerEnter(other);
        }
        #endregion
        #region functions
        public void TransferToTheCustomer(Door customer)
        {
            if (customer == null)
            {
                KeyStepChange(KeySteps.Idle, true);
                return;
            }
            transferToTheCustomerData.owner = customer;
            KeyStepChange(KeySteps.TransferToTheCustomer);
        }
        #region owner
        public void setOwner(HeroController owner)
        {
            this.heroInventoryStoreData.owner = owner;
        }
        public bool HasOwner() { return heroInventoryStoreData.owner != null; }
        #endregion
        public void KeyStepChange(KeySteps step, bool force = false)
        {
            if (!force && currentStep == step) { return; }
            if (keySteps[(int)currentStep] != null) keySteps[(int)currentStep].OnStepFinish();
            if (events != null) events.onPhaseEnd_Invoke();
            currentStep = step;
            if (keySteps[(int)currentStep] != null) keySteps[(int)currentStep].OnStepStart();
            if (events != null) events.onPhaseStart_Invoke();
        }
        #endregion
    }
}