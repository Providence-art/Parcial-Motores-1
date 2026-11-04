#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using HashGame.CubeWorld.OptimizedCube;
using HashGame.CubeWorld.HeroManager;

namespace HashGame.CubeWorld.EditorTools
{
    [CustomEditor(typeof(AutoWalk))]
    public class AutoWalkEditor : Editor
    {
        #region variable
        protected AutoWalk Target => (AutoWalk)target;
        protected Vector3 Position { get => Target.transform.position; }
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
            BasicEditorTools.Tab(ref Target.editor_TabIndex, new BasicEditorTools.StringActionStruct[] {
            new BasicEditorTools.StringActionStruct("Settings",showSettings),
            new BasicEditorTools.StringActionStruct("Events",showEvents),
            new BasicEditorTools.StringActionStruct("Destination",showLocalSettings),
            new BasicEditorTools.StringActionStruct("Buffer",showBuffer),
            });
        }
        private void showLocalSettings()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "localSettings", "Settings");
            BasicEditorTools.PropertyField(serializedObject, "editor_drawDestination", "Draw path");

            BasicEditorTools.Box_Close();
        }
        private void showSettings()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "settings", AutoWalkSettings.NAME);
            if (Target.settings)
            {
                BasicEditorTools.ScriptableObject(Target.settings, typeof(AutoWalkSettingsEditor));
            }
            BasicEditorTools.Box_Close();
        }
        private void showBuffer()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.Info("State", Target.currentState.ToString());
            BasicEditorTools.PropertyField(serializedObject, "buffer.travelCount", "Travel count");
            BasicEditorTools.Info("In the spawn position", Target.buffer.inSpawnPosition.ToString());
            BasicEditorTools.Info("Start position", Target.buffer.StartPosition.ToString());
            BasicEditorTools.Info("Start rotation", Target.buffer.StartRotation.ToString());
            BasicEditorTools.Box_Close();
        }
        private void showEvents()
        {
            BasicEditorTools.Box_Open();
            BasicEditorTools.PropertyField(serializedObject, "events", "Events");
            BasicEditorTools.Box_Close();
        }
        #endregion
        #region OnSceneGUI
        private void OnSceneGUI()
        {
            drawDestination();
        }
        private void drawDestination()
        {
            if (!Target.editor_drawDestination) return;
            if (!Target.localSettings.destination)
            {
                BasicDrawEditorTools.Lable(Position, "No destination");
                return;
            }
            Handles.DrawLine(Position, Target.localSettings.destination.transform.position);
            BasicDrawEditorTools.Lable(Position, "Start");
            BasicDrawEditorTools.Lable(Target.localSettings.destination.transform.position, "Destination");
        }
        #endregion
    }
}
#endif