using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ButtonOptionVSMac : OptionVSMac
    {
        NSView _optionView;
        NSButton _button;
        NSTextField _description;
        NSButton _helpButton;
        HintPopover _hintPopover;

        public ButtonOptionVSMac(ButtonOption option) : base(option)
        {
        }

        public ButtonOption ButtonOption => ((ButtonOption) Option);

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

        private void CreateView()
        {
            // View:     optionView
            _optionView = new AppKit.NSView();
            _optionView.WantsLayer = true;
            _optionView.TranslatesAutoresizingMaskIntoConstraints = false;

            var optionWidthConstraint = _optionView.WidthAnchor.ConstraintEqualToConstant(600f);
            optionWidthConstraint.Active = true;

            var viewHeight = 32f;
            if (!string.IsNullOrWhiteSpace(ButtonOption.Description))
            {
                viewHeight = 65f;
            }

            var optionHeightConstraint = _optionView.HeightAnchor.ConstraintEqualToConstant(viewHeight);
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

            if (!string.IsNullOrEmpty(Option.Hint))
            {
                // View:     helpButton
                _helpButton = new AppKit.NSButton();
                _helpButton.BezelStyle = NSBezelStyle.HelpButton;
                _helpButton.Title = "";
                _helpButton.ControlSize = NSControlSize.Regular;
                _helpButton.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
                _helpButton.TranslatesAutoresizingMaskIntoConstraints = false;

                _optionView.AddSubview(_helpButton);
                var helpButtonWidthConstraint = _helpButton.WidthAnchor.ConstraintEqualToConstant(21f);
                helpButtonWidthConstraint.Priority = (System.Int32) AppKit.NSLayoutPriority.DefaultLow;
                helpButtonWidthConstraint.Active = true;

                _helpButton.RightAnchor.ConstraintEqualToAnchor(_optionView.RightAnchor, -6f).Active = true;
                _helpButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5f).Active = true;

                _helpButton.Activated += (o, args) => ShowHintPopover(Option.Hint, _helpButton);
            }

            // View:     control
            _button = new AppKit.NSButton();
            if (ButtonOption.Type == ButtonOption.ButtonType.Normal)
            {
                _button.BezelStyle = NSBezelStyle.RegularSquare;
                _button.Activated += ButtonOption.ButtonClicked;

            }
            else
            {
                _button.SetButtonType((NSButtonType) ButtonOption.Type);
                SetSatus();
            }

            if (ButtonOption.IsSelected != null)
            {
                _button.Activated += UpdatePropertyFromUI;
                ButtonOption.IsSelected.PropertyChanged += UpdateUIFromProperty;
            }

            _button.ControlSize = NSControlSize.Regular;
            _button.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
            _button.Title = ButtonOption.Name ?? string.Empty;
            _button.TranslatesAutoresizingMaskIntoConstraints = false;
            _button.AccessibilityTitle = "Control";
            _button.AccessibilityHelp = "Provides a control";

            _optionView.AddSubview(_button);

            _button.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor, 220f).Active = true;
            _button.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5f).Active = true;

            if (!string.IsNullOrWhiteSpace(ButtonOption.Description))
            {
                _description = new AppKit.NSTextField();
                _description.Editable = false;
                _description.Bordered = false;
                _description.DrawsBackground = false;
                _description.PreferredMaxLayoutWidth = 1;
                _description.StringValue = ButtonOption.Description ?? string.Empty;
                _description.Alignment = NSTextAlignment.Left;
                _description.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SmallSystemFontSize);
                _description.TextColor = NSColor.SecondaryLabelColor;
                _description.TranslatesAutoresizingMaskIntoConstraints = false;

                _optionView.AddSubview(_description);
                var _descriptionWidthConstraint = _description.WidthAnchor.ConstraintEqualToConstant(354f);
                _descriptionWidthConstraint.Active = true;
                var _descriptionHeightConstraint = _description.HeightAnchor.ConstraintEqualToConstant(354f);
                _descriptionHeightConstraint.Active = true;

                _description.LeftAnchor.ConstraintEqualToAnchor(_button.LeftAnchor, 0f).Active = true;
                _description.TopAnchor.ConstraintEqualToAnchor(_button.BottomAnchor, 0f).Active = true;
            }
        }

        void SetSatus()
        {
            if (ButtonOption.IsSelected == null)
                return;

            var state = ButtonOption.IsSelected.Value ? NSCellStateValue.On : NSCellStateValue.Off;

            if (_button.State == state)
                return;

            _button.State = state;
        }

        void UpdatePropertyFromUI(object sender, EventArgs e)
        {
            var isSelected = (_button.State == NSCellStateValue.On) ? true : false;

            if (ButtonOption.IsSelected.Value == isSelected)
            {
                return;
            }

            ButtonOption.IsSelected.Value = isSelected;

            ButtonOption.UpdateStatus(_button, e);
        }

        void UpdateUIFromProperty(object sender, ViewModelPropertyChangedEventArgs e)
        {
            SetSatus();
        }

        void ShowHintPopover(string message, NSButton button)
        {
            _hintPopover?.Close();
            _hintPopover?.Dispose();
            _hintPopover = new HintPopover(message, null);
            _hintPopover.MaxWidth = 256;
            //TODO:
            //popover.SetAppearance (Appearance);

            var bounds = button.Bounds;
            _hintPopover.Show(bounds, button, NSRectEdge.MaxYEdge);
        }
    }
}
