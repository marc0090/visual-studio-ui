using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public class RadioButtonOptionVSMac : OptionWithLeftLabelVSMac
	{
		NSButton _radioBtn;

		public RadioButtonOptionVSMac (RadioButtonOption option) : base (option)
		{
		}

		public RadioButtonOption RadioBtnOption => ((RadioButtonOption) Option);

		protected override NSView Control
		{
			get {
				if (_radioBtn == null)
				{
					_radioBtn = new AppKit.NSButton ();
					_radioBtn.SetButtonType (NSButtonType.Radio);
					_radioBtn.ControlSize = NSControlSize.Regular;
					_radioBtn.Font = AppKit.NSFont.SystemFontOfSize (AppKit.NSFont.SystemFontSize);
					_radioBtn.Title = RadioBtnOption.Title;
					_radioBtn.State = NSCellStateValue.On;
					_radioBtn.TranslatesAutoresizingMaskIntoConstraints = false;
					//_radioBtn.AccessibilityTitle = "Control";
					//_radioBtn.AccessibilityHelp = "Provides a control";

					var radioViewWidthConstraint = _radioBtn.WidthAnchor.ConstraintEqualToConstant (374f);
					radioViewWidthConstraint.Priority = (System.Int32) AppKit.NSLayoutPriority.DefaultLow;
					radioViewWidthConstraint.Active = true;
				}

				return _radioBtn;
			}
		}
	}
}