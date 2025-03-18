using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace HandCursor
{
    /// <summary>
    /// Helper to store information about problems found.
    /// This is primarily just a data structure used by both,
    /// Problem Scanner utility in Editor and runtime problem
    /// scanning.
    /// 
    /// An Issues oject is a collection of problems found which
    /// is usually created by a 'findProblems' method.
    /// 
    /// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.   
    /// </summary>
    public class Issues
    {
        public enum Severity { Info, Warning, Error };

        /// <summary>
        /// Description of one problem. 
        /// </summary>
        public class Issue
        {
            /// <summary>
            /// Problem description.
            /// </summary>
            public string info = "(Unknown Issue)";
            /// <summary>
            /// Description of steps to solve the problem. 
            /// </summary>
            public string solution = "";
            /// <summary>
            /// Problem classification.
            /// </summary>
            public Severity severity = Severity.Info;
            /// <summary>
            /// Optional pointer to source of problem or location where to fix it.
            /// </summary>
            public Component source = null;

            /// <summary>
            /// Creates an null-safe text representation of the problem source, like "ObjectName > ComponentName".
            /// </summary>
            public string sourceFullName
            {
                get { return (source == null) ? "Null" : source.gameObject.name + " > " + source.GetType().Name; }
            }
        }

        private List<Issue> items = new List<Issue>();

        /// <summary>
        /// The items/problems stored in this collection.
        /// </summary>
        public List<Issue> all
        {
            get { return items;  }
        }

        /// <summary>
        /// Number of problems recorded in this Issues collection.
        /// </summary>
        public int Count
        {
            get { return this.items.Count; }
        }
        
        public Issues() 
        {
        }

        /// <summary>
        /// Records an error which is meant to mark a problem that must be solved.
        /// </summary>
        /// <param name="problem">Description of what is wrong.</param>
        /// <param name="solution">Description of what should be done to solve the problem. </param>
        /// <param name="problemSource">Pointer to object where the problem was found or where it should be fixed.</param>
        /// <param name="printConsoleOutput">If true, a Debug.LogError will be executed, too.</param>
        /// <returns>The Issue object that stores the problem data in a code friendly way.</returns>
        public Issue addError(string problem, string solution, Component problemSource=null, bool printConsoleOutput=false)
        {
            Issue i = new Issue();
            i.info = problem;
            i.solution = solution;
            i.severity = Severity.Error;
            i.source = problemSource;
            items.Add(i);
            if (printConsoleOutput)
                Debug.LogError(i.sourceFullName + ": " + problem +" "+ solution);
            return i;
        }

        /// <summary>
        /// Records a warning which is meant to mark a problem that should be solved, but which may not block the program.
        /// </summary>
        /// <param name="problem">Description of what is wrong.</param>
        /// <param name="solution">Description of what should be done to solve the problem. </param>
        /// <param name="problemSource">Pointer to object where the problem was found or where it should be fixed.</param>
        /// <param name="printConsoleOutput">If true, a Debug.LogWarning will be executed, too.</param>
        /// <returns>The Issue object that stores the problem data in a code friendly way.</returns>
        public Issue addWarning(string problem, string solution, Component problemSource = null, bool printConsoleOutput=false)
        {
            Issue i = new Issue();
            i.info = problem;
            i.solution = solution;
            i.severity = Severity.Warning;
            i.source = problemSource;
            items.Add(i);
            if (printConsoleOutput)
                Debug.LogWarning(i.sourceFullName + ": " + problem + " " + solution);
               
            return i;
        }

    }
}