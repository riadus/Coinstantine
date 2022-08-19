using System;
using System.Collections.Generic;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Domain.Airdrops;

namespace Coinstantine.Core.Services
{
    public interface IGenericInfoItemViewModelConstructor
    {
        IEnumerable<GenericInfoItemViewModel> Construct(IEnumerable<ItemInfo> items);
        IEnumerable<GenericInfoItemViewModel> Construct(IEnumerable<ItemInfo> items, Type valueType);
    }
}

