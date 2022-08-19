using CoreGraphics;
using Foundation;
using Coinstantine.Core;
using Coinstantine.iOS.Views.Extensions;
using System;
using System.Collections.Generic;
using UIKit;

namespace Coinstantine.iOS
{
    public partial class ProgressiveView : UIView
    {
        public ProgressiveView (IntPtr handle) : base (handle)
        {
			ProgessionColor = AppColorDefinition.SecondaryColor.ToUIColor();
			FailedColor = UIColor.Red;
			NumberOfSteps = 4;
        }

        public int NumberOfSteps { get; set; }
        public UIColor ProgessionColor { get; set; }
        public UIColor FailedColor { get; set; }
        public int CurrentStep { get; set; }
		private int _previousStep;
        public bool? Succeeded { get; set; }

        private List<UIView> _states;

        public void Update()
        {
            if (_states == null)
            {
                return;
            }
            if (Succeeded.HasValue)
            {
                SetAllToColor(Succeeded.Value ? ProgessionColor : FailedColor);
                return;
            }
			if (_previousStep >= CurrentStep)
			{
				_states[_previousStep].BackgroundColor = UIColor.White;
			}
			else
			{
				_states[CurrentStep - 1].BackgroundColor = ProgessionColor;
			}
			_previousStep = CurrentStep - 1;
        }

        private void SetAllToColor(UIColor color)
        {
            _states.ForEach(v => v.BackgroundColor = color);
        }

        public void BuildView()
        {
            _states = new List<UIView>();
            var height = Frame.Height / NumberOfSteps;
            var width = Frame.Width;
			nfloat y = Frame.Height - height;
            for (var i = 0; i < NumberOfSteps; i++)
            {
                var frame = new CGRect(0, y, width, height);
                var view = new UIView(frame);
                _states.Add(view);
                Add(view);
				SendSubviewToBack(view);
                y -= height;
            }
        }

        public override void WillMoveToSuperview(UIView newsuper)
        {
            base.WillMoveToSuperview(newsuper);
            BuildView();
        }
    }
}