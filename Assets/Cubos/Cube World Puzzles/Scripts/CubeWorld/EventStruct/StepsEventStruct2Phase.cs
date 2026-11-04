using UnityEngine.Events;
namespace HashGame.CubeWorld
{
    [System.Serializable]
    public class StepsEventStruct2Phase : StepsEventStructBase
    {
        public UnityEvent onPhaseStart;
        public UnityEvent onPhaseEnd;
        public void onPhaseStart_Invoke() => invoke(ref onPhaseStart);
        public void onPhaseEnd_Invoke() => invoke(ref onPhaseEnd);
    }
}
