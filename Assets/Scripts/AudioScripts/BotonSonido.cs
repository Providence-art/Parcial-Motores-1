using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BotonSonido : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioSource fuente;
    [SerializeField] private AudioClip sonidoHover;
    [SerializeField] private AudioClip sonidoClick;
    [SerializeField, Range(0f, 1f)] private float volumen = 1f;

    private Button boton;

    private void Awake()
    {
        boton = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!boton.interactable) return;
        Reproducir(sonidoHover);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!boton.interactable) return;
        Reproducir(sonidoClick);
    }

    private void Reproducir(AudioClip clip)
    {
        if (clip == null || fuente == null) return;
        fuente.PlayOneShot(clip, volumen);
    }
}