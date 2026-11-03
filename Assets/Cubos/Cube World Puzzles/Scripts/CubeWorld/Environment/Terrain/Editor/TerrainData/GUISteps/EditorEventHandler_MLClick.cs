#if UNITY_EDITOR
using HashGame.CubeWorld.OptimizedCube;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class EditorEventHandler_MouseLeftClick : IEditorEventHandlerBase
    {
        public EditorEventHandler_MouseLeftClick(TerrainManagerEditor controller) : base(EditorEventHandlerStates.MouseLeftClick, controller) { }

        public override void InputHandling(Event e)
        {
            var obj = GetMouseOverAnTerrainObject(e);
            if (buffer.SelectedObjects_HasNode(obj))
            {
                buffer.SelectedObjects_Remove(obj);
            }
            else if (buffer.MouseOverObject.hasData)
            {
                if (obj == buffer.MouseOverObject.gameObject)
                {
                    buffer.SelectedObjects_Add(obj);
                }
            }
            ChangeState(e);
        }

        public override void OnFinish()
        {
        }

        public override void OnStart()
        {
            buffer.canDrawSelectedObstacleToMouseDragPosition = !buffer.canDrawSelectedObstacleToMouseDragPosition;
            if (buffer.SelectedObjects_Count() == 1)
            {
                if (buffer.mouseDragToCreateObstacle_IsValid())
                {
                    Obstacle obstacle;
                    if (buffer.SelectedObjects[0] != null && (obstacle = buffer.SelectedObjects[0].GetComponent<Obstacle>()) != null)
                    {
                        Controller.CreateObstacle(obstacle, buffer.mouseDragToCreateObstacle.Face, buffer.mouseDragToCreateObstacle.Count);
                        buffer.mouseDragToCreateObstacle_Clear(); buffer.canDrawSelectedObstacleToMouseDragPosition = false;
                    }
                }
            }
            buffer.SelectedObjects_Clear();
        }
    }
}
#endif
