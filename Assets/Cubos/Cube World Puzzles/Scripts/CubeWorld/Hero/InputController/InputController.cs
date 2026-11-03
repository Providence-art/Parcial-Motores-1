using HashGame.CubeWorld.HeroManager;
using UnityEngine;

namespace HashGame.CubeWorld.InputManager
{
    #region enum
    public enum Flags : int
    {
        Forward = 0, Back = 1, Left = 2, Right = 3,
    }
    public static class FlagsExtentions
    {
        public static bool ToFlag(this Vector3 v, out Flags flag)
        {
            if (v == Vector3.forward)
            {
                flag = Flags.Forward;
                return true;
            }
            if (v == Vector3.right)
            {
                flag = Flags.Right;
                return true;
            }
            if (v == Vector3.back)
            {
                flag = Flags.Back;
                return true;
            }
            if (v == Vector3.left)
            {
                flag = Flags.Left;
                return true;
            }
            flag = default(Flags);
            return false;
        }
    }
    public enum InputType : int
    {
        KeyBoard,
        InputAxis
    }
    public enum ControllerType : int
    {
        Human = 0,
        CPU
    }
    #endregion
    [RequireComponent(typeof(HeroController))]
    public class InputController : MonoBehaviour
    {
        #region variable
        public ControllerType controllerType = ControllerType.Human;
        public bool enableAxisMapping;
        #region Settings
        [HideInInspector] public InputControllerSettings _settings;
        public InputControllerSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    if ((_settings = InputControllerSettings.Instance) == null)
                    {
                        _settings = new InputControllerSettings();
                    }
                }
                return _settings;
            }
        }
        #endregion
        #region hide
        [HideInInspector] public HeroController Controller;
        private bool[] _flags = new bool[System.Enum.GetValues(typeof(Flags)).Length];
        private int[] _flagMapping = new int[System.Enum.GetValues(typeof(Flags)).Length];
        private Vector3 _motionAxis;
        #endregion
        #endregion
        #region Function
        private void OnValidate()
        {
            Controller = GetComponent<HeroController>();
            if (_settings == null)
            {
                _settings = InputControllerSettings.Instance;
            }
        }
        private void Start()
        {
            for (int i = 0; i < _flagMapping.Length; i++) _flagMapping[i] = i;
        }
        private void Update()
        {
            switch (controllerType)
            {
                case ControllerType.Human:
                    humanControllerType();
                    break;
            }
        }
        #endregion
        #region functions
        #region static

        #endregion
        #region Flags
        public bool IsFlag(Flags flag)
        {
            if (_flags[(int)flag])
            {
                _flags[(int)flag] = false;
                return true;
            }
            return false;
        }
        private void raisingFheFlag(Flags flag)
        {
            _flags[(int)getMappedFlag(flag)] = true;
        }
        #endregion
        #region flag mapping
        protected Flags getMappedFlag(Flags flag) => (Flags)_flagMapping[(int)flag];
        public void flagMappingUpdate(Vector3 motionAxis)
        {
            if (_motionAxis == motionAxis) return;
            _motionAxis = motionAxis;
            if (!enableAxisMapping) return;
            if (motionAxis == Vector3.forward)
            {
                _flagMapping[(int)Flags.Forward] = (int)Flags.Forward;
                _flagMapping[(int)Flags.Right] = (int)Flags.Right;
                _flagMapping[(int)Flags.Back] = (int)Flags.Back;
                _flagMapping[(int)Flags.Left] = (int)Flags.Left;
            }
            else if (motionAxis == Vector3.right)
            {
                _flagMapping[(int)Flags.Forward] = (int)Flags.Right;
                _flagMapping[(int)Flags.Right] = (int)Flags.Back;
                _flagMapping[(int)Flags.Back] = (int)Flags.Left;
                _flagMapping[(int)Flags.Left] = (int)Flags.Forward;
            }
            else if (motionAxis == Vector3.left)
            {
                _flagMapping[(int)Flags.Forward] = (int)Flags.Left;
                _flagMapping[(int)Flags.Left] = (int)Flags.Back;
                _flagMapping[(int)Flags.Back] = (int)Flags.Right;
                _flagMapping[(int)Flags.Right] = (int)Flags.Forward;
            }
            if (motionAxis == Vector3.back)
            {
                _flagMapping[(int)Flags.Forward] = (int)Flags.Back;
                _flagMapping[(int)Flags.Back] = (int)Flags.Forward;
                _flagMapping[(int)Flags.Left] = (int)Flags.Right;
                _flagMapping[(int)Flags.Right] = (int)Flags.Left;
            }
        }
        #endregion
        #region Force
        public void AddForce(Vector3 direction)
        {
            if (direction.ToFlag(out var flag))
            {
                raisingFheFlag(flag);
            }
        }
        #endregion
        #region ControllerType.Human
        private void humanControllerType()
        {
            if (Settings.onFrequencySampling && Controller.currentStep != HeroSteps.Idle) return;
            if (Controller.IsStateChangeLock) return;
            switch (Settings.inputType)
            {
                case InputType.KeyBoard:
                    KeyBoardCheck();
                    break;
                case InputType.InputAxis:
                    InputAxisCheck();
                    break;
            }
        }
        #region InputAxis
        private void InputAxisCheck()
        {
            float x = Input.GetAxis("Vertical");
            float y = Input.GetAxis("Horizontal");
            if (x > 0.0f) // && isGrounded
            {
                raisingFheFlag(Flags.Forward);
            }
            else if (x < 0.0f)
            {
                raisingFheFlag(Flags.Back);
            }
            if (y > 0.0f) // && isGrounded
            {
                raisingFheFlag(Flags.Right);
            }
            else if (y < 0.0f) // && isGrounded
            {
                raisingFheFlag(Flags.Left);
            }
        }
        #endregion
        #region KeyBoard
        private void KeyBoardCheck()
        {
            if (Input.GetKeyDown(Settings.keyBoardInput.Forward)) // && isGrounded
            {
                raisingFheFlag(Flags.Forward);
                return;
            }
            if (Input.GetKeyDown(Settings.keyBoardInput.Backward))
            {
                raisingFheFlag(Flags.Back);
                return;
            }
            if (Input.GetKeyDown(Settings.keyBoardInput.Right)) // && isGrounded
            {
                raisingFheFlag(Flags.Right);
                return;
            }
            if (Input.GetKeyDown(Settings.keyBoardInput.Left)) // && isGrounded
            {
                raisingFheFlag(Flags.Left);
                return;
            }
        }
        #endregion
        #endregion
        #endregion
    }

}