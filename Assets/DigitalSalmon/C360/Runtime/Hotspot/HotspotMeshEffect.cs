using UnityEngine;
using UnityEngine.UI;

namespace DigitalSalmon.DarkestFayte {
	public class HotspotMeshEffect : BaseMeshEffect {
		[SerializeField]
		[Range(0, 1)]
		protected float fill;

		[SerializeField]
		[Range(0, 1)]
		protected float iconAlpha;

		[SerializeField]
		[Range(0, 1)]
		protected float iconScale;

		[SerializeField]
		[Range(0, 1)]
		protected float innerRadius;

		[SerializeField]
		[Range(0, 1)]
		protected float outerRadius;

		protected void OnValidate() { graphic.SetVerticesDirty(); }

		public override void ModifyMesh(VertexHelper vh) {
			UIVertex vert = new UIVertex();
			for (int i = 0; i < vh.currentVertCount; i++) {
				vh.PopulateUIVertex(ref vert, i);
				vert.uv1 = new Vector4(fill, iconAlpha, iconScale, 0);
				vert.uv2 = new Vector4(innerRadius, outerRadius, 0, 0);
				vh.SetUIVertex(vert, i);
			}
		}

		public void SetFill(float fill) {
			this.fill = fill;
			graphic.SetVerticesDirty();
		}
	}
}