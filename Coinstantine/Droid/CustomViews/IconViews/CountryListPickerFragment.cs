using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using static Android.Widget.AdapterView;

namespace Coinstantine.Droid.CustomViews.IconViews
{
    public class CountryListPickerFragment : DialogFragment
    {
        public static readonly string TAG = "X:" + typeof(CountryListPickerFragment).Name.ToUpper();
        private ListView _listView;
        private IEnumerable<IconTextfieldItemsViewModel> _items;

        Action<IconTextfieldItemsViewModel> _onCountrySelected = delegate { };

        public static CountryListPickerFragment NewInstance(Action<IconTextfieldItemsViewModel> onCountrySelected, IEnumerable<IconTextfieldItemsViewModel> items)
        {
            CountryListPickerFragment frag = new CountryListPickerFragment
            {
                _onCountrySelected = onCountrySelected,
                _items = items
            };
            return frag;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _listView = new ListView(Context);
            return _listView;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            var countriesArray = _items.Select(x => x.Value).ToArray();
            var arrayAdapter = new ArrayAdapter<string>(Context, Android.Resource.Layout.SimpleListItem1, countriesArray);
            _listView.Adapter = arrayAdapter;
            _listView.ItemClick += ListView_Click;
            base.OnViewCreated(view, savedInstanceState);
        }

        public override void OnDestroy()
        {
            _listView.ItemClick -= ListView_Click;
            base.OnDestroy();
        }

        void ListView_Click(object sender, ItemClickEventArgs e)
        {
            var selectedItem = _items.ElementAt(e.Position);
            _onCountrySelected.Invoke(selectedItem);
            Dismiss();
        }
    }
}
