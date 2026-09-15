using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    [SerializeField] private int speed;
    [SerializeField] private Transform Player;
    private Rigidbody rb;
    private Vector3 direction;
    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
      distance = Vector3.Distance(transform.position, Player.transform.position);


      if(distance < 16)
      {
         direction = Player.transform.position - transform.position;

         float angle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg + 90;
         transform.rotation = Quaternion.Euler(0,angle,0);

         direction.Normalize();

         //transform.Translate(direction*speed*Time.deltaTime);
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


}
