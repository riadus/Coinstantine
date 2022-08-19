using System.Collections;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Coinstantine.Droid.CustomViews.BindableLayouts;
using Coinstantine.Droid.CustomViews.Homepage;
using MvvmCross.Binding.Extensions;

namespace Coinstantine.Droid.Activities
{
    internal class HomepageRecyclerViewAdapter : RecyclerView.Adapter
    {
        private readonly DisplayMetrics _displayMetrics;

        public HomepageRecyclerViewAdapter(Context context, DisplayMetrics displayMetrics)
        {
            Context = context;
            _displayMetrics = displayMetrics;
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
            if (holder is HomepageViewHolder homepageViewHolder)
            {
                if (homepageViewHolder.ItemView is IDataContextProvider bindableView)
                {
                    bindableView.DataContext = ItemsSource.ElementAt(position);
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var layoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, _displayMetrics.HeightPixels);

            if (viewType == 0)
            {
                return new HomepageViewHolder(new HomepagePrincipal(Context)
                {
                    LayoutParameters = layoutParameters
                });
            }
            return new HomepageViewHolder(new HomepageBuyView(Context)
            {
                LayoutParameters = layoutParameters
            });
        }
    }

    public class HomepageViewHolder : RecyclerView.ViewHolder
    {
        public HomepageViewHolder(View itemView) : base(itemView)
        {
        }
    }
}