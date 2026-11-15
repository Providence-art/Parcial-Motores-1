using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void NuevaPartida()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level 2");
    }

}