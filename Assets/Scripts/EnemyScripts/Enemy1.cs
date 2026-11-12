using UnityEngine;

public class EnemyStats : EnemyMovement
{
    [SerializeField] private int damage = 1;

    [Header("Animación")]
    [SerializeField] private Animator animator;

    protected override void Start()
    {
        base.Start();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        ActualizarAnimaciones();
    }

    private void ActualizarAnimaciones()
    {
        if (animator == null) return;

        bool estaMoviendo = (player != null && Vector3.Distance(transform.position, player.position) < 30f);

        animator.SetBool("Caminando", estaMoviendo);
    }

    private void OnTriggerEnter(Collider other)
    {
        HilarioScript hilarioScript = other.GetComponent<HilarioScript>();

        if (hilarioScript != null)
        {
            hilarioScript.TakeDamage(damage);
            DispararAtaque();
            return;
        }

        VidaHilario vidaHilario = other.GetComponent<VidaHilario>();

        if (vidaHilario != null)
        {
            vidaHilario.RecibirDano(damage);
            DispararAtaque();
        }
    }

    private void DispararAtaque()
    {
        if (animator != null)
        {
            animator.SetTrigger("Atacar");
        }
    }
}