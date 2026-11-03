#if UNITY_EDITOR
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public abstract class IEditorEventHandlerBase : IEditorEventHandler
    {
        public IEditorEventHandlerBase(EditorEventHandlerStates state, TerrainManagerEditor controller)
        {
            this.Controller = controller;
            this.State = state;
        }
        #region variable
        public EditorEventHandlerStates State { get; }
        public TerrainManagerEditor Controller { get; }
        public TerrainManagerEditor.TerrainManagerBuffer buffer => TerrainManagerEditor.buffer;
        public TerrainManager terrainManager => Controller.Target;
        #endregion
        #region abstrack
        //public abstract void Draw();
        public abstract void OnStart();
        public abstract void OnFinish();
        public abstract void InputHandling(Event e);
        #endregion
        public void ChangeState(Event e)
        {
            if (isRightClick(e))
            {
                ChangeState(EditorEventHandlerStates.MouseRightClick);
                return;
            }
            if (isMouseLeftClick(e))
            {
                if (e.modifiers == EventModifiers.Control)
                {
                    ChangeState(EditorEventHandlerStates.MouseLeftClickPlusControl);
                    return;
                }
                ChangeState(EditorEventHandlerStates.MouseLeftClick);
                return;
            }
            if (isMoseMove(e))
            {
                ChangeState(EditorEventHandlerStates.MouseMove);
                return;
            }
            ChangeState(EditorEventHandlerStates.Idle);
        }
        public void ChangeState(EditorEventHandlerStates state, bool force = false) => Controller.ChangeState(state, force);
        public GameObject GetMouseOverAnTerrainObject(Event e)
        {
            if (terrainManager == null)
            {
                //Debug.LogError("terrainManager is null");
                return null;
            }
            var obj = SceneGUIHandler.MouseIsOverAnObject(e);
            if (obj == null || obj.transform.parent == terrainManager.transform)
            {
                return obj;
            }
            return null;
        }
        public bool isMoseMove(Event e) => SceneGUIHandler.isMoseMove(e);
        public bool isRightClick(Event e) => SceneGUIHandler.isMouseRightClick(e);
        public bool isMouseLeftClick(Event e) => SceneGUIHandler.isMouseLeftClick(e);
        public bool isMouseLeftClickAndDrag(Event e) => SceneGUIHandler.isMouseLeftClickAndDrag(e);
        public Vector3 getMousePosition(Event e) => SceneGUIHandler.getMousePosition(e);
    }
}
#endif