using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Core/Media Surface")]
	public class MediaSurface : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Constants:
		//-----------------------------------------------------------------------------------------

		private const string TEXTURE_PROPERTY    = "_MainTex";
		private const string STEREO_PROPERTY     = "_Stereoscopic";
		private const string PROJECTION_PROPERTY = "_Projection";
		private const string YFLIP_PROPERTY      = "_YFlip";

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private Renderer surfaceRenderer;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() { surfaceRenderer = GetComponentInChildren<Renderer>(); }

		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		/// <summary>
		/// Sets which texture is drawn by this surface. Used by Images and Videos.
		/// </summary>
		/// <param name="texture"></param>
		public void SetTexture(Texture texture) { surfaceRenderer.material.SetTexture(TEXTURE_PROPERTY, texture); }

		/// <summary>
		/// Sets whether this surface is displaying stereoscopic or monoscopic media.
		/// </summary>
		/// <param name="stereoscopic"></param>
		public void SetStereoscopic(bool stereoscopic) { surfaceRenderer.material.SetFloat(STEREO_PROPERTY, stereoscopic ? 1 : 0); }

		/// <summary>
		/// Sets the surface projection.
		/// </summary>
		public void SetProjection(MediaProjection projection) { surfaceRenderer.material.SetFloat(PROJECTION_PROPERTY, (int) projection); }

		/// <summary>
		/// Sets whether the surface is y-flipped (Some video decoders read top->bottom, others bottom -> top).
		/// </summary>
		/// <param name="flipped"></param>
		public void SetYFlip(bool flipped) { surfaceRenderer.material.SetFloat(YFLIP_PROPERTY, flipped ? 1 : 0); }
	}
}