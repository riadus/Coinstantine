using System;
using System.Threading.Tasks;
using Android.App;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Droid.Fragments;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using Plugin.CurrentActivity;

namespace Coinstantine.Droid.UIServices
{
    [RegisterInterfaceAsLazySingleton]
    public class OverlayService : IOverlayService
    {
        private readonly IMvxMainThreadAsyncDispatcher _threadAsyncDispatcher;
        private const string Tag = "OverlayService";
        readonly object _locker = new object();
        private Activity _currentContext;

        public OverlayService(IMvxMainThreadAsyncDispatcher threadAsyncDispatcher)
        {
            _threadAsyncDispatcher = threadAsyncDispatcher;
        }

        public void Dismiss()
        {
            MvxNotifyTask.Create(HideFragment());
        }

        public void UpdateMessage(string message)
        {
        }

        public void Wait(string message)
        {
            MvxNotifyTask.Create(Show(message));
        }
        int i = 0;
        private async Task Show(string message)
        {
            await _threadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                lock (_locker)
                {
                    var fragment = new FullScreenFragment(message);
                    fragment.Show(CrossCurrentActivity.Current.Activity.FragmentManager, Tag);
                    _currentContext = CrossCurrentActivity.Current.Activity;
                    fragment.StartAnimation(i%2);
                    i++;
                }
            });
        }

        private async Task HideFragment()
        {
            await _threadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                lock (_locker)
                {
                    if (_currentContext != null)
                    {
                        using (var fragmentTransaction = _currentContext.FragmentManager.BeginTransaction())
                        {
                            try
                            {
                                _currentContext.FragmentManager.ExecutePendingTransactions();
                                var dialogFragment = (DialogFragment)_currentContext.FragmentManager.FindFragmentByTag(Tag);
                                if (dialogFragment != null)
                                {
                                    dialogFragment.DismissAllowingStateLoss();
                                    fragmentTransaction.Remove(dialogFragment);
                                }
                            }
                            catch(Java.Lang.NullPointerException)
                            {

                            }
                            _currentContext = null;
                        }
                    }
                }
            });
        }
    }
}