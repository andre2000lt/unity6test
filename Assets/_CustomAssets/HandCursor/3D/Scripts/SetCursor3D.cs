using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace HandCursor
{

	/// <summary>
	/// Changes the 3D mouse cursor based on mouse events. 
	/// Works with 3D objects (require Collider) and
	/// 2D GUI objects (EventTrigger created on demand if not present).
	/// 
	/// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
	/// </summary>
	public class SetCursor3D : MonoBehaviour
	{
		[Tooltip("Cursor to show on MouseOver.")]
		public HandCursor.Cursor3DState state;

		/// <summary>
		/// Optional additional name of animator state 
		/// that will be active while the mouse is pressed. 
		/// </summary>
		[Tooltip("Additional State shown while mouse is pressed.")]
		public HandCursor.Cursor3DState pushState = null;

		/// <summary>
		/// If true, cursor will keep push state while mouse
		/// is released, no matter which other events like mouse exit
		/// happen meanwhile.
		/// </summary>
		[Tooltip("If true, pressed mouse cursor will stay as long as the mouse is pushed, even if exit/enter events occur while holding mouse button.")]
		public bool lockPushState = true;

		/// <summary>
		/// Pointer to the current Cursor3D instance.
		/// </summary>
		private Cursor3D hc3d;

		public Issues findProblems(bool printConsoleOutput=false)
		{
			Issues issues = new Issues();
			if (state == null) issues.addWarning(gameObject.name+" has no state, cursor will not change.","Assign a cursor state to the state field in Inspector.", this, printConsoleOutput);
			if (GetComponent<Collider>() == null && GetComponent<RectTransform>()==null) issues.addWarning("Cursor may not appear as there is no Collider or UI Component.", "Add a collider or UI component to '"+gameObject.name+"'.", this, printConsoleOutput);
			return issues;
		}


		private void Start()
		{
			findProblems(true);

			hc3d = FindObjectOfType<Cursor3D>();
			OnMouseExit(); //init default cursor (if defined)

			if (GetComponent<RectTransform>()!=null) // Using Canvas/UI
			{
				if (GetComponent<EventTrigger>()==null) //if not set up in Editor (else keep what user designed)
					createEventSystemHooks();
			}
		}

		/// <summary>
		/// Returns an existing Event Trigger for a given eventID or creates a new trigger.
		/// </summary>
		/// <param name="et">Event Trigger component to handle event.</param>
		/// <param name="eventID">Event for which callbacks should be added. </param>
		/// <returns>Entry where callback functions can be added in order to call them when events of given ID occur.</returns>
		private EventTrigger.Entry getOrCreateEntry(EventTrigger et, EventTriggerType eventID)
		{
			foreach (EventTrigger.Entry e in et.triggers)
				if (e.eventID == eventID) return e;

			EventTrigger.Entry e2 = new EventTrigger.Entry();
			e2.eventID = eventID;
			return e2;
		}

		/// <summary>
		/// Signature for simple callbacks
		/// </summary>
		delegate void Callback();

		/// <summary>
		/// Hooks a callback into the given event system, reusing existing or creating trigger slots.
		/// </summary>
		/// <param name="et">Event Trigger to hook callback into.</param>
		/// <param name="eventID">Event ID which calls the callback.</param>
		/// <param name="f">Callback to be added for the given event.</param>
		private void addEventTriggerListener(EventTrigger et, EventTriggerType eventID, Callback f)
		{
			EventTrigger.Entry entry = getOrCreateEntry(et, eventID);
			entry.callback.AddListener((eventData) => { f(); });
			et.triggers.Add(entry);
		}

		/// <summary>
		/// Hooks the component's events into the UI system by 
		/// adding or populating the Event Trigger component on this object.
		/// </summary>
		public void createEventSystemHooks()
		{
			EventTrigger et = GetComponent<EventTrigger>();
			if (et==null) et=gameObject.AddComponent<EventTrigger>();

			addEventTriggerListener(et, EventTriggerType.PointerEnter, this.OnMouseEnter);
			addEventTriggerListener(et, EventTriggerType.PointerExit, this.OnMouseExit);
			addEventTriggerListener(et, EventTriggerType.PointerUp, this.OnMouseUp);
			addEventTriggerListener(et, EventTriggerType.PointerDown, this.OnMouseDown);
		}


		/// <summary>
		/// Carries a pressed state across enter-exit events.
		/// </summary>
		private bool mouseOver = false;

		public void OnMouseEnter()
		{
			this.mouseOver = true;
			if (enabled) hc3d.setCursorState(this.state);
		}

		public void OnMouseExit()
		{
			if (enabled) hc3d.setIdleCursor();
			this.mouseOver = false;
		}


		public void OnMouseDown()
		{
			if (enabled & pushState != null)
			{
				hc3d.setCursorState(pushState);
				if (lockPushState) hc3d.lockTo = pushState;
			}
		}

		public void OnMouseUp()
		{
			if (hc3d.lockTo==pushState) hc3d.lockTo = null; //1st
			if (enabled & mouseOver) hc3d.setCursorState(state); //2nd
		}


	}

}