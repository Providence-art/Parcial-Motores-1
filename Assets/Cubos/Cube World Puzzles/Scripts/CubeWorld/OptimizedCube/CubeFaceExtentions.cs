using UnityEngine;
namespace HashGame.CubeWorld.OptimizedCube
{
    public enum CubeFace : int
    {
        Forward = 0,
        Right,
        Back,
        Left,
        Up,
        Down
    }

    public static class CubeFaceExtentions
    {
        public static bool CanCreateSuggestionCubeFrame(this CubeFace face)
        {
            switch (face)
            {
                case CubeFace.Up:
                case CubeFace.Down:
                    return false;
                default: return true;
            }
        }
        public static CubeFace ToOpposite(this CubeFace face)
        {
            switch (face)
            {
                case CubeFace.Forward: return CubeFace.Back;
                case CubeFace.Right: return CubeFace.Left;
                case CubeFace.Back: return CubeFace.Forward;
                case CubeFace.Left: return CubeFace.Right;
                case CubeFace.Up: return CubeFace.Down;
                case CubeFace.Down: return CubeFace.Up;
            }
            return CubeFace.Down;
        }
        public static Vector3 ToVector3(this CubeFace face)
        {
            switch (face)
            {
                case CubeFace.Up: return Vector3.up;
                case CubeFace.Down: return Vector3.down;
                case CubeFace.Left: return Vector3.left;
                case CubeFace.Right: return Vector3.right;
                case CubeFace.Forward: return Vector3.forward;
                case CubeFace.Back: return Vector3.back;
                default: return Vector3.zero;
            }
        }
    }
}