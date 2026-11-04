namespace HashGame.CubeWorld.OptimizedCube
{
    public enum DestructibleStates : int
    {
        Idle = 0,
        Destroy,
    }
    public interface IDestructibleObstacle 
    {
        public DestructibleStates State { get; }
        public DestructibleObstacle Controller { get; }
    }
}
