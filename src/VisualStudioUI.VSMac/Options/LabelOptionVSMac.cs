using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class LabelOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSTextField? _textField;

        public LabelOptionVSMac(LabelOption option) : base(option)
        {
        }

        public LabelOption LabelOption => ((LabelOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_textField == null)
                {
                    _textField = new AppKit.NSTextField();
                    if (LabelOption.IsBold)
                        _textField.Font = AppKit.NSFont.BoldSystemFontOfSize(AppKit.NSFont.SystemFontSize);// .SystemFontOfSize(AppKit.NSFont.BoldSystemFontOfSize .SystemFontSize);
                    else
                        _textField.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
                    _textField.StringValue = LabelOption.Name ?? string.Empty;
                    _textField.TranslatesAutoresizingMaskIntoConstraints = false;
                    _textField.Editable = false;
                    _textField.Bordered = false;
                    _textField.DrawsBackground = false;

                    _textField.WidthAnchor.ConstraintEqualToConstant(470f).Active = true;

                    if (LabelOption.Hidden != null)
                    {
                        LabelOption.Hidden.PropertyChanged += HidView;
                    }
                }

                return _textField;
            }
        }

        private void HidView(object sender, ViewModelPropertyChangedEventArgs e)
        {
            _textField.Hidden = LabelOption.Hidden.Value;
        }
    }
}

