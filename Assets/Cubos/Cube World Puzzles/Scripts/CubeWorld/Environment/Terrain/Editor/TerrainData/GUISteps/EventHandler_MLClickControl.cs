#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class EditorEventHandler_MouseLeftClickPlusControl : IEditorEventHandlerBase
    {
        public EditorEventHandler_MouseLeftClickPlusControl(TerrainManagerEditor controller) : base(EditorEventHandlerStates.MouseLeftClickPlusControl, controller) { }

        public override void InputHandling(Event e)
        {
            ChangeState(e);
        }

        public override void OnFinish()
        {
        }

        public override void OnStart()
        {
            if (buffer.MouseOverObject.obstacle == null)
            {
                return;
            }
            Obstacle mouseOverObstacle = buffer.MouseOverObject.obstacle;
            if (mouseOverObstacle == null) return;
            if (buffer.SelectedObjects_HasNode(mouseOverObstacle.gameObject))
            {
                buffer.SelectedObjects_Remove(mouseOverObstacle.gameObject);
            }
            else
            {
                buffer.SelectedObjects_Add(mouseOverObstacle.gameObject);
            }
        }
    }
}
#endif