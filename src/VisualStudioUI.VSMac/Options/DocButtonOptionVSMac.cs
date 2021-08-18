using AppKit;
using Foundation;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class DocButtonOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSButton _button;

        public DocButtonOptionVSMac(DocButtonOption option) : base(option)
        {
        }

        public DocButtonOption DocButtonOption => ((DocButtonOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_button == null)
                {
                    ViewModelProperty<string> property = DocButtonOption.UrlProperty;

                    _button = new NSButton
                    {
                        BezelStyle = NSBezelStyle.RoundRect,
                        Title = DocButtonOption.ButtonLabel,
                        ControlSize = NSControlSize.Regular,
                        Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
                        TranslatesAutoresizingMaskIntoConstraints = false
                    };
                    _button.SizeToFit();
                    string url = DocButtonOption.UrlProperty.Value;
                    _button.Activated += (o, args) => NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl(url));
                }

                return _button;
            }
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);

            _button.Enabled = enabled;
        }

    }
}