using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Util;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.Homepage
{
    public class PurchaseListItem : BindableConstraintLayout
    {
        public TextView PurchasePhase { get; private set; }
        public TextView Amount { get; private set; }
        public TextView Cost { get; private set; }
        public TextView PurchaseDate { get; private set; }

        public TextView Status { get; private set; }
        public TextView IconStatus { get; private set; }
        public Button StatusButton { get; private set; }

        public PurchaseListItem(Context context) : base(context)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        public PurchaseListItem(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.homepage_purchase_listitem, this);
            PurchasePhase = FindViewById<TextView>(Resource.Id.homepagePurchaseItemPhase);
            Amount = FindViewById<TextView>(Resource.Id.homepagePurchaseItemAmount);
            Cost = FindViewById<TextView>(Resource.Id.homepagePurchaseItemCost);
            PurchaseDate = FindViewById<TextView>(Resource.Id.homepagePurchaseItemDateDetails);
            Status = FindViewById<TextView>(Resource.Id.homepagePurchaseStatusText);
            IconStatus = FindViewById<TextView>(Resource.Id.homepagePurchaseStatusIcon);
            StatusButton = FindViewById<Button>(Resource.Id.homepagePurchaseButton);

            var backgroundView = FindViewById<ConstraintLayout>(Resource.Id.homepagePurchaseTopContainer);
            IconStatus.ToCircle(Context, 30.ToDP(Context), AppColorDefinition.MainBlue.ToAndroidColor());
            backgroundView.SetBackgroundResource(Resource.Drawable.homepage_airdrop_list_item_background);
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<PurchaseListItem, PurchaseItemViewModel>();

            bindingSet.Bind(PurchasePhase)
                      .To(vm => vm.PurchasePhase).WithClearBindingKey("PurchasePhaseBinding");
            bindingSet.Bind(Amount)
                      .To(vm => vm.AmountBoughtStr).WithClearBindingKey("AmountBoughtStrBinding");
            bindingSet.Bind(Cost)
                      .To(vm => vm.CostStr).WithClearBindingKey("CostStrBinding");
            bindingSet.Bind(PurchaseDate)
                      .To(vm => vm.PurchaseDateStr).WithClearBindingKey("PurchaseDateStrBinding");
            bindingSet.Bind(Status)
                      .To(vm => vm.ButtonText).WithClearBindingKey("ButtonTextBinding");
            bindingSet.Bind(IconStatus)
                      .For("AppString")
                      .To(vm => vm.StatusLabel).WithClearBindingKey("StatusLabelBinding");
            bindingSet.Bind(StatusButton)
                      .To(vm => vm.SelectedCommand).WithClearBindingKey("SelectedCommandBinding");

            bindingSet.Apply();
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
                TryClearBindigs("PurchasePhaseBinding");
                TryClearBindigs("AmountBoughtStrBinding");
                TryClearBindigs("CostStrBinding");
                TryClearBindigs("PurchaseDateStrBinding");
                TryClearBindigs("ButtonTextBinding");
                TryClearBindigs("StatusLabelBinding");
                TryClearBindigs("SelectedCommandBinding");
            }
            base.Dispose(disposing);
        }
    }
}
