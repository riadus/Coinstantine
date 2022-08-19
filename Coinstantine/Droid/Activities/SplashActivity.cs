using Android.App;
using Android.OS;
using Coinstantine.Core.LifeTime;
using Coinstantine.Droid.Helpers;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.Platforms.Android.Views;
using Plugin.CurrentActivity;
using Plugin.Fingerprint;

namespace Coinstantine.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/SplashTheme", MainLauncher = true, Icon = "@mipmap/icon")]
    public class SplashActivity : MvxSplashScreenActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            var appCenterKey = AppParameters.GetAppParameter("AppCenterKey");
            CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);
            AppCenter.Start(appCenterKey, typeof(Analytics), typeof(Crashes));
        }
    }
}
