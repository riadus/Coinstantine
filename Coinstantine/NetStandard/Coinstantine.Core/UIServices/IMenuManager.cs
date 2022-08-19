using System.Threading.Tasks;
using Coinstantine.Core.ViewModels;

namespace Coinstantine.Core.UIServices
{
    public interface IMenuManager
    {
		Task ShowMenu(MenuViewModel context);
		Task HideMenu();
		Task ShowMenuFrom(MenuViewModel context, TouchLocation touchLocation);
    }
}
