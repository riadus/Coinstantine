using System;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Messages;
using Coinstantine.iOS.Views.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class MessageView : MvxView
    {
        private bool _bound;

        public MessageView (IntPtr handle) : base (handle)
        {
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            OkButton.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            OkButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            OkButton.TitleLabel.MinimumScaleFactor = 0.3f;

            CancelButton.SetTitleColor(AppColorDefinition.MainBlue.ToUIColor(), UIKit.UIControlState.Normal);
            CancelButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            CancelButton.TitleLabel.MinimumScaleFactor = 0.3f;

            OkAloneButton.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            OkAloneButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
            OkAloneButton.TitleLabel.MinimumScaleFactor = 0.3f;

            if (!_bound)
            {
                SetBindings();
                _bound = true;
            }
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<MessageView, MessageViewModel>();

            bindingSet.Bind(MessageLabel)
                      .To(vm => vm.Message);
            bindingSet.Bind(OkButton)
                      .For("Title")
                      .To(vm => vm.OkText);
            bindingSet.Bind(OkButton)
                      .To(vm => vm.OkCommand);
            bindingSet.Bind(OkButton)
                      .For(v => v.Hidden)
                      .To(vm => vm.WithCancel)
                      .WithRevertedConversion();

            bindingSet.Bind(OkAloneButton)
                      .For("Title")
                      .To(vm => vm.OkText);
            bindingSet.Bind(OkAloneButton)
                      .To(vm => vm.OkCommand);
            bindingSet.Bind(OkAloneButton)
                      .For(v => v.Hidden)
                      .To(vm => vm.WithCancel);
            
            bindingSet.Bind(CancelButton)
                      .For("Title")
                      .To(vm => vm.CancelText);
            bindingSet.Bind(CancelButton)
                      .To(vm => vm.CancelCommand);
            bindingSet.Bind(CancelButton)
                      .For(v => v.Hidden)
                      .To(vm => vm.WithCancel)
                      .WithRevertedConversion();

            bindingSet.Apply();
        }
    }
}