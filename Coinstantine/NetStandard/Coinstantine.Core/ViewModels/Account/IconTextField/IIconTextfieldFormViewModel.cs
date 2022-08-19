using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.IconTextField
{
    public interface IIconTextfieldFormViewModel : IBaseViewModel
    {
        MvxObservableCollection<IIconTextfieldCollectionViewModel> IconTextfieldCollectionViewModels { get; set; }
        IIconTextfieldCollectionViewModel CurrentIconTextfieldCollectionViewModel { get; }
        bool IsMultiPage { get; }
        int Count { get; }
        int CurrentIndex { get; }
        IMvxCommand ButtonCommand { get; }
        string ButtonText { get; }
        bool IsLoading { get; }
    }
}
