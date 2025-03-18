using UnityEngine;
using System.Collections;

namespace HandCursor
{

	/// <summary>
	/// Helper to change a materials primary color to visualize mouse events.
	/// 
	/// Copyright 2015-2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA: 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
	/// </summary>
	public class ChangeMaterialColor : MonoBehaviour
	{
		public Color mouseExit = Color.white;
		public Color mouseEnter = Color.red;
		public Color mouseDown = Color.yellow;

		private bool hover = false;

		private Renderer rend; //Renderer, needed just for color highlight of the sphere.
		void Start()
		{
			rend = GetComponent<Renderer>();
			if (rend == null)
			{
				Debug.LogWarning(gameObject.name + ".ChangeMaterialColor: no renderer found.");
				this.enabled = false;
			}
		}

		void OnMouseEnter()
		{
			hover = true;
			rend.material.color = mouseEnter;
		}

		void OnMouseExit()
		{
			hover = false;
			rend.material.color = mouseExit;
		}

		void OnMouseDown()
		{
			rend.material.color = mouseDown;
		}

		void OnMouseUp()
		{
			rend.material.color = hover ? mouseEnter : mouseExit;
		}


	}

}