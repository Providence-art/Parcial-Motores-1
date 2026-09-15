using UnityEngine;

public class SpawnPlatform : MonoBehaviour
{
    [SerializeField] private GameObject hilario;

    private void Start()
    {
        SpawnHilario();
    }
   


    private void SpawnHilario()
    {

        Instantiate(hilario, transform.position, Quaternion.identity);
    }


}
