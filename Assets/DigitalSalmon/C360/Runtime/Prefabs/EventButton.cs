using UnityEngine;
using UnityEngine.Events;

public class EventButton : Button {
	[Header("Event Button")]
	[SerializeField]
	protected UnityEvent onSubmit;

	protected override void OnSubmitted() {
		base.OnSubmitted();
		onSubmit?.Invoke();
	}
}