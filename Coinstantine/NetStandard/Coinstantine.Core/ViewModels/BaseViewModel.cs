using System;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels.Messages;
using Coinstantine.Domain.Interfaces.Translations;
using Coinstantine.Domain.Messages;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, IBaseViewModel
	{
		protected IAppServices AppServices { get; }
		protected MvxSubscriptionToken _weakReferenceToTranslationsLoadedMessage;
		public bool HasMenu { get; protected set; }

        public string MenuText => Translate(TranslationKeys.MainMenu.Menu);
        public string MenuIcon => "ellipsis-v";
        public string SyncIcon => "sync-alt";

		public IMvxCommand OpenMenu { get; protected set; }
		public IMvxCommand<TouchLocation> OpenMenuFrom { get; }

		protected INavigationService AppNavigationService => AppServices.NavigationService;
		protected IMenuManager MenuManager => AppServices.MenuManager;

		protected BaseViewModel(IAppServices appServices)
		{
			AppServices = appServices;
			var messenger = Mvx.Resolve<IMvxMessenger>();
            _weakReferenceToTranslationsLoadedMessage = messenger.Subscribe<TranslationsLoadedMessage>(obj =>
            {
                RaisePropertyChanged(nameof(Title));
                RaiseAllPropertiesChanged();
            });

			HasMenu = false;
			OpenMenu = new MvxCommand(ShowMenu);
			OpenMenuFrom = new MvxCommand<TouchLocation>(ShowMenuFrom);
            SyncCommand = new MvxAsyncCommand(Sync);
		}

        public override void ViewAppearing()
        {
            base.ViewAppearing();
        }

        protected void TrackPage(string page)
        {
            AppServices.AnalyticsTracker.TrackAppPage(page);
        }

		private void ShowMenu()
		{
			var menuViewModel = Mvx.Resolve<MenuViewModel>();
            menuViewModel.Start();
			MenuManager.ShowMenu(menuViewModel);
		}

        private void ShowMenuFrom(TouchLocation touchLocation)
		{
			var menuViewModel = Mvx.Resolve<MenuViewModel>();
            menuViewModel.Start();
			MenuManager.ShowMenuFrom(menuViewModel, touchLocation);
		}

        protected async Task Sync()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            Wait(TranslationKeys.General.Syncing);
            await AppServices.SyncService.ForceSync().ConfigureAwait(false);
            DismissWaitMessage();
        }

        protected async Task SyncIfNeeded()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            if (await AppServices.SyncService.NeedsToSync())
            {
                Wait(TranslationKeys.General.Syncing);
                await AppServices.SyncService.SyncIfNeeded().ConfigureAwait(false);
                DismissWaitMessage();
            }
        }

        protected string Translate(TranslationKey key)
		{
			return AppServices.TranslationService.Translate(key);
		}

        protected void Wait(string message)
        {
            AppServices.OverlayService.Wait(message);
        }

        protected void Wait(TranslationKey message)
        {
            AppServices.OverlayService.Wait(Translate(message));
        }

        protected void UpdateMessage(string message)
        {
            AppServices.OverlayService.UpdateMessage(message);
        }

        protected void DismissWaitMessage()
        {
            AppServices.OverlayService.Dismiss();
        }

        protected void ShowNoConnectionMessage()
		{
			Alert(TranslationKeys.General.NoInternet);
		}

        protected void Alert(string message)
        {
            AppServices.MessageService.Alert(new MessageViewModel(message, DismissAlert, null, Ok, null));
        }

        protected void Alert(TranslationKey message, string argument)
        {
            AppServices.MessageService.Alert(new MessageViewModel(string.Format(Translate(message), argument), DismissAlert, null, Ok, null));
        }

        protected void Alert(TranslationKey translationKey)
        {
            Alert(Translate(translationKey));
        }
        protected string Ok => Translate(TranslationKeys.General.Ok);
        protected string Cancel => Translate(TranslationKeys.General.Cancel);

        public virtual TranslationKey Title { get; set; }

        public string TitleIcon { get; protected set; }

        public IMvxCommand SyncCommand { get; }

        protected void Alert(string message, Action okAction)
        {
            AppServices.MessageService.Alert(new MessageViewModel(message, okAction, DismissAlert, Ok, Cancel));
        }

        protected void AlertWithAction(TranslationKey message, Action okAction)
        {
            AppServices.MessageService.Alert(new MessageViewModel(Translate(message), () => { DismissAlert(); okAction(); }, null, Ok, null));
        }

        protected void Alert(TranslationKey message, Action okAction, TranslationKey okString, TranslationKey cancelString)
        {
            AppServices.MessageService.Alert(new MessageViewModel(Translate(message), okAction, DismissAlert, Translate(okString), Translate(cancelString)));
        }

        protected void Alert(string message, Action okAction, TranslationKey okString, TranslationKey cancelString)
        {
            AppServices.MessageService.Alert(new MessageViewModel(message, okAction, DismissAlert, Translate(okString), Translate(cancelString)));
        }

        protected void DismissAlert()
        {
            AppServices.MessageService.Dismiss();
        }

        protected bool CheckConnectivity()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                Alert(TranslationKeys.General.NoInternet);
                return false;
            }
            return true;
        }

        protected bool CheckConnectivitySilently()
        {
            return AppServices.Connectivity.IsConnected;
        }

        public string GetTitle()
        {
            return Translate(Title);
        }

        protected void Close(IMvxViewModel viewmodel)
        {
            ChangePresentation(new MvxClosePresentationHint(viewmodel));
        }

        protected void ChangePresentation(MvxPresentationHint mvxPresentationHint)
        {
            AppServices.MvxNavigationService.ChangePresentation(mvxPresentationHint);
        }

        protected string FormatM(DateTime dateTime)
        {
            return AppServices.StringFormatter.FormatDateM(dateTime);
        }

        protected string FormatD(DateTime dateTime)
        {
            return AppServices.StringFormatter.FormatDateD(dateTime);
        }

        protected string FormatF(DateTime dateTime)
        {
            return AppServices.StringFormatter.FormatDateF(dateTime);
        }

        protected string FormatT(DateTime dateTime)
        {
            return AppServices.StringFormatter.FormatDateT(dateTime);
        }
    }

    public abstract class BaseViewModel<TInit> : BaseViewModel, IMvxViewModel<TInit>
    {
        protected BaseViewModel(IAppServices appServices)
            : base(appServices) { }

        public abstract void Prepare(TInit parameter);

        public override void ViewAppearing()
        {
            base.ViewAppearing();
        }
    }

    public interface IBaseViewModel : IMvxViewModel, IMvxNotifyPropertyChanged
	{
		bool HasMenu { get; }
		string MenuText { get; }
        string MenuIcon { get; }
        string SyncIcon { get; }
        IMvxCommand OpenMenu { get; }
        IMvxCommand SyncCommand { get; }
		IMvxCommand<TouchLocation> OpenMenuFrom { get; }
        TranslationKey Title { get; }
        string GetTitle();
        string TitleIcon { get; }
	}

	public class TouchLocation
	{
		public TouchLocation(float x, float y)
		{
			X = x;
			Y = y;
		}

		public float X { get; }
		public float Y { get; }
	}
}
