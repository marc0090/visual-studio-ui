using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class StepperOptionVSMac : OptionVSMac
    {

        private NSStackView _optionView;
        private NSTextField _textField;
        private NSStepper _stepper;

        private NSButton? _helpButton;
        private NSTextField? _leftLabel;


        public StepperOptionVSMac(StepperOption option) : base(option)
        {
        }

        public StepperOption StepperOption => ((StepperOption)Option);

        public override NSView View
        {
            get
            {
                if (_optionView == null)
                {
                    CreateView();
                }

                return _optionView;
            }
        }

        public void CreateView()
        {
            if (_optionView == null)
            {
                _optionView = new NSStackView()
                {
                    Orientation = NSUserInterfaceLayoutOrientation.Horizontal,
                    TranslatesAutoresizingMaskIntoConstraints = false
                };

                _leftLabel = CreateLabelView();

                _stepper = new NSStepper()
                {
                    IntValue = StepperOption.Property.Value,
                    MaxValue = StepperOption.Maximum,
                    MinValue = StepperOption.Minimum,
                    Increment = StepperOption.Increment,
                    TranslatesAutoresizingMaskIntoConstraints = false
                };

                _stepper.Activated += (s, e) =>
                {
                    _textField.StringValue = _stepper.IntValue.ToString();
                    StepperOption.Property.Value = _stepper.IntValue;
                };

                _textField = new NSTextField()
                {
                    StringValue = StepperOption.Property.Value.ToString(),
                    Alignment = NSTextAlignment.Right,
                    TranslatesAutoresizingMaskIntoConstraints = false
                };

                _textField.Changed += (s, e) =>
                {
                    StepperOption.Property.Value = _textField.IntValue;
                    _stepper.IntValue = _textField.IntValue;

                };


                _helpButton = CreateHelpButton();


                if (_leftLabel != null) _optionView.AddArrangedSubview(_leftLabel);

                _optionView.AddArrangedSubview(_textField);
                _optionView.AddArrangedSubview(_stepper);

                if (_helpButton != null) _optionView!.AddArrangedSubview(_helpButton);

                _textField.WidthAnchor.ConstraintEqualToConstant(50f).Active = true;
                _textField.HeightAnchor.ConstraintEqualToConstant(21f).Active = true;
                _textField.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 203).Active = true;

                _optionView.WidthAnchor.ConstraintEqualToConstant(600f).Active = true;


            }

        }


    }
}
