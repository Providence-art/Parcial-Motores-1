#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace HashGame.CubeWorld.EditorTools
{
    public class BasicEditorTools
    {
        #region line
        public static void Line(int n = 1)
        {
            for (int i = 0; i < n; i++)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
        #endregion
        #region Lable
        public static void Label<T>(T text) => Label(text.ToString());
        public static void Label(string text) => GUILayout.Label(text);
        #endregion
        #region Field
        public static string TextField(string name, string value) => EditorGUILayout.TextField(name, value);
        public static int IntField(string name, int value) => EditorGUILayout.IntField(name, value);
        public static float FloatField(string name, float value) => EditorGUILayout.FloatField(name, value);
        public static bool Toggle(string name, bool value) => EditorGUILayout.Toggle(name, value);
        public static Color ColorField(string name, Color value) => EditorGUILayout.ColorField(name, value);
        #endregion
        #region Box
        public static void Box_Open(string name = null)
        {

            GUILayout.BeginVertical("box");
            if (!string.IsNullOrEmpty(name))
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name, EditorStyles.boldLabel);
                GUILayout.EndHorizontal();
            }
        }
        public static void Box_Close() => GUILayout.EndVertical();
        #endregion
        #region Change check
        public static void ChangeCheck_Begin() => EditorGUI.BeginChangeCheck();
        public static bool ChangeCheck_End() => EditorGUI.EndChangeCheck();
        #endregion
        #region info
        public static void Info(string caption, string value)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(caption + ": " + value);
            GUILayout.EndHorizontal();
        }
        #endregion
        #region tab
        public static void Tab(ref int tabIndex, string[] tabNames)
        {
            if (tabNames == null) return;
            if (tabIndex > tabNames.Length) tabIndex = 0;
            tabIndex = GUILayout.Toolbar(tabIndex, tabNames);
        }
        public static void Tab(ref int tabIndex, string[] tabNames, Action[] functions)
        {
            Tab(ref tabIndex, tabNames);
            if (functions != null && tabIndex >= 0 && tabIndex < functions.Length && functions[tabIndex] != null)
            {
                functions[tabIndex].Invoke();
            }
        }
        public static void Tab(ref int tabIndex, StringActionStruct[] nodes)
        {
            if (nodes == null || nodes.Length == 0) return;
            string[] names = new string[nodes.Length];
            for (int i = 0; i < names.Length; i++) names[i] = nodes[i].name;
            Tab(ref tabIndex, names);
            if (nodes[tabIndex].action != null) nodes[tabIndex].action.Invoke();
        }
        #endregion
        #region scroll
        public static void Scroll_Begin(ref Vector2 scrollPosition) { scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition); }
        public static void Scroll_End() => EditorGUILayout.EndScrollView();
        #endregion
        #region Button
        public static bool Button(string name) => GUILayout.Button(name);
        public static void Button(string name, Action function)
        {
            if (function == null) return;
            if (Button(name)) function.Invoke();
        }
        public static void Buttons(StringActionStruct[] nodes, int columnCount = 1, bool includeEmptySpace = false)
        {
            int _columnCount = columnCount < 1 ? 1 : columnCount;
            GUILayout.BeginHorizontal();
            if (nodes == null || nodes.Length == 0) return;
            int c = 0;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (c++ == _columnCount)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    c = 0;
                }
                GUILayout.BeginVertical();
                if (Button(nodes[i].name))
                {
                    if (nodes[i].action != null)
                    {
                        nodes[i].action.Invoke();
                    }
                }
                GUILayout.EndVertical();
            }
            if (includeEmptySpace)
            {
                while (++c < _columnCount)
                {
                    GUILayout.BeginVertical();
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
        }
        #endregion
        #region drop down list
        public static void DropDownList(string lable, ref int selected, string[] options)
        {
            selected = DropDownList(lable, selected, options);
        }
        public static int DropDownList(string lable, int selected, string[] options) => EditorGUILayout.Popup(lable, selected, options);
        #endregion
        #region editor
        #endregion
        #region ScriptableObject
        public static void ScriptableObject(UnityEngine.Object targetObject, Type editorType = null)
        {
            if (targetObject == null) return;
            Editor editor = Editor.CreateEditor(targetObject, editorType);
            editor.OnInspectorGUI();
        }
        #endregion
        #region Dirty
        public static void DirtyObject(Transform target) => EditorUtility.SetDirty(target);
        public static void SceneDirty() => EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        public static void SceneDirty(Transform target) => EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
        #endregion
        public static bool Foldout(ref bool foldout, string title = null) => foldout = EditorGUILayout.Foldout(foldout, title);
        public static SerializedProperty PropertyField(SerializedObject input, string name, string caption = null, string hint = null)
        {
            if (string.IsNullOrEmpty(name)) return null;

            var result = input.FindProperty(name);

            if (string.IsNullOrEmpty(caption))
            {
                EditorGUILayout.PropertyField(result);
            }
            else
            {
                if (string.IsNullOrEmpty(hint)) EditorGUILayout.PropertyField(result, new GUIContent(caption));
                else EditorGUILayout.PropertyField(result, new GUIContent(caption, hint));
            }
            return result;
        }
        public static void FocusOnSelectedObject()
        {
            if (Selection.activeGameObject) SceneView.lastActiveSceneView.FrameSelected();
        }
        #region struct
        [System.Serializable]
        public struct StringActionStruct
        {
            public string name;
            public Action action;
            public StringActionStruct(string n, Action a)
            {
                action = a;
                name = n;
            }
        }
        #endregion
    }
}
#endif