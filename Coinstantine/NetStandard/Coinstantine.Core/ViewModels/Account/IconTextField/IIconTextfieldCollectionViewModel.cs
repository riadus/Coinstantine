using System.Threading.Tasks;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.IconTextField
{
    public interface IIconTextfieldCollectionViewModel : IMvxViewModel, IMvxNotifyPropertyChanged
    {
        MvxObservableCollection<IIconTextfieldViewModel> IconTextFields { get; set; }
        Task<bool> ValidateForm();
        string Error { get; }
        string ButtonText { get; }
        IMvxCommand ButtonCommand { get; }
        bool ButtonVisible { get; }
    }

    public interface IIconTextfieldCollectionViewModel<T> : IIconTextfieldCollectionViewModel
    {
        T SetData(T accountCreationModel);
    }
}
