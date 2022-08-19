using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coinstantine.Core.ViewModels.Account.IconTextField
{
    public interface IIconTextfieldViewModel
    {
        string Icon { get; set; }
        string Placeholder { get; set; }
        string Value { get; set; }
        IconTextFieldType Type { get; set; }
        object GrossValue { get; set; }
        IEnumerable<IconTextfieldItemsViewModel> Items { get; set; }
        Task<bool> Validate();
        string ErrorMessage { get; }
        bool IsError { get; set; }
    }
}