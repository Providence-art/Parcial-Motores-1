using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    [SerializeField] private GameObject hilario;
    [SerializeField] private CinemachineCamera camara;
    [SerializeField] private LowpassPorVida filtroVida;
    [SerializeField] private float respawnDelay = 1.5f;

    private GameObject actual;
    private bool respawneando;

    private void Start()
    {
        SpawnHilario();
    }

    private void OnEnable()
    {
        VidaHilario.AlMorirCualquiera += OnMurio;
    }

    private void OnDisable()
    {
        VidaHilario.AlMorirCualquiera -= OnMurio;
    }

    private void Update()
    {
        if (!respawneando && (actual == null || !actual.activeInHierarchy))
        {
            Debug.Log("[Spawner] Hilario desapareció, iniciando respawn");
            IniciarRespawn();
        }
    }

    private void OnMurio(VidaHilario quien)
    {
        Debug.Log("[Spawner] Evento de muerte recibido");
        IniciarRespawn();
    }

    private void IniciarRespawn()
    {
        if (respawneando) return;
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        respawneando = true;

        yield return new WaitForSecondsRealtime(respawnDelay);

        if (actual != null) Destroy(actual);
        SpawnHilario();

        respawneando = false;
    }

    private void SpawnHilario()
    {
        actual = Instantiate(hilario, transform.position, Quaternion.identity);
        Debug.Log("[Spawner] Hilario instanciado");

        VidaHilario vida = actual.GetComponentInChildren<VidaHilario>();
        if (vida != null) vida.AsignarFiltro(filtroVida);
        else Debug.LogWarning("[Spawner] El prefab no tiene VidaHilario");

        if (camara != null)
        {
            camara.Target.TrackingTarget = actual.transform;
            camara.PreviousStateIsValid = false;
        }
        else
        {
            Debug.LogWarning("[Spawner] Falta asignar la CinemachineCamera");
        }
    }
}