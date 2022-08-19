using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.Homepage
{
    public class AirdropListItem : BindableConstraintLayout
    {
        public TextView AirdropTitle { get; private set; }
        public TextView OtherInfo { get; private set; }
        public TextView DetailsDate { get; private set;}
        public TextView Amount { get; private set; }
        public TextView Status { get; private set; }
        public TextView IconStatus { get; private set; }
        public Button StatusButton { get; private set; }

        public AirdropListItem(Context context) : base(context)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        public AirdropListItem(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<AirdropListItem, AirdropItemViewModel>();

            bindingSet.Bind(AirdropTitle)
                      .To(vm => vm.AirdropTitle).WithClearBindingKey("TitleBinding");
            bindingSet.Bind(Amount)
                      .To(vm => vm.AmountToAirdropStr).WithClearBindingKey("AmountBinding");
            bindingSet.Bind(DetailsDate)
                      .To(vm => vm.LatestUpdate).WithClearBindingKey("LatestUpdateBinding");
            bindingSet.Bind(IconStatus)
                      .For("AppString")
                      .To(vm => vm.StatusLabel).WithClearBindingKey("IconStatusBinding");
            bindingSet.Bind(Status)
                      .To(vm => vm.ButtonText).WithClearBindingKey("ButtonTextBinding");
            bindingSet.Bind(OtherInfo)
                      .To(vm => vm.AdditionalInfo).WithClearBindingKey("AdditionalInfoBinding");
            bindingSet.Bind(StatusButton)
                      .To(vm => vm.SelectedCommand).WithClearBindingKey("SelectedCommandBinding");

            bindingSet.Apply();

            SetSyles();
        }

        private void SetSyles()
        {
            IconStatus.ToCircle(Context, 30.ToDP(Context), Color.White);
            IconStatus.SetTextSize(ComplexUnitType.Dip, 30);

            if (IconStatus.CurrentTextColor == Color.White)
            {
                IconStatus.ToCircle(Context, 30.ToDP(Context), AppColorDefinition.MainBlue.ToAndroidColor());
                IconStatus.SetTextSize(ComplexUnitType.Dip, 13);
            }
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.homepage_airdrop_listitem, this);
            AirdropTitle = FindViewById<TextView>(Resource.Id.homepageAirdropItemTitle);
            OtherInfo = FindViewById<TextView>(Resource.Id.homepageAirdropItemOtherInfo);
            DetailsDate = FindViewById<TextView>(Resource.Id.homepageAirdropItemDateDetails);
            Amount = FindViewById<TextView>(Resource.Id.homepageAirdropItemAmount);
            Status = FindViewById<TextView>(Resource.Id.homepageAirdropStatusText);
            IconStatus = FindViewById<TextView>(Resource.Id.homepageAirdropStatusIcon);
            StatusButton = FindViewById<Button>(Resource.Id.homepageAirdropButton);
            var backgroundView = FindViewById<ConstraintLayout>(Resource.Id.homepageAirdropTopContainer);
            backgroundView.SetBackgroundResource(Resource.Drawable.homepage_airdrop_list_item_background);
        }

        private void TryClearBindigs(string bindingKey)
        {
            try
            {
                this.ClearBindings(bindingKey);
            }
            catch (Exception)
            { }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TryClearBindigs("TitleBinding");
                TryClearBindigs("AmountBinding");
                TryClearBindigs("LatestUpdateBinding");
                TryClearBindigs("IconStatusBinding");
                TryClearBindigs("ButtonTextBinding");
                TryClearBindigs("AdditionalInfoBinding");
                TryClearBindigs("SelectedCommandBinding");
            }
            base.Dispose(disposing);
        }
    }
}
