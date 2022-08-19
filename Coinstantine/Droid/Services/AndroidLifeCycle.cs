using System;
using Android.App;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.LifeTime;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Platforms.Android.Views;

namespace Coinstantine.Droid.Services
{
    [RegisterInterfaceAsLazySingleton]
    public class AndroidLifeCycle : IAndroidLifeCycle
    {
        private readonly IMvxAndroidActivityLifetimeListener _mvxAndroidActivityLifetimeListener;
        private readonly ILifeCycle _lifeCycle;
        private Activity _currentActivity;
        public AndroidLifeCycle(IMvxAndroidActivityLifetimeListener mvxAndroidActivityLifetimeListener,
                                ILifeCycle lifeCycle)
        {
            _mvxAndroidActivityLifetimeListener = mvxAndroidActivityLifetimeListener;
            _lifeCycle = lifeCycle;
        }

        public void Initialize()
        {
            _mvxAndroidActivityLifetimeListener.ActivityChanged += MvxAndroidActivityLifetimeListener_ActivityChanged;
        }

        void MvxAndroidActivityLifetimeListener_ActivityChanged(object sender, MvvmCross.Platforms.Android.Views.MvxActivityEventArgs e)
        {
            switch(e.ActivityState)
            {
                case MvxActivityState.OnCreate:
                    HandleOnCreate(e.Activity);
                    break;
                case MvxActivityState.OnDestroy:
                    HandleOnDestroy(e.Activity);
                    break;
                case MvxActivityState.OnNewIntent:
                    HandleOnNewIntent(e.Activity);
                    break;
                case MvxActivityState.OnPause:
                    HandleOnPause(e.Activity);
                    break;
                case MvxActivityState.OnRestart:
                    HandleOnRestart(e.Activity);
                    break;
                case MvxActivityState.OnResume:
                    HandleOnResume(e.Activity);
                    break;
                case MvxActivityState.OnSaveInstanceState:
                    HandleOnSaveInstanceState(e.Activity);
                    break;
                case MvxActivityState.OnStart:
                    HandleOnStart(e.Activity);
                    break;
                case MvxActivityState.OnStop:
                    HandleOnStop(e.Activity);
                    break;
            }
        }

        private void HandleOnStop(Activity activity)
        {
            if (_currentActivity == activity)
            {
                _currentActivity = null;
                _lifeCycle.FireOnClose();
            }
        }

        private void HandleOnStart(Activity activity)
        {
            if (_currentActivity == null)
            {
                _lifeCycle.FireOnStart();
            }
        }

        private void HandleOnSaveInstanceState(Activity activity)
        {
        }

        private void HandleOnResume(Activity activity)
        {
            _currentActivity = activity;
        }

        private void HandleOnRestart(Activity activity)
        {
            if(_currentActivity == null)
            {
                _lifeCycle.FireOnRestart();
            }
            _currentActivity = activity;
        }

        private void HandleOnPause(Activity activity)
        {
        }

        private void HandleOnNewIntent(Activity activity)
        {
        }

        private void HandleOnDestroy(Activity activity)
        {
        }

        private void HandleOnCreate(Activity activity)
        {
        }
    }

    public interface IAndroidLifeCycle
    {
        void Initialize();
    }
}
