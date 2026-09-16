using UnityEngine;

public class EnemyStats : EnemyMovement
{
    [SerializeField] private int damage = 1; 
    private void OnTriggerEnter(Collider other)
    {
        HilarioScript Player = other.GetComponent<HilarioScript>();

        if (Player != null)
        {
            Player.TakeDamage(damage);
        }
    }
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    
}
