using AppKit;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public abstract class OptionWithLeftLabelVSMac : OptionVSMac
	{
		private NSView _optionView;

		public OptionWithLeftLabelVSMac(Option option) : base(option) { }

		public override NSView View
		{
			get
			{
				if (_optionView == null)
				{
					_optionView = CreateView();
				}

				return _optionView;
			}
		}

		protected abstract NSView Control { get; }

		private NSView CreateView()
		{
			// View:     optionView
			var optionView = new AppKit.NSView();
			optionView.WantsLayer = true;
			optionView.TranslatesAutoresizingMaskIntoConstraints = false;

			var optionWidthConstraint = optionView.WidthAnchor.ConstraintEqualToConstant (600f);
			optionWidthConstraint.Active = true;
			var optionHeightConstraint = optionView.HeightAnchor.ConstraintEqualToConstant (32f);
			optionHeightConstraint.Active = true;

			// View:     label
			var label = new AppKit.NSTextField();
			label.Editable = false;
			label.Bordered = false;
			label.DrawsBackground = false;
			label.PreferredMaxLayoutWidth = 1;
			label.StringValue = Option.Label ?? string.Empty;
			label.Alignment = NSTextAlignment.Right;
			label.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
			label.TextColor = NSColor.LabelColor;
			label.TranslatesAutoresizingMaskIntoConstraints = false;

			optionView.AddSubview (label);
			var preferenceLabel1WidthConstraint = label.WidthAnchor.ConstraintEqualToConstant (205f);
			preferenceLabel1WidthConstraint.Active = true;
			var preferenceLabel1HeightConstraint = label.HeightAnchor.ConstraintEqualToConstant (16f);
			preferenceLabel1HeightConstraint.Active = true;

			label.LeftAnchor.ConstraintEqualToAnchor (optionView.LeftAnchor, 6f).Active = true;
			label.TopAnchor.ConstraintEqualToAnchor (optionView.TopAnchor, 7f).Active = true;

			if (!string.IsNullOrEmpty(Option.Hint))
			{
				// View:     helpButton
				var helpButton = new AppKit.NSButton();
				helpButton.BezelStyle = NSBezelStyle.HelpButton;
				helpButton.Title = "";
				helpButton.ControlSize = NSControlSize.Regular;
				helpButton.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
				helpButton.TranslatesAutoresizingMaskIntoConstraints = false;

				optionView.AddSubview(helpButton);
				var helpButton1WidthConstraint = helpButton.WidthAnchor.ConstraintEqualToConstant(21f);
				helpButton1WidthConstraint.Priority = (System.Int32) AppKit.NSLayoutPriority.DefaultLow;
				helpButton1WidthConstraint.Active = true;

				helpButton.RightAnchor.ConstraintEqualToAnchor(optionView.RightAnchor, -6f).Active = true;
				helpButton.TopAnchor.ConstraintEqualToAnchor(optionView.TopAnchor, 5f).Active = true;
			}

			// View:     control
			var control = new AppKit.NSTextField();
			control.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
			control.StringValue = "1.0";
			control.TranslatesAutoresizingMaskIntoConstraints = false;
			control.AccessibilityLabel = "Control";
			control.AccessibilityHelp = "Provides a control";

			optionView.AddSubview (control);
			var controlWidthConstraint = control.WidthAnchor.ConstraintEqualToConstant (196f);
			controlWidthConstraint.Active = true;

			control.LeftAnchor.ConstraintEqualToAnchor (optionView.LeftAnchor, 220f).Active = true;
			control.TopAnchor.ConstraintEqualToAnchor (optionView.TopAnchor, 5f).Active = true;

			return optionView;
		}
	}
}
