using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class FileEntryOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSView _controlView;
        private NSTextField? _textField;
        private NSButton _button;

        public FileEntryOptionVSMac(FileEntryOption option) : base(option)
        {
        }

        public FileEntryOption FileEntryOption => ((FileEntryOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_controlView == null)
                {
                    _controlView = new NSView();
                    _controlView.WantsLayer = true;
                    _controlView.TranslatesAutoresizingMaskIntoConstraints = false;

                    ViewModelProperty<string> property = FileEntryOption.Property;

                    _textField = new NSTextField
                    {
                        Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
                        StringValue = property.Value ?? string.Empty,
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        Editable = FileEntryOption.Editable,
                        Bordered = FileEntryOption.Bordered,
                        DrawsBackground = FileEntryOption.DrawsBackground
                    };

                    _controlView.AddSubview(_textField);

                    _textField.WidthAnchor.ConstraintEqualToConstant(196f).Active = true;
                    _textField.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
                    _textField.LeadingAnchor.ConstraintEqualToAnchor(_controlView.LeadingAnchor).Active = true;
                    _textField.TopAnchor.ConstraintEqualToAnchor(_controlView.TopAnchor).Active = true;


                    property.PropertyChanged += delegate (object o, ViewModelPropertyChangedEventArgs args)
                    {
                        _textField.StringValue = ((string)args.NewValue) ?? string.Empty;
                    };

                    _textField.Changed += delegate { property.Value = _textField.StringValue; };

                    _button = new NSButton();
                    _button.BezelStyle = NSBezelStyle.RoundRect;
                    _button.Title = FileEntryOption.ButtonLabel;
                    _button.SizeToFit();
                    _button.Activated += (s, e) =>
                    {
                        var openPanel = new NSOpenPanel();
                        openPanel.CanChooseFiles = true;
                        var response = openPanel.RunModal();
                        if (response == 1 && openPanel.Url != null)
                        {
                            _textField.StringValue = openPanel.Url.AbsoluteString;
                        }
                    };
                    _controlView.AddSubview(_button);

                    _button.CenterYAnchor.ConstraintEqualToAnchor(_textField.CenterYAnchor).Active = true;
                    _button.LeadingAnchor.ConstraintEqualToAnchor(_textField.TrailingAnchor, 5f).Active = true;
                    _controlView.WidthAnchor.ConstraintEqualToConstant(226f).Active = true;
                    _controlView.HeightAnchor.ConstraintEqualToAnchor(_textField.HeightAnchor).Active = true;

                }
                return _controlView;
            }
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);
            if (_textField != null)
                _textField.Enabled = enabled;
        }
    }
}
