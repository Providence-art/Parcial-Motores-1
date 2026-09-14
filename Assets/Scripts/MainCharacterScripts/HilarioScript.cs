using System;
using UnityEngine;
//Este script se encarga de manejar todo lo que sea relacionado con el personaje principal. Por ahora, solo la vida; agreguen más funciones más tarde
public class HilarioScript : MonoBehaviour
{
    private int health = 3;


    public void takeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
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
