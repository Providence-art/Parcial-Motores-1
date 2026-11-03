namespace HashGame.CubeWorld.OptimizedCube
{
    public enum PortalStates : int
    {
        Idle = 0,
        Active,
        Teleport
    }
    public static class IPortalStatesExtentions
    {
        public static int ToIndex(this PortalStates state) => (int)state;
    }
    public interface IPortal 
    {
        public PortalStates States { get; }
        public Portal Controller { get; }
    }
}