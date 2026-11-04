#if UNITY_EDITOR
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class EditorEventHandler_Idle : IEditorEventHandlerBase
    {
        public EditorEventHandler_Idle(TerrainManagerEditor controller) : base(EditorEventHandlerStates.Idle, controller) { }
        public override void InputHandling(Event e) => ChangeState(e);
        public override void OnFinish() { }
        public override void OnStart() { }
    }
}
#endif
