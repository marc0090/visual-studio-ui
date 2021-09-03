// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Text;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class CheckBoxOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSButton? _button;

        public CheckBoxOptionVSMac(CheckBoxOption option) : base(option)
        {
        }

        public CheckBoxOption CheckBoxOption => ((CheckBoxOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_button == null)
                {
                    ViewModelProperty<bool> property = CheckBoxOption.Property;

                    _button = NSButton.CreateCheckbox(CheckBoxOption.ButtonLabel, CheckBoxSelected);
                    _button.SetButtonType(NSButtonType.Radio);
                    _button.ControlSize = NSControlSize.Regular;
                    _button.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                    _button.Title = CheckBoxOption.ButtonLabel;
                    _button.TranslatesAutoresizingMaskIntoConstraints = false;
                    _button.State = CheckBoxOption.Property.Value ? NSCellStateValue.On : NSCellStateValue.Off;

                    var maxLength = 77; 
                    if (_button.Title.Length > (maxLength/2) && !_button.Title.Contains("\n"))
                    {
                        // Handle long strings of different language without inserted "\n"
                        var charLength = Encoding.UTF8.GetByteCount(_button.Title);
                        if (charLength > _button.Title.Length)
                        {
                            if ((charLength / _button.Title.Length) > 2)
                                maxLength = maxLength / 2; // language is double-byte
                            else
                                maxLength = (int)(maxLength * 0.75); // the language is mixes with double-byte and single-byte
                        }

                        if (_button.Title.Length > maxLength)
                        {
                            _button.Title = InsertNewLine(_button.Title, maxLength);
                        }
                    }

                    _button.Cell.LineBreakMode = NSLineBreakMode.CharWrapping;
                    _button.SizeToFit();
                    _button.Cell.Alignment = NSTextAlignment.Left;
                    
                    property.PropertyChanged += delegate
                    {
                        _button.State = CheckBoxOption.Property.Value ? NSCellStateValue.On : NSCellStateValue.Off;
                    };
                }

                return _button!;
            }
        }

        private string InsertNewLine(string str, int max, int searchStart = 0)
        {
            if (str.Length <= max) return str;

            int removeFrom = searchStart > 0 ? searchStart : max;

            var index = str.Remove(removeFrom, str.Length - removeFrom).LastIndexOf(" ");
            if (index > 0)
                str = str.Remove(index, 1).Insert(index, "\n");
            else
            {
                index = removeFrom;
                str = str.Insert(index, "\n");
            }

            if (str.Remove(0, index).Length > max)
            {
                return InsertNewLine(str, max, index + max);
            }

            return str;
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);

            if (_button != null)
                _button.Enabled = enabled;
        }

        private void CheckBoxSelected()
        {
            CheckBoxOption.Property.Value = (_button?.State == NSCellStateValue.On) ? true : false;
        }
    }
}
