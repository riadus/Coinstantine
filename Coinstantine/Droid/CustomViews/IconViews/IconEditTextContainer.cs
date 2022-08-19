using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Core;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.CustomViews.Homepage;
using Coinstantine.Droid.Extensions;
using MvvmCross.Binding.Extensions;

namespace Coinstantine.Droid.CustomViews.IconViews
{
    public class IconEditTextContainer : BindableConstraintLayout
    {
        private AppRecyclerView _appRecyclerView;

        public IconEditTextContainer(Context context) : base(context)
        {
        }

        public IconEditTextContainer(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public IconEditTextContainer(int templateId, Context context, IAttributeSet attrs) : base(templateId, context, attrs)
        {
        }

        protected IconEditTextContainer(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        private IIconTextfieldFormViewModel ViewModel => DataContext as IIconTextfieldFormViewModel;

        protected override void OnAttachedToWindow()
        {
            ViewTreeObserver.GlobalLayout -= ViewTreeObserver_GlobalLayout;
            ViewTreeObserver.GlobalLayout += ViewTreeObserver_GlobalLayout;
            base.OnAttachedToWindow();
        }
        private bool _built;
        void ViewTreeObserver_GlobalLayout(object sender, EventArgs e)
        {
            if (_built)
            {
                return;
            }
            BuildView();
            _built = true;
        }

        protected override void OnDetachedFromWindow()
        {
            ViewTreeObserver.GlobalLayout -= ViewTreeObserver_GlobalLayout;
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            base.OnDetachedFromWindow();
        }

        private void BuildView()
        {
            if(ViewModel == null)
            {
                return;
            }
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            var circleIndicator = new CircleIndicator(Context)
            {
                Id = GenerateViewId()
            };

            if(!ViewModel.IsMultiPage)
            {
                circleIndicator.Visibility = ViewStates.Gone;
            }

            var circleIndicatorParams = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent)
            {
                LeftToLeft = LayoutParams.ParentId,
                RightToRight = LayoutParams.ParentId,
                BottomToBottom = LayoutParams.ParentId
            };

            _appRecyclerView = new AppRecyclerView(Context)
            {
                HasFixedSize = true
            };
            _appRecyclerView.SetAdapter(new IconEditTextRecyclerViewAdapter(Context)
            {
                ItemsSource = ViewModel.IconTextfieldCollectionViewModels
            });
            var recyclerViewLayoutParams = new LayoutParams(ViewGroup.LayoutParams.MatchParent, 0)
            {
                TopToTop = LayoutParams.ParentId,
                LeftToLeft = LayoutParams.ParentId,
                RightToRight = LayoutParams.ParentId,
                BottomToTop = circleIndicator.Id
            };
            circleIndicator.SetColors(AppColorDefinition.SecondaryColor.ToAndroidColor(), Color.White);
            circleIndicator.SetRecyclerView(_appRecyclerView);
            AddView(circleIndicator, circleIndicatorParams);
            AddView(_appRecyclerView, recyclerViewLayoutParams);
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(ViewModel.CurrentIndex))
            {
                if (Context is Activity activity)
                {
                    activity.RunOnUiThread(() =>
                    {
                        _appRecyclerView.EnableScroll();
                        _appRecyclerView.ScrollToPosition(ViewModel.CurrentIndex);
                    });
                }
            }
        }
    }

    internal class IconEditTextRecyclerViewAdapter : RecyclerView.Adapter
    {
        public IconEditTextRecyclerViewAdapter(Context context)
        {
            Context = context;
        }

        public override int ItemCount => ItemsSource.Count();

        public IEnumerable ItemsSource { get; set; }
        public Context Context { get; }

        public override int GetItemViewType(int position)
        {
            return position;
        }


        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (holder is IconEditTextViewHolder iconEditTextViewHolder)
            {
                if (iconEditTextViewHolder.ItemView is IDataContextProvider bindableView)
                {
                    bindableView.DataContext = ItemsSource.ElementAt(position);
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

            return new IconEditTextViewHolder(new IconEditTexts(Context)
            {
                LayoutParameters = layoutParameters
            });
        }
    }

    public class IconEditTextViewHolder : RecyclerView.ViewHolder
    {
        public IconEditTextViewHolder(View itemView) : base(itemView)
        {
        }
    }
}
