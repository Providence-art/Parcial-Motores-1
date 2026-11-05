using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DisparadorDeVoz : MonoBehaviour
{
    [SerializeField] AudioClip linea;
    [SerializeField] string tagJugador = "Player";
    [SerializeField] bool soloUnaVez = true;
    AudioSource fuente;
    bool yaSono;

    void Awake() { fuente = GetComponent<AudioSource>(); }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagJugador)) Reproducir();
    }

    public void Reproducir()
    {
        if (soloUnaVez && yaSono) return;
        if (fuente.isPlaying) return;
        yaSono = true;
        fuente.clip = linea;
        fuente.Play();
    }
}
