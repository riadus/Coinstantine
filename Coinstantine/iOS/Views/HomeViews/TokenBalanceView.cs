using System;
using CoreGraphics;
using Coinstantine.Core;
using Coinstantine.iOS.Views.Extensions;
using Coinstantine.Common;
using UIKit;
using Coinstantine.Core.ViewModels.Home;
using MvvmCross.Platforms.Ios.Binding.Views;

namespace Coinstantine.iOS
{
    public partial class TokenBalanceView : MvxView
    {
        public TokenBalanceView (IntPtr handle) : base (handle)
        {
        }

        private string _tokenName;
        private decimal _balance;
        private string _tokenSymbol;
        private string _symbol;
        private bool _availableLater;
        private string _availableLaterText;
        private bool _built;

        void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateFromDataContext();
            UpdateText();
        }


        private void SetBindings()
        {
            if (_built) return;
            var vm = DataContext as TokenBalanceViewModel;
            vm.PropertyChanged += Vm_PropertyChanged;
            BuildView();
            _built = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetBindings();
        }

        public override void MovedToSuperview()
		{
			base.MovedToSuperview();         
			BackgroundColor = UIColor.Clear;
		}


        private UILabel _tokenSymbolLabel;
        private UILabel _tokenNameLabel;
        private UILabel _balanceLabel;

        private void UpdateText()
        {
            _tokenSymbolLabel.Text = _tokenSymbol;
            _tokenNameLabel.Text = _tokenName;
            _balanceLabel.Text = _balance.ToString();

            var tokenSymbolSize = _tokenSymbolLabel.SizeThatFits(Frame.Size);
            var tokenNameSize = _tokenNameLabel.SizeThatFitsWithMargin(Frame.Size, 1);
            var balanceSize = _balanceLabel.SizeThatFitsWithMargin(Frame.Size, 5);

            _tokenSymbolLabel.SetSize(tokenSymbolSize);
            _tokenNameLabel.SetSize(tokenNameSize);
            _balanceLabel.SetSize(balanceSize);

            _tokenSymbolLabel.Center = new CGPoint(Frame.Width / 2, Frame.Height / 4);
            _tokenNameLabel.Center = new CGPoint(Frame.Width / 2, Frame.Height / 2);
            _balanceLabel.Center = new CGPoint(Frame.Width / 2, 3 * Frame.Height / 4);
        }

        private void UpdateFromDataContext()
        {
            var vm = DataContext as TokenBalanceViewModel;
            _tokenName = vm.Name;
            _tokenSymbol = vm.Symbole.ToCode();
            _symbol = vm.Symbole;
            _balance = vm.Balance;
            _availableLater = vm.AvailableLater;
            _availableLaterText = vm.AvailableLaterText;
        }

		public void BuildView()
		{
            UpdateFromDataContext();

			if(_tokenSymbol.IsNullOrEmpty() || _tokenName.IsNullOrEmpty())
			{
				return;
			}

			var circleView = new UIView(Frame);
			circleView.SetPosition(new CGPoint(0, 0));
			circleView.Layer.CornerRadius = Frame.Width / 2;
			circleView.Layer.BorderColor = UIColor.White.CGColor;
			circleView.Layer.BorderWidth = 1;

			Add(circleView);

			_tokenSymbolLabel = new UILabel
			{
                Font = _symbol.ToUIFont(25),
                TextColor = UIColor.White,
			};

			_tokenNameLabel = new UILabel
			{
				Font = UIFont.SystemFontOfSize(15, UIFontWeight.Regular),
                TextColor = UIColor.White,
                MinimumScaleFactor = 0.3f,
                AdjustsFontSizeToFitWidth = true
			};

			_balanceLabel = new UILabel
			{
				Font = UIFont.SystemFontOfSize(15, UIFontWeight.Black),
                Text = _balance.ToString(),
                TextColor = AppColorDefinition.SecondaryColor.ToUIColor(),
                MinimumScaleFactor = 0.3f,
                AdjustsFontSizeToFitWidth = true
			};

			var availableLaterView = new UIView
			{
				BackgroundColor = UIColor.White
			};
			availableLaterView.Layer.CornerRadius = 5;

            var availableLaterLabel = new UILabel
            {
                Font = UIFont.SystemFontOfSize(9, UIFontWeight.Regular),
                Text = _availableLaterText,
                TextColor = AppColorDefinition.MainBlue.ToUIColor(),
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap
            };

            UpdateText();

            var availableLaterLabelSize = availableLaterLabel.SizeThatFits(Frame.Size);

			availableLaterLabel.Frame = new CGRect(10, 2.5, availableLaterLabelSize.Width, availableLaterLabelSize.Height);
			availableLaterView.WiderThan(availableLaterLabelSize, 20);
            availableLaterView.HigherThan(availableLaterLabelSize, 5);
            availableLaterView.Add(availableLaterLabel);

			Add(_tokenSymbolLabel);
			Add(_tokenNameLabel);
			Add(_balanceLabel);
			if (_availableLater)
			{
				availableLaterView.CenterHorizontally(this);
				availableLaterView.PlaceAtBottomOf(this);
				availableLaterView.PlaceMoreDown((float) availableLaterLabel.Frame.Height / 2);
				Add(availableLaterView);
			}
		}
	}
}