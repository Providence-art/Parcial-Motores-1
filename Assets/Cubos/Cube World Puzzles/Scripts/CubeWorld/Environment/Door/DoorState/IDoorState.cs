namespace HashGame.CubeWorld.OptimizedCube
{
    public enum DoorState
    {
        Idle,
        Opening,
        Closing
    }
    public interface IDoorState
    {
        public DoorState State { get; }
        public Door Controller { get; }
    }
}