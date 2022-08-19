using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Settings;
using Coinstantine.Droid.Converters;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews
{
    public class PincodeView : BindableLinearLayout
    {
        public PincodeView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        private PincodeViewModel ViewModel => DataContext as PincodeViewModel;

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<PincodeView, PincodeViewModel>();

            bindingSet.Bind(LockLabel)
                      .For("AppString")
                      .To(vm => vm.LockLabel);

            bindingSet.Bind(Input1)
                      .For("Pin")
                      .To(vm => vm.Pin1Label)
                      .WithConversion(new PinConverter());

            bindingSet.Bind(Input2)
                      .For("Pin")
                      .To(vm => vm.Pin2Label)
                      .WithConversion(new PinConverter());

            bindingSet.Bind(Input3)
                      .For("Pin")
                      .To(vm => vm.Pin3Label)
                      .WithConversion(new PinConverter());

            bindingSet.Bind(Input4)
                      .For("Pin")
                      .To(vm => vm.Pin4Label)
                      .WithConversion(new PinConverter());

            bindingSet.Bind(InformationLabel)
                      .To(vm => vm.InformationLabel);

            bindingSet.Bind(Pin0)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin0);

            bindingSet.Bind(Pin1)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin1);

            bindingSet.Bind(Pin2)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin2);

            bindingSet.Bind(Pin3)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin3);

            bindingSet.Bind(Pin4)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin4);

            bindingSet.Bind(Pin5)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin5);

            bindingSet.Bind(Pin6)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin6);

            bindingSet.Bind(Pin7)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin7);

            bindingSet.Bind(Pin8)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin8);

            bindingSet.Bind(Pin9)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Pin9);

            bindingSet.Bind(PinFingerprint)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.FingerPrint);

            bindingSet.Bind(PinDel)
                      .To(vm => vm.PinCommand)
                      .CommandParameter(PincodeViewModel.Pin.Del);
            bindingSet.Bind(PinDel)
                      .For("AppString")
                      .To(vm => vm.DeleteText);

            bindingSet.Bind(PinFingerprint)
                       .For(v => v.Visibility)
                       .To(vm => vm.ShowFingerPrintButton)
                       .WithVisibilityConversion();

            bindingSet.Bind(PinFingerprint)
                      .For("AppString")
                      .To(vm => vm.FingerPrintText);
            bindingSet.Bind(PinFingerprint)
                      .For("TextColor")
                      .To(vm => vm.FingerPrintEnabled)
                      .WithConversion(new BoolToColorWithDefaultConverter(AppColorDefinition.SecondaryColor.ToAndroidColor(), AppColorDefinition.Red.ToAndroidColor(), AppColorDefinition.MainBlue.ToAndroidColor(), () => ViewModel.FingerPrintToUnlock));

            bindingSet.Apply();

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Error))
            {
                LockLabel.ShakeHorizontally(Context);
            }
        }

        public TextView LockLabel { get; set; }
        public Button Pin1 { get; private set; }
        public Button Pin2 { get; private set; }
        public Button Pin3 { get; private set; }
        public Button Pin4 { get; private set; }
        public Button Pin5 { get; private set; }
        public Button Pin6 { get; private set; }
        public Button Pin7 { get; private set; }
        public Button Pin8 { get; private set; }
        public Button Pin9 { get; private set; }
        public Button Pin0 { get; private set; }
        public Button PinFingerprint { get; private set; }
        public Button PinDel { get; private set; }
        public View Input1 { get; private set; }
        public View Input2 { get; private set; }
        public View Input3 { get; private set; }
        public View Input4 { get; private set; }
        public TextView InformationLabel { get; private set; }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.pincode, this);
            LockLabel = FindViewById<TextView>(Resource.Id.pincode_locker);
            InformationLabel = FindViewById<TextView>(Resource.Id.pincode_information);
            Pin1 = FindViewById<Button>(Resource.Id.pincode_1);
            Pin2 = FindViewById<Button>(Resource.Id.pincode_2);
            Pin3 = FindViewById<Button>(Resource.Id.pincode_3);
            Pin4 = FindViewById<Button>(Resource.Id.pincode_4);
            Pin5 = FindViewById<Button>(Resource.Id.pincode_5);
            Pin6 = FindViewById<Button>(Resource.Id.pincode_6);
            Pin7 = FindViewById<Button>(Resource.Id.pincode_7);
            Pin8 = FindViewById<Button>(Resource.Id.pincode_8);
            Pin9 = FindViewById<Button>(Resource.Id.pincode_9);
            Pin0 = FindViewById<Button>(Resource.Id.pincode_0);
            PinFingerprint = FindViewById<Button>(Resource.Id.pincode_fingerprint);
            PinDel = FindViewById<Button>(Resource.Id.pincode_del);
            Input1 = FindViewById<View>(Resource.Id.pincode_pin_1);
            Input2 = FindViewById<View>(Resource.Id.pincode_pin_2);
            Input3 = FindViewById<View>(Resource.Id.pincode_pin_3);
            Input4 = FindViewById<View>(Resource.Id.pincode_pin_4);
        }
    }
}