using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public abstract class OptionWithLeftLabelVSMac : OptionVSMac
	{
		private NSView? _optionView;
		private NSButton? _helpButton;
		private HintPopover? _hintPopover;

		public OptionWithLeftLabelVSMac(Option option) : base(option) { }

		public override NSView View
		{
			get
			{
				if (_optionView == null)
				{
					CreateView();
				}

				return _optionView!;
			}
		}

		protected abstract NSView Control { get; }

		private void CreateView()
		{
			// View:     optionView
			_optionView = new AppKit.NSView();
			_optionView.WantsLayer = true;
			_optionView.TranslatesAutoresizingMaskIntoConstraints = false;

			var optionWidthConstraint = _optionView.WidthAnchor.ConstraintEqualToConstant (600f);
			optionWidthConstraint.Active = true;
			var optionHeightConstraint = _optionView.HeightAnchor.ConstraintEqualToConstant (32f);
			optionHeightConstraint.Active = true;

			if (!string.IsNullOrEmpty(Option.Label))
			{
				// View:     label
				var label = new AppKit.NSTextField();
				label.Editable = false;
				label.Bordered = false;
				label.DrawsBackground = false;
				label.PreferredMaxLayoutWidth = 1;
				// TODO: Make the colon be localization friendly
				label.StringValue = Option.Label + ":";

				label.Alignment = NSTextAlignment.Right;
				label.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
				label.TextColor = NSColor.LabelColor;
				label.TranslatesAutoresizingMaskIntoConstraints = false;

				_optionView.AddSubview(label);
				label.WidthAnchor.ConstraintEqualToConstant(205f).Active = true;
				label.HeightAnchor.ConstraintEqualToConstant(16f).Active = true;

				label.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor, 6f).Active = true;
				label.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 7f).Active = true;
			}

			// View:     control
			var control = Control;
			// TODO: Set a11y info properly
			control.AccessibilityLabel = "Control";
			control.AccessibilityHelp = "Provides a control";

			_optionView.AddSubview(control);

			control.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor, 220f).Active = true;
			control.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5f).Active = true;

			UpdateHelpButton();

			if (Option.ValidationMessage != null)
			{
				Option.ValidationMessage.PropertyChanged += (sender, args) => UpdateHelpButton();
			}
		}

		void UpdateHelpButton()
		{
			if (_helpButton != null)
			{
				_helpButton.RemoveFromSuperview();
				_helpButton.Dispose();  // TODO: Is this needed?
				_helpButton = null;
			}

			Message? validationMessage = Option.ValidationMessage?.Value;
			string? messageText = validationMessage?.Text ?? Option.Hint;
			if (messageText ==  null)
				return;

			// View:     helpButton
			_helpButton = new AppKit.NSButton();
			_helpButton.BezelStyle = NSBezelStyle.HelpButton;
			_helpButton.Title = "";
			_helpButton.ControlSize = NSControlSize.Regular;
			_helpButton.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
			_helpButton.TranslatesAutoresizingMaskIntoConstraints = false;

			_optionView!.AddSubview(_helpButton);
			var helpButtonWidthConstraint = _helpButton.WidthAnchor.ConstraintEqualToConstant(21f);
			helpButtonWidthConstraint.Priority = (System.Int32) AppKit.NSLayoutPriority.DefaultLow;
			helpButtonWidthConstraint.Active = true;

			_helpButton.RightAnchor.ConstraintEqualToAnchor(_optionView.RightAnchor, -6f).Active = true;
			_helpButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5f).Active = true;

			_helpButton.Activated += (o, args) => ShowHelpPopover (messageText, _helpButton);
		}

		void ShowHelpPopover (string message, NSButton button)
		{
			_hintPopover?.Close();
			_hintPopover?.Dispose();
			_hintPopover = new HintPopover (message, null);
			_hintPopover.MaxWidth = 256;
			//TODO:
			//popover.SetAppearance (Appearance);

			var bounds = button.Bounds;
			_hintPopover.Show (bounds, button, NSRectEdge.MaxYEdge);
		}
	}
}
