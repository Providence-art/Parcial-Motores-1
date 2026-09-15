using UnityEngine;
using Unity.Cinemachine;
public class SpawnPlatform : MonoBehaviour
{
    [SerializeField] private GameObject hilario;
    [SerializeField] private CinemachineCamera vcam;
    private void Start()
    {
        if (FindFirstObjectByType<CinemachineCamera>()  == null)
        {
            SpawnCamera();
        }
        SpawnHilario();

    }
   


    private void SpawnHilario()
    {

        Instantiate(hilario, transform.position, Quaternion.identity);
    }

    private void SpawnCamera()
    {
        Instantiate(vcam, transform.position, Quaternion.identity);
    }
}
