using UnityEngine;
using System.Collections.Generic;

namespace HandCursor
{
	/// <summary>
	/// Controller that will synchronize additional cursor decorations (magnify glass, white orb, book, ...)
	/// with the current cursor state.
	/// 
	/// Add this script to the "Symbol Pivot" object and add the actual rendering objects below that empty object.
	/// Then assign the "symbol controller" bone from the hand rig to the symbolController field.
	/// 
	/// If needed, use Inspector to add show/hide conditions for the rendering objects.
	/// 
	/// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
	/// </summary>
	[ExecuteInEditMode]
	public class CursorSymbol : MonoBehaviour
	{
		/// <summary>
		/// Set this to the bone that controls position and size
		/// of the symbol. 
		/// </summary>
		public Transform symbolController = null;

		/// <summary>
		/// Defines what should happen to a symbol object when a state is triggered.
		/// </summary>
		public enum ObjectModification { 
			/// <summary>Do not change anything.</summary>
			Nothing, 
			/// <summary>Set object active and/or perform a zoom-appearance effect if modifier's growAndShrink option is set.</summary>
			Activate,
			/// <summary>Set object inactive and/or perform a zoom-hide effect if modifier's growAndShrink option is set.</summary>
			Deactivate
		};

		/// <summary>
		/// Helper structure that holds the relation bewtween a cursor state
		/// and the modifications that will be triggered if the state is activated.
		/// </summary>
		[System.Serializable]
		public class StateBasedModification
		{
			/// <summary>
			/// If cursor state matches this state, the modification will be applied.
			/// </summary>
			public Cursor3DState state;
			/// <summary>
			/// Modification to apply if cursor state is activated.
			/// </summary>
			public ObjectModification modification;

			public StateBasedModification() { }
			/// <summary>
			/// Create new StateBasedModification with settings copied from another StateBasedModification.
			/// </summary>
			/// <param name="source">Copy source</param>
			public StateBasedModification(StateBasedModification source)
			{
				this.state = source.state;
				this.modification = source.modification;
			}
		}

		/// <summary>
		/// Collection of modifications that shall be applied to a symbol object if 
		/// the cursor changes in a specific way. 
		/// </summary>
		[System.Serializable]
		public class ObjectModifier
		{
			/// <summary>
			/// The symbol render object that shall be shown or hidden. 
			/// </summary>
			[Tooltip("The symbol render object that shall be shown or hidden. ")]
			public GameObject target;
			
			/// <summary>
			/// Collection of modification definitions for the target object.
			/// </summary>
			public List<StateBasedModification> modifications = new List<StateBasedModification>();

			/// <summary>
			/// Effect if none of the conditions apply.
			/// </summary>
			[Tooltip("Effect if none of the conditions apply.")]
			public ObjectModification otherwise;

			/// <summary>
			/// If set, de/activate does not change the active state of object, but changes it size through in an visible animation.
			/// </summary>
			[Tooltip("If set, de/activate does not change the active state of object, but changes it size through in an visible animation.")]
			public bool growAndShrink=false;

			/// <summary>
			/// Cache of unchanged scale as designed in Editor. 
			/// Required to grow back to design size when using growAndShrink.
			/// </summary>
			private Vector3 initalScale;

			public void Start()
			{
				initalScale = target.transform.localScale;
			}

			private Vector3 growFrom;
			private Vector3 growTo;
			private float growProgress=10f;
			private float growSpeed = 0.1f;
			public void Update()
			{
				if (!target.activeSelf) return;

				if (growProgress <= 1f && growFrom != growTo)
				{
					growProgress += Time.deltaTime / growSpeed;
					target.transform.localScale = Vector3.Lerp(growFrom, growTo, growProgress);
				}
			}

