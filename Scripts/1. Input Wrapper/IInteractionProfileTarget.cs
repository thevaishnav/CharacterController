namespace CCN.InputSystemWrapper
{
    public interface IInteractionProfileTarget
    {
        public bool IsInteracting(InteractionProfileBase profile);
        public void StartInteraction(InteractionProfileBase profile);
        public void EndInteraction(InteractionProfileBase profile);
    }
}