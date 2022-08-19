using System.Threading.Tasks;
using Android.App;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels.Messages;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using Coinstantine.Droid.Fragments;
using MvvmCross.Base;
using MvvmCross.ViewModels;
using Plugin.CurrentActivity;

namespace Coinstantine.Droid.UIServices
{
    [RegisterInterfaceAsDynamic]
    public class MessageService : IMessageService
    {
        private readonly IMvxMainThreadAsyncDispatcher _mvxMainThreadAsyncDispatcher;
        private readonly ITranslationService _translationService;
        private const string Tag = "MessageService";
        readonly object _locker = new object();
        private Activity _currentContext;

        public MessageService(IMvxMainThreadAsyncDispatcher mvxMainThreadAsyncDispatcher,
                              ITranslationService translationService)
        {
            _mvxMainThreadAsyncDispatcher = mvxMainThreadAsyncDispatcher;
            _translationService = translationService;
        }

        public void Alert(MessageViewModel context)
        {
            MvxNotifyTask.Create(Show(context));
        }

        public void Dismiss()
        {
            MvxNotifyTask.Create(HideFragment());
        }

        private async Task Show(MessageViewModel context)
        {
            await _mvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                lock (_locker)
                {
                    var fragment = new AppDialogFragment
                    {
                        DataContext = context
                    };
                    fragment.Show(CrossCurrentActivity.Current.Activity.FragmentManager, Tag);
                    _currentContext = CrossCurrentActivity.Current.Activity;
                }
            });
        }

        private async Task HideFragment()
        {
            await _mvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
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

        public void Alert(TranslationKey translationKey)
        {
            Alert(_translationService.Translate(translationKey));
        }

        public void Alert(string message)
        {
            Alert(new MessageViewModel(message, Dismiss, null, _translationService.Translate(TranslationKeys.General.Ok), null));
        }
    }
}