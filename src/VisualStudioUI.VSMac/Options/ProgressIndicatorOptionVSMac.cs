using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ProgressIndicatorOptionVSMac : OptionVSMac
    {
        private NSView _optionView;
        private NSProgressIndicator _progressIndicator;
        private NSStackView _childrenControl;

        public ProgressIndicatorOptionVSMac(ProgressIndicatorOption option) : base(option)
        {
        }

        public ProgressIndicatorOption ProgressIndicatorOption => ((ProgressIndicatorOption)Option);

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

        private void CreateView()
        {
            _optionView = new NSView();
            _optionView.WantsLayer = true;
            _optionView.TranslatesAutoresizingMaskIntoConstraints = false;

            _optionView.WidthAnchor.ConstraintEqualToConstant(600f).Active = true;
            _optionView.HeightAnchor.ConstraintEqualToConstant(100f).Active = true;

            if (!string.IsNullOrEmpty(Option.Label))
            {
                // View:     label
                var label = new NSTextField();
                label.Editable = false;
                label.Bordered = false;
                label.DrawsBackground = false;
                label.PreferredMaxLayoutWidth = 1;
                // TODO: Make the colon be localization friendly
                label.StringValue = Option.Label + ":";

                label.Alignment = NSTextAlignment.Right;
                label.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                label.TextColor = NSColor.LabelColor;
                label.TranslatesAutoresizingMaskIntoConstraints = false;

                _optionView.AddSubview(label);
                label.WidthAnchor.ConstraintEqualToConstant(205f).Active = true;
                label.HeightAnchor.ConstraintEqualToConstant(16f).Active = true;

                label.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor, 6f).Active = true;
                label.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 7f).Active = true;
            }

            _progressIndicator = new NSProgressIndicator(new CoreGraphics.CGRect(0, 0, 18, 18));
            _progressIndicator.Style = NSProgressIndicatorStyle.Spinning;
            _progressIndicator.ControlSize = NSControlSize.Small;
            _progressIndicator.IsDisplayedWhenStopped = false;
            _progressIndicator.TranslatesAutoresizingMaskIntoConstraints = false;
            _progressIndicator.SizeToFit();
            _optionView.AddSubview(_progressIndicator);

            _progressIndicator.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor, 215f).Active = true;
            _progressIndicator.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor, 5f).Active = true;

            // Default is start
            _progressIndicator.StartAnimation(null);

            ProgressIndicatorOption.ShowSpinner.PropertyChanged += SpinnerChanged;

            // Add other option element
            _childrenControl = new NSStackView()
            {
                Orientation = NSUserInterfaceLayoutOrientation.Vertical,
                Distribution = NSStackViewDistribution.Fill,
                TranslatesAutoresizingMaskIntoConstraints = false,
            };

            if ( ProgressIndicatorOption.Element != null)
            {
                NSView optionView = ((OptionVSMac)ProgressIndicatorOption.Element.Platform).View;
                _childrenControl.AddArrangedSubview(optionView);
            }

            _optionView.AddSubview(_childrenControl);

            _childrenControl.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor).Active = true;
            _childrenControl.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, _progressIndicator.FittingSize.Width).Active = true;
        }

        public void SpinnerChanged(object sender, ViewModelPropertyChangedEventArgs e)
        {
            if (ProgressIndicatorOption.ShowSpinner.Value)
            {
                _progressIndicator.StartAnimation(null);
            }
            else
            {
                _progressIndicator.StopAnimation(null);
            }
        }
    }
}
