using UnityEngine;
using System.Collections;

namespace HandCursor
{

	/// <summary>
	/// Changes the native cursor to an image or disables it 
	/// in editor or always.
	/// 
	/// Copyright 2020, Rene Buehling, www.buehling.org.
	/// This script is licensed under Unity Assets Customer EULA, 
	/// <see cref="https://unity3d.com/legal/as_terms#section-asset-store-end-user-license-agreement" />.
	/// </summary>
	public class SetNativeCursor : MonoBehaviour
	{
		[Tooltip("Cursor to show on MouseOver.")]
		public Texture2D pointerImage;
		[Tooltip("Hotspot coordinates in Pointer Image.")]
		public Vector2 pointerHotspot = new Vector2(16f, 16f);

		public enum NativeCursorMode
		{
			/// <summary>
			/// Always show the native system mouse pointer arrow.
			/// </summary>
			Show,
			/// <summary>
			/// Show the native system mouse pointer in editor, but not in build.
			/// </summary>
			ShowInEditorOnly,
			/// <summary>
			/// Always hide the native system mouse pointer arrow.
			/// </summary>
			Hide
		};

		/// <summary>
		/// Controls the behaviour of the native mouse cursor.
		/// </summary>
		[Tooltip("Controls the behaviour of the native mouse cursor.")]
		public NativeCursorMode nativeCursor = NativeCursorMode.Hide;

		// Use this for initialization
		void Start()
		{
			if (pointerImage != null) Cursor.SetCursor(pointerImage, pointerHotspot, CursorMode.Auto);

			if (nativeCursor == NativeCursorMode.Hide || (nativeCursor == NativeCursorMode.ShowInEditorOnly && !Application.isEditor))
				Cursor.visible = false;
		}
	}

}