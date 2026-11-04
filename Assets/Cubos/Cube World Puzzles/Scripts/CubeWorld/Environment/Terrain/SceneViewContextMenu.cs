#if UNITY_EDITOR
using System;
using HashGame.CubeWorld.OptimizedCube;
using UnityEditor;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    [InitializeOnLoad]
    public class SceneViewContextMenu
    {
        private static bool showContextMenu = true;
        public static void ShowContextMenu_Deactivate() => setShowContextMenu(false);
        public static void setShowContextMenu(bool result)
        {
            showContextMenu = result;
        }
        public static void ShowContextMenu_DeactivateAndShowHelpWindow()
        {
            ShowContextMenu_Deactivate();
            TextWindow window = (TextWindow)EditorWindow.GetWindow(typeof(TextWindow), true, "Info");
            window.Descriptions = new string[] {
                "You can build your own game infrastructure by visiting the address below. (Menu bar)",
                TerrainMenu.NAME,
                "Good luck." };
        }
        static SceneViewContextMenu()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }
        private static void OnSceneGUI(SceneView sceneView)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                if (showContextMenu)
                {
                    TerrainManager terrain = UnityEngine.Object.FindFirstObjectByType<TerrainManager>();
                    if (terrain == null) return;
                    if (terrain.CubesCount == 0)
                    {
                        GenericMenu menu = ContextMenuEditorTools.genericMenu();
                        ContextMenuEditorTools.AddItem(ref menu, "Create first terrain", terrain.CreateFirstCube);
                        ContextMenuEditorTools.SeparatorItem(ref menu);
                        ContextMenu_Construction(ref menu);
                        ContextMenuEditorTools.SeparatorItem(ref menu);
                        ContextMenuEditorTools.AddItem(ref menu, "Never show this menu/Yes I`m sure.", ShowContextMenu_DeactivateAndShowHelpWindow);
                        ContextMenuEditorTools.SeparatorItem(ref menu);
                        ContextMenuEditorTools.AddItem(ref menu, "Close");
                        ContextMenuEditorTools.OpenMenuInMousePosition(ref menu);
                        Event.current.Use();
                        return;
                    }
                }
                var theObjectThatMouseIsOn = SceneGUIHandler.MouseIsOverAnObject();
                if (theObjectThatMouseIsOn != null
                    && theObjectThatMouseIsOn.GetComponent<DynamicCubeFaceRendering>() != null)
                {
                    TerrainManager terrain = UnityEngine.Object.FindFirstObjectByType<TerrainManager>();
                    Selection.objects = new UnityEngine.Object[] { terrain.gameObject };
                }
            }
        }
        private static void ContextMenu_Construction(ref GenericMenu menu)
        {
            TerrainManager terrain = UnityEngine.Object.FindFirstObjectByType<TerrainManager>();
            if (terrain == null) return;
            const string str = "Construction/";

            ContextMenuEditorTools.AddItem(ref menu, str + "Quadrilateral", () => { TerrainLand_QuadrilateralEditorWindow.OpenWindow(terrain.gameObject); });
            ContextMenuEditorTools.AddItem(ref menu, str + "Random walk",
                 () =>
                 {
                     TerrainLand_RandomWalkEditorWindow.OpenWindow(terrain, null);
                 });
            ContextMenuEditorTools.AddItem(ref menu, str + "Room and hallway",
                 () =>
                 {
                     TerrainLand_DungeonEditorWindow.OpenWindow(terrain, null);
                 });
            ContextMenuEditorTools.AddItem(ref menu, str + TerrainLand_StairwayEditorWindow.NAME,
                 () =>
                 {
                     TerrainLand_StairwayEditorWindow.OpenWindow(terrain, null);
                 });


        }
    }
}
#endif