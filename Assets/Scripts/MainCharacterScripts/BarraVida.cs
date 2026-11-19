using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    [SerializeField] private Image relleno;
    [SerializeField] private Color colorLleno = new Color(0.25f, 0.8f, 0.25f);
    [SerializeField] private Color colorMedio = new Color(1f, 0.55f, 0f);
    [SerializeField] private Color colorBajo = new Color(0.85f, 0.15f, 0.15f);
    [SerializeField] private float velocidad = 2f;

    private HilarioScript jugador;
    private float fillActual = 1f;

    private void Update()
    {
        if (jugador == null)
            jugador = FindFirstObjectByType<HilarioScript>();

        float objetivo = 0f;
        if (jugador != null && jugador.MaxHealth > 0)
            objetivo = Mathf.Clamp01((float)jugador.Health / jugador.MaxHealth);

        fillActual = Mathf.MoveTowards(fillActual, objetivo, velocidad * Time.unscaledDeltaTime);

        relleno.fillAmount = fillActual;
        relleno.color = ColorPara(fillActual);
    }

    private Color ColorPara(float t)
    {
        if (t > 0.5f)
            return Color.Lerp(colorMedio, colorLleno, (t - 0.5f) / 0.5f);
        return Color.Lerp(colorBajo, colorMedio, t / 0.5f);
    }
}