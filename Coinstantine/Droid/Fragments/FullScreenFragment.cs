using System;
using System.Threading.Tasks;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;

namespace Coinstantine.Droid.Fragments
{
    public class FullScreenFragment : DialogFragment
    {
        private string _message;
        private LottieAnimationView _lottieAnimationView;

        public FullScreenFragment(string message)
        {
            _message = message;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.overlay_fragment, container, false);
            var textView = view.FindViewById<TextView>(Resource.Id.overlayFragmentMessageTextView);
            textView.Text = _message;
            _lottieAnimationView = view.FindViewById<LottieAnimationView>(Resource.Id.overlayFragmentAnimationView);
            StartAnimation();
            return view;
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return new AppDialog(Context);
        }

        public override void OnStart()
        {
            base.OnStart();
            if (Dialog != null)
            {
                var backgroundColor = new ColorDrawable(Color.White);
                backgroundColor.SetAlpha(150);
                Dialog.Window.SetBackgroundDrawable(backgroundColor);
                Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

            }
        }

        public void UpdateMessage(string message)
        {
            _message = message;
            var textView = View.FindViewById<TextView>(Resource.Id.overlayFragmentMessageTextView);
            textView.Text = message;
        }

        public void StartAnimation(int i)
        {
            _animationId = i;
            if (_animationId > 1) { _animationId = 1; }
            if (_animationId < 0) { _animationId = 0; }
        }
        private int _animationId;
        private void StartAnimation()
        {
            _lottieAnimationView.SetAnimation($"Wait_1.json");
            _lottieAnimationView.PlayAnimation();
            var width = 300;
            var height = 300;
            _lottieAnimationView.LayoutParameters = new LinearLayout.LayoutParams(width, height);
        }

        public override void Show(FragmentManager manager, string tag)
        {

            using (var transaction = manager?.BeginTransaction())
            {
                try
                {
                    if (transaction != null)
                    {
                        transaction.Add(this, tag);
                        transaction.CommitAllowingStateLoss();
                    }
                }
                catch
                {

                }
            }
        }
    }
}