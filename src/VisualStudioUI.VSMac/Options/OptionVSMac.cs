using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public abstract class OptionVSMac : OptionPlatform
    {
        private HintPopover? _helpPopover;

        public OptionVSMac(Option option) : base(option)
        {
            if (Option.ValidationMessage != null)
            {
                Option.ValidationMessage.PropertyChanged += (sender, args) => UpdateHelpButton();
            }
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

            label.Alignment = NSTextAlignment.Right;
            label.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
            label.TextColor = NSColor.LabelColor;
            label.TranslatesAutoresizingMaskIntoConstraints = false;

            label.WidthAnchor.ConstraintEqualToConstant(205f).Active = true;
            label.HeightAnchor.ConstraintEqualToConstant(16f).Active = true;

            return label;
        }

        protected NSTextField? CreateDescriptionView()
        {
            string descriptionString = Option.Description ?? string.Empty;
            if (descriptionString.Length == 0)
                return null;

            var description = new AppKit.NSTextField();
            description.Editable = false;
            description.Bordered = false;
            description.DrawsBackground = false;
            description.PreferredMaxLayoutWidth = 354f;
            description.StringValue = descriptionString;
            description.Alignment = NSTextAlignment.Left;
            description.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SmallSystemFontSize);
            description.TextColor = NSColor.SecondaryLabelColor;
            description.TranslatesAutoresizingMaskIntoConstraints = false;

            description.WidthAnchor.ConstraintEqualToConstant(354f).Active = true;

            return description;
        }

        protected virtual void UpdateHelpButton() { }

        protected NSButton? CreateHelpButton()
        {
            Message? validationMessage = Option.ValidationMessage?.Value;
            string? messageText = validationMessage?.Text ?? Option.Hint;
            if (messageText == null)
                return null;

            // View:     helpButton
            var helpButton = new AppKit.NSButton();
            helpButton.BezelStyle = NSBezelStyle.HelpButton;
            helpButton.Title = "";
            helpButton.ControlSize = NSControlSize.Regular;
            helpButton.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
            helpButton.TranslatesAutoresizingMaskIntoConstraints = false;

            var hintButtonWidthConstraint = helpButton.WidthAnchor.ConstraintEqualToConstant(21f);
            hintButtonWidthConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
            hintButtonWidthConstraint.Active = true;

            helpButton.Activated += (o, args) => ShowHelpPopover(messageText, helpButton);

            return helpButton;
        }

        private void ShowHelpPopover(string message, NSButton button)
        {
            _helpPopover?.Close();
            _helpPopover?.Dispose();
            _helpPopover = new HintPopover(message, null);
            _helpPopover.MaxWidth = 256;
            //TODO:
            //popover.SetAppearance (Appearance);

            var bounds = button.Bounds;
            _helpPopover.Show(bounds, button, NSRectEdge.MaxYEdge);
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