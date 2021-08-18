// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

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
                    _textField = new NSTextField();
                    if (LabelOption.IsBold)
                        _textField.Font = NSFont.BoldSystemFontOfSize(NSFont.SystemFontSize);
                    else
                        _textField.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                    _textField.StringValue = LabelOption.Name ?? string.Empty;
                    _textField.TranslatesAutoresizingMaskIntoConstraints = false;
                    _textField.Editable = false;
                    _textField.Bordered = false;
                    _textField.DrawsBackground = false;
                    _textField.SizeToFit();
                    _textField.WidthAnchor.ConstraintLessThanOrEqualToConstant(420f).Active = true;
                    _textField.HeightAnchor.ConstraintEqualToConstant(18f).Active = true;

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
            if(_textField != null)
            _textField.Hidden = LabelOption.Hidden.Value;
        }
    }
}

