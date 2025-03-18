using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Reflection;

namespace HandCursor
{
    /// <summary>
    /// Disables the Transform component in Inspector if a CursorSymbol script is present
    /// which controls the Transform properties.
    /// 
    /// If any problems occur because of this script, simply delete it. 
    /// It is not required for the cursor asset to work. 
    /// 
    /// 
    /// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
    /// </summary>
    [CustomEditor(typeof(Transform))]
    public class TransformEditor : Editor
    {
        //Unity's built-in editor
        private Editor defaultEditor;

        void OnEnable()
        {
            //When this inspector is created, also create the built-in inspector
            defaultEditor = Editor.CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
        }

        void OnDisable()
        {
            //When OnDisable is called, the default editor we created should be destroyed to avoid memory leakage.
            //Also, make sure to call any required methods like OnDisable
            MethodInfo disableMethod = defaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (disableMethod != null) disableMethod.Invoke(defaultEditor, null);
            DestroyImmediate(defaultEditor);
        }

        override public void OnInspectorGUI()
        {
            Transform t = target as Transform;

            CursorSymbol cs = t.GetComponent<CursorSymbol>();
            if (cs != null && cs.symbolController != null)
            {
                GUI.enabled = false;
                defaultEditor.OnInspectorGUI();  //DrawDefaultInspector();
                GUI.enabled = true;
                EditorGUILayout.HelpBox("Transform of this game object is controlled by CursorSymbol component.", MessageType.Warning);
            }
            else
                defaultEditor.OnInspectorGUI();
        }
    }
}