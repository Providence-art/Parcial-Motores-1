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
    [CustomEditor(typeof(Hero_CircleDesign))]
    public class Hero_CircleDesignEditor : Editor
    {
        #region variable
        public Hero_CircleDesign Target => (Hero_CircleDesign)target;
        #endregion
        #region Inspector
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();
            BasicEditorTools.Line();
            if (BasicEditorTools.Button("Create"))
            {
                Target.CreateCircles();
            }
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}
#endif