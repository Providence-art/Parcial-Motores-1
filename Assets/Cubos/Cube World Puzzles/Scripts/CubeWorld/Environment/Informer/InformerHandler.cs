using HashGame.CubeWorld.HeroManager;
using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace HashGame.CubeWorld.Informer
{
    [RequireComponent(typeof(BoxCollider), typeof(Rigidbody))]
    public class InformerHandler : MonoBehaviour
    {
        #region variable
        #region const
        public const string NAME = "Informer";
        public const string Prefab = "Prefabs/Extera/Informer/" + NAME;
        #endregion
        public GameObject popupPanel;
        public Text displayText;
        public bool destroyAfterRead = false;
        public InfoEvents events = new InfoEvents();
        //[Header("Message")]
        [Tooltip("Showing message.")]
        [TextArea(5, 15)]
        public string message;
        public InformerData data = new InformerData()
        {
            rotationSpeed = 50.0f
        };
        #region HideInInspector
        [HideInInspector] public BoxCollider boxCollider;
        [HideInInspector] public Rigidbody rb;
        #endregion
        #endregion
        #region Functions
        private void OnValidate()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            boxCollider = rb.GetComponent<BoxCollider>();
        }
        private void Awake()
        {
            rb.isKinematic = true;
            boxCollider.isTrigger = true;
        }
        void Start()
        {
            if (popupPanel) popupPanel.SetActive(false);
            StayOnGround();
        }

        void Update()
        {
            if (this != null) this.transform.Rotate(0, data.rotationSpeed * Time.deltaTime, 0);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (string.IsNullOrEmpty(message)) return;
            if (other == null) return;
            HeroController hero = other.GetComponent<HeroController>();
            if (hero == null) return;
            if (hero.inputController.controllerType != InputManager.ControllerType.Human) return;
            ShowPopup(message);
        }
        private void OnTriggerExit(Collider other)
        {
            if (string.IsNullOrEmpty(message)) return;
            if (other == null) return;
            HeroController hero = other.GetComponent<HeroController>();
            if (hero == null) return;
            if (hero.inputController.controllerType != InputManager.ControllerType.Human) return;
            ClosePopup();
        }
        #endregion
        #region functions
        public void ShowPopup(string message)
        {
            displayText.text = message;
            popupPanel.SetActive(true);
            if (events.onShowMessage != null) events.onShowMessage.Invoke();
        }
        public void ClosePopup()
        {
            popupPanel.SetActive(false);
            if (events.onCloseMessage != null) events.onCloseMessage.Invoke();
            if (destroyAfterRead)
            {
                Destroy(gameObject);
            }
        }
        public bool StayOnGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
            {
                if (hit.collider != null)
                {
                    GameObject cube = hit.collider.gameObject;
                    if (cube == null) return false;
                    return StandOnCube(cube.GetComponent<Obstacle>());
                }
            }
            return false;
        }
        public bool StandOnCube(Obstacle obstacle)
        {
            if (obstacle == null) return false;
            transform.position = obstacle.Position + Vector3.up * (obstacle.sizeUp + boxCollider.bounds.size.y);
            return true;
        }
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
        #endregion
        #region struct
        [System.Serializable]
        public struct InformerData
        {
            public float rotationSpeed;
        }
        [System.Serializable]
        public struct InfoEvents
        {
            public UnityEvent onShowMessage;
            public UnityEvent onCloseMessage;
        }
        #endregion
    }
}