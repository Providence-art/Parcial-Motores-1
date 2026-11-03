namespace HashGame.CubeWorld.OptimizedCube
{
    public enum AutoWalkStates : int
    {
        Init = 0,
        Idle,
        Move,
        Unloading
    }
    public static class AutoWalkStatesExtentions
    {
        public static int ToIndex(this AutoWalkStates state) => (int)state;
    }
    public interface IAutoWalk
    {
        public AutoWalkStates State { get; }
        public AutoWalk Controller { get; }
    }
}