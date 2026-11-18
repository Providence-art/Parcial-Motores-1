using UnityEngine;
using System.Collections.Generic;

public class spiderBehabiour : MonoBehaviour
{
   [SerializeField] private float speed;
   
   private Transform moveObjective;
   private int index = 0;
   private Rigidbody rb;
   private int damage = 150;

   private List<Transform> movePoints = new List<Transform>();


    void Start()
    {
      rb = GetComponent<Rigidbody>();

      for(int i=1; i<=9; i++)
      {
        GameObject point = GameObject.Find("point"+i);
        if(point != null)
        {
          movePoints.Add(point.transform);
        }
        
      }

      if(movePoints.Count > 0)
      {
        moveObjective = movePoints[0];
      }
    }


    void FixedUpdate()
    {
      if(moveObjective == null){return;}

      Vector3 targetPosition = new Vector3 (moveObjective.position.x, transform.position.y, moveObjective.position.z);
      Vector3 direction = (targetPosition - transform.position).normalized;

      rb.linearVelocity = new Vector3(direction.x * speed, rb.linearVelocity.y, direction.z * speed);

      if(direction != Vector3.zero)
      {
        float angle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg + 180;
        rb.MoveRotation(Quaternion.Euler(0,angle,0));
      }

      
    }

    private void OnTriggerEnter(Collider other)
    {
      if(other.CompareTag("nextPoint") && other.transform == moveObjective)
      {
        index++;

        if(index < movePoints.Count)
        {
            moveObjective = movePoints[index];
        }
        else
        {
            moveObjective = null;
            rb.linearVelocity = Vector3.zero;
        }
      }
     }

     private void OnCollisionEnter(Collision collision)
     {
       if(collision.gameObject.CompareTag("Player"))
       {
         HilarioScript hilarioScript = collision.gameObject.GetComponent<HilarioScript>();

         if (hilarioScript != null)
         {
            hilarioScript.TakeDamage(damage);
         }
        }
      }

}