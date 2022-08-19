using Coinstantine.Common.Attributes;
using Coinstantine.Core.Services;
using MvvmCross.Droid.Support.V7.AppCompat;
using Plugin.CurrentActivity;
using Plugin.Xablu.Walkthrough;
using Splat;

namespace Coinstantine.Droid.UIServices
{
    [RegisterInterfaceAsDynamic]
    public class CrossWalkthroughInitializer : ICrossWalkthroughInitializer
    {
        public void Inititalize()
        {
            var currentActivity = CrossCurrentActivity.Current.Activity as MvxAppCompatActivity;
            CrossWalkthrough.Current.Init(currentActivity);
        }
    }
}