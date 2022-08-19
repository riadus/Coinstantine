using System.ComponentModel;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Generic
{
    public interface IGenericInfoViewModel : IBaseViewModel, INotifyPropertyChanged
    {
        string InfoTitle { get; }
        MvxObservableCollection<GenericInfoItemViewModel> GenericInfoItems { get; }

        IMvxCommand PrincipalButtonCommand { get; }
        IMvxCommand SecondaryButtonCommand { get; }
        IMvxCommand RefreshCommand { get; }
        bool HasRefreshingCapability { get; }

        bool IsRefreshing { get; }
        string RefreshText { get; }

        bool ShowSecondaryButton { get; }
        string SecondaryButtonText { get; }

        string RemainingTime { get; }
        bool StillTimeToEdit { get; }

        bool ShowRegularBehaviourText { get; }
        bool ShowPrincipalButton { get; }
        string PrincipalButtonText { get; }

        bool EnabledAction { get; }
    }
}
