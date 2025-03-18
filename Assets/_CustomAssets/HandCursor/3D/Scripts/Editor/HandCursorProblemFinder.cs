using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace HandCursor
{
    /// <summary>
    /// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
    /// </summary>
    public class HandCursorProblemFinder : EditorWindow
    {
        [MenuItem("Window/Analysis/Hand Cursor Debugger")]
        static void Init()
        {
            HandCursorProblemFinder window = (HandCursorProblemFinder)EditorWindow.GetWindow(typeof(HandCursorProblemFinder));
            window.Show();
        }

        private DateTime lastCheck = DateTime.Now;
        private int problemTotalCount = -1;
        private int problemEnumerator = -1;

        private List<Issues> reports = new List<Issues>();

        public void OnEnable()
        {
            this.titleContent = new GUIContent("Cursor Debug");
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Scan")) { this.scan(); }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical();
            if (problemTotalCount < 0) EditorGUILayout.HelpBox("Press 'Scan' to inspect the scene.", MessageType.None);
            else
            {
                EditorGUILayout.LabelField("Last checked: " + lastCheck.ToString());
                if (problemTotalCount == 0) EditorGUILayout.HelpBox("No problems found.", MessageType.Info);
                else
                {
                    problemEnumerator = 0;
                    foreach (Issues report in reports)
                        renderReport(report);
                }
            }
                       
            EditorGUILayout.EndVertical();

        }

        private void scan()
        {
            reports.Clear();
            lastCheck = DateTime.Now;

            problemTotalCount = 0;
            Cursor3D cs = FindObjectOfType<Cursor3D>();
            if (cs == null)
            {
                Issues iss = new Issues();
                iss.addError("No Cursor3D instance found.", "Add the Cursor3D prefab to the hierarchy.", null, false);
                reports.Add(iss);
            }
            else
            {
                reports.Add(cs.findProblems());
                foreach (CursorSymbol csym in FindObjectsOfType<CursorSymbol>()) reports.Add(csym.findProblems());
                foreach (SetCursor3D sc3d in FindObjectsOfType<SetCursor3D>()) reports.Add(sc3d.findProblems());
            }

            foreach (Issues report in reports)
                problemTotalCount += report.Count;

        }

        private void renderReport(Issues issues)
        {
            GUIStyle textErr = new GUIStyle(EditorStyles.boldLabel); textErr.normal.textColor = Color.red;
            GUIStyle textWarn = new GUIStyle(EditorStyles.boldLabel); textWarn.normal.textColor = Color.yellow;

            GUIStyle textSmall = new GUIStyle(EditorStyles.wordWrappedLabel);
            GUIStyle textSmallBold = new GUIStyle(EditorStyles.boldLabel); textSmallBold.fontStyle = FontStyle.Bold;

            GUIStyle hStyle = new GUIStyle(EditorStyles.boldLabel);

            foreach (Issues.Issue issue in issues.all)
            {
                problemEnumerator++;
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                if (issue.severity == Issues.Severity.Error) EditorGUILayout.LabelField(problemEnumerator + ". ERROR", textErr);
                else if (issue.severity == Issues.Severity.Warning) EditorGUILayout.LabelField(problemEnumerator + ". WARNING", textWarn);
                else EditorGUILayout.LabelField(problemEnumerator + ". INFO", hStyle);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Source:", textSmallBold, GUILayout.Width(60), GUILayout.ExpandWidth(false));
                if (issue.source == null) EditorGUILayout.LabelField("- Not given -", textSmall, GUILayout.ExpandWidth(true));
                else if (GUILayout.Button(new GUIContent(issue.sourceFullName, "Select problem source"))) Selection.activeGameObject = issue.source.gameObject;
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Problem:", textSmallBold, GUILayout.Width(60), GUILayout.ExpandWidth(false));
                EditorGUILayout.LabelField(issue.info, textSmall, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Solution:", textSmallBold, GUILayout.Width(60), GUILayout.ExpandWidth(false));
                EditorGUILayout.LabelField(issue.solution, textSmall, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndVertical();
            }
        }
    }
}