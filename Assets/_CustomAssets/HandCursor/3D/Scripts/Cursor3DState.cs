using UnityEngine;
using System.Collections;

namespace HandCursor
{
	/// <summary>
	/// Describes a cursor state by storing an animator state name
	/// and additional parameters.
	/// 
	/// Create as many State assets as you need and trigger them
	/// by reference in a <see cref="SetCursor3D"/> script.
	/// 
	/// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
	/// </summary>
	[CreateAssetMenu(menuName ="Hand Cursor/State")]
	public class Cursor3DState : ScriptableObject
	{
		/// <summary>
		/// Name of the animator state to play 
		/// as primary, default state. This is the
		/// unpressed mouse state.
		/// </summary>
		[Tooltip("Name of the animator state to play as primary, default state. This is the unpressed mouse state.")]
		public string state = "";

		/// <summary>
		/// Time in seconds used to blend into this state.
		/// </summary>
		[Tooltip("Time in seconds used to blend into this state.")]
		public float transitionTime=0.3f;

		/// <summary>
		/// Optional delay that helps to skip very short intermediate states.
		/// 
		/// Usecase: 
		/// 
		/// Let's have two hotspots in the scene which are located pretty close to each other.
		/// When users move the mouse from one hotspot to the other the pointer is between 
		/// the two hotspots for a very short time which causes the idle cursor to appear
		/// before the second hotspot's cursor is enabled. 
		/// 
		/// We set the lazyUpdateDelay for idle cursor state to i.e. 0.2 to solve this.
		/// Now, when the user moves the mouse pointer from one to the other hotspot the
		/// idle cursor is skipped as the mouse travels across the small gap between the two
		/// hotspots in less than 0.2 seconds. Therefore the pointer of hotspot 1 will 
		/// directly transform into cursor of hotspot 2.
		/// </summary>
		[Tooltip("Wait seconds before activating this state.")]
		public float lazyUpdateDelay = 0f;
	}


}