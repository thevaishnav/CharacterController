using System;

namespace Omnix.CCN.InputSystemWrapper
{
    public abstract class IInteraction
    {
        public InteractionProfileBase profile;
        
        /// <returns> Is the object interacting with respect to given profile (An object can have multiple interaction profiles) </returns>
        public abstract bool IsInteracting();

        /// <returns> Start interaction with respect to given profile (An object can have multiple interaction profiles) </returns>
        public abstract void StartInteraction();

        /// <returns> End interaction with respect to given profile (An object can have multiple interaction profiles) </returns>
        public abstract void EndInteraction();

        public void Toggle()
        {
            if (IsInteracting()) EndInteraction();
            else StartInteraction();
        }
    }


    public class IPT_Action : IInteraction
    {
        private readonly Func<bool> isInteracting;
        private readonly Action startInteraction;
        private readonly Action endInteraction;

        public IPT_Action(Func<bool> isInteracting, Action startInteraction, Action endInteraction)
        {
            this.isInteracting = isInteracting;
            this.startInteraction = startInteraction;
            this.endInteraction = endInteraction;
        }


        public override bool IsInteracting() => isInteracting();
        public override void StartInteraction() => startInteraction();
        public override void EndInteraction() => endInteraction();
    }
    
    public class IPT_FuncBool : IInteraction
    {
        public delegate bool FuncBool();
        
        private readonly FuncBool isInteracting;
        private readonly FuncBool startInteraction;
        private readonly FuncBool endInteraction;

        public IPT_FuncBool(FuncBool isInteracting, FuncBool startInteraction, FuncBool endInteraction)
        {
            this.isInteracting = isInteracting;
            this.startInteraction = startInteraction;
            this.endInteraction = endInteraction;
        }


        public override bool IsInteracting() => isInteracting();
        public override void StartInteraction() => startInteraction();
        public override void EndInteraction() => endInteraction();
    }
}