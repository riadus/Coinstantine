using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Constraints;
using Android.Support.V4.Content.Res;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Messages;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace Coinstantine.Droid.Fragments
{
    public class AppDialogFragment : MvxDialogFragment
    {
        private TextView MessageTextView { get; set; }
        private Button OkAloneButton { get; set; }
        private Button OkButton { get; set; }
        private Button CancelButton { get; set; }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<AppDialogFragment, MessageViewModel>();

            bindingSet.Bind(MessageTextView)
                      .To(vm => vm.Message);

            bindingSet.Bind(OkAloneButton)
                      .To(vm => vm.OkCommand);
            bindingSet.Bind(OkAloneButton)
                      .For(v => v.Text)
                      .To(vm => vm.OkText);
            bindingSet.Bind(OkAloneButton)
                      .For(v => v.Visibility)
                      .To(vm => vm.WithCancel)
                      .WithVisibilityConversion(true);

            bindingSet.Bind(OkButton)
                      .To(vm => vm.OkCommand);
            bindingSet.Bind(OkButton)
                      .For(v => v.Text)
                      .To(vm => vm.OkText);
            bindingSet.Bind(OkButton)
                      .For(v => v.Visibility)
                      .To(vm => vm.WithCancel)
                      .WithVisibilityConversion();
            var vim = ViewModel;
            bindingSet.Bind(CancelButton)
                      .To(vm => vm.CancelCommand);
            bindingSet.Bind(CancelButton)
                      .For(v => v.Text)
                      .To(vm => vm.CancelText);
            bindingSet.Bind(CancelButton)
                      .For(v => v.Visibility)
                      .To(vm => vm.WithCancel)
                      .WithVisibilityConversion();

            bindingSet.Apply();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.EnsureBindingContextIsSet(inflater);
            var view = inflater.Inflate(Resource.Layout.app_dialog_fragment, container, false);
            var layoutContainer = view.FindViewById<ConstraintLayout>(Resource.Id.app_dialog_container);
            MessageTextView = view.FindViewById<TextView>(Resource.Id.app_dialog_message);
            OkAloneButton = view.FindViewById<Button>(Resource.Id.app_dialog_ok_button_alone);
            OkButton = view.FindViewById<Button>(Resource.Id.app_dialog_ok_button);
            CancelButton = view.FindViewById<Button>(Resource.Id.app_dialog_cancel_button);
            SetBindings();

            layoutContainer.SetCornerRadius(Context, Color.White);
            OkAloneButton.SetCornerRadius(Context, AppColorDefinition.MainBlue.ToAndroidColor());
            OkButton.SetCornerRadius(Context, AppColorDefinition.MainBlue.ToAndroidColor());
            CancelButton.SetCornerRadius(Context, Color.LightGray);
            return view;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            var dialog = new AppDialog(Context);
            return dialog;
        }

        public override void OnStart()
        {
            base.OnStart();
            if (Dialog != null)
            {
                var backgroundColor = new ColorDrawable(Color.Black);
                backgroundColor.SetAlpha(55);
                Dialog.Window.SetBackgroundDrawable(backgroundColor);
                Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
                this.DelayBind(SetBindings);
            }
        }

        public override void Show(FragmentManager manager, string tag)
        {
            using (var transaction = manager?.BeginTransaction())
            {
                try
                {
                    if (transaction != null)
                    {
                        transaction.Add(this, tag);
                        transaction.CommitAllowingStateLoss();
                    }
                }
                catch
                {

                }
            }
        }
    }

    public class AppDialog : Dialog
    {
        public AppDialog(Context context) : base(context)
        {
            Window.RequestFeature(WindowFeatures.NoTitle);
        }

        public override void OnBackPressed()
        {
        }
    }
}