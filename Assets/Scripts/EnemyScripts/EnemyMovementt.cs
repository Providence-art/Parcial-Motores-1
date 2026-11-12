using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private MusicaAdaptativa musica;
    [SerializeField] private float speed = 5f;
    protected private Transform player;
    private float distance;
    [SerializeField] private LayerMask obstacle;

    protected virtual void Start()
    {
        player = null;

        if (musica != null)
        {
            musica.EnemigoAparecio();
        }
    }

    protected virtual void Update()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }

        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.position);

            if (distance < 30)
            {
                if (musica != null)
                {
                    musica.pelea.TransitionTo(0.4f);
                }

                Vector3 direction = player.position - transform.position;
                direction.y = 0;
                Vector3 rayDirection = direction.normalized;

                if (Physics.Raycast(transform.position, rayDirection, distance, obstacle) == false)
                {
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 180f;
                    transform.rotation = Quaternion.Euler(0, angle, 0);

                    transform.position += direction.normalized * speed * Time.deltaTime;
                }
                else
                {
                    if (musica != null)
                    {
                        musica.exploracion.TransitionTo(3.0f);
                    }
                }
            }
            else
            {
                if (musica != null)
                {
                    musica.exploracion.TransitionTo(3.0f);
                }
            }
        }
    }
}
