using System;
using UnityEngine;
//Este script se encarga de manejar todo lo que sea relacionado con el personaje principal. Por ahora, solo la vida; agreguen más funciones más tarde
public class HilarioScript : MonoBehaviour
{
    [SerializeField] private int health = 3;
    [SerializeField] private int maxHealth = 3;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }   
    public void Heal(int healing)
    {
        if (health + healing <= maxHealth)
        {
            health += healing;
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        health += amount;
    }



    // Update is called once per frame
    void Update()
    {
        
    }





    private void Die()
    {
        Destroy(gameObject);
    }
}
