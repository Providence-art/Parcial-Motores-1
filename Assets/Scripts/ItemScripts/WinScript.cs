using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net.Security;

public class WinScript : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    void Start()
    {

    }

        
    private void OnTriggerEnter(Collider other) 
    {
        HilarioScript Player = other.GetComponent<HilarioScript>();

        if (Player != null)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    } 
    
       
    

    // Update is called once per frame
     
}
