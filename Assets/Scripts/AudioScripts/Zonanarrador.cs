using UnityEngine;

// Cuando el jugador entra en esta zona (Collider con Is Trigger), el narrador dice la línea indicada.
// Ponelo en un objeto vacío cerca del final del nivel, con un Box Collider marcado como Is Trigger.
public class ZonaNarrador : MonoBehaviour
{
    [SerializeField] NarradorGameplay narrador;
    [SerializeField] int indiceLinea = 1;
    [SerializeField] string tagJugador = "Player";

    bool yaSono;

    void OnTriggerEnter(Collider other)
    {
        if (yaSono || !other.CompareTag(tagJugador)) return;
        yaSono = true;
        narrador.ReproducirLinea(indiceLinea);
    }
}
