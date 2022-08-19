using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels;
using Coinstantine.Droid.CustomViews.BackgroundBubbles;
using Coinstantine.Droid.Extensions;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Coinstantine.Droid
{
    public class BaseActivity<T> : MvxAppCompatActivity<T> where T : class, IBaseViewModel
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Window.DecorView.SetBackgroundColor(AppColorDefinition.MainBlue.ToAndroidColor());
            if (Toolbar != null)
            {
                SetSupportActionBar(Toolbar);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                SupportActionBar.SetHomeButtonEnabled(true);
            }
            if (WithSpecialBackground)
            {
                DrawCircles();
            }
        }
        protected virtual bool WithSpecialBackground { get; }
        public Toolbar Toolbar
        {
            get;
            set;
        }

        protected virtual int LayoutResource
        {
            get;
        }

        protected int ActionBarIcon
        {
            set { Toolbar?.SetNavigationIcon(value); }
        }

        private void DrawCircles()
        {
            var title = ViewModel.Title != null ? ViewModel.GetTitle() : string.Empty;
            _backgroundBubbles = new BackgroundBubbles(this, 300.ToDP(this), ViewModel.TitleIcon, title);
            var viewGroup = FindViewById(Resource.Id.Content);
            AddContentView(_backgroundBubbles, new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
            _backgroundBubbles.TranslationX = -65.ToDP(this);
            _backgroundBubbles.TranslationY = -75.ToDP(this);
            _backgroundBubbles.SendViewToBack();
        }
        private BackgroundBubbles _backgroundBubbles;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Title))
            {
                _backgroundBubbles.SetTitle(ViewModel.GetTitle());
            }
        }

        protected override void OnDestroy()
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            base.OnDestroy();
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Down)
            {
                if (CurrentFocus is Android.Widget.EditText)
                {
                    CurrentFocus.ClearFocus();
                    var inputMethodManager = (InputMethodManager)GetSystemService(InputMethodService);
                    inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, 0);
                }
            }
            return base.DispatchTouchEvent(ev);
        }
    }
}
