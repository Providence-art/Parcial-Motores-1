using System;
using UnityEngine;
//Este script se encarga de manejar todo lo que sea relacionado con el personaje principal. Por ahora, solo la vida; agreguen más funciones más tarde
public class HilarioScript : MonoBehaviour
{
    [SerializeField] private int health = 6;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private SfxVariado sfxDaño;
    [SerializeField] private SfxVariado sfxMuerte;

    public void TakeDamage(int damage)
    {
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
        if (sfxMuerte != null)
        {
            GameObject objetoAudio = sfxMuerte.gameObject;

            // Separamos el audio del personaje.
            objetoAudio.transform.SetParent(null, true);

            // Reproducimos el sonido.
            sfxMuerte.Reproducir();

            // Lo eliminamos cuando haya terminado.
            Destroy(objetoAudio, 1.5f);
        }

        // Destruimos a Hilario.
        Destroy(gameObject);
    }
}

