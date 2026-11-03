#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.TerrainConstruction;
using Unity.VisualScripting;
using HashGame.CubeWorld.Informer;
using HashGame.CubeWorld.EditorHandler;
using System.Collections.Generic;
using HashGame.CubeWorld.HeroManager;
using HashGame.CubeWorld.Extensions;

namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(TerrainManager))]
    public class TerrainManagerEditor : Editor
    {
        #region variable
        protected TerrainManager _Target;
        public TerrainManager Target
        {
            get
            {
                if (_Target == null)
                {
                    if ((_Target = (TerrainManager)target) == null)
                    {
                        _Target = FindAnyObjectByType<TerrainManager>();
                    }
                }
                return _Target;
            }
        }
        protected static TerrainEditorDataStore terrainEditorDataStore;
        protected static TerrainEditorSettings settings
        {
            get
            {
                if (terrainEditorDataStore == null || terrainEditorDataStore.settings == null)
                    return TerrainEditorSettings.Instance;
                return terrainEditorDataStore.settings;
            }
        }
        private static int editor_TabIndex;
        #endregion
        #region Functions
        static TerrainManagerEditor()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        private void OnEnable()
        {
            terrainEditorDataStore = Target.GetComponent<TerrainEditorDataStore>() ?? Target.AddComponent<TerrainEditorDataStore>();
            initEditorEventHandlersArray();
        }
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Params();
            serializedObject.ApplyModifiedProperties();
        }

        private void Params()
        {
            BasicEditorTools.Tab(ref editor_TabIndex, new BasicEditorTools.StringActionStruct[] {
                new BasicEditorTools.StringActionStruct("Layout",materialsShow),
                new BasicEditorTools.StringActionStruct("Create terrain",createTerrainShow),
                new BasicEditorTools.StringActionStruct("Local settings",localSettingsShow),
                new BasicEditorTools.StringActionStruct("Events",eventsShow),
                new BasicEditorTools.StringActionStruct("Audio",audioSettingsShow),
                new BasicEditorTools.StringActionStruct("Information",informationShow),
            });
            //if (!BasicEditorTools.Foldout(ref Target.parametersEnable, "Parameter(s)")) return;
            //BasicEditorTools.PropertyField(serializedObject, "collisionObjectTags", "Collision object tags");
        }
        #region selectedOptionShow
        #endregion
        private void eventsShow()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Events");
            BasicEditorTools.Box_Close();
        }
        private void audioSettingsShow()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "_audioSettings", TerrainAudioSettings.NAME);
            if (Target._audioSettings)
            {
                BasicEditorTools.ScriptableObject(Target._audioSettings);
            }
            BasicEditorTools.Box_Close();
        }
        private void localSettingsShow()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "localSettings.useLayoutMaterials", "Using layout material(s)");
            if (!Target.localSettings.useLayoutMaterials)
            {
                BasicEditorTools.PropertyField(serializedObject, "localSettings.brushMaterial", "Brush material");
            }
            //BasicEditorTools.Line();
            //BasicEditorTools.PropertyField(serializedObject, "localSettings.limitedVisibility", "Limited visibility");
            //if (Target.localSettings.limitedVisibility)
            //{
            //    BasicEditorTools.PropertyField(serializedObject, "localSettings.visionRadius", "Vision radius");
            //}
            BasicEditorTools.Box_Close();
        }
        private void materialsShow()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "terrainLayout", TerrainLayout.NAME);
            if (Target.terrainLayout != null)
            {
                BasicEditorTools.ScriptableObject(Target.terrainLayout, typeof(TerrainLayoutEditor));
            }
            BasicEditorTools.Line();
            BasicEditorTools.Button("Material Fixing", () => { Target.reCalculate();
                BasicEditorTools.DirtyObject(Target.transform);
            });
            BasicEditorTools.Box_Close();
        }
        #region createTerrain
        private void createTerrainShow()
        {
            BasicEditorTools.Box_Open();
            if (Target.CubesCount == 0)
                BasicEditorTools.Button("Add first obstacle", CreateObstacle);
            BasicEditorTools.Button("Optimize cubes", optimizeCubes);
            //
            BasicEditorTools.Buttons(new BasicEditorTools.StringActionStruct[]
            {
                new BasicEditorTools.StringActionStruct("Quadrilateral",()=>{ TerrainLand_QuadrilateralEditorWindow.OpenWindow(Target.gameObject); }),
                new BasicEditorTools.StringActionStruct("Draw Land",()=>{ TerrainLand_DrawLandEditorWindow.OpenWindow(Target.gameObject); })

            }, 2);
            //
            BasicEditorTools.Line();
            if (Target.CubesCount != 0) BasicEditorTools.Button("Clear Terrain", ClearTerrain);
            BasicEditorTools.Button("----- Cleaning -----", () => { Target.Cleaning(); });
            BasicEditorTools.Box_Close();
        }
        public void optimizeCubes()
        {
            const string name = "Optimize cubes";
            Undo.SetCurrentGroupName(name);
            int group = Undo.GetCurrentGroup();
            foreach (var node in Target.cubesList)
            {
                if (node == null) continue;
                Undo.RecordObject(Target, name);
                var func = node.GetComponent<Obstacle>();
                func.TryOptimizing();
            }
            Undo.CollapseUndoOperations(group);
        }
        #endregion
        #region Information
        private void informationShow()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.Info("Cube(s) number", Target.CubesCount.ToString());
            if (Target.terrainData.Max_Y != null)
            {
                BasicEditorTools.Info("Highest altitude", Target.terrainData.Max_Y.transform.position.y.ToString());
            }
            if (Target.terrainData.Min_Y != null)
            {
                BasicEditorTools.Info("Lowest altitude", Target.terrainData.Min_Y.transform.position.y.ToString());
            }
            BasicEditorTools.Box_Close();
        }
        #endregion
        #region Cube -> obstacle basic functions
        public void OnDestroy_Buffer_SelectedObject()
        {
            if (buffer.SelectedObjects == null) buffer.SelectedObjects = new List<GameObject>();
            foreach (var cube in buffer.SelectedObjects)
            {
                if (cube == null) continue;
                OnDestroy_Cube(cube);
            }
            buffer.SelectedObjects_Clear();
        }
        public void OnDestroy_Cube(GameObject cube)
        {
            if (cube == null) return;
            const string name = "Delete " + Obstacle.NAME;
            Obstacle obstacle = cube.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                obstacle.neighborData.ChangeMyNeighborsSideMeFaceDisplay(true);
            }
            Undo.SetCurrentGroupName(name);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(Target, name);
            Undo.RecordObject(Target.terrainData, name);
            Undo.DestroyObjectImmediate(cube);
            Target.DeleteCube(cube);
            Undo.CollapseUndoOperations(group);
        }
        public void CreateInformer(GameObject[] cubes)
        {
            if (cubes == null) return;
            foreach (GameObject cube in cubes)
            {
                if (cube == null) continue;
                CreateInformer(cube.GetComponent<Obstacle>());

            }
        }
        public void CreateInformer(Obstacle fromCube)
        {
            if (fromCube == null || !fromCube.IsClimbable || fromCube.neighborData.HasNeighbor(CubeFace.Up)) return;
            GameObject informer = (GameObject)GameObject.Instantiate(Resources.Load(InformerHandler.Prefab));
            Undo.RegisterCreatedObjectUndo(informer, "Add " + InformerHandler.NAME);
            if (informer == null) return;
            InformerHandler func = informer.GetComponent<InformerHandler>();
            if (func == null) func = informer.AddComponent<InformerHandler>();
            func.StandOnCube(fromCube);
        }
        public void CreateKey(GameObject[] cubes)
        {
            if (cubes == null) return;
            foreach (GameObject cube in cubes)
            {
                if (cube == null) continue;
                CreateKey(cube.GetComponent<Obstacle>());
            }
        }
        public void CreateKey(Obstacle fromCube)
        {
            if (fromCube == null || !fromCube.IsClimbable || fromCube.neighborData.HasNeighbor(CubeFace.Up)) return;
            GameObject key = (GameObject)GameObject.Instantiate(Resources.Load(KeyController.Prefab));
            Undo.RegisterCreatedObjectUndo(key, "Add " + KeyController.NAME);
            if (key == null) return;
            KeyController func = key.GetComponent<KeyController>();
            if (func == null) func = key.AddComponent<KeyController>();
            func.StandOnCube(fromCube);
        }
        public void ClearTerrain()
        {
            const string name = "Clear terrain";
            Undo.SetCurrentGroupName(name);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(Target, name);
            Undo.RecordObject(Target.terrainData, name);
            //foreach (var node in Target.cubesList)
            //{
            //    if (node == null) continue;
            //    Undo.DestroyObjectImmediate(node);
            //}
            //Target.CleanCubes();
            Undo.RecordObject(Target, name);
            Target.DeleteCubes();
            Undo.CollapseUndoOperations(group);
        }
        public void CreateObstacle(GameObject[] cubes, CubeFace face)
        {
            if (cubes == null) return;
            foreach (var cube in cubes)
            {
                createObstacle(cube, face);
            }
        }
        public void CreateObstacle(Obstacle obstacle, CubeFace face, int count)
        {
            if (obstacle == null || !obstacle.IsOptimizable) return;
            Obstacle lead = obstacle;
            for (int i = 0; i < count; i++)
            {
                if (lead.neighborData.HasNeighbor(face))
                {
                    DynamicCubeFaceRendering obj = lead.neighborData.getNeighbor(face);
                    lead = obj.GetComponent<Obstacle>();
                    if (lead == null || obj.GetComponent<Door>()) break;
                    continue;
                }
                lead = createObstacle(lead.gameObject, face);
            }
        }
        protected Obstacle createObstacle(GameObject fromCube, CubeFace face)
        {
            if (fromCube == null) return null;
            const string name = "Add " + Obstacle.NAME;
            Undo.SetCurrentGroupName(name);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(Target, name);
            Undo.RecordObject(Target.terrainData, name);
            var node = Target.CreateCube(fromCube, face);
            if (node != null)
            {
                Undo.RegisterCreatedObjectUndo(node, name);
            }
            Undo.CollapseUndoOperations(group);
            if (node == null) return null;
            return node.GetComponent<Obstacle>();
        }
        public void CreateObstacle()
        {
            const string name = "Add " + Obstacle.NAME;
            Undo.SetCurrentGroupName(name);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(Target, name);
            Undo.RecordObject(Target.terrainData, name);
            Undo.RegisterCreatedObjectUndo(Target.CreateCube(), name);
            Undo.CollapseUndoOperations(group);
        }
        #endregion
        #region hero
        public void CreateHero(Obstacle obstacle, GameObject heroPrefab)
        {
            if (obstacle == null || heroPrefab == null) return;
            const string name = "Add hero";
            Undo.SetCurrentGroupName(name);
            int group = Undo.GetCurrentGroup(); Undo.RecordObject(Target, name);
            Undo.RecordObject(Target.terrainData, name);
            var node = GameObject.Instantiate<GameObject>(heroPrefab);
            Undo.RegisterCreatedObjectUndo(node, name);
            Undo.CollapseUndoOperations(group);
            HeroController hero = node.GetComponent<HeroController>();
            if (!hero)
            {
                hero = node.AddComponent<HeroController>();
            }
            hero.inputController.controllerType = InputManager.ControllerType.Human;
            hero.setTerrainManager(Target);
            hero.UpdateHeroPositionFromObstacle(obstacle);
        }
        public void CreateHero(Obstacle obstacle)
        {
            if (obstacle == null) return;
            const string name = "Add hero";
            Undo.SetCurrentGroupName(name);
            int group = Undo.GetCurrentGroup(); Undo.RecordObject(Target, name);
            Undo.RecordObject(Target.terrainData, name);
            GameObject node =new GameObject("Hero");
            Undo.RegisterCreatedObjectUndo(node, name);
            Undo.CollapseUndoOperations(group);
            HeroController hero = node.GetComponent<HeroController>();
            if (!hero)
            {
                hero = node.AddComponent<HeroController>();
            }
            hero.inputController.controllerType = InputManager.ControllerType.Human;
            hero.setTerrainManager(Target);
            hero.UpdateHeroPositionFromObstacle(obstacle);
        }
        #endregion
        #region Door
        public void ConvertToDoor(List<GameObject> cubes)
        {
            if (!Door.CanConvertToDoor(cubes))
            {
                TextWindow.OpenWindow("Convert to door", new string[] { "This selection can not convert to door.",
                    "Please try to select cube shape and it`s all default obstacle(s).",
                "Thanks."});
                return;
            }
            const string name = "Add " + Door.NAME;
            Undo.SetCurrentGroupName(name);
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(Target, name);
            Undo.RecordObject(Target.terrainData, name);
            Undo.RegisterCreatedObjectUndo(Target.CreateDoor(cubes), name);
            foreach (GameObject cube in cubes)
            {
                Undo.DestroyObjectImmediate(cube);
                Target.DeleteCube(cube);
            }
            Undo.CollapseUndoOperations(group);
        }
        #endregion
        #region obstacle type
        public void changeObstacleType(ObstacleType type, GameObject[] obstacles)
        {
            if (obstacles == null) return;
            foreach (var node in obstacles)
            {
                Obstacle obstacle = null;
                if (node == null || (obstacle = node.GetComponent<Obstacle>()) == null) continue;
                changeObstacleType(type, obstacle);
            }
        }
        public void changeObstacleType(ObstacleType type, Obstacle obstacle)
        {
            if (obstacle == null) return;
            int group = Undo.GetCurrentGroup();
            Undo.RecordObject(obstacle.gameObject, "Change ObstacleType");
            obstacle.ChangeObstacleType(type);
            Undo.RecordObject(obstacle.gameObject, "Change material");
            obstacle.SetMaterial(Target.GetMaterial(type));
            Undo.CollapseUndoOperations(group);
        }
        #endregion
        #endregion
        #region SceneGUI
        #region variable
        [HideInInspector] public static TerrainManagerBuffer buffer = new TerrainManagerBuffer();
        [SerializeField]
        protected static IEditorEventHandlerBase[] editorEventHandlersArray;
        protected static EditorEventHandlerStates currentEventState;
        #endregion
        private void OnSceneGUI()
        {
            Draw();
        }
        private void Draw()
        {
            if (Target == null) return;
            if (!Target.localSettings.limitedVisibility) return;
            Handles.DrawWireDisc(Target.transform.position, Vector3.up, Target.localSettings.visionRadius);
            Handles.DrawWireDisc(Target.transform.position, Vector3.forward, Target.localSettings.visionRadius);
            Handles.DrawWireDisc(Target.transform.position, Vector3.right, Target.localSettings.visionRadius);
        }
        private static void OnSceneGUI(SceneView sceneView)
        {
            //Handles.BeginGUI();
            if (Event.current.type == EventType.Repaint || buffer.needsRepaint)
            {
                //editorEventHandlersArray[(int)currentEventState].Draw();
                DrawBuffer();

                buffer.needsRepaint = false;
            }
            else if (Event.current.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }
            else
            {
                editorEventHandlersArray[(int)currentEventState].InputHandling(Event.current);
                if (buffer.needsRepaint)
                {
                    HandleUtility.Repaint();
                }
            }
            //Handles.EndGUI();
        }
        #region functions
        #region event state handler
        private void initEditorEventHandlersArray()
        {
            if (editorEventHandlersArray != null) return;
            editorEventHandlersArray = new IEditorEventHandlerBase[System.Enum.GetValues(typeof(EditorEventHandlerStates)).Length];
            editorEventHandlersArray[(int)EditorEventHandlerStates.Idle] = new EditorEventHandler_Idle(this);
            editorEventHandlersArray[(int)EditorEventHandlerStates.MouseRightClick] = new EditorEventHandler_MouseRightClick(this);
            editorEventHandlersArray[(int)EditorEventHandlerStates.MouseMove] = new EditorEventHandler_MouseMove(this);
            editorEventHandlersArray[(int)EditorEventHandlerStates.MouseLeftClick] = new EditorEventHandler_MouseLeftClick(this);
            editorEventHandlersArray[(int)EditorEventHandlerStates.MouseLeftClickPlusControl] = new EditorEventHandler_MouseLeftClickPlusControl(this);


            //
            ChangeState(EditorEventHandlerStates.Idle, true);
        }
        public void ChangeState(EditorEventHandlerStates state, bool force = false)
        {
            if (currentEventState == state && !force) return;
            editorEventHandlersArray[(int)currentEventState].OnFinish();
            editorEventHandlersArray[(int)(currentEventState = state)].OnStart();
        }
        #endregion
        #region draw
        private static void DrawBuffer()
        {
            drawMouseOverObject();
            drawSelectedObjects();
            drawSelectedObstacleToMouseDragPosition();
            buffer.needsRepaint = false;
        }
        private static void drawSelectedObstacleToMouseDragPosition()
        {
            buffer.mouseDragToCreateObstacle_Clear();
            if (!buffer.canDrawSelectedObstacleToMouseDragPosition) return;
            if (buffer.SelectedObjects_Count() != 1 || buffer.SelectedObjects[0] == null) return;
            Obstacle obstacle = buffer.SelectedObjects[0].gameObject.GetComponent<Obstacle>();
            if (obstacle == null || !obstacle.IsOptimizable) return;
            //
            Vector2 size = obstacle.RealSize.ToXZ();
            if (size.x == 0 || size.y == 0) return;
            //
            float desiredHeight = obstacle.RealSize.y;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * desiredHeight);
            float distance;
            if (!horizontalPlane.Raycast(ray, out distance)) return;
            Vector2 mousePosition = ray.GetPoint(distance).ToXZ();
            // y = {m}x + {b}
            // m = (y2 - y1) / (x2 - x1);
            // b = y1 - m * x1;
            Vector2 deltaP = mousePosition - obstacle.Position.ToXZ();
            int countX = (int)System.MathF.Abs(deltaP.x / size.x);
            int countY = (int)System.MathF.Abs(deltaP.y / size.y);
            //
            CubeFace faceX = CubeFace.Right;
            if (deltaP.x < 0) faceX = faceX.ToOpposite();
            CubeFace faceY = CubeFace.Forward;
            if (deltaP.y < 0) faceY = faceY.ToOpposite();
            //
            Vector3 deltaX = obstacle.Position + faceX.ToVector3() * countX * size.x;
            Vector3 deltaY = obstacle.Position + faceY.ToVector3() * countY * size.y;
            //
            if (Vector2.Distance(deltaX.ToXZ(), mousePosition) < size.x * 2)
            {
                buffer.mouseDragToCreateObstacle_Set(faceX, countX);
                for (int i = 1; i <= countX; i++)
                {
                    Vector3 position = obstacle.Position + faceX.ToVector3() * i * size.x;
                    Handles.DrawWireCube(position, obstacle.RealSize);
                }
                drawLineToDetectionObstacle(deltaX, faceX);
            }
            else if (Vector2.Distance(deltaY.ToXZ(), mousePosition) < size.y * 2)
            {
                buffer.mouseDragToCreateObstacle_Set(faceY, countY);
                for (int i = 1; i <= countY; i++)
                {
                    Vector3 position = obstacle.Position + faceY.ToVector3() * i * size.y;
                    Handles.DrawWireCube(position, obstacle.RealSize);
                }
                drawLineToDetectionObstacle(deltaY, faceY);
            }
        }
        private static void drawLineToDetectionObstacle(Vector3 position, CubeFace faceLook)
        {
            foreach (CubeFace face in System.Enum.GetValues(typeof(CubeFace)))
            {
                if (face == faceLook.ToOpposite()) continue;
                if (Physics.Raycast(position, face.ToVector3(), out var hit))
                {
                    if (hit.collider == null) continue;
                    if (hit.collider.GetComponent<Obstacle>() == null) continue;
                    Handles.DrawLine(position, hit.point);
                }
            }
        }
        private static void drawMouseOverObject()
        {
            if (buffer.MouseOverObject.obstacle != null)
            {
                var obstacle = buffer.MouseOverObject.obstacle;
                Handles.color = settings.MoseOverObstacleColor;
                Handles.DrawWireCube(obstacle.Position, obstacle.RealSize);
            }
            if (buffer.MouseOverObject.door != null)
            {
                var door = buffer.MouseOverObject.door;
                Handles.color = settings.MoseOverObstacleColor;
                Handles.DrawWireCube(door.Position, door.RealSize);
            }
        }
        private static void drawSelectedObjects()
        {
            Handles.color = settings.SelectedObstacleColor;
            foreach (var node in buffer.SelectedObjects)
            {
                if (node == null) continue;
                DynamicCubeFaceRendering obstacle = node.GetComponent<DynamicCubeFaceRendering>();
                if (obstacle != null)
                {
                    Handles.CubeHandleCap(0, obstacle.Position, obstacle.transform.rotation * Quaternion.LookRotation(Vector3.up), Obstacle.SIZE * .5f, EventType.Repaint);
                    Handles.DrawWireCube(obstacle.Position, obstacle.RealSize);
                    continue;
                }
            }
        }
        #endregion
        #endregion
        #endregion
        #region struct
        [System.Serializable]
        public class TerrainManagerBuffer
        {
            public bool needsRepaint;
            public MoveOverObjectBuffer MouseOverObject = new MoveOverObjectBuffer();
            public List<CubeFrameStruct> cubeFrames = new List<CubeFrameStruct>();
            public List<GameObject> SelectedObjects = new List<GameObject>();
            public MouseDragToCreateObstacle mouseDragToCreateObstacle;
            public bool canDrawSelectedObstacleToMouseDragPosition;
            public System.Type lastSelectedObjectType;
            #region MouseOverObject 
            public void SetMouseOverObject(GameObject obj)
            {
                MouseOverObject.SetObject(obj);
                needsRepaint = true;
            }
            public void MouseOverObjectClear()
            {
                MouseOverObject.Clear();
                CubeFramesClear();
                needsRepaint = true;
            }
            #endregion
            #region SelectedObjects
            public bool SelectedObjects_HasNode(GameObject node)
            {
                if (node == null) return false;
                return SelectedObjects.IndexOf(node) >= 0;
            }
            public bool SelectedObjects_HasData() => SelectedObjects_Count() > 0;
            public int SelectedObjects_Count() => SelectedObjects.Count;
            public void SelectedObjects_Add(GameObject obj)
            {
                if (obj == null) return;
                if (SelectedObjects.IndexOf(obj) > -1) return;
                System.Type currentType = lastSelectedObjectType;
                if (obj.GetComponent<Obstacle>() != null)
                {
                    currentType = typeof(Obstacle);
                }
                else if (obj.GetComponent<Door>() != null)
                {
                    currentType = typeof(Door);
                }
                if (currentType != lastSelectedObjectType)
                {

                    lastSelectedObjectType = currentType;
                    SelectedObjects_Clear();
                }
                SelectedObjects.Add(obj);

                needsRepaint = true;
            }
            public void SelectedObjects_Remove(GameObject obj)
            {
                SelectedObjects.Remove(obj);
                needsRepaint = true;
            }
            public void SelectedObjects_Clear()
            {
                SelectedObjects.Clear();
                needsRepaint = true;
            }

            #endregion
            #region CubeFrameStruct
            public void CubeFramesAdd(CubeFrameStruct node)
            {
                cubeFrames.Add(node);
                needsRepaint = true;
            }
            public void CubeFramesClear()
            {
                cubeFrames.Clear();
                needsRepaint = true;
            }
            #endregion
            #region mouseDragToCreateObstacle
            public bool mouseDragToCreateObstacle_IsValid() => mouseDragToCreateObstacle != null;
            public void mouseDragToCreateObstacle_Set(CubeFace face, int count) => mouseDragToCreateObstacle = new MouseDragToCreateObstacle(face, count);
            public void mouseDragToCreateObstacle_Clear() => mouseDragToCreateObstacle = null;
            #endregion
            public void Clear()
            {
                MouseOverObject.Clear();
                CubeFramesClear();
                SelectedObjects_Clear();
                needsRepaint = true;
            }
        }
        [System.Serializable]
        public class MouseDragToCreateObstacle
        {
            public MouseDragToCreateObstacle(CubeFace face, int count)
            {
                this.Face = face;
                this.Count = count;
            }
            public CubeFace Face;
            public int Count;
        }
        [System.Serializable]
        public class MoveOverObjectBuffer
        {
            public GameObject gameObject;
            public Obstacle obstacle;
            public Door door;
            public KeyController key;
            public HeroController hero;
            public bool hasData => gameObject != null;
            public bool SetObject(GameObject obj)
            {
                if (obj == gameObject) return false;
                if (obj == null)
                {
                    Clear();
                    return true;
                }
                gameObject = obj;
                if (obstacle = gameObject.GetComponent<Obstacle>()) return true;
                if (key = gameObject.GetComponent<KeyController>()) return true;
                if (hero = gameObject.GetComponent<HeroController>()) return true;
                if (door = gameObject.GetComponent<Door>()) return true;
                return false;
            }
            public void Clear()
            {
                gameObject = null;
                obstacle = null;
                key = null;
                hero = null;
                door = null;
            }
        }
        [System.Serializable]
        public struct CubeFrameStruct
        {
            public Vector3 Position;
            public Vector3 Size;
        }
        #endregion
    }
}
#endif