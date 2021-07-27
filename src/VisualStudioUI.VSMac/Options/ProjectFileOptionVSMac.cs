using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ProjectFileOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSStackView _controlView;
        private NSTextField _textField;
        private NSButton _button;

        public ProjectFileOptionVSMac(ProjectFileOption option) : base(option)
        {
        }

        public ProjectFileOption ProjectFileOption => ((ProjectFileOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_controlView == null)
                {
                    _controlView = new NSStackView();
                    _controlView.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
                    _controlView.TranslatesAutoresizingMaskIntoConstraints = false;

                    ViewModelProperty<string> property = ProjectFileOption.Property;

                    _textField = new NSTextField
                    {
                        Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
                        StringValue = property.Value ?? string.Empty,
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        Editable = true,
                        Bordered = true,
                        DrawsBackground = true,
                    };

                    _controlView.AddArrangedSubview(_textField);


                    property.PropertyChanged += delegate (object o, ViewModelPropertyChangedEventArgs args)
                    {
                        _textField.StringValue = ((string)args.NewValue) ?? string.Empty;
                    };

                    _textField.Changed += delegate { property.Value = _textField.StringValue; };

                    _button = new NSButton
                    {
                        BezelStyle = NSBezelStyle.RoundRect,
                        Bordered = true,
                        LineBreakMode = NSLineBreakMode.TruncatingTail,
                        Title = ProjectFileOption.Name ?? "···"
                    };
                    _button.SizeToFit();

                    _button.Activated += (s, e) =>
                    {
                        ProjectFileOption.ButtonClicked(s, e);
                    };

                    _controlView.AddArrangedSubview(_button);
                    _controlView.HeightAnchor.ConstraintEqualToConstant(21f).Active = true;

                    _textField.WidthAnchor.ConstraintEqualToConstant(196f).Active = true;
                    _textField.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
                    _textField.LeadingAnchor.ConstraintEqualToAnchor(_controlView.LeadingAnchor).Active = true;
                    _textField.TopAnchor.ConstraintEqualToAnchor(_controlView.TopAnchor).Active = true;

                    _button.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
                    _button.TrailingAnchor.ConstraintEqualToAnchor(_controlView.TrailingAnchor).Active = true;



                }
                return _controlView;
            }
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);

            _textField.Enabled = enabled;

            _button.Enabled = enabled;
        }
    }
}
