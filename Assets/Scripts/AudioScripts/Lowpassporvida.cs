using UnityEngine;
using UnityEngine.Audio;

// Cierra el filtro Lowpass de la música a medida que baja la vida de Hilario
public class LowpassPorVida : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] string parametro = "CutoffMusica";
    [SerializeField] float umbralVida = 0.4f;
    [SerializeField] float cutoffNormal = 22000f;
    [SerializeField] float cutoffMinimo = 800f;
    [SerializeField] float suavizado = 4f;
    float objetivo = 22000f;
    float actual = 22000f;

    // Llamar desde el script de vida del jugador cada vez que cambie
    public void ActualizarVida(float vidaActual, float vidaMaxima)
    {
        float t = Mathf.Clamp01(vidaActual / vidaMaxima);
        if (t >= umbralVida)
            objetivo = cutoffNormal;
        else
            objetivo = Mathf.Lerp(cutoffMinimo, cutoffNormal, t / umbralVida);
    }

    void Update()
    {
        actual = Mathf.Lerp(actual, objetivo, suavizado * Time.deltaTime);
        mixer.SetFloat(parametro, actual);
    }
}