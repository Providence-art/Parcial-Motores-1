using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    [SerializeField] private int speed;
    [SerializeField] private Transform Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player != null)
        {
            Debug.Log("holA");
        Vector3 direction = (Player.transform.position-transform.position);
        direction.Normalize();
        transform.Translate(direction*speed*Time.deltaTime);
        }
        
    }
}
