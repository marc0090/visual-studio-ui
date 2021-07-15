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
                    _button.Font = AppKit.NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                    _button.Title = CheckBoxOption.ButtonLabel;
                    _button.TranslatesAutoresizingMaskIntoConstraints = false;
                    _button.AccessibilityTitle = "Control";
                    _button.AccessibilityHelp = "Provides a control";
                    _button.State = CheckBoxOption.Property.Value ? NSCellStateValue.On : NSCellStateValue.Off;

                    property.PropertyChanged += delegate
                    {
                        _button.State = CheckBoxOption.Property.Value ? NSCellStateValue.On : NSCellStateValue.Off;
                    };
                }

                return _button!;
            }
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
