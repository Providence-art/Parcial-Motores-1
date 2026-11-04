#if UNITY_EDITOR
using HashGame.CubeWorld.Informer;
using System;
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.TerrainConstruction;
using UnityEditor;
using UnityEngine;
using HashGame.CubeWorld.HeroManager;
using NUnit.Framework;
using System.Collections.Generic;

namespace HashGame.CubeWorld.EditorTools
{

    public class EditorEventHandler_MouseRightClick : IEditorEventHandlerBase
    {
        public EditorEventHandler_MouseRightClick(TerrainManagerEditor controller) : base(EditorEventHandlerStates.MouseRightClick, controller) { }
        private bool openObstacleMenu;
        private bool openDoorMenu;
        public override void InputHandling(Event e)
        {
            if (openObstacleMenu) contextMenu_Obstacle();
            if (openDoorMenu) contextMenu_Door();
            ChangeState(e);
        }

        public override void OnFinish()
        {
        }

        public override void OnStart()
        {
            buffer.canDrawSelectedObstacleToMouseDragPosition = false;
            if (openObstacleMenu = checkObstacleMenu()) return;
            if (openDoorMenu = checkDoorMenu()) return;
            // check if there is no menu = buffer clear
            if (!openObstacleMenu && !openDoorMenu)
            {
                buffer.Clear();
            }
        }
        private bool checkDoorMenu()
        {
            if (buffer.MouseOverObject.door == null)
            {
                return false;
            }
            Door mouseOverDoor = buffer.MouseOverObject.door;
            if (mouseOverDoor == null) return false;
            if (buffer.SelectedObjects_HasNode(buffer.MouseOverObject.gameObject))
            {
                return true;
            }
            else
            {
                buffer.SelectedObjects_Clear();
                buffer.SelectedObjects_Add(mouseOverDoor.gameObject);
                return true;
            }
        }
        private bool checkObstacleMenu()
        {
            // there is no terrain object
            if (buffer.MouseOverObject.obstacle == null)
            {
                return false;
            }
            //check mouseOverObstacle equal to any selected objects
            Obstacle mouseOverObstacle = buffer.MouseOverObject.obstacle;
            if (mouseOverObstacle == null) return false;
            if (buffer.SelectedObjects_HasNode(buffer.MouseOverObject.gameObject))
            {
                return true;
            }
            else
            {
                buffer.SelectedObjects_Clear();
                buffer.SelectedObjects_Add(mouseOverObstacle.gameObject);
                return true;
            }
        }
        #region contextMenu_Door()
        public void contextMenu_Door()
        {
            if (buffer.MouseOverObject.door == null || buffer.lastSelectedObjectType != typeof(Door)) return;
            Door door = buffer.MouseOverObject.door;
            int count = 1;
            if (door == null)
            {
                count = buffer.SelectedObjects_Count();
                if (count == 1)
                {
                    door = buffer.SelectedObjects[0].GetComponent<Door>();
                }
            }
            GenericMenu menu = ContextMenuEditorTools.genericMenu();
            ContextMenuEditorTools.AddItem(ref menu, "Select game object (Hierarchy)", () =>
            { Selection.objects = new UnityEngine.Object[] { door.gameObject }; });
            if (count == 1)
            {
                ContextMenuEditorTools.SeparatorItem(ref menu);
                ContextMenuEditorTools.AddItem(ref menu, "Options",
                    () => { DoorEditorWindow.OpenWindow(buffer.MouseOverObject.door); });
            }
            ContextMenuEditorTools.AddItem(ref menu, "Delete " + Door.NAME + "/Yes I`m sure",
                () => { Controller.OnDestroy_Buffer_SelectedObject(); });
            ContextMenuEditorTools.SeparatorItem(ref menu);
            ContextMenuEditorTools.AddItem(ref menu, "Close", () => { buffer.Clear(); });
            ContextMenuEditorTools.OpenMenuInMousePosition(ref menu);
            ContextMenuEditorTools.UseEventAtTheEnd();
        }
        #endregion
        #region contextMenu_Obstacle
        public void contextMenu_Obstacle()
        {
            if (!buffer.SelectedObjects_HasData())
            {
                if (buffer.MouseOverObject.obstacle == null) return;
            }
            Obstacle obstacle = buffer.MouseOverObject.obstacle;
            int obstacleCount = buffer.SelectedObjects_Count();
            GenericMenu menu = ContextMenuEditorTools.genericMenu();
            //
            if (obstacleCount == 1)
            {
                ContextMenu_AddNeighbor(ref menu, buffer.SelectedObjects[0].GetComponent<Obstacle>());
            }
            else
            {
                ContextMenu_AddNeighbor(ref menu);
            }
            ContextMenu_Construction(ref menu);
            //
            ContextMenuEditorTools.SeparatorItem(ref menu);
            ContextMenu_ChangeType(ref menu);
            //
            if (obstacleCount > 1)
            {
                ContextMenuEditorTools.SeparatorItem(ref menu);
                ContextMenu_Convert(ref menu);
            }
            //
            ContextMenuEditorTools.SeparatorItem(ref menu);
            ContextMenu_optimization(ref menu, obstacle);
            if (obstacleCount == 1)
            {
                ContextMenuEditorTools.SeparatorItem(ref menu);
                ContextMenuEditorTools.AddItem(ref menu, "Select game object (Hierarchy)",
                    () => { Selection.objects = new UnityEngine.Object[] { obstacle.gameObject }; });
                ContextMenuEditorTools.SeparatorItem(ref menu);
                ContextMenuEditorTools.AddItem(ref menu, "Options",
                    () => { ObstacleEditorWindow.OpenWindow(obstacle); });
            }
            ContextMenuEditorTools.SeparatorItem(ref menu);
            ContextMenuEditorTools.AddItem(ref menu, "Delete " + Obstacle.NAME + (obstacleCount > 1 ? "(s)" : "") + " --> (Count: " + (obstacleCount.ToString()) + ")/Yes I`m sure",
                () => { Controller.OnDestroy_Buffer_SelectedObject(); });
            ContextMenuEditorTools.SeparatorItem(ref menu);
            ContextMenuEditorTools.AddItem(ref menu, "Close", () => { buffer.Clear(); });
            ContextMenuEditorTools.OpenMenuInMousePosition(ref menu);
            ContextMenuEditorTools.UseEventAtTheEnd();
        }
        #region optimization
        private void ContextMenu_optimization(ref GenericMenu menu, Obstacle func)
        {
            const string str = "Optimization/";
            ContextMenu_FaceRender(ref menu, func);
            if (func.obstacleType.IsIntegrationable())
            {
                ContextMenuEditorTools.AddItem(ref menu, str + "Integration/Down-Up", () => { func.IntegrationDownUp(); });
                //ContextMenuEditorTools.AddItem(ref menu, str + "Integration/Left-Right", () => { func.IntegrationLeftRight(); });
                //ContextMenuEditorTools.AddItem(ref menu, str + "Integration/Back-Forward", () => { func.IntegrationBackForward(); });
            }
            ContextMenuEditorTools.AddItem(ref menu, str + "Optimize all cubes", () => { Controller.optimizeCubes(); });
        }
        private void ContextMenu_FaceRender(ref GenericMenu menu, DynamicCubeFaceRendering func)
        {
            const string str = "Optimization/Face rendering/";
            var cubes = buffer.SelectedObjects.ToArray();
            foreach (var node in Enum.GetValues(typeof(CubeFace)))
            {
                ContextMenuEditorTools.AddItem(ref menu, str + node.ToString() + "/Enable", () => { faceRendering(cubes, (CubeFace)node, true); });
                ContextMenuEditorTools.AddItem(ref menu, str + node.ToString() + "/Disable", () => { faceRendering(cubes, (CubeFace)node, false); });
            }
            ContextMenuEditorTools.SeparatorItem(ref menu, str);
            ContextMenuEditorTools.AddItem(ref menu, str + "Enable all", () => { func.DisplayeAllFaces(true); });
        }
        private void faceRendering(GameObject[] cubes, CubeFace face, bool enable)
        {
            foreach (var cube in cubes)
            {
                if (cube == null) continue;
                Obstacle obstacle = cube.GetComponent<Obstacle>();
                if (obstacle == null) continue;
                obstacle.DisplayFace(face, enable);
            }
        }
        #endregion
        #region Construction
        private void ContextMenu_Construction(ref GenericMenu menu)
        {
            const string str = "Construction/";
            if (buffer.SelectedObjects_Count() == 1 && buffer.lastSelectedObjectType == typeof(Obstacle))
            {
                ContextMenuEditorTools.SeparatorItem(ref menu);
                ContextMenuEditorTools.AddItem(ref menu, str + "Quadrilateral",
                     () => { TerrainLand_QuadrilateralEditorWindow.OpenWindow(Controller.Target.gameObject, buffer.SelectedObjects); });
                ContextMenuEditorTools.AddItem(ref menu, str + "Snake",
                     () =>
                     {
                         TerrainLand_SnakePathEditorWindow.OpenWindow(Controller.Target, buffer.SelectedObjects[0].GetComponent<Obstacle>());
                     });
                ContextMenuEditorTools.AddItem(ref menu, str + "Random walk",
                     () =>
                     {
                         TerrainLand_RandomWalkEditorWindow.OpenWindow(Controller.Target, buffer.SelectedObjects[0].GetComponent<Obstacle>());
                     });
                ContextMenuEditorTools.AddItem(ref menu, str + "Room and hallway",
                     () =>
                     {
                         TerrainLand_DungeonEditorWindow.OpenWindow(Controller.Target, buffer.SelectedObjects[0].GetComponent<Obstacle>());
                     });
                ContextMenuEditorTools.AddItem(ref menu, str + TerrainLand_StairwayEditorWindow.NAME,
                     () =>
                     {
                         TerrainLand_StairwayEditorWindow.OpenWindow(Controller.Target, buffer.SelectedObjects[0].GetComponent<Obstacle>());
                     });

            }
            else if (!buffer.SelectedObjects_HasData())
            {
                ContextMenuEditorTools.SeparatorItem(ref menu);
                ContextMenuEditorTools.AddItem(ref menu, str + "Quadrilateral",
                     () => { TerrainLand_QuadrilateralEditorWindow.OpenWindow(Controller.Target.gameObject, null); });
                ContextMenuEditorTools.AddItem(ref menu, str + "Room and hallway",
                     () =>
                     {
                         TerrainLand_DungeonEditorWindow.OpenWindow(Controller.Target, null);
                     });
            }

        }
        #endregion
        #region ChangeType
        private void ContextMenu_Convert(ref GenericMenu menu)
        {
            const string str = "Convert/";
            ContextMenuEditorTools.AddItem(ref menu, str + "Door", () => { Controller.ConvertToDoor(buffer.SelectedObjects); });
        }
        private void ContextMenu_ChangeType(ref GenericMenu menu)
        {
            if (!buffer.SelectedObjects_HasData()) return;
            const string str = Obstacle.NAME + " type/";
            int obstacleTypeCount = Enum.GetValues(typeof(ObstacleType)).Length;
            int[] obstacleTypes = new int[obstacleTypeCount];
            //
            for (int i = 0; i < buffer.SelectedObjects_Count(); i++)
            {
                Obstacle obstacle;
                if (buffer.SelectedObjects[i] == null || (obstacle = buffer.SelectedObjects[i].GetComponent<Obstacle>()) == null) continue;
                obstacleTypes[(int)obstacle.obstacleType]++;
            }
            //
            int index = 0;
            for (int i = 0; i < obstacleTypeCount; i++)
            {
                ObstacleType obstacleType = (ObstacleType)i;
                if (!obstacleType.CanCreateContextMenu()) continue;
                string attacher = obstacleType == ObstacleType.GoalHome ? " *** " : "";
                ContextMenuEditorTools.AddItem(ref menu,
                    (str + (++index).ToString() + ". "
                    + attacher
                    + obstacleType.ToString()
                    + attacher
                    + (obstacleTypes[i] == 0 ? "" : " --> (Count: " + obstacleTypes[i].ToString() + ")")),
                    () => { Controller.changeObstacleType(obstacleType, buffer.SelectedObjects.ToArray()); });
            }
        }
        #endregion
        #region material
        private void ContextMenu_Materials(ref GenericMenu menu, Obstacle obstacle)
        {
            ContextMenuEditorTools.SeparatorItem(ref menu);
            const string str = "Change material/";
            ContextMenuEditorTools.AddItem(ref menu, str + "Unlit Color", () => { changeMaterial(new Material(Shader.Find("Unlit/Color")), obstacle); });
            ContextMenuEditorTools.AddItem(ref menu, str + "Default", () => { changeMaterial(terrainManager.terrainMaterials.Obstacle, obstacle); });
            ContextMenuEditorTools.AddItem(ref menu, str + "Undercover", () => { changeMaterial(terrainManager.terrainMaterials.UndercoverObstacle, obstacle); });
        }
        private void changeMaterial(Material mat, Obstacle func)
        {
            if (func == null) return;
            Undo.RecordObject(func.gameObject, "Change material");
            func.SetMaterial(mat);
        }
        #endregion
        #region AddNeighbor
        private void ContextMenu_AddNeighbor(ref GenericMenu menu)
        {
            if (!buffer.SelectedObjects_HasData()) return;
            const string addStr = "Add/";
            GameObject[] cubes = buffer.SelectedObjects.ToArray();
            foreach (CubeFace face in Enum.GetValues(typeof(CubeFace)))
            {
                ContextMenuEditorTools.AddItem(ref menu, addStr + Obstacle.NAME + "/" + face.ToString(),
                    () => { Controller.CreateObstacle(cubes, (CubeFace)face); });
            }
            List<Obstacle> nodes = new List<Obstacle>();
            for (int i = 0; i < cubes.Length; i++)
            {
                if (cubes[i] == null) continue;
                Obstacle node = cubes[i].GetComponent<Obstacle>();
                if (node == null) continue;
                nodes.Add(node);
            }
            ContextMenuEditorTools.SeparatorItem(ref menu, addStr + Obstacle.NAME + "/");
            ContextMenuEditorTools.AddItem(ref menu, addStr + Obstacle.NAME + "/Custom ...",
                       () => { terrainCubeContextMenu_AddNeighborEditorWindow.OpenWindow(nodes.ToArray(), terrainManager.gameObject); });
            //ContextMenuEditorTools.AddItem(ref menu, addStr + Wall.NAME + "/Up", () => { Controller.CreateWall(obstacle); });
            ContextMenuEditorTools.AddItem(ref menu, addStr + KeyController.NAME, () => { Controller.CreateKey(cubes); });
            ContextMenuEditorTools.AddItem(ref menu, addStr + InformerHandler.NAME, () => { Controller.CreateInformer(cubes); });
        }
        private void ContextMenu_AddNeighbor(ref GenericMenu menu, Obstacle obstacle)
        {
            if (!obstacle.IsOptimizable) return;
            bool hasNeighbor = false;
            bool somethingOnFaceUp = false;
            const string addStr = "Add/";
            foreach (var node in Enum.GetValues(typeof(CubeFace)))
            {
                if (obstacle.neighborData.HasNeighbor((CubeFace)node))
                {
                    if ((CubeFace)node == CubeFace.Up)
                    {
                        somethingOnFaceUp = true;
                    }
                }
                else
                {
                    ContextMenuEditorTools.AddItem(ref menu, addStr + Obstacle.NAME + "/" + node.ToString(),
                        () => { Controller.CreateObstacle(buffer.SelectedObjects.ToArray(), (CubeFace)node); });
                    hasNeighbor = true;
                }

            }
            if (hasNeighbor)
            {
                ContextMenuEditorTools.SeparatorItem(ref menu, addStr + Obstacle.NAME + "/");
                ContextMenuEditorTools.AddItem(ref menu, addStr + Obstacle.NAME + "/Custom ...",
                           () => { terrainCubeContextMenu_AddNeighborEditorWindow.OpenWindow(new Obstacle[] { obstacle }, terrainManager.gameObject); });
            }
            if (!somethingOnFaceUp)
            {
                ContextMenu_AddHeroPrefabs(ref menu, addStr);
                ContextMenuEditorTools.AddItem(ref menu, addStr + KeyController.NAME, () => { Controller.CreateKey(obstacle); });
                ContextMenuEditorTools.AddItem(ref menu, addStr + InformerHandler.NAME, () => { Controller.CreateInformer(obstacle); });
                //ContextMenuEditorTools.AddItem(ref menu, addStr + Portal.NAME, () => { createPortal(selectionData.MouseOverObject); });
            }
        }
        private void ContextMenu_AddHeroPrefabs(ref GenericMenu menu, string contexParentPath)
        {
            if (buffer.SelectedObjects_Count() != 1 || buffer.SelectedObjects[0] == null) return;
            Obstacle obstacle = buffer.SelectedObjects[0].GetComponent<Obstacle>();
            if (obstacle == null) return;
            string path = contexParentPath + "Heros/";
            ContextMenuEditorTools.AddItem(ref menu, path + "Empty Hero", () => { Controller.CreateHero(obstacle); });
            ContextMenuEditorTools.SeparatorItem(ref menu, path);
            //
            LoadHeroPrefabs(ref menu, PrefabManager.HeroPrefabFilePath_Default, path+"Default", obstacle);
            LoadHeroPrefabs(ref menu, PrefabManager.HeroPrefabFilePath_Rubiks, path+"Rubik", obstacle);
            LoadHeroPrefabs(ref menu, PrefabManager.HeroPrefabFilePath_Characters, path+"Character", obstacle);
        }
        private void LoadHeroPrefabs(ref GenericMenu menu, string filePath, string menuName, Obstacle obstacle)
        {
            try
            {
                //Rubiks
                GameObject[] heros = PrefabManager.LoadHeroPrefabs(filePath);
                int index = 0;
                foreach (var hero in heros)
                {
                    if (hero == null) continue;
                    if (hero.GetComponent<HeroController>() == null) continue;
                    ContextMenuEditorTools.AddItem(ref menu, menuName + "/" + (++index).ToString() + ". " + hero.name, () => { Controller.CreateHero(obstacle, hero); });
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        #endregion
        #endregion
    }
}
#endif