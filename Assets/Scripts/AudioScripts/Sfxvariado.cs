using UnityEngine;

// Reproduce un clip al azar sin repetir el anterior, con pitch y volumen aleatorios.

[RequireComponent(typeof(AudioSource))]
public class SfxVariado : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] Vector2 rangoPitch = new Vector2(0.9f, 1.1f);
    [SerializeField] Vector2 rangoVolumen = new Vector2(0.85f, 1f);
    AudioSource fuente;
    int ultimo = -1;

    void Awake() { fuente = GetComponent<AudioSource>(); }

    // Llamar desde la acción del juego
    public void Reproducir()
    {
        if (clips == null || clips.Length == 0) return;

        int i;
        do { i = Random.Range(0, clips.Length); }
        while (clips.Length > 1 && i == ultimo);
        ultimo = i;

        fuente.pitch = Random.Range(rangoPitch.x, rangoPitch.y);
        fuente.PlayOneShot(clips[i], Random.Range(rangoVolumen.x, rangoVolumen.y));
    }
}
