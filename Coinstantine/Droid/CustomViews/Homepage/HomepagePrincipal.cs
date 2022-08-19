using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core.ViewModels;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace Coinstantine.Droid.CustomViews.Homepage
{
    public class HomepagePrincipal : BindableConstraintLayout
    {
        public TextView UsernameTextView { get; private set; }
        public TextView EnvironmentTextView { get; private set; }
        public TextView EthAddressTextView { get; private set; }
        public Button ShareButton { get; private set; }
        public BalanceView CoinstantineBalance { get; private set; }
        public BalanceView EthBalance { get; private set; }
        //public Button GoBuyCsn { get; private set; }
        public MvxListView AirdropList { get; private set; }
        public MvxSwipeRefreshLayout SwipeRefreshList { get; set; }
        public MvxSwipeRefreshLayout SwipeRefreshBalance { get; set; }

        public HomepagePrincipal(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        public HomepagePrincipal(Context context) : base(context)
        {
            this.DelayBind(SetBindings);
            Initialize();
        }

        protected HomepagePrincipal(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private void SetBindings()
        {
            var bindingSet = this.CreateBindingSet<HomepagePrincipal, PrincipalViewModel>();

            AirdropList.Adapter = new GenericAdapter<AirdropItemViewModel, AirdropListItem, PurchaseItemViewModel, PurchaseListItem>(Context, BindingContext, c => new AirdropListItem(c), c => new PurchaseListItem(c));

            bindingSet.Bind(UsernameTextView)
                      .To(vm => vm.UsernameLabel);
            bindingSet.Bind(EthAddressTextView)
                       .To(vm => vm.EthAddress);
            bindingSet.Bind(EnvironmentTextView)
                      .For(v => v.Visibility)
                      .To(vm => vm.ShowEnvironment)
                      .WithVisibilityConversion();
            bindingSet.Bind(EnvironmentTextView)
                      .To(vm => vm.Environment);
            bindingSet.Bind(ShareButton)
                      .For("AppString")
                      .To(vm => vm.ShareButtonText);
            bindingSet.Bind(ShareButton)
                      .To(vm => vm.ShareCommand);
            bindingSet.Bind(CoinstantineBalance)
                      .For(v => v.DataContext)
                      .To(vm => vm.CoinstantineBalance);
            bindingSet.Bind(EthBalance)
                      .For(v => v.DataContext)
                      .To(vm => vm.EtherBalance);
            bindingSet.Bind(SwipeRefreshList)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshListCommand);
            bindingSet.Bind(SwipeRefreshList)
                      .For(v => v.Refreshing)
                      .To(vm => vm.IsListLoading);
            bindingSet.Bind(SwipeRefreshBalance)
                      .For(v => v.Refreshing)
                      .To(vm => vm.IsRefreshing);
            bindingSet.Bind(SwipeRefreshBalance)
                      .For(v => v.RefreshCommand)
                      .To(vm => vm.RefreshCommand);
            /* bindingSet.Bind(GoBuyCsn)
                       .For(v => v.Text)
                       .To(vm => vm.BuyCsnText);
             bindingSet.Bind(GoBuyCsn)
                       .To(vm => vm.BuyCsnCommand);*/
            bindingSet.Bind(AirdropList)
                      .For(l => l.ItemsSource)
                      .To(vm => vm.Airdrops);
            bindingSet.Apply();

        }

        private void Initialize()
        {
            this.BindingInflate(Resource.Layout.homepage_principal, this);
            UsernameTextView = FindViewById<TextView>(Resource.Id.homepagePrincipalUsername);
            EnvironmentTextView = FindViewById<TextView>(Resource.Id.homepagePrincipalEnvironment);
            EthAddressTextView = FindViewById<TextView>(Resource.Id.hompagePrincipalEthAddress);
            ShareButton = FindViewById<Button>(Resource.Id.homepagePrincipalShareButton);
            CoinstantineBalance = FindViewById<BalanceView>(Resource.Id.homepagePrincipalBalanceCsn);
            EthBalance = FindViewById<BalanceView>(Resource.Id.homepagePrincipalBalanceEth);
            AirdropList = FindViewById<MvxListView>(Resource.Id.homepageAirdropList);
            SwipeRefreshList = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.homepageSwipeToRefreshList);
            SwipeRefreshBalance = FindViewById<MvxSwipeRefreshLayout>(Resource.Id.homepageSwipeToRefreshBalance);
            //GoBuyCsn = FindViewById<Button>(Resource.Id.homepagePrincipalGoBuyCsn);
            //GoBuyCsn.SetBackgroundResource(Resource.Drawable.black_button);
            ShareButton.TranslationX = 10.ToDP(Context);
        }
    }

    public class HomepageAdapter : MvxAdapter
    {
        public HomepageAdapter(Context context, IMvxAndroidBindingContext bindingContext) : base(context, bindingContext)
        {
        }

        protected HomepageAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            if (dataContext is AirdropItemViewModel)
            {
                var view = new AirdropListItem(Context)
                {
                    DataContext = dataContext
                };
                return view;
            }
            return base.GetBindableView(convertView, dataContext, parent, templateId);
        }
    }

    public class GenericAdapter<TViewModel, TItemView> : MvxAdapter where TViewModel : BaseViewModel
        where TItemView : ViewGroup, IDataContextProvider
    {
        private readonly Func<Context, TItemView> _viewCtor;

        public GenericAdapter(Context context, IMvxBindingContext bindingContext, Func<Context, TItemView> viewCtor) : base(context, (IMvxAndroidBindingContext)bindingContext)
        {
            _viewCtor = viewCtor;
        }

        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            if(convertView != null)
            {
                convertView.Dispose();
            }

            if (dataContext is TViewModel)
            {
                var view = _viewCtor(Context);
                view.DataContext = dataContext;
                return view;
            }
            return base.GetBindableView(convertView, dataContext, parent, templateId);
        }

    }

    public class GenericAdapter<TViewModel1, TItemView1, TViewModel2, TItemView2> : GenericAdapter<TViewModel1, TItemView1>
        where TViewModel1 : BaseViewModel
        where TItemView1 : ViewGroup, IDataContextProvider
        where TViewModel2 : BaseViewModel
        where TItemView2 : ViewGroup, IDataContextProvider
    {
        private readonly Func<Context, TItemView2> _view2Ctor;

        public GenericAdapter(Context context, IMvxBindingContext bindingContext, Func<Context, TItemView1> view1Ctor, Func<Context, TItemView2> view2Ctor) : base(context, bindingContext, view1Ctor)
        {
            _view2Ctor = view2Ctor;
        }

        protected override View GetBindableView(View convertView, object dataContext, ViewGroup parent, int templateId)
        {
            if (convertView != null)
            {
                convertView.Dispose();
            }

            if (dataContext is TViewModel2)
            {
                var view = _view2Ctor(Context);
                view.DataContext = dataContext;
                return view;
            }
            return base.GetBindableView(convertView, dataContext, parent, templateId);
        }
    }
}