//
// SwitchableGroupOptionVSMac.cs
//
// Author:
//       marcbookpro19 <v-marcguo@microsoft.com>
//
// Copyright (c) 2021 
//
using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class SwitchableGroupOptionVSMac : OptionVSMac
    {

        private NSView _optionView;
        private NSSwitch _switchButton;
        private NSTextField _title;
        private NSTextField _description;
        private NSButton _helpButton;
        private HintPopover _hintPopover;
        private NSProgressIndicator _progressIndicator;

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

                return _optionView;
            }
        }

        private void CreateView()
        {
            _optionView = new NSStackView() { Orientation = NSUserInterfaceLayoutOrientation.Vertical };
            _optionView.WantsLayer = true;
            _optionView.TranslatesAutoresizingMaskIntoConstraints = false;

            _switchButton = new NSSwitch();
            _switchButton.ControlSize = NSControlSize.Regular;
            _switchButton.State = ((SwitchableGroupOption)Option).IsOn.Value ? 1 : 0;
            _switchButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _switchButton.AccessibilityHelp = "Provides a control";

            _switchButton.Activated += SwitchButtonActivated;

            SwitchableGroupOption.IsOn.PropertyChanged += SwitchPropertyChanged;
            SwitchableGroupOption.ShowSpinner.PropertyChanged += SpinnerChanged;

            _optionView.AddSubview(_switchButton);
            var _switchButtonWidthConstraint = _switchButton.WidthAnchor.ConstraintEqualToConstant(38f);
            _switchButtonWidthConstraint.Active = true;

            _switchButton.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 48f).Active = true;
            _switchButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 30f).Active = true;


            if (!string.IsNullOrEmpty(Option.Label))
            {
                _title = new NSTextField();
                _title.Editable = false;
                _title.Bordered = false;
                _title.DrawsBackground = false;
                _title.StringValue = Option.Label;
                _title.Alignment = NSTextAlignment.Left;
                _title.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize, NSFontWeight.Bold);
                _title.TextColor = NSColor.LabelColor;
                _title.TranslatesAutoresizingMaskIntoConstraints = false;
                _title.SizeToFit();

                _optionView.AddSubview(_title);

                _title.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 101f).Active = true;
                _title.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 24f).Active = true;
            }

            _progressIndicator = new NSProgressIndicator(new CoreGraphics.CGRect(0, 0, 18, 18));
            _progressIndicator.Style = NSProgressIndicatorStyle.Spinning;
            _progressIndicator.ControlSize = NSControlSize.Small;
            _progressIndicator.IsDisplayedWhenStopped = false;
            _progressIndicator.TranslatesAutoresizingMaskIntoConstraints = false;
            _optionView.AddSubview(_progressIndicator);

            _progressIndicator.LeadingAnchor.ConstraintEqualToAnchor(_title.TrailingAnchor, 6).Active = true;
            _progressIndicator.CenterYAnchor.ConstraintEqualToAnchor(_title.CenterYAnchor).Active = true;

            if (!string.IsNullOrEmpty(Option.Label))
            {
                _description = new NSTextField();
                _description.Editable = false;
                _description.Bordered = false;
                _description.DrawsBackground = false;
                _description.PreferredMaxLayoutWidth = 1;
                _description.StringValue = Option.Name;
                _description.Alignment = NSTextAlignment.Left;
                _description.Font = NSFont.SystemFontOfSize(NSFont.SmallSystemFontSize);
                _description.TextColor = NSColor.SecondaryLabelColor;
                _description.TranslatesAutoresizingMaskIntoConstraints = false;
                _description.LineBreakMode = NSLineBreakMode.ByWordWrapping;
                _optionView.AddSubview(_description);

                _description.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 101f).Active = true;
                _description.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 42f).Active = true;
                _description.BottomAnchor.ConstraintEqualToAnchor(_optionView.BottomAnchor, -21).Active = true;

                var bestHeight = _description.Cell.CellSizeForBounds(new CoreGraphics.CGRect(0, 0, 354, 600)).Height;

                var _descriptionHeightConstraint = _description.HeightAnchor.ConstraintEqualToConstant(bestHeight);
                _descriptionHeightConstraint.Active = true;
                var _descriptionWidthConstraint = _description.WidthAnchor.ConstraintEqualToConstant(354f);
                _descriptionWidthConstraint.Active = true;
            }

            if (!string.IsNullOrEmpty(Option.Hint))
            {
                _helpButton = new NSButton();
                _helpButton.BezelStyle = NSBezelStyle.HelpButton;
                _helpButton.Title = "";
                _helpButton.ControlSize = NSControlSize.Regular;
                _helpButton.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                _helpButton.TranslatesAutoresizingMaskIntoConstraints = false;

                _optionView.AddSubview(_helpButton);
                var _helpButtonWidthConstraint = _helpButton.WidthAnchor.ConstraintEqualToConstant(21f);
                _helpButtonWidthConstraint.Priority = (int)NSLayoutPriority.DefaultLow;
                _helpButtonWidthConstraint.Active = true;

                _helpButton.TrailingAnchor.ConstraintEqualToAnchor(_optionView.TrailingAnchor, -6f).Active = true;
                _helpButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 30f).Active = true;
                _helpButton.Activated += (o, args) => ShowHintPopover(Option.Hint, _helpButton);

            }

            var _optionViewWidthConstraint = _optionView.WidthAnchor.ConstraintEqualToConstant(600f);
            _optionViewWidthConstraint.Active = true;

        }


        private void SwitchButtonActivated(object sender, EventArgs e)
        {
            ((SwitchableGroupOption)Option).IsOn.PropertyChanged -= SwitchPropertyChanged;
            ((SwitchableGroupOption)Option).IsOn.Value = (_switchButton.State == 1);
            ((SwitchableGroupOption)Option).SwitchChangedInvoke(sender, e);
            ((SwitchableGroupOption)Option).IsOn.PropertyChanged += SwitchPropertyChanged;

        }

        private void SwitchPropertyChanged(object sender, ViewModelPropertyChangedEventArgs e)
        {
            _switchButton.Activated -= SwitchButtonActivated;
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
                _progressIndicator.StartAnimation(null);
            }
            else
            {
                _progressIndicator.StopAnimation(null);
            }
        }

    }

}
