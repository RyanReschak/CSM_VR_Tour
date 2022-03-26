using DigitalSalmon.DarkestFayte;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Hotspot/Hotspot")]
	public class Hotspot : AnimatedBehaviour {
		//-----------------------------------------------------------------------------------------
		// Protected Fields:
		//-----------------------------------------------------------------------------------------
		
		[Header("Hotspot")]
		[SerializeField]
		protected TMP_Text textComponent;

		[SerializeField]
		protected RawImage rawImage;
		
		//-----------------------------------------------------------------------------------------
		// Public Properties:
		//-----------------------------------------------------------------------------------------

		public HotspotElement HotspotElement { get; private set; }

		protected override bool CanBeVisible => base.CanBeVisible && !IsHiding && HotspotElement != null;

		protected bool IsHiding { get; private set; }
		
		//-----------------------------------------------------------------------------------------
		// Public Methods:
		//-----------------------------------------------------------------------------------------

		public void HideAndDestroy() {
			IsHiding = true;
			const float TIMEOUT = 0.5f;
			sequence.Do(TIMEOUT, () => Destroy(gameObject));
		}

		/// <summary>
		/// Switches this Hotspots internal data and visual style to settings defined by 'element'.
		/// </summary>
		public virtual void Construct(Node node, HotspotElement element) {
			if (element == null) {
				HideAndDestroy();
				return;
			}

			HotspotElement = element;

			ResetState();

			if (textComponent != null) {
				textComponent.text = element?.TargetNode?.Name;
			}

			TimelineHelper.AssignElement(element);

			UpdateMappedElementPosition(transform, node, element);
			SetIcon(((IMappedElement) element).Icon);
			OnHoveredDeltaUpdate(0);
		}

		

		protected override void OnTimelineThresholdCrossed() {
			base.OnTimelineThresholdCrossed();
			ITimedElement timedElement = HotspotElement;
			if (timedElement.EntryTime == timedElement.ExitTime) {
				Submit(true);
			}
		}

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		protected override void ResetState() {
			base.ResetState();
			IsHiding = false;
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected void SetIcon(Texture icon) {
			const string ICON_PROPERTY = "_Icon";
			if (rawImage != null) rawImage.texture = icon;
		}

		protected override void OnSubmitted() {
			base.OnSubmitted();
			Complete360Tour.GoToMedia(HotspotElement.TargetNode);
			DisableInteraction();
		}
		
		//-----------------------------------------------------------------------------------------
		// Private Methods:
		//-----------------------------------------------------------------------------------------

		private static void UpdateMappedElementPosition(Transform transform, Node node, IMappedElement mappedElement, float distance = -1) {
			MediaProjection projection = MediaProjection.Equirectangular;
			if (node is MediaNode mediaNode) {
				projection = mediaNode.Projection;
			}

			Vector2 hotspotPosition = Vector2.zero;
			switch (projection) {
				case MediaProjection.Equirectangular:
					hotspotPosition = new Vector2(mappedElement.Position.x, 1 - mappedElement.Position.y);
					break;
				case MediaProjection.VR180:
					hotspotPosition = new Vector2(mappedElement.Position.x / 2 + 0.25f, 1 - mappedElement.Position.y);
					break;
			}

			const float MAPPED_ELEMENT_CAMERA_DISTANCE = 8;
			transform.localPosition = MathUtilities.EquirectangularProjection(hotspotPosition) * (distance == -1 ? MAPPED_ELEMENT_CAMERA_DISTANCE : distance);
			transform.rotation = Quaternion.LookRotation(transform.localPosition.normalized, Vector3.up);
		}
	}
}