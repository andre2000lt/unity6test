using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandCursor
{
	/// <summary>
	/// Super simple script to create a tabbed UI.
	/// Usage: 
	/// - Create UI buttons for the tabs.
	/// - Create any object (2D or 3D) that work as pages to be shown/hidden.
	/// - Add SimpleTab to the UI button and set 'active' property to the page to be shown by this tab. 
	///   Also add onClick to the buttons click event trigger.
	/// 
	/// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
	/// </summary>
	public class SimpleTab : MonoBehaviour
	{
		/// <summary>
		/// Object to set active when this tab is clicked.
		/// </summary>
		public GameObject activate;
		/// <summary>
		/// Set to true for the tab that is active when starting.
		/// </summary>
		public bool selectOnStart=false;
		public void onClick()
		{
			//Debug.Log("Click on "+gameObject.name+" -> "+activate);
			SimpleTab[] tabs = transform.parent.GetComponentsInChildren<SimpleTab>();
			foreach(SimpleTab tab in tabs)
			{
				Button b = tab.GetComponent<Button>();
				if (b == null)
				{
					//Debug.LogWarning("No Button on "+tab.gameObject.name);
					continue;
				}

				ColorBlock cb = new ColorBlock(); 
				cb = b.colors;

				if (tab==this)
				{
					if (tab.activate == null) Debug.LogWarning("No activate target on " + tab.gameObject.name);
					else tab.activate.SetActive(true);
					cb.normalColor = Color.white;
				}
				else
				{
					if (tab.activate == null) Debug.LogWarning("No activate target on " + tab.gameObject.name);
					else tab.activate.SetActive(false);
					cb.normalColor = Color.gray;
				}
				b.colors = cb;
			}
		}

		// Use this for initialization
		void Start()
		{
			if (selectOnStart) this.onClick();
		}

	}

}