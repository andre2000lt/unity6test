using UnityEngine;
using System.Collections;

/// <summary>
/// Simple demo script which changes the mouse
/// cursor on mouse over.
/// 
/// Copyright 2015, Rene Buehling, www.buehling.org, see <see cref="http://www.buehling.org/go/unityMaterials"/>.
/// This script is licensed under Unity Assets Customer EULA, see <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
/// </summary>
public class SetCursor : MonoBehaviour 
{
	[Tooltip("Cursor to show on MouseExit.")]
	public Texture2D defaultImage;

	[Tooltip("Hotspot coordinates in Default Image.")]
	public Vector2 defaultHotspot=new Vector2();

	[Tooltip("Cursor to show on MouseOver.")]
	public Texture2D pointerImage;
	[Tooltip("Hotspot coordinates in Pointer Image.")]
	public Vector2 pointerHotspot=new Vector2();


	[Tooltip("Optional: Cursor to show on MousePressed.")]
	public Texture2D pressedImage;
	[Tooltip("Hotspot coordinates in Pressed Image.")]
	public Vector2 pressedHotspot=new Vector2();


	
	private Renderer rend; //Renderer, needed just for color highlight of the sphere.
	void Start() {
		rend = GetComponent<Renderer>();
		OnMouseExit (); //init default cursor (if defined)
	}

	void OnMouseEnter()
	{
		if (!enabled) return;
		if (pointerImage) Cursor.SetCursor (pointerImage, pointerHotspot, CursorMode.ForceSoftware);

		rend.material.color = Color.red;
	}

	void OnMouseExit()
	{
		if (!enabled) return;
		if (defaultImage) Cursor.SetCursor (defaultImage, defaultHotspot, CursorMode.ForceSoftware);

		rend.material.color = Color.white;
	}

	void OnMouseDown()
	{
		if (!enabled) return;
		if (pressedImage) Cursor.SetCursor (pressedImage, pressedHotspot, CursorMode.ForceSoftware);
		rend.material.color = Color.yellow;
	}

	void OnMouseUp()
	{
		if (!enabled) return;
		if (pressedImage && pointerImage) Cursor.SetCursor (pointerImage, pointerHotspot, CursorMode.ForceSoftware);
		rend.material.color = Color.red;
	}


}
