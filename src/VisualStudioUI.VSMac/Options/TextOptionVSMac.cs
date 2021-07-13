using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class TextOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSTextField? _textField;

        public TextOptionVSMac(TextOption option) : base(option)
        {
        }

        public TextOption TextOption => ((TextOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_textField == null)
                {
                    ViewModelProperty<string> property = TextOption.Property;

                    _textField = new AppKit.NSTextField();
                    _textField.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
                    _textField.StringValue = property.Value ?? string.Empty;
                    _textField.TranslatesAutoresizingMaskIntoConstraints = false;
                    _textField.Editable = TextOption.Editable;
                    _textField.Bordered = TextOption.Bordered;
                    _textField.DrawsBackground = TextOption.DrawsBackground;

                    _textField.WidthAnchor.ConstraintEqualToConstant(196f).Active = true;

                    property.PropertyChanged += delegate (object o, ViewModelPropertyChangedEventArgs args)
                    {
                        _textField.StringValue = ((string)args.NewValue) ?? string.Empty;
                    };

                    _textField.Changed += delegate { property.Value = _textField.StringValue; };
                }

                return _textField;
            }
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);
            if (_textField != null)
                _textField.Enabled = enabled;
        }

        /*
        public override void Dispose ()
        {
            Property.PropertyChanged -= UpdatePopUpBtnValue;
            textField.Changed -= UpdatePropertyValue;

            base.Dispose ();
        }
        */
    }
}
