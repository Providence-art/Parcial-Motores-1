using UnityEngine;
using UnityEngine.Audio;

public class MusicaAdaptativa : MonoBehaviour
{
    [SerializeField] AudioMixerSnapshot exploracion;
    [SerializeField] AudioMixerSnapshot pelea;
    [SerializeField] AudioMixerSnapshot victoria;
    [SerializeField] float tPelea = 0.4f;
    [SerializeField] float tVictoria = 2.5f;
    [SerializeField] float tExploracion = 3f;
    [SerializeField] float segundosEnVictoria = 6f;
    int enemigosVivos;
    bool enPelea;

    void Start() { exploracion.TransitionTo(0f); }

    public void EnemigoAparecio()
    {
        enemigosVivos++;
        if (!enPelea)
        {
            enPelea = true;
            CancelInvoke(nameof(VolverAExploracion));
            pelea.TransitionTo(tPelea);
        }
    }

    public void EnemigoDerrotado()
    {
        enemigosVivos = Mathf.Max(0, enemigosVivos - 1);
        if (enPelea && enemigosVivos == 0)
        {
            enPelea = false;
            victoria.TransitionTo(tVictoria);
            Invoke(nameof(VolverAExploracion), segundosEnVictoria);
        }
    }

    void VolverAExploracion() { exploracion.TransitionTo(tExploracion); }
}