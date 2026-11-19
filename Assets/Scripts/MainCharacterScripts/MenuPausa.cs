using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private string escenaMenu = "Pantalla Inicio";

    public static bool EnPausa { get; private set; }

    private CursorLockMode lockPrevio;
    private bool cursorVisiblePrevio;
    private CharacterMovement movimiento;

    private void Start()
    {
        EnPausa = false;
        Time.timeScale = 1f;
        if (panelPausa != null) panelPausa.SetActive(false);
    }

    private void Update()
    {
        if (!TeclaPausa()) return;

        if (EnPausa) Reanudar();
        else Pausar();
    }

    private bool TeclaPausa()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#else
        return Input.GetKeyDown(KeyCode.Escape);
#endif
    }

    public void Pausar()
    {
        EnPausa = true;
        panelPausa.SetActive(true);
        Time.timeScale = 0f;

        lockPrevio = Cursor.lockState;
        cursorVisiblePrevio = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        movimiento = FindFirstObjectByType<CharacterMovement>();
        if (movimiento != null) movimiento.enabled = false;
    }

    public void Reanudar()
    {
        EnPausa = false;
        panelPausa.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = lockPrevio;
        Cursor.visible = cursorVisiblePrevio;

        if (movimiento != null) movimiento.enabled = true;
    }

    public void ReiniciarNivel()
    {
        SalirDePausa();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolverAlMenu()
    {
        SalirDePausa();
        SceneManager.LoadScene(escenaMenu);
    }

    private void SalirDePausa()
    {
        EnPausa = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}