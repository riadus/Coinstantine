using System;
namespace Coinstantine.Core.LifeTime
{
	public interface ILifeCycle
    {
        event EventHandler<LifeCycleEventArgs> LifeCycleStateChanged;
        void FireOnStart();
        void FireOnRestart();
        void FireOnClose();
        void FireOnStartForOthers();
        event EventHandler<LifeCycleEventArgs> LifeCycleChangedForOther;
    }
}
