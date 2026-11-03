#if UNITY_EDITOR
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class EditorEventHandler_MouseMove : IEditorEventHandlerBase
    {
        public EditorEventHandler_MouseMove(TerrainManagerEditor controller) : base(EditorEventHandlerStates.MouseMove, controller) { }

        public override void InputHandling(Event e)
        {
            buffer.SetMouseOverObject(GetMouseOverAnTerrainObject(e));
            ChangeState(e);
        }

        public override void OnFinish()
        {
        }

        public override void OnStart()
        {
        }
    }
}
#endif
