using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerScript : MonoBehaviour
{
    static private InputManagerScript instance;

    static public InputManagerScript Instance => instance;

    [SerializeField] public InputActionReference moveAction;
    [SerializeField] public InputActionReference lookAction;
    [SerializeField] public InputActionReference sprintAction;
    [SerializeField] public InputActionReference crouchAction;
    [SerializeField] public InputActionReference jumpAction;
    

       private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

    



}
