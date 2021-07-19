using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class SwitchableGroupOptionVSMac : OptionVSMac
    {
        private NSView? _optionView;
        private NSSwitch? _switchButton;
        private HintPopover? _hintPopover;
        private NSProgressIndicator? _progressIndicator;
        private NSStackView? _childrenControl;
        private NSLayoutConstraint? _descriptionBottomConstraints;
        private NSLayoutConstraint? _childrenControlBottomConstraints;

        public SwitchableGroupOptionVSMac(SwitchableGroupOption option) : base(option)
        {
        }

        public SwitchableGroupOption SwitchableGroupOption => ((SwitchableGroupOption)Option);

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
            bool enable = ((SwitchableGroupOption)Option).IsOn.Value;

            _optionView = new NSStackView() { Orientation = NSUserInterfaceLayoutOrientation.Vertical };
            _optionView.WantsLayer = true;
            _optionView.TranslatesAutoresizingMaskIntoConstraints = false;

            _switchButton = new NSSwitch();
            _switchButton.ControlSize = NSControlSize.Regular;
            _switchButton.State = enable ? 1 : 0;
            _switchButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _switchButton.AccessibilityHelp = "Provides a control";

            _switchButton.Activated += SwitchButtonActivated;

            SwitchableGroupOption.IsOn.PropertyChanged += SwitchPropertyChanged;
            SwitchableGroupOption.ShowSpinner.PropertyChanged += SpinnerChanged;

            _optionView.AddSubview(_switchButton);
            _switchButton.WidthAnchor.ConstraintEqualToConstant(38f).Active = true;
            _switchButton.HeightAnchor.ConstraintEqualToConstant(21f).Active = true;
            _switchButton.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 68f).Active = true;
            _switchButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 30f).Active = true;

            string? buttonLabel = SwitchableGroupOption.ButtonLabel;
            // Temporarily use Label as a fallback; remove when no longer needed
            if (string.IsNullOrEmpty(buttonLabel))
                buttonLabel = SwitchableGroupOption.Label;
            if (buttonLabel == null)
                buttonLabel = "";

            var title = new NSTextField();
            title.Editable = false;
            title.Bordered = false;
            title.DrawsBackground = false;
            title.StringValue = buttonLabel!;
            title.Alignment = NSTextAlignment.Left;
            title.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize, NSFontWeight.Bold);
            title.TextColor = NSColor.LabelColor;
            title.TranslatesAutoresizingMaskIntoConstraints = false;
            title.SizeToFit();

            _optionView.AddSubview(title);

            //_title.WidthAnchor.ConstraintEqualToConstant(38f).Active = true;
            //_title.HeightAnchor.ConstraintEqualToConstant(28f).Active = true;
            title.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 121f).Active = true;
            title.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 24f).Active = true;

            _progressIndicator = new NSProgressIndicator(new CoreGraphics.CGRect(0, 0, 18, 18));
            _progressIndicator.Style = NSProgressIndicatorStyle.Spinning;
            _progressIndicator.ControlSize = NSControlSize.Small;
            _progressIndicator.IsDisplayedWhenStopped = false;
            _progressIndicator.TranslatesAutoresizingMaskIntoConstraints = false;
            _optionView.AddSubview(_progressIndicator);

            _progressIndicator.LeadingAnchor.ConstraintEqualToAnchor(title.TrailingAnchor, 6).Active = true;
            _progressIndicator.TopAnchor.ConstraintEqualToAnchor(title.TopAnchor).Active = true;

            string? descriptionText = Option.Description;
            if (descriptionText == null)
                descriptionText = Option.Name;

            NSTextField? description = null;
            if (!string.IsNullOrEmpty(descriptionText))
            {
                description = new NSTextField();
                description.Editable = false;
                description.Bordered = false;
                description.DrawsBackground = false;
                description.PreferredMaxLayoutWidth = 1;
                description.StringValue = descriptionText!;
                description.Alignment = NSTextAlignment.Left;
                description.Font = NSFont.SystemFontOfSize(NSFont.SmallSystemFontSize);
                description.TextColor = NSColor.SecondaryLabelColor;
                description.TranslatesAutoresizingMaskIntoConstraints = false;
                description.LineBreakMode = NSLineBreakMode.ByWordWrapping;
                _optionView.AddSubview(description);

                description.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 121f).Active = true;
                description.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 42f).Active = true;

                var bestHeight = description.Cell.CellSizeForBounds(new CoreGraphics.CGRect(0, 0, 354, 600)).Height;
                var _descriptionHeightConstraint = description.HeightAnchor.ConstraintEqualToConstant(bestHeight);
                _descriptionHeightConstraint.Active = true;
                var _descriptionWidthConstraint = description.WidthAnchor.ConstraintEqualToConstant(354f);
                _descriptionWidthConstraint.Active = true;

                float bottomeSpace = -24;
                if (bestHeight > 26) { bottomeSpace = -14; }
                _descriptionBottomConstraints = description.BottomAnchor.ConstraintEqualToAnchor(_optionView.BottomAnchor, bottomeSpace);
            }

            if (!string.IsNullOrEmpty(Option.Hint))
            {
                var helpButton = new NSButton();
                helpButton.BezelStyle = NSBezelStyle.HelpButton;
                helpButton.Title = "";
                helpButton.ControlSize = NSControlSize.Regular;
                helpButton.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                helpButton.TranslatesAutoresizingMaskIntoConstraints = false;

                _optionView.AddSubview(helpButton);
                var _helpButtonWidthConstraint = helpButton.WidthAnchor.ConstraintEqualToConstant(21f);
                _helpButtonWidthConstraint.Priority = (int)NSLayoutPriority.DefaultLow;
                _helpButtonWidthConstraint.Active = true;

                helpButton.TrailingAnchor.ConstraintEqualToAnchor(_optionView.TrailingAnchor, -6f).Active = true;
                helpButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 30f).Active = true;
                helpButton.Activated += (o, args) => ShowHintPopover(Option.Hint!, helpButton);
            }

            var _optionViewWidthConstraint = _optionView.WidthAnchor.ConstraintEqualToConstant(600f);
            _optionViewWidthConstraint.Active = true;

            _childrenControl = new NSStackView()
            {
                Orientation = NSUserInterfaceLayoutOrientation.Vertical,
                Spacing = SwitchableGroupOption.Space,
                Distribution = NSStackViewDistribution.Fill,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            foreach (Option option in SwitchableGroupOption.ChildrenOptions)
            {
                NSView optionView = ((OptionVSMac)option.Platform).View;
                _childrenControl.AddArrangedSubview(optionView);
            }

            _optionView.AddSubview(_childrenControl);

            // TODO: Need an else here?
            if (description != null)
            {
                _childrenControl.TopAnchor.ConstraintEqualToAnchor(description.BottomAnchor, SwitchableGroupOption.Space).Active = true;
            }

            _childrenControl.TrailingAnchor.ConstraintEqualToAnchor(_optionView.TrailingAnchor).Active = true;
            _childrenControl.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor).Active = true;
            _childrenControlBottomConstraints = _childrenControl.BottomAnchor.ConstraintEqualToAnchor(_optionView.BottomAnchor, -SwitchableGroupOption.Space);
            _childrenControlBottomConstraints.Active = true;

            ShowChildrenOption(enable);
        }

        public override void OnEnableChanged(bool enabled)
        {
            _switchButton!.Enabled = enabled;
        }

        private void ShowChildrenOption(bool enable)
        {
            if (enable && SwitchableGroupOption.ChildrenOptions.Count > 0)
            {
                _childrenControl!.Hidden = false;
                _childrenControlBottomConstraints!.Active = true;
                _descriptionBottomConstraints!.Active = false;
            }
            else
            {
                _childrenControl!.Hidden = true;
                _childrenControlBottomConstraints!.Active = false;
                _descriptionBottomConstraints!.Active = true;
            }
        }

        private void SwitchButtonActivated(object sender, EventArgs e)
        {
            bool enable = (_switchButton!.State == 1);

            ((SwitchableGroupOption)Option).IsOn.PropertyChanged -= SwitchPropertyChanged;
            ((SwitchableGroupOption)Option).IsOn.Value = enable;
            ((SwitchableGroupOption)Option).SwitchChangedInvoke(sender, e);
            ((SwitchableGroupOption)Option).IsOn.PropertyChanged += SwitchPropertyChanged;

            ShowChildrenOption(enable);
        }

        private void SwitchPropertyChanged(object sender, ViewModelPropertyChangedEventArgs e)
        {
            _switchButton!.Activated -= SwitchButtonActivated;
            _switchButton.State = ((SwitchableGroupOption)Option).IsOn.Value ? 1 : 0;
            _switchButton.Activated += SwitchButtonActivated;

        }

        private void ShowHintPopover(string message, NSButton button)
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

        public void SpinnerChanged(object sender, ViewModelPropertyChangedEventArgs e)
        {
            if (SwitchableGroupOption.ShowSpinner.Value)
            {
                _progressIndicator!.StartAnimation(null);
            }
            else
            {
                _progressIndicator!.StopAnimation(null);
            }
        }
    }
}
