using UnityEngine;
namespace HashGame.CubeWorld.InputManager
{
    [CreateAssetMenu(fileName = NAME, menuName = Globals.ProjectName + "/Game/Hero/"+ NAME)]
    public class InputControllerSettings : ScriptableObject
    {
        public const string NAME = "InputControllerSettings";
        #region Instance
        private static InputControllerSettings _instance;
        public static InputControllerSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    if ((_instance = Resources.Load<InputControllerSettings>("Settings/"+ NAME)) == null)
                        _instance = new InputControllerSettings();
                }
                return _instance;
            }
        }
        #endregion
        #region variable
        public InputType inputType = InputType.KeyBoard;
        public bool onFrequencySampling = true;
        public KeyBoardInputData keyBoardInput;
        #endregion
        #region struct
        [System.Serializable]
        public class KeyBoardInputData
        {
            public KeyCode Forward= KeyCode.UpArrow;
            public KeyCode Backward = KeyCode.DownArrow;
            public KeyCode Right = KeyCode.RightArrow;
            public KeyCode Left = KeyCode.LeftArrow;
        }
        #endregion
    }
}