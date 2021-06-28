using AppKit;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public abstract class OptionWithLeftLabelVSMac : OptionVSMac
    {
        private NSView? _optionView;
        private NSButton? _helpButton;

        public OptionWithLeftLabelVSMac(Option option) : base(option)
        {
        }

        public override NSView View
        {
            get
            {
                if (_optionView == null)
                {
                    CreateView();
                }

                return _optionView!;
            }
        }

        protected abstract NSView ControlView { get; }

        private void CreateView()
        {
            // View:     optionView
            _optionView = new AppKit.NSView();
            _optionView.WidthAnchor.ConstraintEqualToConstant(600f).Active = true;

            NSTextField? label = CreateLabelView();
            if (label != null)
            {
                _optionView.AddSubview(label);

                label.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 6f).Active = true;
                label.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 7f).Active = true;
            }

            var control = ControlView;
            // TODO: Set a11y info properly
            control.AccessibilityLabel = "Control";
            control.AccessibilityHelp = "Provides a control";

            _optionView.AddSubview(control);

            control.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 220f).Active = true;
            control.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5f).Active = true;

            UpdateHelpButton();

            NSTextField? descriptionView = CreateDescriptionView();
            if (descriptionView != null)
            {
                _optionView.AddSubview(descriptionView);

                descriptionView.LeadingAnchor.ConstraintEqualToAnchor(control.LeadingAnchor, 0f).Active = true;
                descriptionView.TopAnchor.ConstraintEqualToAnchor(control.BottomAnchor, 0f).Active = true;

                _optionView.BottomAnchor.ConstraintEqualToAnchor(descriptionView.BottomAnchor).Active = true;
            }
            else
            {
                _optionView.BottomAnchor.ConstraintEqualToAnchor(control.BottomAnchor).Active = true;
            }
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

            _optionView!.AddSubview(_helpButton);

            _helpButton.TrailingAnchor.ConstraintEqualToAnchor(_optionView.TrailingAnchor, -6f).Active = true;
            _helpButton.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5f).Active = true;
        }
    }
}
