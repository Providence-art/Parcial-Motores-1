using UnityEngine;

public class FountainScript : Interactable
{
   [SerializeField] private int healAmount = 1;
    public override void Actionate(HilarioScript player)
    {
        player.Heal(healAmount);             
    }
}
