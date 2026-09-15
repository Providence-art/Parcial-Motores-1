using UnityEngine;
using Unity.Cinemachine;
public class LookScript : MonoBehaviour
{
    void Start()
    {
        CinemachineCamera vcam = FindFirstObjectByType<CinemachineCamera>();
        if ( vcam != null)
        {
            vcam.Follow = transform;
            vcam.LookAt = transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
