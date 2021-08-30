﻿using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public abstract class OptionVSMac : OptionPlatform
    {
        private HintPopover? _hintPopover;

        public OptionVSMac(Option option) : base(option)
        {

        }

        public abstract NSView View { get; }

        public override void OnPropertiesChanged()
        {
        }

        public override void OnEnableChanged(bool enabled)
        {

        }

        protected NSTextField? CreateLabelView()
        {
            string labelString = Option.Label ?? string.Empty;
            if (labelString.Length == 0)
                return null;

            // View:     label
            var label = new NSTextField();
            label.Editable = false;
            label.Bordered = false;
            label.DrawsBackground = false;
            label.PreferredMaxLayoutWidth = 1;
            // TODO: Make the colon be localization friendly
            label.StringValue = labelString + ":";
            label.HeightAnchor.ConstraintEqualToConstant(16f).Active = true;

            label.Alignment = NSTextAlignment.Right;
            label.TextColor = NSColor.LabelColor;
            label.TranslatesAutoresizingMaskIntoConstraints = false;

            if(labelString.Length < 32)
            {
                label.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
            }
            else
            {
                label.Font = NSFont.SystemFontOfSize(11f);
            }

            label.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(213f).Active = true;

            return label;
        }

        protected NSTextField? CreateDescriptionView()
        {
            string descriptionString = Option.Description ?? string.Empty;
            if (descriptionString.Length == 0)
                return null;

            var description = new NSTextField();
            description.Editable = false;
            description.Bordered = false;
            description.DrawsBackground = false;
            description.PreferredMaxLayoutWidth = 354f;
            description.StringValue = descriptionString;
            description.Alignment = NSTextAlignment.Left;
            description.Font = NSFont.SystemFontOfSize(NSFont.SmallSystemFontSize);
            description.TextColor = NSColor.SecondaryLabelColor;
            description.TranslatesAutoresizingMaskIntoConstraints = false;

            description.WidthAnchor.ConstraintEqualToConstant(354f).Active = true;

            return description;
        }

        protected virtual void UpdateHintButton() { }

        protected NSButton? CreateHintButton()
        {
            Message? validationMessage = Option.ValidationMessage?.Value;
            string? messageText = validationMessage?.Text ?? Option.Hint;

            NSButton? hintButton = null;
            if (validationMessage != null)
            {
                hintButton = new NSButton();
                hintButton.BezelStyle = NSBezelStyle.RoundRect;
                hintButton.Bordered = false;
                hintButton.ImagePosition = NSCellImagePosition.ImageOnly;
                hintButton.ImageScaling = NSImageScale.ProportionallyUpOrDown;
                hintButton.TranslatesAutoresizingMaskIntoConstraints = false;
                switch (validationMessage.Severity)
                {
                    case MessageSeverity.Warning:
                        hintButton.Image = NSImage.ImageNamed("NSCaution");
                        break;
                    case MessageSeverity.Error:
                        hintButton.Image = NSImage.GetSystemSymbol("xmark.octagon.fill", null);
                        hintButton.ContentTintColor = NSColor.Red;
                        break;

                }
            }
            else if (Option.Hint != null)
            {
                hintButton = new NSButton();
                hintButton.BezelStyle = NSBezelStyle.HelpButton;
                hintButton.Title = "";
                hintButton.ControlSize = NSControlSize.Regular;
                hintButton.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                hintButton.TranslatesAutoresizingMaskIntoConstraints = false;
            }
            else
            {
                return null;
            }

            hintButton.HeightAnchor.ConstraintEqualToConstant(20f).Active = true;
            hintButton.WidthAnchor.ConstraintEqualToConstant(20f).Active = true;

            hintButton.Activated += (o, args) => ShowHintPopover(messageText!, hintButton);

            return hintButton;
        }

        private void ShowHintPopover(string message, NSButton button)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            _hintPopover?.Close();
            _hintPopover?.Dispose();
            _hintPopover = new HintPopover(message, null);
            _hintPopover.MaxWidth = 256;
            //TODO:
            //popover.SetAppearance (Appearance);

            var bounds = button.Bounds;
            _hintPopover.Show(bounds, button, NSRectEdge.MaxYEdge);
        }

        internal float IndentValue()
        {
            switch (Option.Indent)
            {
                case OptionIndent.Normal:
                    return 0;
                case OptionIndent.SubOption:
                    return 30;
                case OptionIndent.SubSubOption:
                    return 60;

                default:
                    return 0;
            }
        }
    }
}