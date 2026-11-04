#if UNITY_EDITOR
namespace HashGame.CubeWorld.EditorTools
{
    public enum EditorEventHandlerStates
    {
        Idle,
        MouseMove,
        MouseRightClick,
        MouseLeftClick,
        MouseLeftClickPlusControl,
    }
    public interface IEditorEventHandler
    {
        public EditorEventHandlerStates State { get; }
        public TerrainManagerEditor Controller { get; }
    }
}
#endif