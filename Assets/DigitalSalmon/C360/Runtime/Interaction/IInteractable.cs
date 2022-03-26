namespace DigitalSalmon.C360 {
	public interface IInteractable {
		bool IsInteractive { get; }

		void BeginInteract();
		void EndInteract();
		void Submit();

		void SetInteractionTime(float t, float totalTime);
	}
}