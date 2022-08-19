using System.Reflection;
using MvvmCross.Binding;
using MvvmCross.Binding.Bindings.Target;

namespace Coinstantine.iOS.CustomBindings
{
    public class IconTextfieldBinding : MvxPropertyInfoTargetBinding<IconTextField>
    {
        public IconTextfieldBinding(object target, PropertyInfo targetPropertyInfo) : base(target, targetPropertyInfo)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public override void SubscribeToEvents()
        {
            base.SubscribeToEvents();
            View.GrossHiddenValueChanged += View_GrossHiddenValueChanged;
        }

        void View_GrossHiddenValueChanged(object sender, System.EventArgs e)
        {
            FireValueChanged(View.GrossHiddenValue);
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                View.GrossHiddenValueChanged -= View_GrossHiddenValueChanged;
            }
        }
    }
}
