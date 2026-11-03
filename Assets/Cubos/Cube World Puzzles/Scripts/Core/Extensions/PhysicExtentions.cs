namespace HashGame.CubeWorld.Extensions
{
    using UnityEngine;

    public static class PhysicExtentions
    {
        public static Ray ScreenPointToRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        public static bool Physics2D_IsHit(Collider2D node)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Infinity);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, mousePosition - Camera.main.ScreenToWorldPoint(mousePosition), Mathf.Infinity);
            return hit && node == hit.collider;
        }
        public static bool Physics_IsHit(Collider node, out RaycastHit hit, float maxDistance = Mathf.Infinity)
        {
            return node.Raycast(ScreenPointToRay(), out hit, maxDistance);
        }
        public static bool Physics_IsHit(out RaycastHit hit, LayerMask mask, float maxDistance = Mathf.Infinity)
        {
            return Physics.Raycast(ScreenPointToRay(), out hit, maxDistance, mask);
        }
    }
}