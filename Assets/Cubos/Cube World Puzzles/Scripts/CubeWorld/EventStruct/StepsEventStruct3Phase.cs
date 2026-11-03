using UnityEngine.Events;
namespace HashGame.CubeWorld
{
    [System.Serializable]
    public class StepsEventStruct3Phase : StepsEventStruct2Phase
    {
        public UnityEvent onPhaseFrequency;
        public void onPhaseFrequency_Invoke() => invoke(ref onPhaseFrequency);
    }
}