using Coinstantine.Common.Attributes;
using Coinstantine.Core.Services;
using Foundation;
using Plugin.Xablu.Walkthrough;
using UIKit;

namespace Coinstantine.iOS.Services
{
    [RegisterInterfaceAsDynamic]
    public class CrossWalkthroughInitializer : NSObject, ICrossWalkthroughInitializer
    {
		public void Inititalize()
		{
			var activeViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
			CrossWalkthrough.Current.Init(activeViewController);
		}
	}
}
