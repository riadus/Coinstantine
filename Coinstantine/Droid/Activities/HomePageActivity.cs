using Android.App;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Home;
using Coinstantine.Droid.CustomViews.Homepage;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.BindingContext;

namespace Coinstantine.Droid.Activities
{
    [Activity(Label = "HomePageActivity", Theme = "@style/AppTheme")]
    public class HomePageActivity : BaseActivity<HomeViewModel>
    {
        protected override bool WithSpecialBackground => true;
        protected AppRecyclerView _recyclerView;
        protected override void OnViewModelSet()
        {
            SetContentView(Resource.Layout.homepage);
            _recyclerView = FindViewById<AppRecyclerView>(Resource.Id.homepageRecyclerView);
            var menuButton = FindViewById<Button>(Resource.Id.homeMenuButton);
            var syncButton = FindViewById<Button>(Resource.Id.homeSyncButton);
            var changePageButton = FindViewById<Button>(Resource.Id.homeChangepageButton);
            _recyclerView.HasFixedSize = true;
            var metrics = new DisplayMetrics();
            WindowManager.DefaultDisplay.GetMetrics(metrics);
            var adapter = new HomepageRecyclerViewAdapter(this, metrics)
            {
                ItemsSource = ViewModel.ViewModels
            };
            _recyclerView.SetAdapter(adapter);

            var bindingSet = this.CreateBindingSet<HomePageActivity, HomeViewModel>();

            bindingSet.Bind(menuButton)
                      .For("AppString")
                      .To(vm => vm.MenuIcon);
            bindingSet.Bind(menuButton)
                      .To(vm => vm.OpenMenu);
            bindingSet.Bind(syncButton)
                      .For("AppString")
                      .To(vm => vm.SyncIcon);
            bindingSet.Bind(syncButton)
                      .To(vm => vm.SyncCommand);
            bindingSet.Bind(changePageButton)
                      .For(b => b.Text)
                      .To(vm => vm.ChangePageText);
            bindingSet.Bind(changePageButton)
                      .For("AppString")
                      .To(vm => vm.ArrowButton);
            bindingSet.Bind(changePageButton)
                      .To(vm => vm.ChangePageCommand);
            bindingSet.Apply();

            menuButton.ToCircle(this, 40, Color.White);
            syncButton.ToCircle(this, 40, Color.White);
            changePageButton.ToCircle(this, 40, AppColorDefinition.MainBlue.ToAndroidColor());
            changePageButton.SetTextColor(Color.White);
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.ActiveViewModel))
            {
                _recyclerView.EnableScroll();
                if(ViewModel.ActiveViewModel is BuyViewModel)
                {
                    _recyclerView.SmoothScrollToPosition(1);
                }
                else
                {
                    _recyclerView.SmoothScrollToPosition(0);
                }
            }
        }

        public override void OnBackPressed()
        {
        }
    }
}
