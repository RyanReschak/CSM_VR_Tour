using DigitalSalmon.C360;
using UnityEngine;

public class NodeButton : Button {
	[Header("Node Button")]
	[SerializeField]
	protected Node node;

	protected override void OnSubmitted() {
		base.OnSubmitted();
		Complete360Tour.GoToMedia(node);
	}

	public void AssignNode(Node node) {
		this.node = node;
		SetLabelText(node.Name);
	}
}