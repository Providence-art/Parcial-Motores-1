using System;
using UnityEngine;
//Este script se encarga de manejar todo lo que sea relacionado con el personaje principal. Por ahora, solo la vida; agreguen más funciones más tarde
public class HilarioScript : MonoBehaviour
{
    [SerializeField] private int health = 6;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private SfxVariado sfxDaño;
    [SerializeField] private SfxVariado sfxMuerte;
    private CharacterMovement movement;


    void Awake()
    {
        movement = GetComponent<CharacterMovement>();
    }
    public void TakeDamage(int damage)
    {
        if (movement.IsDashing == true)
        {
            return;
        }
        if (health <= 0) return;

        health -= damage;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            if (sfxDaño != null)
            {
                sfxDaño.Reproducir();
            }
        }
    }

    public bool JustParried()
    {
        if (movement.IsDashing == true)
        {
            return true;
        }
        else
        {
            return false;
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

    public void Die()
    {
        if (sfxMuerte != null)
        {
            GameObject objetoAudio = sfxMuerte.gameObject;

            objetoAudio.transform.SetParent(null, true);

            
            sfxMuerte.Reproducir();

            Destroy(objetoAudio, 1.5f);
        }


        Destroy(gameObject);
    }
}

