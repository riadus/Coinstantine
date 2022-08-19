using Coinstantine.Core.Converters;
using Coinstantine.iOS.Converters;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.iOS.Views.Extensions
{
    public static class Extensions
    {
        public static MvxFluentBindingDescription<TOwningTarget, TSource> WithRevertedConversion<TOwningTarget, TSource>(this MvxFluentBindingDescription<TOwningTarget, TSource> self) where TOwningTarget : class
        {
            return self.WithConversion(new NotConverter());
        }

        public static MvxFluentBindingDescription<TOwningTarget, TSource> WithUIColorConversion<TOwningTarget, TSource>(this MvxFluentBindingDescription<TOwningTarget, TSource> self) where TOwningTarget : class
        {
            return self.WithConversion(new AppColorConverter());
        }
    }
}
