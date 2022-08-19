using System;
using System.Collections.Generic;
using System.Linq;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Domain.Airdrops;
using MvvmCross;

namespace Coinstantine.Core.Services
{
    [RegisterInterfaceAsDynamic]
    public class GenericInfoItemViewModelConstructor : IGenericInfoItemViewModelConstructor
    {
        public IEnumerable<GenericInfoItemViewModel> Construct(IEnumerable<ItemInfo> items, Type valueType)
        {
            var vm = Mvx.Resolve<GenericInfoItemViewModel>();
            var initialized = false;
            foreach (var item in items)
            {
                if (item.Display == Display.Grouped)
                {
                    if (valueType != null && valueType == typeof(bool))
                    {
                        vm.Value3 = item.Value;
                    }
                    else
                    {
                        vm.TranslatableTitle2 = item.Title;
                        vm.Value2 = item.Value;
                    }
                    initialized = false;
                    yield return vm;
                }
                else
                {
                    if (initialized && vm.Title1.IsNotNull())
                    {                        
                        yield return vm;
                    }
                    initialized = true;
                    vm = Mvx.Resolve<GenericInfoItemViewModel>();
                    vm.TranslatableTitle1 = item.Title;
                    vm.Value1 = item.Value;
                    vm.IsTitle = item.Display == Display.Title;
                    if (item.Title == items.Last().Title)
                    {
                        yield return vm;
                    }
                }
            }
        }

        public IEnumerable<GenericInfoItemViewModel> Construct(IEnumerable<ItemInfo> items)
        {
            return Construct(items, null);
        }
    }
}

