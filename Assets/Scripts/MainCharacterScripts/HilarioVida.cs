using UnityEngine;
using UnityEngine.Events;

public class VidaHilario : MonoBehaviour
{
    [SerializeField] float vidaMaxima = 100f;
    [SerializeField] float segundosParaCurar = 10f;
    [SerializeField] float curacionPorSegundo = 10f;
    [SerializeField] LowpassPorVida filtroVida;
    [SerializeField] UnityEvent alRecibirDano;
    [SerializeField] UnityEvent alMorir;

    float vidaActual;
    float tiempoUltimoDano = -999f;
    bool muerto;

    public float VidaActual => vidaActual;

    void Start()
    {
        vidaActual = vidaMaxima;
        NotificarAudio();
    }

    public void RecibirDano(float cantidad)
    {
        if (muerto || cantidad <= 0f) return;

        vidaActual = Mathf.Max(0f, vidaActual - cantidad);
        tiempoUltimoDano = Time.time;
        alRecibirDano.Invoke();
        NotificarAudio();

        if (vidaActual <= 0f)
        {
            muerto = true;
            alMorir.Invoke();
        }
    }

    void Update()
    {
        if (muerto || vidaActual >= vidaMaxima) return;
        if (Time.time - tiempoUltimoDano < segundosParaCurar) return;

        vidaActual = Mathf.Min(vidaMaxima, vidaActual + curacionPorSegundo * Time.deltaTime);
        NotificarAudio();
    }

    void NotificarAudio()
    {
        if (filtroVida != null) filtroVida.ActualizarVida(vidaActual, vidaMaxima);
    }
}