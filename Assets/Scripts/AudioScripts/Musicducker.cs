using UnityEngine;
using UnityEngine.Audio;

// Baja la música mientras suena una línea de voz (ducking por código)
public class MusicDucker : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioSource[] voces;
    [SerializeField] string parametro = "MusicaVol";
    [SerializeField] float dbReposo = 0f;
    [SerializeField] float dbBajado = -12f;
    [SerializeField] float dbPorSegBajada = 60f;
    [SerializeField] float dbPorSegSubida = 15f;
    float actual;

    void Start() { actual = dbReposo; }

    void Update()
    {
        bool hablando = false;
        foreach (var v in voces)
            if (v != null && v.isPlaying) { hablando = true; break; }

        float objetivo = hablando ? dbBajado : dbReposo;
        float vel = hablando ? dbPorSegBajada : dbPorSegSubida;
        actual = Mathf.MoveTowards(actual, objetivo, vel * Time.deltaTime);
        mixer.SetFloat(parametro, actual);
    }
}