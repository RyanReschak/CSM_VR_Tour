namespace DigitalSalmon.C360 {
	public interface IMappedPrefab {
		void UpdateState(TransitionState state);
		void UpdateData(PrefabElement element, Node node);
	}
}