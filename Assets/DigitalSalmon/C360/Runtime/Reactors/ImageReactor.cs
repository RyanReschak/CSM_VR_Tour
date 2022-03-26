using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DigitalSalmon.C360 {
	[AddComponentMenu("Complete 360 Tour/Core/Image Reactor")]
	public class ImageReactor : MediaReactor, ITimeController {
		//-----------------------------------------------------------------------------------------
		// Private Fields:
		//-----------------------------------------------------------------------------------------

		private MediaClock clock;
		
		[SerializeField]
		protected MediaTransition mediaTransition;

		//-----------------------------------------------------------------------------------------
		// Interface Fields:
		//-----------------------------------------------------------------------------------------

		int ITimeController.Priority => 0;
		double ITimeController.Time => clock.TimeMs;

		//-----------------------------------------------------------------------------------------
		// Event Handlers:
		//-----------------------------------------------------------------------------------------

		protected override void C360_MediaSwitch(TransitionState state, Node node) {
			if (state == TransitionState.Switch) SwitchMedia(node);
		}

		//-----------------------------------------------------------------------------------------
		// Protected Methods:
		//-----------------------------------------------------------------------------------------

		protected void SwitchMedia(Node node) {
			if (!(node is ImageNode imageNode)) return;

			if (imageNode.Loop) clock = MediaClock.Looped(imageNode.DurationMs);
			else clock = MediaClock.Limited(imageNode.DurationMs);

			Time.RegisterController(this);
			
			LoadImageTexture(imageNode); // TODO loading.
			Surface.SetStereoscopic(imageNode.IsStereoscopic);
			Surface.SetProjection(imageNode.Projection);
			Surface.SetYFlip(false);
		}
		
		protected async Task LoadImageTexture(ImageNode imageNode) {
			mediaTransition?.RegisterLoadObject(this);

			List<IImageLoadData> loadDatas = ListPool<IImageLoadData>.New();
			imageNode.GetLoadData(loadDatas);

			foreach (IImageLoadData loadData in loadDatas) {
				if (!loadData.CanAttemptLoad()) continue;
				
				Task<Texture> loadTask = loadData.LoadThumbnail();
				await loadTask;
				
				if (loadTask.Result != null) Surface.SetTexture(loadTask.Result);
				break;
			}
			
			mediaTransition?.UnregisterLoadObject(this);
			ListPool<IImageLoadData>.Return(loadDatas);
		}
	}
}