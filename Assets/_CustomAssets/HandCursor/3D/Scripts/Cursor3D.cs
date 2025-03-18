using UnityEngine;
using System.Collections;

namespace HandCursor
{
	/// <summary>
	/// Controller that renders the handcursor and coordinates exchange of cursor states. 
	/// 
	/// There must be exactly one Cursor3D script present.
	/// Usually this script is attached to the hand cursor model root (the empty parent of cursor mesh and symbol objects).
	/// 
	/// Use SetCursor3D scripts to link Cursor3D to events.
	/// Optional: Use a SetNativeCursor script sibling to control the behaviour of the native system cursor.
	/// 
	/// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
	/// </summary>
	public class Cursor3D : MonoBehaviour
	{
		/// <summary>
		/// The camera that renders the 3D Cursor. 
		/// </summary>
		[Tooltip("The camera that renders the 3D Cursor.")]
		public Camera cam;

		/// <summary>
		/// Name of the animator state activated if no other cursor state is set.
		/// </summary>
		[Tooltip("Name of the animator state activated if no other cursor state is set.")]
		public Cursor3DState idleState;


		/// <summary>
		/// Animator of the hand model, will be set to first animator found in children.
		/// </summary>
		private Animator animator;

		/// <summary>
		/// Depth value for cursor, calculated from initial situation.
		/// </summary>
		private float targetZ;

		[Tooltip("Set to create debug console output.")]
		public bool debug = false;


		public Issues findProblems(bool printConsoleOutput=false)
		{
			Issues issues = new Issues();
			// Cam setup issues
			if (cam == null) issues.addError("Hand Cursor Camera not set.","Assign property 'cam' for HandCursor3D script.", this ,printConsoleOutput);
			else if (cam.transform.GetComponentsInParent<Camera>().Length<2) issues.addWarning("Cursor Camera is not below the main camera which may cause unexpected render results.", "Make cursor camera object a child of the main camera in hierarchy.", this, printConsoleOutput);

			// Layer issues
			if (this.gameObject.layer == 0) issues.addError("Cursor object is placed on Layer 0, which is probably wrong.", "Use a separate Render Layer.", this, printConsoleOutput);
			else if (LayerMask.LayerToName(gameObject.layer) == "") issues.addWarning("Cursor's layer has an empty name, which probably means that layers are not set up correctly.", "Create a render layer for cursor only and assign it to cursor objects.", this, printConsoleOutput);
			else if (Camera.main == null) issues.addWarning("Main Camera not set.","Cannot check camera conditions as there is no camera marked as Main. ",null,false);
			else if (Camera.main != null && (Camera.main.cullingMask & (1 << gameObject.layer)) != 0) issues.addWarning("Main Camera renders cursor layer which may lead to double rendering.", "Remove cursor layer from " + Camera.main.name + "'s 'Culling Mask' field in Inspector.", Camera.main, printConsoleOutput);
			else if (cam != null && (cam.cullingMask==-1)) issues.addWarning("Cursor camera's culling mask includes Everything which may lead to double renderings.", "Set " + cam.name + "'s 'Culling Mask' field to only the cursor layer in Inspector.", Camera.main, printConsoleOutput);
			else if (cam != null && (cam.cullingMask==0)) issues.addWarning("Cursor camera's culling mask does not render any layer which will not render the cursor.", "Set " + cam.name + "'s 'Culling Mask' field to only the cursor layer in Inspector.", Camera.main, printConsoleOutput);

			// Other
			if (FindObjectsOfType<Cursor3D>().Length>1) issues.addError("More than one Cursor3D component detected. Multiple Cursor3D scripts may block each other.", "Delete duplicate Cursor3D scripts!", this, printConsoleOutput);

			return issues;
		}

