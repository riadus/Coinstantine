using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;

namespace Coinstantine.iOS.UIServices
{
	[RegisterInterfaceAsDynamic]
    public class OverlayService : OverlayManager<WaitingOverlayView>, IOverlayService
    {
        protected override bool UseDefaultOverlay => false;
        public void Dismiss()
        {
            RemoveView();
        }

        public void Show(string message)
        {
            ShowMessage(message, true);
        }

        public void UpdateMessage(string message)
        {
            BeginInvokeOnMainThread(() =>
            {
				CreateViewIfNeeded();
                View.UpdateMessage(message);
            });
        }

        public void Wait(string message)
        {
            ShowMessage(message, false);
        }

        private void ShowMessage(string message, bool tapToDismiss)
        {
            BeginInvokeOnMainThread(async () =>
            {
				CreateViewIfNeeded();
                TapToDismiss = tapToDismiss;
                await ShowView();
                View.UpdateMessage(message);
                View.SetActivityIndicator(tapToDismiss);
                var rnd = new Random();
                var i = rnd.Next(0, 2);
                await View.StartAnimation(i);
            });
        }
    }
}
