using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Examples/Base Plate")]
	public class BasePlate : BaseBehaviour {
		//-----------------------------------------------------------------------------------------
		// Inspector Variables:
		//-----------------------------------------------------------------------------------------

		[Header("Base Plate")]
		[Range(0f, 0.45f)]
		[SerializeField]
		protected float discRadius = 1f;

		[SerializeField]
		protected float logoPadding = 0.1f;

		[SerializeField]
		protected bool isBottom = true;

		[SerializeField]
		protected Material radialMask;

		[Header("Logos")]
		[Range(0.01f, 2)]
		[SerializeField]
		protected float logoScale = 1f;

		[SerializeField]
		protected Texture logo;

		[SerializeField]
		protected Transform logoContainer;

		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private RawImage[] logos;

		//-----------------------------------------------------------------------------------------
		// Unity Lifecycle:
		//-----------------------------------------------------------------------------------------

		protected void Awake() { UpdateAll(); }

		protected void OnValidate() { UpdateAll(); }

		private void UpdateAll() {
			SetRadialSize();
			GetLogoObjects();
			SetLogoSizes();
			SetLogoPositions();
			SetLogos();
		}

		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private void SetTransform() {
			transform.position = new Vector3(0, isBottom ? -5 : 5, 0);
			transform.rotation = Quaternion.Euler(isBottom ? 90 : -90, 0, 0);
		}

		private void SetRadialSize() {
			if (radialMask != null) radialMask.SetFloat("_Radius", discRadius);
		}

		private void GetLogoObjects() {
			if (logoContainer == null) return;

			logos = logoContainer.GetComponentsInChildren<RawImage>();
		}

		private void SetLogoSizes() {
			if (logos == null || logoContainer == null) return;

			Vector3 scale = Vector3.one * logoScale;
			foreach (RawImage logoObject in logos) {
				logoObject.transform.localScale = scale;
			}
		}

		private void SetLogoPositions() {
			if (logos == null || logoContainer == null) return;

			for (int i = 0; i < logos.Length; i++) {
				RawImage logoObject = logos[i];
				float alpha = (float) i / logos.Length;
				Vector2 dir = new Vector2(Mathf.Cos(alpha * 360 * Mathf.Deg2Rad), Mathf.Sin(alpha * 360 * Mathf.Deg2Rad));
				float r = discRadius - logoPadding;
				logoObject.rectTransform.localPosition = new Vector3(dir.x, dir.y, 0) * r;
				
				logoObject.rectTransform.localRotation = Quaternion.Euler(0, 0, 270+(alpha * 360));
			}
		}

		private void SetLogos() {
			if (logos == null || logo == null) return;

			foreach (RawImage logoImage in logos) {
				logoImage.texture = logo;
			}
		}
	}
}