using Coinstantine.Core.Services;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels
{
    public class MenuItemViewModel : BaseViewModel
    {
        public MenuItemViewModel(IAppServices appServices) : base(appServices)
        {
        }

        public string IconText { get; set; }
        public string ImageUrl { get; set; }
        public bool HasIcon { get; set; }
        public string Text { get; set; }
        public IMvxCommand SelectionCommand { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
