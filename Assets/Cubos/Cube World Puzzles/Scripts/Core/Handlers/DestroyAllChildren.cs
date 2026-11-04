namespace HashGame.CubeWorld {
    using HashGame.CubeWorld.Extensions;
    using UnityEngine;

public class DestroyAllChildren : MonoBehaviour
{
        public void destroyAllChildren()
        {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                BasicExtensions.DestroyObject(transform.GetChild(i).gameObject);
            }
        }
    }
}