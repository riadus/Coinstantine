using MvvmCross.Binding.BindingContext;
using Coinstantine.Core.Converters;
using Coinstantine.Droid.Converters;

namespace Coinstantine.Droid.Extensions
{
    public static class BindingExtensions
    {
        public static MvxFluentBindingDescription<TOwningTarget, TSource> WithRevertedConversion<TOwningTarget, TSource>(this MvxFluentBindingDescription<TOwningTarget, TSource> self) where TOwningTarget : class
        {
            return self.WithConversion(new NotConverter());
        }

        public static MvxFluentBindingDescription<TOwningTarget, TSource> WithVisibilityConversion<TOwningTarget, TSource>(this MvxFluentBindingDescription<TOwningTarget, TSource> self, bool reverted = false) where TOwningTarget : class
        {
            return self.WithConversion(new BoolToAndroidVisibilityConverter(reverted));
        }

        public static MvxFluentBindingDescription<TOwningTarget, TSource> WithVisibilityGoneConversion<TOwningTarget, TSource>(this MvxFluentBindingDescription<TOwningTarget, TSource> self) where TOwningTarget : class
        {
            return self.WithConversion(new BoolToGoneVisibilityConverter());
        }

        public static MvxFluentBindingDescription<TOwningTarget, TSource> WithAndroidColorConversion<TOwningTarget, TSource>(this MvxFluentBindingDescription<TOwningTarget, TSource> self) where TOwningTarget : class
        {
            return self.WithConversion(new AndroidColorConverter());
        }
    }
}
