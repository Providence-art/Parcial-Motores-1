using Microsoft.VisualBasic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManagerScript : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public InputActionReference sprintAction;
    public InputActionReference crouchAction;
    public InputActionReference jumpAction;

    static private InputManagerScript instance;
    static public InputManagerScript Instance => instance;

        
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
