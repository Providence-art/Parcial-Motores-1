using Microsoft.VisualBasic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class InputManagerScript : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public InputActionReference sprintAction;
    public InputActionReference crouchAction;
    public InputActionReference jumpAction;

    public InputActionReference Reset;

    static private InputManagerScript instance;
    static public InputManagerScript Instance => instance;
    private string currentSceneName;
        
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
    private void OnEnable()
    {
        Reset.action.Enable();

    }
    private void OnDisable()
    {
        Reset.action.Disable();
    }
    private void Update()
    {
       bool resetting = Reset.action.IsPressed(); 
        if (resetting == true){
            currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}
