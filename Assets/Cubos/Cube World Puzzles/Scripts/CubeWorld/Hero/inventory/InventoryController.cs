using System.Collections.Generic;
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.TerrainConstruction;
using UnityEngine;
namespace HashGame.CubeWorld.HeroManager
{
    public class InventoryController : MonoBehaviour
    {
        #region variable
        [HideInInspector] public List<GameObject> inventory = new List<GameObject>();
        [HideInInspector] public HeroController controller;
        #endregion
        #region function
        public void AddInventory(GameObject node)
        {
            if (node == null) return;
            if (node.GetComponent<CollectableObject>() == null) return;
            if (inventory.Contains(node)) return;
            inventory.Add(node);
            if (controller == null && (controller = GetComponent<HeroController>()) == null) return;
            if (controller.IsAudioClipsEnable) AudioManager.Instance.PlaySFX(controller.AudioClips.Collecting);
        }
        private T FetchInventory<T>()
        {
            T t;
            for (int i = 0; i < inventory.Count; i++)
            {
                if ((t = inventory[i].GetComponent<T>()) != null)
                {
                    inventory.RemoveAt(i);
                    return t;
                }
            }
            return default(T);
        }
        public void MakeTrade(Door customer)
        {
            if (customer == null) return;
            KeyController key;
            bool success = false;
            while (true)
            {
                if (customer.NeedKey())
                {
                    if ((key = FetchInventory<KeyController>()) != null)
                    {
                        key.TransferToTheCustomer(customer);
                        customer.KeyIsOnWay();
                        success = true;
                        continue;
                    }
                }
                break;
            }
            if (success && controller.IsAudioClipsEnable) AudioManager.Instance.PlaySFX(controller.AudioClips.Trading);
        }
        #endregion
    }
}