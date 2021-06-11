using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public class ButtonOptionVSMac : OptionWithLeftLabelVSMac
	{
		NSButton _radioBtn;

		public ButtonOptionVSMac (ButtonOption option) : base (option)
		{
		}

		public ButtonOption ButtonOption => ((ButtonOption) Option);

		protected override NSView Control
		{
			get {
				if (_radioBtn == null)
				{
					_radioBtn = new AppKit.NSButton ();
                    if (ButtonOption.Type != 0) // 0 means noraml button
                    {
						_radioBtn.SetButtonType((NSButtonType)ButtonOption.Type);
					}
					_radioBtn.ControlSize = NSControlSize.Regular;
					_radioBtn.Font = AppKit.NSFont.SystemFontOfSize (AppKit.NSFont.SystemFontSize);
					_radioBtn.Title = ButtonOption.Title ?? string.Empty;
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