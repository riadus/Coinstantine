using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Support.V7.App;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using Plugin.CurrentActivity;

namespace Coinstantine.Droid.UIServices
{
    [RegisterInterfaceAsDynamic]
    public class UserOptionService : IUserOptionService
    {
        private readonly ITranslationService _translationService;

        public UserOptionService(ITranslationService translationService)
        {
            _translationService = translationService;
        }
        public Task<UserOption<T>> ShowOptions<T>(string message, string cancel, IEnumerable<UserOption<T>> options)
        {
            DismissEventualActionSheets();
            var result = new TaskCompletionSource<UserOption<T>>();
            _currentAlertDialog = new AlertDialog.Builder(CrossCurrentActivity.Current.Activity)
                                         .SetTitle(message)
                                         .SetItems(options.Select(x => x.Label).ToArray(), (o, args) => result.TrySetResult(options.ElementAt(args.Which)))
                                         .SetPositiveButton(cancel, (sender, e) => { })
                                         .Create();
            _currentAlertDialog.SetCancelable(true);
            _currentAlertDialog.SetCanceledOnTouchOutside(true);
            _currentAlertDialog.Show();

            return result.Task;
        }

        public void DismissEventualActionSheets()
        {
            _currentAlertDialog?.Dismiss();
        }

        public async Task<ShareOption> ShowShareOptions(string message, string cancel)
        {
            var options = new List<UserOption<ShareOption>>
            {
                new UserOption<ShareOption>(ShareOption.Send, _translationService.Translate(TranslationKeys.Home.SendShareOption)),
                new UserOption<ShareOption>(ShareOption.Copy, _translationService.Translate(TranslationKeys.Home.CopyShareOption))
            };
            var result = await ShowOptions(message, cancel, options);
            return result.Value;
        }

        private AlertDialog _currentAlertDialog;

    }
}