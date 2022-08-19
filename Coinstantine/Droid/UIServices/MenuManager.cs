using System.Threading.Tasks;
using Android.App;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels;
using Coinstantine.Droid.CustomViews.Menu;
using MvvmCross.Base;
using Plugin.CurrentActivity;

namespace Coinstantine.Droid.UIServices
{
    [RegisterInterfaceAsDynamic]
    public class MenuManager : IMenuManager
    {
        public Task ShowMenuFrom(MenuViewModel context, TouchLocation touchLocation)
        {
            return Task.FromResult(0);
        }

        private readonly IMvxMainThreadAsyncDispatcher _threadAsyncDispatcher;
        private const string Tag = "MenuManager";
        readonly object _locker = new object();
        private Activity _currentContext;

        public MenuManager(IMvxMainThreadAsyncDispatcher threadAsyncDispatcher)
        {
            _threadAsyncDispatcher = threadAsyncDispatcher;
        }

        public Task HideMenu()
        {
            return HideFragment();
        }

        public async Task ShowMenu(MenuViewModel dataContext)
        {
            await _threadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                lock (_locker)
                {
                    _currentContext = CrossCurrentActivity.Current.Activity;
                    var fragment = new FloatingMenuFragment(_currentContext, dataContext, HideFragment);
                    fragment.Show(CrossCurrentActivity.Current.Activity.FragmentManager, Tag);
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
                            _currentContext.FragmentManager.ExecutePendingTransactions();
                            var dialogFragment = (DialogFragment)_currentContext.FragmentManager.FindFragmentByTag(Tag);
                            if (dialogFragment != null)
                            {
                                dialogFragment.DismissAllowingStateLoss();
                                fragmentTransaction.Remove(dialogFragment);
                            }
                            _currentContext = null;
                        }
                    }
                }
            });
        }
    }
}