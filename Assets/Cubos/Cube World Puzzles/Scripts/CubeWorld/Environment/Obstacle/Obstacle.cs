using HashGame.CubeWorld.HeroManager;
using UnityEngine;
using UnityEngine.Events;
using HashGame.CubeWorld.Extensions;
using System.Collections.Generic;
using System;
namespace HashGame.CubeWorld.OptimizedCube
{
    #region enum
    public enum ObstacleType : int
    {
        Default = 0,
        Undercover,
        Slide,
        Checkpoint,
        Portal,
        PortalDestination,
        Destructible,
        AutoWalk,
        AutoWalkDestination,
        Spawner,
        SpawnerButton,
        DoorLever,
        GoalHome
    }
    public static class ObstacleTypeExtentions
    {
        public static bool IsIntegrationable(this ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.Default: return true;
                default: return false;
            }
        }
        public static bool CanDisableFace(this ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.Portal:
                case ObstacleType.AutoWalk:
                case ObstacleType.Spawner:
                case ObstacleType.GoalHome:
                    return false;
                default:
                    return true;
            }
        }
        public static bool IsClimbable(this ObstacleType type)
        {
            switch (type)
            {
                case ObstacleType.Portal:
                case ObstacleType.Slide:
                case ObstacleType.PortalDestination:
                case ObstacleType.Spawner:
                    return false;
                default:
                    return true;
            }
        }
        public static bool CanCreateContextMenu(this ObstacleType type)
        {
            return true;
        }
    }
    #endregion
    public class Obstacle : DynamicCubeFaceRendering
    {
        #region variables
        public const string NAME = "Obstacle";
        public ObstacleType obstacleType = ObstacleType.Default;
        #region hide
        [HideInInspector] public ObstacleTypesBase obstacleTypeClass;
        #endregion
        #region events
        public UnityEvent OnObjectIsStandingOnMeEvent = null;
        #endregion
        [HideInInspector] public TerrainManager _terrainManager;
        [HideInInspector] public bool IsOptimizable = true;
        [HideInInspector] public bool _isClimbable = true;
        public bool IsClimbable { get { return _isClimbable & obstacleType.IsClimbable(); } set { _isClimbable = value; } }
        public bool fixPositionToGroundOnStart;
        public TerrainManager terrainManager
        {
            get
            {
                if (_terrainManager == null)
                {
                    _terrainManager = GameObject.FindAnyObjectByType<TerrainManager>();
                }
                return _terrainManager;
            }
            set { _terrainManager = value; }
        }
        #endregion
        #region Functions
        private void Awake()
        {
            if (terrainManager == null)
            {
                terrainManager = FindFirstObjectByType<TerrainManager>();
                if (terrainManager != null) terrainManager.Cubes_Check(this.gameObject);
            }
            obstacleTypeInit();
            if (fixPositionToGroundOnStart)
            {
                neighborData.FixPositionFromSide(CubeFace.Down);
            }
            if (terrainManager && terrainManager.localSettings.limitedVisibility)
            {
                DisplayeAllFaces(false);
            }
        }
        private void OnDestroy()
        {
            if (terrainManager != null)
            {
                terrainManager.OnDestroy_Cube(this.gameObject);
            }
        }
        #endregion
        #region terrainManager
        public void setTerrainManager(TerrainManager t)
        {
            t = terrainManager;
        }
        #endregion
        #region events
        public void ObjectIsStanding(GameObject obj)
        {
            if (obj == null) return;
            obstacleType_ObjectIsStanding(obj);
            if (obj.GetComponent<HeroController>() != null)
            {
                if (OnObjectIsStandingOnMeEvent != null) OnObjectIsStandingOnMeEvent.Invoke();
            }
        }
        #endregion
        #region ObstacleType
        #region ChangeObstacleType
        public void ObstacleTypeChangingFunctions(ObstacleType type)
        {
            ChangeObstacleType(type, true);
        }
        public void ChangeObstacleType(ObstacleType type, bool force = false)
        {
            if (type == obstacleType && !force) return;
            if (obstacleTypeClass) obstacleTypeClass.OnDestroyClass();
            remove_checkPoint();
            remove_GoalHome();
            BasicExtensions.DestroyObject(obstacleTypeClass);
            gameObject.RemoveClass<Undercover>();
            gameObject.RemoveClass<GoalHome>();
            gameObject.RemoveClass<Portal>();
            gameObject.RemoveClass<DestructibleObstacle>();
            gameObject.RemoveClass<PortalDestination>();
            gameObject.RemoveClass<AutoWalk>();
            gameObject.RemoveClass<AutoWalkDestination>();
            gameObject.RemoveClass<Spawner>();
            gameObject.RemoveClass<SpawnerButton>();
            gameObject.RemoveClass<DoorLever>();
            obstacleType = type;
            // TODO: alot
            if (terrainManager == null) return;
            switch (obstacleType)
            {
                case ObstacleType.Undercover:
                    create_ObstacleTypeClass<Undercover>();
                    break;
                case ObstacleType.GoalHome:
                    obstacleTypeClass = create_ObstacleTypeClass<GoalHome>();
                    terrainManager.GoalHome_Add((GoalHome)obstacleTypeClass);
                    break;
                case ObstacleType.Checkpoint:
                    terrainManager.checkPointCubes_Add(create_ObstacleTypeClass<CheckPoint>());
                    break;
                case ObstacleType.Portal:
                    create_ObstacleTypeClass<Portal>();
                    CheckSeatRequirement();
                    break;
                case ObstacleType.Destructible:
                    create_ObstacleTypeClass<DestructibleObstacle>();
                    break;
                case ObstacleType.PortalDestination:
                    create_ObstacleTypeClass<PortalDestination>();
                    CheckSeatRequirement();
                    break;
                case ObstacleType.AutoWalk:
                    create_ObstacleTypeClass<AutoWalk>();
                    break;
                case ObstacleType.AutoWalkDestination:
                    create_ObstacleTypeClass<AutoWalkDestination>();
                    break;
                case ObstacleType.Spawner:
                    create_ObstacleTypeClass<Spawner>();
                    break;
                case ObstacleType.SpawnerButton:
                    create_ObstacleTypeClass<SpawnerButton>();
                    break;
                case ObstacleType.DoorLever:
                    create_ObstacleTypeClass<DoorLever>();
                    break;
                default:
                    break;
            }
            obstacleTypeClass = gameObject.GetComponent<ObstacleTypesBase>();
            if (!obstacleType.CanDisableFace()) { DisplayeAllFaces(true); }
            gameObject.name = obstacleType.ToString() + " " + (int)UnityEngine.Random.Range(0, int.MaxValue);
        }
        private void CheckSeatRequirement()
        {
            if (terrainManager == null) return;
            if (neighborData.HasNeighbor(CubeFace.Down)) return;
            var seat = terrainManager.CreateCube(this.gameObject, CubeFace.Down);
            seat.transform.position = this.Position;
            this.transform.position = Position + (Vector3.up * RealSize.y);
        }
        private T create_ObstacleTypeClass<T>() where T : MonoBehaviour
        {
            T node = gameObject.GetComponent<T>();
            if (node == null) node = gameObject.AddComponent<T>();
            return node;
        }
        private void remove_checkPoint()
        {
            CheckPoint checkPoint = GetComponent<CheckPoint>();
            if (checkPoint == null) return;
            terrainManager.checkPointCubes_Remove(checkPoint);
            BasicExtensions.DestroyObject(checkPoint);
        }
        private void remove_GoalHome()
        {
            GoalHome g = GetComponent<GoalHome>();
            if (g == null) return;
            terrainManager.GoalHome_Remove(g);
            BasicExtensions.DestroyObject(g);
        }
        #endregion
        private void obstacleType_ObjectIsStanding(GameObject obj)
        {
            if (obstacleTypeClass != null)
            {
                obstacleTypeClass.ObjectIsStanding(obj);
                return;
            }
            if (terrainManager == null) return;
            terrainManager.PlaySFX(obstacleType);
        }
        private void obstacleTypeInit()
        {
            obstacleTypeClass = gameObject.GetComponent<ObstacleTypesBase>();
            //if ((obstacleTypeClass = gameObject.GetComponent<ObstacleTypesBase>()) == null) obstacleTypeClass = gameObject.AddComponent<ObstacleTypesBase>();
        }
        #endregion
        #region RenderDistanceController
        public void TryDisplayeAllFaces(bool value)
        {
            if (!obstacleType.CanDisableFace()) return;
            if (obstacleTypeClass)
            {
                if (obstacleTypeClass.GetType() == typeof(Undercover))
                {
                    Undercover undercover = GetComponent<Undercover>();
                    if (undercover != null)
                    {
                        if (undercover.currentState == UndercoverStates.Stealth) return;
                    }
                }
            }
            DisplayeAllFaces(value);
        }
        #endregion
        #region Integration
        public void IntegrationDownUp() => Integration(Vector3.up);
        public void IntegrationLeftRight() => Integration(Vector3.right);
        public void IntegrationBackForward() => Integration(Vector3.forward);
        private void Integration(Vector3 direction)
        {
            if (!this.obstacleType.IsIntegrationable()) return;
            const bool updateCenter = true;
            Obstacle pointer = this;
            Obstacle lead = this;
            Obstacle temp;
            Vector3 leadDirection = direction * -1;
            List<Obstacle> nodes = new List<Obstacle>();
            while (true)
            {
                temp = (Obstacle)lead.neighborData.GetNeighbor<Obstacle>(leadDirection);
                if (temp == null || !temp.IsOptimizable || !temp.obstacleType.IsIntegrationable()) break;
                nodes.Add(lead);
                lead = temp;
            }
            while (true)
            {
                temp = (Obstacle)pointer.neighborData.GetNeighbor<Obstacle>(direction);
                if (temp == null || !temp.IsOptimizable || !temp.obstacleType.IsIntegrationable()) break;
                nodes.Add(pointer);
                pointer = temp;
            }
            nodes.Add(pointer);
            //if (nodes.Count > 0) nodes.RemoveAt(nodes.Count - 1);
            if (pointer == lead) return;
            nodes.Remove(lead);
            float X = Vector3.Distance(pointer.Position, lead.Position);

            if (updateCenter)
            {
                lead.transform.position = (lead.Position + pointer.Position) / 2;
            }
            if (direction == Vector3.up)
            {
                lead.UpdateMesh(ScaledVertices_Up(X), updateCenter);
            }
            else if (direction == Vector3.right)
            {
                lead.UpdateMesh(ScaledVertices_Right(X), updateCenter);
            }
            else if (direction == Vector3.forward)
            {
                lead.UpdateMesh(ScaledVertices_Forward(X), updateCenter);
            }
            else return;

            lead.IsOptimizable = (nodes.Count == 0);
            if (direction != Vector3.up)
            {
                lead.IsClimbable = nodes.Count == 0;
            }
            lead.DisplayeAllFaces(true);
            if (terrainManager != null && terrainManager.terrainLayout != null && terrainManager.terrainLayout.IntegrationObstacle != null)
            {
                lead.SetMaterial(terrainManager.terrainLayout.IntegrationObstacle);
            }
            foreach (var node in nodes)
            {
                if (node == null) continue;
                if (terrainManager != null)
                {
                    terrainManager.DeleteCube(node.gameObject);
                }
                else
                {
                    BasicExtensions.DestroyObject(node.gameObject);
                }
            }
        }
        #endregion
        #region optimaization

        public void TryOptimizing()
        {
            if (!IsOptimizable) return;
            if (!obstacleType.CanDisableFace())
            {
                DisplayeAllFaces(true);
                return;
            }
            bool canDestroy = true;
            int n = Enum.GetValues(typeof(CubeFace)).Length;
            for (int i = 0; i < n; i++)
            {
                if (neighborData.CheckNeighbor((CubeFace)i, false))
                {
                    if (i != (int)CubeFace.Down)
                        canDestroy &= true;
                    continue;
                }
                canDestroy = false;
            }
            if (canDestroy)
            {
                BasicExtensions.DestroyObject(this.gameObject);
            }
            else
            {
                UpdateIncludeFaces();
            }
        }
        #endregion
    }
}