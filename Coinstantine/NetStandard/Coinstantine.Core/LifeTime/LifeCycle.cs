using System;
using Coinstantine.Common.Attributes;

namespace Coinstantine.Core.LifeTime
{
    [RegisterInterfaceAsLazySingleton]
    public class LifeCycle : ILifeCycle
    {
        public event EventHandler<LifeCycleEventArgs> LifeCycleStateChanged;
        public event EventHandler<LifeCycleEventArgs> LifeCycleChangedForOther;

        public void FireOnClose()
        {
            FireLifetimeChanged(LifeCycleEvent.Closing);
        }

        public void FireOnRestart()
        {
            FireLifetimeChanged(LifeCycleEvent.Restarting);
        }

        public void FireOnStart()
        {
            FireLifetimeChanged(LifeCycleEvent.Starting);
        }

        public void FireOnStartForOthers()
        {
            LifeCycleChangedForOther?.Invoke(this, new LifeCycleEventArgs(LifeCycleEvent.Starting));
        }

        private void FireLifetimeChanged(LifeCycleEvent lifetimeEvent)
        {
            LifeCycleStateChanged?.Invoke(this, new LifeCycleEventArgs(lifetimeEvent));
        }
    }
}
