using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class TriggerVictoria : MonoBehaviour
{
    [SerializeField] private float demora = 0.5f;

    private bool activado;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;
        if (other.GetComponentInParent<HilarioScript>() == null) return;

        activado = true;
        Invoke(nameof(CargarVictoria), demora);
    }

    private void CargarVictoria()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("EndScene");
    }
}