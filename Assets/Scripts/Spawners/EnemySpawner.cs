using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int spawnTime;
    [SerializeField] private GameObject entityType;
    private int accumulativeTicks = 0;
   private void OnEnable()
    {
        Clock.Instance.Tick += Ticked;
    }
   private void OnDisable()
    {
        Clock.Instance.Tick -= Ticked;
    }



    private void Ticked()
    {
        accumulativeTicks++;
        if (accumulativeTicks >= spawnTime) {
            SpawnEntity();
        }
    }
    private void SpawnEntity()
    {
        Instantiate(entityType, transform.position, Quaternion.identity);
        accumulativeTicks -= spawnTime;
    }   
}
