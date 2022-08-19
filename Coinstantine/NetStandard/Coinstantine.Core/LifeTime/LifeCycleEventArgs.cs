using System;
namespace Coinstantine.Core.LifeTime
{
    public class LifeCycleEventArgs : EventArgs
    {
        public LifeCycleEventArgs(LifeCycleEvent lifetimeEvent)
        {
            LifetimeEvent = lifetimeEvent;
        }

        public LifeCycleEvent LifetimeEvent { get; }
    }
}
