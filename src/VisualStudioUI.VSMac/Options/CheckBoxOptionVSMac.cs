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
        private NSStackView _buttonView;

        public CheckBoxOptionVSMac(CheckBoxOption option) : base(option)
        {
        }

        public CheckBoxOption CheckBoxOption => ((CheckBoxOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_buttonView == null)
                {
                    _buttonView = new NSStackView() {
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        Orientation = NSUserInterfaceLayoutOrientation.Horizontal,
                        Distribution = NSStackViewDistribution.Fill,
                        Alignment = NSLayoutAttribute.Top,
                    };

                    ViewModelProperty<bool> property = CheckBoxOption.Property;

                    _button = NSButton.CreateCheckbox(string.Empty, CheckBoxSelected);
                    _button.SetButtonType(NSButtonType.Radio);
                    _button.TranslatesAutoresizingMaskIntoConstraints = false;
                    _button.State = CheckBoxOption.Property.Value ? NSCellStateValue.On : NSCellStateValue.Off;

                    _buttonView.AddArrangedSubview(_button);
                    _button.SetContentHuggingPriorityForOrientation((int)NSLayoutPriority.DefaultHigh, NSLayoutConstraintOrientation.Horizontal);

                    var field = new NSTextField() { TranslatesAutoresizingMaskIntoConstraints = false};
                    field.Bezeled = false;
                    field.DrawsBackground = false;
                    field.Editable = false;
                    field.Selectable = false;
                    field.StringValue = CheckBoxOption.ButtonLabel;

                    _buttonView.AddArrangedSubview(field);
                    field.LineBreakMode = NSLineBreakMode.ByWordWrapping;
                    field.SetContentCompressionResistancePriority((int)NSLayoutPriority.DefaultLow, NSLayoutConstraintOrientation.Horizontal);

                    property.PropertyChanged += delegate
                    {
                        _button.State = CheckBoxOption.Property.Value ? NSCellStateValue.On : NSCellStateValue.Off;
                    };
                }

                return _buttonView!;
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
