using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class spiderBehabiour : MonoBehaviour
{
   [SerializeField] private Animator animator;

   [SerializeField] private float speed;
   [SerializeField] private SplineContainer spline;
   
   
   private Rigidbody rb;
   private float walkedDistance;
   private float totalLength;
   [SerializeField] private float groundOffset;
  


    void Start()
    {
      if (animator == null) animator = GetComponentInChildren<Animator>();

      rb = GetComponent<Rigidbody>();
      spline = GameObject.Find("scapeVent").GetComponent<SplineContainer>();

       if(spline != null)
       {
         totalLength = spline.CalculateLength();
       }
    }

    void Update()
    {
      ActualizarAnimaciones();
    }


    void FixedUpdate()
    {
       if(spline == null || totalLength <= 0f){return;}

       walkedDistance += speed * Time.fixedDeltaTime;
       float rotation = Mathf.Clamp01(walkedDistance / totalLength);

       spline.Evaluate(rotation, out float3 position, out float3 tangent, out float3 above);

       Vector3 down = ((Vector3)above).sqrMagnitude > 0.01f ? -((Vector3)above).normalized : Vector3.down;
       Vector3 groundPosition = (Vector3)position + (down * groundOffset);

       rb.MovePosition(groundPosition);
       Vector3 direction = new Vector3(tangent.x, 0f, tangent.z);

       if(direction.sqrMagnitude > 0.01f)
       {
         float angle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg + 180f;
         rb.MoveRotation(Quaternion.Euler(0f,angle,0f));
       }
    }


     private void OnTriggerEnter(Collider other)
     {
       if(other.gameObject.CompareTag("Player"))
       {
         HilarioScript hilarioScript = other.gameObject.GetComponent<HilarioScript>();

         if (hilarioScript != null)
         {
            hilarioScript.Die();
         }
        }
      }


     private void ActualizarAnimaciones()
     {
      animator.SetBool("Caminando", true);
     } 
}