			/// <summary>
			/// Applies all modifications for the given state 
			/// where the modification conditions apply.
			/// </summary>
			/// <param name="newCursorState"></param>
			public void apply(Cursor3DState newCursorState)
			{
				if (target == null) return;
				foreach (StateBasedModification sm in modifications)
					if (sm.state == null) continue;
					else if (sm.state.Equals(newCursorState))
					{
						growSpeed = newCursorState.transitionTime;
						apply(sm.modification);
						return;
					}
				apply(otherwise);
			}
			/// <summary>
			/// Applies a specific modification (without checking any conditions).
			/// This makes the object active/inactive or starts the growAndShrink transitions.
			/// </summary>
			/// <param name="mod">Modification to apply.</param>
			public void apply(ObjectModification mod)
			{
				if (mod == ObjectModification.Nothing) return;
				else if (mod == ObjectModification.Activate)
				{
					if (growAndShrink) {growFrom = target.transform.localScale; growTo = initalScale; growProgress = 0; }
					else target.SetActive(true);
				}
				else if (mod == ObjectModification.Deactivate)
				{
					if (growAndShrink) { growFrom = target.transform.localScale; growTo = Vector3.zero; growProgress = 0; }
					else target.SetActive(false);

				}
			}
			/// <summary>
			/// Utility for Inspector: Adds a duplicate of the last StateBasedModification in the list to the end of the list
			/// (or creates a new one if list is empty).
			/// </summary>
			public void DuplicateLastMod()
			{
				if (modifications.Count == 0) modifications.Add(new StateBasedModification());
				else modifications.Add(new StateBasedModification(modifications[modifications.Count - 1]));
			}

		}

		/// <summary>
		/// List of all modifications defined for cursor symbol objects.
		/// </summary>
		public List<ObjectModifier> modifiers = new List<ObjectModifier>();

		/// <summary>
		/// Cached pointer to the cursor3D instance which is needed to 
		/// trigger state changes.
		/// </summary>
		private Cursor3D c3d;

		private void onCursorChanged(Cursor3DState newCursorState)
		{
			if (c3d == null) return;
			foreach (ObjectModifier om in modifiers)
				om.apply(newCursorState);
		}

		private void OnEnable()
		{
			c3d = FindObjectOfType<Cursor3D>();
			if (Application.isPlaying && c3d!=null) c3d.onCursorChanged += onCursorChanged;
		}

		private void OnDisable()
		{
			if (Application.isPlaying && c3d!=null) c3d.onCursorChanged -= onCursorChanged;
		}

		public Issues findProblems(bool printConsoleOutput=false)
		{
			Issues issues = new Issues();
			if (symbolController == null)
			{
				issues.addError("Symbol Controller not set and not found below " + transform.parent.name + ".", "Set field 'Symbol Controller' to the cursor's symbol bone (or disable/remove CursorSymbol component).",this, printConsoleOutput);
			}
			foreach (Collider c in GetComponentsInChildren<Collider>())
				if (c.enabled)
					issues.addWarning("Child object contains a " + c.GetType().Name + " which may block mouse events.","Remove oder disable colliders below CursorSymbol.",c,printConsoleOutput);

			c3d = FindObjectOfType<Cursor3D>();
			if (c3d.gameObject.layer != gameObject.layer)
				issues.addWarning("CursorSymbol is on different renderlayer than Cursor3D, which may cause wrong rendering.","Set CursorSymbol to same layer as Cursor3D.", this, printConsoleOutput);

			for (int i = 0; i < transform.childCount; i++)
				if (transform.GetChild(i).gameObject.layer != gameObject.layer)
					issues.addWarning("CursorSymbol child '" + transform.GetChild(i).gameObject.name + "' is on different render layer than Cursor3D, which may cause wrong rendering.","Set all CursorSymbol children to same layer as Cursor3D.", transform.GetChild(i), printConsoleOutput);
			return issues;
		}


		// Use this for initialization
		void Start()
		{
			/*
			if (symbolController == null)
			{			
				symbolController = transform.parent.FindChild("symbolcontroller");
			}*/

			if (Application.isPlaying)
			{
				findProblems(true);
				foreach (ObjectModifier om in modifiers) om.Start();
			}
		
		}

		// Update is called once per frame
		void Update()
		{
			if (symbolController == null) return;
			
			//follow controller bone: 
			transform.position = symbolController.transform.position;
			transform.localScale = symbolController.transform.localScale;
			transform.rotation = symbolController.transform.rotation;

			//update transitions:
			foreach (ObjectModifier om in modifiers) om.Update();
		}
	}

}