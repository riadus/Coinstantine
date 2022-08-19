using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Droid.Extensions;
using Plugin.Fingerprint.Dialog;

namespace Coinstantine.Droid.CustomViews
{
    public class AppFingerprintDialogFragment : FingerprintDialogFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            var image = view.FindViewById<ImageView>(Resource.Id.fingerprint_imgFingerprint);
            image.SetColorFilter(AppColorDefinition.MainBlue.ToAndroidColor(), PorterDuff.Mode.SrcIn);
            return view;
        }
    }
}