		void Start()
		{
			// Health check
			findProblems(true);		

			// Calculate cursor depth, as seen in https://youtu.be/7OJQ6MbHuvQ
			Vector3 toObjectVector = transform.position - cam.transform.position;
			Vector3 linearDistanceVector = Vector3.Project(toObjectVector, cam.transform.forward);
			targetZ = linearDistanceVector.magnitude;

			// init components and state
			animator = GetComponentInChildren<Animator>();		
			
		}

		// Update is called once per frame
		void Update()
		{
			Vector3 mousePosition = Input.mousePosition;
			mousePosition.z = targetZ;
			transform.position = cam.ScreenToWorldPoint(mousePosition);

			//if (Input.GetKeyUp(KeyCode.A)) setCursorState("test");
			//else if (Input.GetKeyUp(KeyCode.W)) setCursorState("idle");
		}

		/// <summary>
		/// Temporary pointer to the state to be activated at 
		/// the end of this update cycle.
		/// </summary>
		private Cursor3DState nextState = null;

		/// <summary>
		/// If a cursor state is assigned to this member,
		/// calls of setCursorState() will have no effect
		/// unless this is set to null. 
		/// 
		/// Can be used i.e. to lock an cursor image during
		/// drag operations etc.
		/// </summary>
		[HideInInspector]
		public Cursor3DState lockTo = null;

		/// <summary>
		/// Signature of a listener that can be added to <see cref="onCursorChanged"/>.
		/// </summary>
		/// <param name="state">Info about the state that caused the event.</param>
		public delegate void StateCallback(Cursor3DState state);

		/// <summary>
		/// Add listeners of form <see cref="StateCallback"/> to this list
		/// to be notified when the cursor changes. This is only fired when
		/// there is an actual change. <see cref="lockTo"/> therefore prevents
		/// this event.
		/// </summary>
		public event StateCallback onCursorChanged;

		/// <summary>
		/// If there was a time set to wait before the next pose is activated (<see cref="Cursor3DState.lazyUpdateDelay"/>), 
		/// this value will hold the remaining time like a countdown.
		/// </summary>
		private float delay = 0f;

		/// <summary>
		/// When the update cycle has finished (which allowed any scripts to trigger any change of cursor appearance)
		/// finally apply changes as needed.
		/// </summary>
		private void LateUpdate()
		{
			if (lockTo != null && lockTo != nextState) return;
			if (delay>0f)
			{
				delay -= Time.deltaTime;
			}
			else if (animator!=null && nextState!=null && !animator.IsInTransition(0)) //in transition switch required as sometimes two calls after each other to not work
			{
				animator.CrossFadeInFixedTime("Base Layer." + nextState.state, nextState.transitionTime);
				if (debug) Debug.Log("crossfade to Base Layer." + nextState.state);
				if (onCursorChanged!=null) onCursorChanged(nextState);
				nextState = null;
			}
		}



		/// <summary>
		/// Sets the cursor to the given animation state.
		/// Use to set a specific cursor visualization.
		/// 
		/// Note that the change will not happen immediately, but in 
		/// the next late update call. This prevents race problems
		/// if setCursorState is called multiple times which would 
		/// trigger overlapping animation blendings.
		/// 
		/// </summary>
		/// <param name="statename">Animation state name to show.</param>
		public void setCursorState(Cursor3DState state)
		{
			if (debug) Debug.Log("setCursorState to "+state);
			this.nextState = state;
			this.delay = state.lazyUpdateDelay;
		}

		/// <summary>
		/// Sets the current cursor to idle state.
		/// Shortcut for handcursor3d.setCursorState(handcursor3d.idleState).
		/// </summary>
		public void setIdleCursor()
		{
			this.setCursorState(this.idleState);
		}

#if UNITY_EDITOR
		void OnDrawGizmos()
		{
			// Display the explosion radius when selected
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(transform.position, 0.1f);
		}
#endif

		//private void OnGUI()
		//{
		//	GUI.Label(new Rect(0, 0, 1000, 1000), "next: " + this.nextState);
		//}

	}

}