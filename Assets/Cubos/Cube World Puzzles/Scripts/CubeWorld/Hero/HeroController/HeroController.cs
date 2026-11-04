using HashGame.CubeWorld.InputManager;
using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;
using HashGame.CubeWorld.TerrainConstruction;
using UnityEngine.Events;
using HashGame.CubeWorld.CameraSystem;
using HashGame.CubeWorld.Extensions;
namespace HashGame.CubeWorld.HeroManager
{
    [RequireComponent(typeof(InputController))]
    [RequireComponent(typeof(HeroLineSensorController))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InventoryController))]
    public class HeroController : MonoBehaviour
    {
        #region variables
        #region readonly
        public const int StepsRecordingCount = 5;
        public bool IsHuman => inputController.controllerType == ControllerType.Human;
        public bool IsInitAsHuman { get; protected set; }
        #endregion
        public GameObject CubeWheel;
        public GameObject Head;
        public HeroControllerEvents events = new HeroControllerEvents();
        public HeroControllerBuffer buffer = new HeroControllerBuffer();
        [HideInInspector] public HeroAudioSettings _audioClips;
        public HeroAudioSettings AudioClips
        {
            get
            {
                if (_audioClips == null)
                {
                    if ((_audioClips = HeroAudioSettings.Instance) == null)
                    {
                        _audioClips = new HeroAudioSettings();
                    }
                }
                return _audioClips;
            }
        }
        public bool IsAudioClipsEnable => AudioClips;
        [HideInInspector] public HeroSettings _settings;
        public HeroSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    if ((_settings = HeroSettings.Instance) == null)
                    {
                        _settings = new HeroSettings();
                    }
                }
                return _settings;
            }
        }
        public bool isRespawnable = true;
        #region IHeroStep
        public HeroSteps currentStep
        {
            get => steps.Last;
            protected set
            {
                AddMovementSteps(value);
                steps.Add(value);
            }
        }
        public QueueHandler<HeroSteps> steps { get; protected set; }
        [HideInInspector] public IHeroStep_Base[] heroSteps;
        public QueueHandler<HeroSteps> movementSteps { get; protected set; }
        #endregion
        #region Read only
        public Vector3 HeroSize { get => boxCollider.bounds.size; }
        public Vector3 Position { get => transform.position; }
        public float UpDownSize { get => HeroSize.y; }
        public float CenterToUpDownSize => UpDownSize / 2;
        public float ForwardBackSize { get => HeroSize.z; }
        public float CenterToForwardBackSize => ForwardBackSize / 2;
        public float RightLeftSize { get => HeroSize.x; }
        public float CenterToRightLeftSize => RightLeftSize / 2;
        #endregion
        #region hide
        [HideInInspector] public InputController inputController;
        [HideInInspector] public HeroLineSensorController lineSensorController;
        [HideInInspector] public BoxCollider boxCollider;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public InventoryController inventoryController;
        [HideInInspector] public CameraBasic _camera;
        [HideInInspector] public TerrainManager terrainManager;
        [HideInInspector] public RenderDistanceController renderDistanceController;
        #endregion
        private Vector3 _startPosition;
        public bool IsStateChangeLock { get; protected set; }
        public bool isLock { get; set; }
        #endregion
        #region Functions
        private void OnValidate()
        {
            inputController = GetComponent<InputController>();
            lineSensorController = GetComponent<HeroLineSensorController>();
            boxCollider = GetComponent<BoxCollider>();
            inventoryController = GetComponent<InventoryController>();
            rb = GetComponent<Rigidbody>();
            if (!_settings)
            {
                _settings = HeroSettings.Instance;
            }
            if (_audioClips == null)
            {
                _audioClips = HeroAudioSettings.Instance;
            }
        }
        private void Awake()
        {
            steps = new QueueHandler<HeroSteps>(StepsRecordingCount);
            movementSteps = new QueueHandler<HeroSteps>(StepsRecordingCount);
            rb.isKinematic = true;
            //
            heroSteps = new IHeroStep_Base[System.Enum.GetValues(typeof(HeroSteps)).Length];
            heroSteps[(int)HeroSteps.Init] = new IHeroStep_Init(this);
            heroSteps[(int)HeroSteps.Idle] = new HeroStep_Idle(this);
            heroSteps[(int)HeroSteps.GoForward] = new IHeroStep_Movement(this, HeroSteps.GoForward);
            heroSteps[(int)HeroSteps.GoBack] = new IHeroStep_Movement(this, HeroSteps.GoBack);
            heroSteps[(int)HeroSteps.GoRight] = new IHeroStep_Movement(this, HeroSteps.GoRight);
            heroSteps[(int)HeroSteps.GoLeft] = new IHeroStep_Movement(this, HeroSteps.GoLeft);
            heroSteps[(int)HeroSteps.GoUpBack] = new IHeroStep_Movement(this, HeroSteps.GoUpBack);
            heroSteps[(int)HeroSteps.GoUpForward] = new IHeroStep_Movement(this, HeroSteps.GoUpForward);
            heroSteps[(int)HeroSteps.GoUpRight] = new IHeroStep_Movement(this, HeroSteps.GoUpRight);
            heroSteps[(int)HeroSteps.GoUpLeft] = new IHeroStep_Movement(this, HeroSteps.GoUpLeft);
            heroSteps[(int)HeroSteps.Sliding] = new IHeroStep_Sliding(this);
            heroSteps[(int)HeroSteps.Falling] = new IHeroStep_Falling(this);
            heroSteps[(int)HeroSteps.Destroy] = new IHeroStep_Destroy(this);

            if (CubeWheel == null)
            {
                CubeWheel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                CubeWheel.transform.parent = transform;
                var col = CubeWheel.GetComponent<Collider>();
                if (col)
                {
                    BasicExtensions.DestroyObject(col);
                }
            }
            if (!terrainManager)
            {
                terrainManager = GameObject.FindAnyObjectByType<TerrainManager>();
            }
        }
        private void Start()
        {
            IsInitAsHuman = IsHuman;
            _startPosition = Position;
            resetHeroPositionAndRotation();
            ChangeStep(HeroSteps.Init, true);

            if (IsHuman && terrainManager)
            {
                if (terrainManager.localSettings.limitedVisibility)
                {
                    renderDistanceController = GetComponent<RenderDistanceController>();
                    if (!renderDistanceController) renderDistanceController = gameObject.AddComponent<RenderDistanceController>();
                    renderDistanceController.sphereCollider.radius = terrainManager.localSettings.visionRadius;
                }
            }
        }
        private void Update()
        {
            if (isLock) return;
            heroSteps[(int)currentStep].OnUpdate();
        }
        #endregion
        #region functions
        private void resetHeroPositionAndRotation()
        {
            this.transform.rotation = Quaternion.identity;
            if (CubeWheel != null)
            {
                CubeWheel.transform.localScale = Vector3.one;
                CubeWheel.transform.localPosition = Vector3.zero;
                CubeWheel.transform.rotation = Quaternion.identity;
            }
        }
        public void ResetHero()
        {
            if (IsHuman)
            {
                if (terrainManager == null || terrainManager.CheckPointLead == null)
                {
                    this.transform.position = _startPosition;
                }
                else
                {
                    this.transform.position = terrainManager.CheckPointLead.obstacle.UpCenter + 1.5f * Vector3.up * UpDownSize;
                }
            }
            else
            {
                this.transform.position = _startPosition;
            }
            resetHeroPositionAndRotation();
        }
        public bool StayOnGround()
        {
            GameObject cube = lineSensorController.GroundFinding();
            if (cube == null) return false;
            Obstacle obstacle;
            HeroController anotherHero;
            if (obstacle = cube.GetComponent<Obstacle>())
            {
                UpdateHeroPositionFromObstacle(obstacle);
                return true;
            }
            if (anotherHero = cube.gameObject.GetComponent<HeroController>())
            {
                if (!anotherHero.currentStep.IsStable()) return false;
                transform.position = anotherHero.Position + Vector3.up * (anotherHero.CenterToUpDownSize + CenterToUpDownSize);
                return true;
            }
            return false;
        }
        public void UpdateHeroPositionFromObstacle(Obstacle obstacle)
        {
            transform.position = obstacle.Position + Vector3.up * (obstacle.sizeUp + CenterToUpDownSize);
        }
        #region Auto walk
        public void PreparateToAutowalk()
        {
            IsStateChangeLock = true;
        }
        public void AutowalkIsOver()
        {
            IsStateChangeLock = false;
        }
        #endregion
        #region Collectable
        public bool IsCollecting(GameObject obj)
        {
            if (obj == null || obj.GetComponent<CollectableObject>() == null) return false;
            inventoryController.AddInventory(obj);
            return true;
        }
        #endregion
        #region material
        public virtual void SetMaterial(Material material)
        {
            if (CubeWheel == null) return;
            MeshRenderer renderer = CubeWheel.GetComponent<MeshRenderer>();
            if (renderer) renderer.material = material;
        }
        #endregion
        #region IHeroStep
        public void ChangeStep(HeroSteps step, bool force = false)
        {
            if (currentStep == step && !force) return;
            heroSteps[(int)currentStep].OnStepFinish();
            heroSteps[(int)(currentStep = step)].OnStepStart();
            events.onHeroPhaseChangeInvoke();
        }
        #endregion
        #region movementSteps
        public void AddMovementSteps(HeroSteps step)
        {
            if (step.IsMovementStep()) movementSteps.Add(step);
        }
        #endregion
        #region teleport
        public void PreparateToTeleport() => IsStateChangeLock = true;
        public void TeleportingIsFinish()
        {
            StayOnGround();
            ChangeStep(HeroSteps.Init, true);
            IsStateChangeLock = false;
        }
        #endregion
        #region destroy
        public void ForceToDestroyPhase() => ChangeStep(HeroSteps.Destroy, true);
        #endregion
        #region terrainManager
        public void setTerrainManager(TerrainManager manager)
        {
            terrainManager = manager;
        }
        #endregion
        #region Camera
        public void SetCamera(CameraBasic cam)
        {
            _camera = cam;
            if (_camera)
            {
                inputController.enableAxisMapping = _camera.IncludeAxisMapping;
            }
        }
        #endregion
        #endregion
        #region struct
        [System.Serializable]
        public class HeroControllerBuffer
        {
            public int CurrentSteps;
        }
        [System.Serializable]
        public class HeroControllerEvents : StepsEventStructBase
        {
            public UnityEvent onHeroInited;
            public UnityEvent onHeroIdle;
            public UnityEvent onHeroFalling;
            public UnityEvent onHeroSliding;
            public UnityEvent onHeroMovement;
            public UnityEvent onHeroPhaseChange;
            public UnityEvent onHeroDead;
            public void onHeroInitedInvoke() => invoke(ref onHeroInited);
            public void onHeroPhaseChangeInvoke() => invoke(ref onHeroPhaseChange);
            public void onHeroMovementInvoke() => invoke(ref onHeroMovement);
            public void onHeroDeadInvoke() => invoke(ref onHeroDead);
        }
        #endregion
    }
}