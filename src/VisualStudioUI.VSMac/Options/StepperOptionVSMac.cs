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
                    if (_textField.IntValue > StepperOption.Maximum)
                    {
                        _textField.IntValue = StepperOption.Maximum;
                    }
                    else if (_textField.IntValue < StepperOption.Minimum)
                    {
                        _textField.IntValue = StepperOption.Minimum;
                    }

                    StepperOption.Property.Value = _textField.IntValue;
                    _stepper.IntValue = _textField.IntValue;
                };


                _helpButton = CreateHelpButton();

                if (_leftLabel != null) _optionView.AddArrangedSubview(_leftLabel);

                _optionView.AddArrangedSubview(_textField);
                _optionView.AddArrangedSubview(_stepper);

                if (_helpButton != null) _optionView!.AddArrangedSubview(_helpButton);

                _leftLabel.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, IndentValue()).Active = true;

                _textField.WidthAnchor.ConstraintEqualToConstant(64f).Active = true;
                _textField.HeightAnchor.ConstraintEqualToConstant(21f).Active = true;
                _textField.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 222 + IndentValue()).Active = true;
                _textField.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5).Active = true;
                _textField.BottomAnchor.ConstraintEqualToAnchor(_optionView.BottomAnchor, 5).Active = true;
                _stepper.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5).Active = true;
                _stepper.LeadingAnchor.ConstraintEqualToAnchor(_textField.TrailingAnchor, 10).Active = true;
                _optionView.WidthAnchor.ConstraintEqualToConstant(600f).Active = true;
            }

        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);

            if (_leftLabel != null)
                _leftLabel.Enabled = enabled;

            if (_textField != null)
                _textField.Enabled = enabled;

            _stepper.Enabled = enabled;
        }


        protected override void UpdateHelpButton()
        {
            if (_helpButton != null)
            {
                _helpButton.RemoveFromSuperview();
                _helpButton.Dispose(); // TODO: Is this needed?
                _helpButton = null;
            }

            _helpButton = CreateHelpButton();

            if (_helpButton == null)
                return;

            _optionView!.AddArrangedSubview(_helpButton);
        }


    }
}
