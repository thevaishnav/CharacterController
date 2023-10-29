namespace CCN.InputSystemWrapper
{
    public interface IInteractionProfileTarget
    {
        /// <param name="profile"> Interaction profile that is calling the method </param>
        /// <returns> Is the object interacting with respect to given profile (An object can have multiple interaction profiles) </returns>
        public bool IsInteracting(InteractionProfileBase profile);
        
        /// <param name="profile"> Interaction profile that is calling the method </param>
        /// <returns> Start interaction with respect to given profile (An object can have multiple interaction profiles) </returns>
        public void StartInteraction(InteractionProfileBase profile);
        
        /// <param name="profile"> Interaction profile that is calling the method </param>
        /// <returns> End interaction with respect to given profile (An object can have multiple interaction profiles) </returns>
        public void EndInteraction(InteractionProfileBase profile);
    }
}