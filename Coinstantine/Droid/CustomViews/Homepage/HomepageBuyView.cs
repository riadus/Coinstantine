using System;
using Android;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content.Res;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.Converters;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.CustomViews.Homepage
{
    public class HomepageBuyView : BindableLinearLayout, IDataContextProvider
    {
        protected TextView CharacterInfo { get; set; }
        protected TextView InformationTextView { get; set; }

        protected TextView CsnValueTextView { get; set; }
        protected TextView CsnPriceInEth { get; set; }
        protected TextView CsnPriceInDollar { get; set; }

        protected ConversionView ConversionView { get; set; }

        protected View BonusContainer { get; set; }
        protected TextView BonusLabel { get; set; }

        protected TextView TotalTextView { get; set; }
        protected TextView TotalEth { get; set; }
        protected TextView TotalCsn { get; set; }

        protected Button BuyButton { get; set; }

        public HomepageBuyView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public HomepageBuyView(Context context) : base(context)
        {
            Initialize();
            this.DelayBind(SetBindings);
        }

        private void Initialize()
        {
            Inflate(Context, Resource.Layout.homepage_buyview, this);

            CharacterInfo = FindViewById<TextView>(Resource.Id.homepage_buyview_info_character);
            InformationTextView = FindViewById<TextView>(Resource.Id.homepage_buyview_information);
            CsnValueTextView = FindViewById<TextView>(Resource.Id.homepage_buyview_csn_value);
            CsnPriceInEth = FindViewById<TextView>(Resource.Id.homepage_buyview_csn_price_eth);
            CsnPriceInDollar = FindViewById<TextView>(Resource.Id.homepage_buyview_csn_price_dollar);
            ConversionView = FindViewById<ConversionView>(Resource.Id.homepage_buyview_conversion_container);
            BonusContainer = FindViewById<View>(Resource.Id.homepage_buyview_bonus_view_container);
            BonusLabel = FindViewById<TextView>(Resource.Id.homepage_buyview_bonus);
            TotalTextView = FindViewById<TextView>(Resource.Id.homepage_buyview_total);
            TotalEth = FindViewById<TextView>(Resource.Id.homepage_buyview_total_csn);
            TotalCsn = FindViewById<TextView>(Resource.Id.homepage_buyview_total_eth);
            BuyButton = FindViewById<Button>(Resource.Id.homepage_buyview_buy_button);

            var drawable = ResourcesCompat.GetDrawable(Resources, Resource.Drawable.bonus_background, null);
            drawable.SetColorFilter(AppColorDefinition.SecondaryColor.ToAndroidColor(), PorterDuff.Mode.SrcIn);
            drawable.SetAlpha(127);
            BonusContainer.Background = drawable;
            BuyButton.ToCircle();
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<HomepageBuyView, BuyViewModel>();

            bindingSet.Bind(CharacterInfo)
                      .For("AppString")
                      .To(vm => vm.InfoCharacter);
            bindingSet.Bind(CharacterInfo)
                      .For("TextColor")
                      .To(vm => vm.Status)
                      .WithConversion(new StatusToColorConverter(c => c.ToAndroidColor()));

            bindingSet.Bind(InformationTextView)
                      .To(vm => vm.InfoLabel);

            bindingSet.Bind(CsnValueTextView)
                      .To(vm => vm.CsnValueText);
            bindingSet.Bind(CsnPriceInEth)
                    .To(vm => vm.CsnPriceInETHText);
            bindingSet.Bind(CsnPriceInDollar)
                      .To(vm => vm.CsnPriceDollarText);

            bindingSet.Bind(ConversionView.ConversionEditText1)
                      .For(v => v.Hint)
                      .To(vm => vm.CoinstantineAmountText);
            bindingSet.Bind(ConversionView.ConversionEditText1)
                      .To(vm => vm.CoinstantineAmount);

            bindingSet.Bind(ConversionView.ConversionEditText2)
                     .For(v => v.Hint)
                     .To(vm => vm.CoinstantineCostText);
            bindingSet.Bind(ConversionView.ConversionEditText2)
                      .To(vm => vm.CoinstantineCost);

            bindingSet.Bind(ConversionView)
                      .For(v => v.GeneralColor)
                      .To(vm => vm.CorrectInput)
                      .WithConversion(new BoolToColorConverter(AppColorDefinition.MainBlue, AppColorDefinition.Error, c => c.ToAndroidColor()));

            bindingSet.Bind(BonusLabel)
                      .To(vm => vm.BonusText);

            bindingSet.Bind(TotalTextView)
                      .To(vm => vm.TotalLabel);
            bindingSet.Bind(TotalEth)
                      .To(vm => vm.TotalInETHText);
            bindingSet.Bind(TotalCsn)
                      .To(vm => vm.TotalCsnText);

            bindingSet.Bind(BuyButton)
                    .To(vm => vm.BuyCommand);
            bindingSet.Bind(BuyButton)
                      .For("CSN")
                      .To(vm => vm.BuyCsnText);

            bindingSet.Apply();
        }
    }
}