using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    [SerializeField] private int speed;
    private Transform Player;
    private Rigidbody rb;
    private Vector3 direction;
    private float distance;
    [SerializeField] private LayerMask obstacle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
       rb = GetComponent<Rigidbody>();
       Player = null;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
     if (Player == null)
     {
       GameObject foundPlayer = GameObject.FindWithTag("Player");              
       if(foundPlayer != null)
       {
            Player = foundPlayer.transform;
            Debug.Log("encontrado");
       }
     }

     if (Player != null)
     { 
       distance = Vector3.Distance(transform.position, Player.position);
     

      if(distance < 30 && Player != null)
      {
         direction = Player.transform.position - transform.position;
         Vector3 rayDirection = direction.normalized;

         if(Physics.Raycast(transform.position, rayDirection, distance, obstacle) == false)
         {
           float angle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg + 90;
           transform.rotation = Quaternion.Euler(0,angle,0);
           direction.Normalize();
         }
         else
         {
           direction = Vector3.zero;
         }
      }

      else
      {
         direction = Vector3.zero;
      }

     }
     else
      {
         direction = Vector3.zero;
      }
    }

    protected virtual void FixedUpdate()
    {
       if(rb != null && direction != Vector3.zero)
       {
          rb.linearVelocity = new Vector3(direction.x*speed,rb.linearVelocity.y, direction.z*speed);
       }
    }
   //private void OnTriggerEnter(Collider other)
    //{
      //if(other.CompareTag ("PLayer"))
      //{
       // Player = other.GetComponent<Transform>();
      ///}
    //}

}
