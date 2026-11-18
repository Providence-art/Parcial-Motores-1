using UnityEngine;

public class spiderSpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform spawnPoint;

    private bool alreadySpawned = false;

    private void OnTriggerExit(Collider other)
    {
        if(!alreadySpawned && other.CompareTag("Player"))
        {
          Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
          alreadySpawned = true;
        }
    }
}
