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

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class SwitchableGroupOptionVSMac : OptionVSMac
    {

        private NSView _optionView;
        private NSSwitch _switchButton;
        private NSTextField _title;
        private NSTextField _description;
        private NSButton _helpButton;
        private NSView _childrenView;
        private HintPopover _hintPopover;

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
            _optionView = new AppKit.NSView();
            _optionView.WantsLayer = true;
            _optionView.TranslatesAutoresizingMaskIntoConstraints = false;

            var _optionViewWidthConstraint = _optionView.WidthAnchor.ConstraintEqualToConstant(600f);
            _optionViewWidthConstraint.Active = true;
            var _optionViewHeightConstraint = _optionView.HeightAnchor.ConstraintEqualToConstant(81f);
            _optionViewHeightConstraint.Active = true;


            _switchButton = new AppKit.NSSwitch();
            _switchButton.ControlSize = NSControlSize.Regular;
            _switchButton.State = 1;
            _switchButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _switchButton.AccessibilityHelp = "Provides a control";


            _optionView.AddSubview(_switchButton);
            var _switchButtonWidthConstraint = _switchButton.WidthAnchor.ConstraintEqualToConstant(38f);
            _switchButtonWidthConstraint.Active = true;

            _switchButton.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor, 48f).Active = true;
            _switchButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 30f).Active = true;

            _title = new AppKit.NSTextField();
            _title.Editable = false;
            _title.Bordered = false;
            _title.DrawsBackground = false;
            _title.PreferredMaxLayoutWidth = 1;
            _title.StringValue = Option.Label;
            _title.Alignment = NSTextAlignment.Left;
            _title.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize, AppKit.NSFontWeight.Bold);
            _title.TextColor = NSColor.LabelColor;
            _title.TranslatesAutoresizingMaskIntoConstraints = false;

            _optionView.AddSubview(_title);
            var _titleWidthConstraint = _title.WidthAnchor.ConstraintEqualToConstant(467f);
            _titleWidthConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
            _titleWidthConstraint.Active = true;
            var _titleHeightConstraint = _title.HeightAnchor.ConstraintEqualToConstant(16f);
            _titleHeightConstraint.Active = true;

            _title.RightAnchor.ConstraintEqualToAnchor(_optionView.RightAnchor, -32f).Active = true;
            _title.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor, 101f).Active = true;
            _title.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 24f).Active = true;

            _description = new AppKit.NSTextField();
            _description.Editable = false;
            _description.Bordered = false;
            _description.DrawsBackground = false;
            _description.PreferredMaxLayoutWidth = 1;
            _description.StringValue = Option.Name;
            _description.Alignment = NSTextAlignment.Left;
            _description.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SmallSystemFontSize);
            _description.TextColor = NSColor.SecondaryLabelColor;
            _description.TranslatesAutoresizingMaskIntoConstraints = false;

            _optionView.AddSubview(_description);
            var _descriptionWidthConstraint = _description.WidthAnchor.ConstraintEqualToConstant(354f);
            _descriptionWidthConstraint.Active = true;
            var _descriptionHeightConstraint = _description.HeightAnchor.ConstraintEqualToConstant(42f);
            _descriptionHeightConstraint.Active = true;

            _description.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor, 101f).Active = true;
            _description.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 42f).Active = true;

            if (!string.IsNullOrEmpty(Option.Hint))
            {
                _helpButton = new AppKit.NSButton();
                _helpButton.BezelStyle = NSBezelStyle.HelpButton;
                _helpButton.Title = "";
                _helpButton.ControlSize = NSControlSize.Regular;
                _helpButton.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
                _helpButton.TranslatesAutoresizingMaskIntoConstraints = false;

                _optionView.AddSubview(_helpButton);
                var _helpButtonWidthConstraint = _helpButton.WidthAnchor.ConstraintEqualToConstant(21f);
                _helpButtonWidthConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
                _helpButtonWidthConstraint.Active = true;

                _helpButton.RightAnchor.ConstraintEqualToAnchor(_optionView.RightAnchor, -6f).Active = true;
                _helpButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 30f).Active = true;
                _helpButton.Activated += (o, args) => ShowHintPopover(Option.Hint, _helpButton);

            }

            _childrenView = new NSView();

            foreach (Option option in ((SwitchableGroupOption)Option).ChildOptions)
            {
                var optionVSMac = (OptionVSMac)option.Platform;
                _optionView.AddSubview(optionVSMac.View);
            }

            UpdateChildOptions();
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

        private void UpdateChildOptions()
        {

            _optionView.AddSubview(_childrenView);

            //if (_switchButton)
            //{
            //    _optionView.AddSubview(_childrenView);
            //}
            //else
            //{
            //    _childrenView.RemoveFromSuperview();
            //}
        }
    }

}
