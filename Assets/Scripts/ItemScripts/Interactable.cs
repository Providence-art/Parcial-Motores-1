using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Interactable : MonoBehaviour
{
    private bool isActive = true;
   public void OnTriggerStay(Collider other)
    {
        HilarioScript Player = other.GetComponent<HilarioScript>();
        

        if (Keyboard.current.eKey.isPressed && Player != null && isActive == true)
        {
            Actionate(Player); 
            isActive = false;
        } 

    }
    
      public virtual void Actionate(HilarioScript player)
    {
        return;
    }



}
