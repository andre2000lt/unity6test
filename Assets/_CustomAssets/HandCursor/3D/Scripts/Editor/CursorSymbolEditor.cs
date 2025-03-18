using UnityEngine;
using System.Collections;
using UnityEditor;

namespace HandCursor
{
	/// <summary>
	/// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
	/// </summary>
	[CustomEditor(typeof(CursorSymbol))]
	public class CursorSymbolEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			//base.OnInspectorGUI();
			serializedObject.Update();
			CursorSymbol cs = target as CursorSymbol;
			Color guiColor = GUI.color;

			cs.symbolController=EditorGUILayout.ObjectField("Symbol Controller",cs.symbolController, typeof(Transform), true) as Transform;

			EditorGUILayout.LabelField("Modifiers:",EditorStyles.boldLabel);

			foreach(CursorSymbol.ObjectModifier om in cs.modifiers)
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);;

				EditorGUILayout.BeginHorizontal();
				if (om.target == null) GUI.color = Color.red;
				om.target=EditorGUILayout.ObjectField(om.target, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
				GUI.color = guiColor;
				if (GUILayout.Button("×", GUILayout.Width(20))){ cs.modifiers.Remove(om); return; }
				EditorGUILayout.EndHorizontal();

				foreach (CursorSymbol.StateBasedModification sbm in om.modifications)
				{
					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button("×")) { om.modifications.Remove(sbm); return; }
					sbm.modification = (CursorSymbol.ObjectModification) EditorGUILayout.EnumPopup(sbm.modification);
					GUILayout.Label("if cursor is");
					sbm.state=(Cursor3DState) EditorGUILayout.ObjectField(sbm.state, typeof(Cursor3DState),false);
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("+ Add State Condition")){om.DuplicateLastMod(); return;}
				EditorGUILayout.EndHorizontal();


				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				om.otherwise=(CursorSymbol.ObjectModification) EditorGUILayout.EnumPopup(om.otherwise);
				GUILayout.Label("in all other cases");
				EditorGUILayout.EndHorizontal();
				om.growAndShrink = EditorGUILayout.ToggleLeft("Grow & Shrink instead of De/Activate",om.growAndShrink);

				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();
			}

			if (GUILayout.Button("+ Add Object")) { cs.modifiers.Add(new CursorSymbol.ObjectModifier()); return; }

			serializedObject.ApplyModifiedProperties();
		}
	}

}