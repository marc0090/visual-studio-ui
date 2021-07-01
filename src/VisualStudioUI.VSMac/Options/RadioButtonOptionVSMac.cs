using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class RadioButtonOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSButton? _button;

        public RadioButtonOptionVSMac(RadioButtonOption option) : base(option)
        {
        }

        public RadioButtonOption RadioButtonOption => ((RadioButtonOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_button == null)
                {
                    ViewModelProperty<bool> property = RadioButtonOption.Property;

                    _button = NSButton.CreateRadioButton(RadioButtonOption.ButtonLabel, OnRadioButtonSelected);
                    _button.SetButtonType(NSButtonType.Radio);
                    _button.ControlSize = NSControlSize.Regular;
                    _button.Font = AppKit.NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                    _button.Title = RadioButtonOption.ButtonLabel;
                    _button.TranslatesAutoresizingMaskIntoConstraints = false;
                    _button.AccessibilityTitle = "Control";
                    _button.AccessibilityHelp = "Provides a control";
                    _button.State = RadioButtonOption.Property.Value ? NSCellStateValue.On : NSCellStateValue.Off;

                    property.PropertyChanged += delegate
                    {
                        _button.State = RadioButtonOption.Property.Value ? NSCellStateValue.On : NSCellStateValue.Off;
                    };
                }

                return _button!;
            }
        }

        private void OnRadioButtonSelected()
        {
            RadioButtonOption.RadioButtonGroup.Select(RadioButtonOption);
        }
    }
}
