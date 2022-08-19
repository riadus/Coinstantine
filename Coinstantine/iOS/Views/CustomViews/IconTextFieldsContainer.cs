using Coinstantine.Core.ViewModels.Account.AccountCreation;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using CoreGraphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coinstantine.iOS
{
    public partial class IconTextFieldsContainer : BaseScrollView
    {
        private bool _built;
        private List<IconTextFields> _iconTextFields;
        private int _currentView;

        public IconTextFieldsContainer(IntPtr handle) : base(handle)
        {
            ScrollEnabled = false;
            _iconTextFields = new List<IconTextFields>();
        }

        public IIconTextfieldFormViewModel DataContext { get; set; }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            BuildView();
        }

        public void BuildView()
        {
            if (_built || DataContext == null)
            {
                return;
            }
            var width = ParentViewController.View.Frame.Width;
            foreach (var context in DataContext.IconTextfieldCollectionViewModels)
            {
                var offsetX = width * _iconTextFields.Count();
                var iconTextField = new IconTextFields(ParentViewController.View.Frame.Width)
                {
                    DataContext = context,
                    Frame = new CGRect(Bounds.X + offsetX, Bounds.Y, width, Bounds.Height)
                };
                Add(iconTextField);
                _iconTextFields.Add(iconTextField);
            }

            ContentSize = new CGSize(width * DataContext.IconTextfieldCollectionViewModels.Count(), Bounds.Height);

            _built = true;
        }

        public void Release()
        {
            _iconTextFields.ForEach(x => x.Release());
        }

        public int CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                UpdateViewToShow();
            }
        }

        private void UpdateViewToShow()
        {
            Animate(0.3, () =>
            {
                if (_iconTextFields.Any())
                {
                    var viewToShow = _iconTextFields[_currentView];
                    ContentOffset = viewToShow.Frame.Location;
                }
            });
        }
    }
}