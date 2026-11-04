using UnityEngine.Events;
namespace HashGame.CubeWorld
{
    [System.Serializable]
    public class StepsEventStructBase
    {
        protected void invoke(UnityEvent e) => invoke(ref e);
        protected void invoke(ref UnityEvent e)
        {
            if (e == null) return;
            e.Invoke();
        }
    }
}