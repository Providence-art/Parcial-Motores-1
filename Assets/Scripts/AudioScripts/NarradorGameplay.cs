using System.Collections;
using UnityEngine;

[System.Serializable]
public class LineaNarrador
{
    public AudioClip clip;
    public float esperaAntes = 2f;
}

[RequireComponent(typeof(AudioSource))]
public class NarradorGameplay : MonoBehaviour
{
    [SerializeField] LineaNarrador[] lineas;
    [SerializeField] int lineasAutomaticas = 1;

    AudioSource fuente;
    Coroutine secuencia;

    void Awake()
    {
        fuente = GetComponent<AudioSource>();
        fuente.playOnAwake = false;
        fuente.loop = false;
    }

    void Start()
    {
        if (lineasAutomaticas > 0) secuencia = StartCoroutine(Secuencia());
    }

    IEnumerator Secuencia()
    {
        int cantidad = Mathf.Min(lineasAutomaticas, lineas.Length);
        for (int i = 0; i < cantidad; i++)
        {
            yield return new WaitForSeconds(lineas[i].esperaAntes);
            if (lineas[i].clip == null) continue;
            fuente.clip = lineas[i].clip;
            fuente.Play();
            yield return new WaitWhile(() => fuente.isPlaying);
        }
        secuencia = null;
    }

    public void ReproducirLinea(int indice)
    {
        if (indice < 0 || indice >= lineas.Length || lineas[indice].clip == null) return;
        if (secuencia != null) { StopCoroutine(secuencia); secuencia = null; }
        fuente.Stop();
        fuente.clip = lineas[indice].clip;
        fuente.Play();
    }
}