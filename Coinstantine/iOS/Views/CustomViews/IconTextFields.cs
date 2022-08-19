using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.iOS.Views.Extensions;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace Coinstantine.iOS
{
    public class IconTextFields : MvxView
    {
        private List<UITextField> _textfields;

        private nfloat _keyboardHeight;
        private TaskCompletionSource<nfloat> _keyboardTsc;
        private const float Transparent = 0.0f;
        private const float NonTransparent = 1.0f;
        private nfloat _width;
        public IconTextFields(nfloat width)
        {
            _width = width;
            _keyboardTsc = new TaskCompletionSource<nfloat>();
            NSObject notification = null;
            notification = UIKeyboard.Notifications.ObserveWillShow((s, e) =>
            {
                var r = UIKeyboard.FrameBeginFromNotification(e.Notification);
                _keyboardTsc.SetResult(r.Height);
                NSNotificationCenter.DefaultCenter.RemoveObserver(notification);
            });
        }

        private IIconTextfieldCollectionViewModel ViewModel => DataContext as IIconTextfieldCollectionViewModel;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _textfields = new List<UITextField>();
            var height = Frame.Height / (ViewModel.IconTextFields.Count() + 1);
            var margin = 30;
            var previousY = height;
            var bindingSet = this.CreateBindingSet<IconTextFields, IIconTextfieldCollectionViewModel>();
            var index = 0;
            foreach (var iconTextfiledViewModel in ViewModel.IconTextFields)
            {
                var textfield = new IconTextField
                {
                    Frame = new CGRect(margin, previousY + margin, _width - 2 * margin, 47),
                    BackgroundColor = UIColor.White,
                    Tag = index,
                    ReturnKeyType = UIReturnKeyType.Next
                };
                textfield.Layer.CornerRadius = 5;
                Add(textfield);
                _textfields.Add(textfield);
                BindField(bindingSet, textfield, index);
                previousY += textfield.Frame.Height + margin;
                index++;
            }
            var button = new UIButton(new CGRect(margin, previousY + margin, _width - 2 * margin, 100));
            button.TitleLabel.Lines = 0;
            button.TitleLabel.TextAlignment = UITextAlignment.Center;
            button.ToRegularStyle();
            button.TitleLabel.Font = UIFont.FromName("Quicksand-Regular", 17);
            var label = new UILabel
            {
                Frame = new CGRect(margin, previousY + margin, _width - 2 * margin, Frame.Height - previousY - (2 * margin)),
                TextColor = UIColor.White,
                TextAlignment = UITextAlignment.Justified,
                Lines = 0,
                AdjustsFontSizeToFitWidth = true,
            };

            bindingSet.Bind(button)
                      .For("Title")
                      .To(vm => vm.ButtonText);
            bindingSet.Bind(button)
                      .For(v => v.Hidden)
                      .To(vm => vm.ButtonVisible)
                      .WithRevertedConversion();
            bindingSet.Bind(button)
                      .To(vm => vm.ButtonCommand);

            bindingSet.Bind(label)
                .To(vm => vm.Error);
            Add(label);
            Add(button);
            bindingSet.Apply();

            _textfields.ForEach(AnimateContactField);
        }

        private void BindField(MvxFluentBindingDescriptionSet<IconTextFields, IIconTextfieldCollectionViewModel> bindingSet, IconTextField iconTextField, int index)
        {
            bindingSet.Bind(iconTextField)
                      .To(vm => vm.IconTextFields[index].Value);
            bindingSet.Bind(iconTextField)
                      .For("AppString")
                      .To(vm => vm.IconTextFields[index].Icon);
            bindingSet.Bind(iconTextField)
                      .For(v => v.Placeholder)
                      .To(vm => vm.IconTextFields[index].Placeholder);
            bindingSet.Bind(iconTextField)
                       .For(v => v.Type)
                       .To(vm => vm.IconTextFields[index].Type);
            bindingSet.Bind(iconTextField)
                       .For(v => v.IsError)
                       .To(vm => vm.IconTextFields[index].IsError);
            bindingSet.Bind(iconTextField)
                   .For("GrossHiddenValue")
                   .To(vm => vm.IconTextFields[index].GrossValue);
            bindingSet.Bind(iconTextField)
                  .For(v => v.Items)
                  .To(vm => vm.IconTextFields[index].Items);
        }

        private void AnimateContactField(UITextField tf)
        {
            tf.ShouldReturn += TextFieldShouldReturn;
            var originalFrame = tf.Frame;
            tf.EditingDidBegin += async (sender, e) =>
            {
                _keyboardHeight = _keyboardHeight > 0 ? _keyboardHeight : await GetKeyboardHeight();
                await AnimateAsync(0.6, () =>
                {
                    foreach (var textfield in _textfields)
                    {
                        if (!tf.Equals(textfield))
                        {
                            textfield.Alpha = Transparent;
                            textfield.Hidden = true;
                        }
                    }
                    var y = (Frame.Height - _keyboardHeight) / 2;
                    var rect = tf.Frame;
                    rect.Y = y;
                    tf.Frame = rect;
                });
            };

            tf.EditingDidEnd += (sender, e) =>
            {
                Animate(0.4, () =>
                {
                    foreach (var textfield in _textfields)
                    {
                        textfield.Alpha = NonTransparent;
                        textfield.Hidden = false;
                    }
                });

                Animate(0.6, 0, UIViewAnimationOptions.CurveEaseOut, () =>
                {
                    var rect = tf.Frame;
                    rect.Y = originalFrame.Y;
                    tf.Frame = rect;
                }, null);
            };
        }

        bool TextFieldShouldReturn(UITextField textfield)
        {
            nint nextTag = textfield.Tag + 1;
            UIResponder nextResponder = ViewWithTag(nextTag);
            if (nextResponder != null)
            {
                nextResponder.BecomeFirstResponder();
            }
            else
            {
                textfield.ResignFirstResponder();
            }
            return false;
        }

        private async Task<nfloat> GetKeyboardHeight()
        {
            if (_keyboardHeight == 0)
            {
                _keyboardHeight = await _keyboardTsc.Task.ConfigureAwait(false);
            }
            return _keyboardHeight;
        }

        public void Release()
        {
            _textfields.ForEach(x => x.ResignFirstResponder());
        }
    }
}