#if UNITY_EDITOR
using System.Collections.Generic;
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class ObstacleEditorWindow : EditorWindow
    {
        public static Obstacle Target;
        private Vector2 scrollPosition;
        private static bool highlighting = true;
        public static Color highlightColor = Color.red;
        private int tabIndex;
        private const float Scale = 1.1f;
        //
        private static List<DrawObject> highlighterObstacles = new List<DrawObject>();

        public static void OpenWindow(Obstacle obstacle)
        {
            Target = obstacle;
            GetWindow<ObstacleEditorWindow>("Obstacle data");
            highlighterObstaclesInit();
        }
        #region Functions
        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }
        #endregion
        #region OnGUI
        private void OnGUI()
        {
            BasicEditorTools.Scroll_Begin(ref scrollPosition);
            obstacleGUI();
            BasicEditorTools.Scroll_End();
            BasicEditorTools.Button("Close", onClose);
        }
        private void obstacleGUI()
        {
            BasicEditorTools.Tab(ref tabIndex, new BasicEditorTools.StringActionStruct[]{
                new BasicEditorTools.StringActionStruct("Obstacle", obstacleTab1),
                new BasicEditorTools.StringActionStruct("Obstacle Types",obstacleTypesBaseGUI),
                new BasicEditorTools.StringActionStruct("OnGUI", obstacleOnGUI),
            });
        }
        private void obstacleTab1()
        {
            BasicEditorTools.Line();
            BasicEditorTools.Box_Open();
            BasicEditorTools.ScriptableObject(Target, typeof(ObstacleEditor));
            BasicEditorTools.Box_Close();
        }
        private void obstacleOnGUI()
        {
            BasicEditorTools.Line();
            BasicEditorTools.Box_Open();
            BasicEditorTools.ChangeCheck_Begin();
            highlighting = BasicEditorTools.Toggle("Highlight obstacle", highlighting);
            highlightColor = BasicEditorTools.ColorField("Highlight Color", highlightColor);
            if (BasicEditorTools.ChangeCheck_End())
            {
                BasicEditorTools.DirtyObject(Target.transform);
            }
            BasicEditorTools.Box_Close();
        }
        private void obstacleTypesBaseGUI()
        {
            BasicEditorTools.Line();
            ObstacleTypesBase func = Target.GetComponent<ObstacleTypesBase>();
            if (!func)
            {
                BasicEditorTools.Box_Open();
                BasicEditorTools.Label("This Object has no ObstacleType class.");
                BasicEditorTools.Box_Close();
                return;
            }
            BasicEditorTools.ChangeCheck_Begin();
            BasicEditorTools.Box_Open();
            switch (Target.obstacleType)
            {
                case ObstacleType.Default:
                case ObstacleType.Slide:
                case ObstacleType.AutoWalkDestination:
                case ObstacleType.PortalDestination:
                    {
                        BasicEditorTools.ScriptableObject(func, null);

                        BasicEditorTools.Box_Open();
                        BasicEditorTools.Label("This Object has Obstacle type class, but it`s not important.");
                        BasicEditorTools.Label("For this reason, and to avoid complexity, we have refrained from showing it.");
                        BasicEditorTools.Label("But you can access this class if you want.");
                        BasicEditorTools.Label("Wish you all the best.");
                        BasicEditorTools.Box_Close();
                    }
                    break;
                case ObstacleType.AutoWalk:
                    BasicEditorTools.ScriptableObject(func, typeof(AutoWalkEditor));
                    break;
                case ObstacleType.Portal:
                    BasicEditorTools.ScriptableObject(func, typeof(PortalEditor));
                    break;
                case ObstacleType.Destructible:
                    BasicEditorTools.ScriptableObject(func, typeof(DestructibleObstacleEditor));
                    break;
                case ObstacleType.Checkpoint:
                    BasicEditorTools.ScriptableObject(func, typeof(CheckPointEditor));
                    break;
                case ObstacleType.Undercover:
                    BasicEditorTools.ScriptableObject(func, typeof(UndercoverEditor));
                    break;
                case ObstacleType.Spawner:
                    BasicEditorTools.ScriptableObject(func, typeof(SpawnerEditor));
                    break;
                case ObstacleType.SpawnerButton:
                    BasicEditorTools.ScriptableObject(func, typeof(SpawnerButtonEditor));
                    break;
                case ObstacleType.DoorLever:
                    BasicEditorTools.ScriptableObject(func, typeof(DoorLeverEditor));
                    break;
                default:
                    break;
            }
            BasicEditorTools.Box_Close();
            if (BasicEditorTools.ChangeCheck_End())
            {
                highlighterObstaclesInit();
                BasicEditorTools.DirtyObject(Target.transform);
            }
        }
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI(SceneView sceneView)
        {
            using (new Handles.DrawingScope(highlightColor))
            {
                //Handles.BeginGUI();
                onSceneGUIFunctions();
                //Handles.EndGUI();
            }
        }
        private void onSceneGUIFunctions()
        {
            highlightingObjectsGUI();
        }
        private void highlightingObjectsGUI()
        {
            if (!highlighting) return;
            for (int i = 0; i < highlighterObstacles.Count; i++)
            {
                if (highlighterObstacles[i] == null)
                {
                    highlighterObstacles.RemoveAt(i--);
                    continue;
                }
                var node = highlighterObstacles[i];
                if (node.obstacle != null)
                {
                    drawObstacle(node.obstacle.Position, node.obstacle.boxCollider.size, node.name);
                    continue;
                }
                if (node.transform != null)
                {
                    drawObstacle(node.transform.position, node.size, node.name);
                    continue;
                }
                highlighterObstacles[i] = null;
            }
        }
        private void drawObstacle(Vector3 position, Vector3 size, string name = null)
        {
            Vector3 _size = Scale * size;
            Handles.DrawWireCube(position, _size);
            if (string.IsNullOrEmpty(name)) return;
            Vector3 p1 = position + Vector3.back * size.z / 2 + Vector3.up * size.y / 2 + Vector3.left * size.x / 2;
            Vector3 p2 = p1 + Vector3.up * size.y;
            Handles.DrawLine(p1, p2);
            Handles.Label(p2, name);
        }
        private static void highlighterObstaclesInit()
        {
            highlighterObstacles.Clear();
            highlighterObstacles.Add(new DrawObject()
            {
                name = Target.name,
                obstacle = Target
            });
            ObstacleTypesBase func = Target.GetComponent<ObstacleTypesBase>();
            if (!func)
            {
                return;
            }
            switch (Target.obstacleType)
            {
                case ObstacleType.AutoWalk:
                    {
                        AutoWalk f = Target.GetComponent<AutoWalk>();
                        if (f != null && f.localSettings.destination != null)
                            highlighterObstacles.Add(new DrawObject()
                            {
                                name = f.localSettings.destination.name,
                                obstacle = f.localSettings.destination.obstacle
                            });
                    }
                    break;
                case ObstacleType.Portal:
                    {
                        Portal f = Target.GetComponent<Portal>();
                        if (f == null) break;
                        if (f.destinationPortalSettings.useFreeTransform && f.destinationPortalSettings.destinationTransform != null)
                        {
                            highlighterObstacles.Add(new DrawObject()
                            {
                                name = f.destinationPortalSettings.destinationTransform.name,
                                transform = f.destinationPortalSettings.destinationTransform,
                                size = Vector3.one
                            });
                        }
                        else if (!f.destinationPortalSettings.useFreeTransform && f.destinationPortalSettings.portalDestination != null)
                        {
                            highlighterObstacles.Add(new DrawObject()
                            {
                                name = f.destinationPortalSettings.portalDestination.name,
                                obstacle = f.destinationPortalSettings.portalDestination.obstacle
                            });
                        }
                    }
                    break;
                case ObstacleType.SpawnerButton:
                    {
                        SpawnerButton f = Target.GetComponent<SpawnerButton>();
                        if (f == null) break;
                        if (f.Target == null) break;
                        highlighterObstacles.Add(new DrawObject()
                        {
                            name = f.Target.name,
                            obstacle = f.Target.obstacle,
                        });
                    }
                    break;
                case ObstacleType.DoorLever:
                    {
                        DoorLever d = Target.GetComponent<DoorLever>();
                        if (d == null) break;
                        if (d.doors != null)
                        {
                            foreach (var door in d.doors)
                            {
                                if (door == null) continue;
                                highlighterObstacles.Add(new DrawObject()
                                {
                                    name = door.name,
                                    transform = door.transform,
                                    size = door.RealSize
                                });
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        private void onClose()
        {
            Target = null;
            this.Close();
        }
        #region struct
        [System.Serializable]
        public class DrawObject
        {
            public string name;
            public Transform transform;
            public Obstacle obstacle;
            public Vector3 size;
        }
        #endregion
    }
}
#endif