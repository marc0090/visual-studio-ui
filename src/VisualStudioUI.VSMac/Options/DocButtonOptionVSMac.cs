using System;
using AppKit;
using Foundation;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class DocButtonOptionVSMac : OptionWithLeftLabelVSMac
    {
        NSButton _button;

        public DocButtonOptionVSMac(DocButtonOption option) : base(option)
        {
        }

        public DocButtonOption DocButtonOption => ((DocButtonOption) Option);

        protected override NSView ControlView
        {
            get
            {
                if (_button == null)
                {
                    ViewModelProperty<string> property = DocButtonOption.UrlProperty;

                    _button = new AppKit.NSButton();
                    _button.BezelStyle = NSBezelStyle.RoundRect;
                    _button.Title = DocButtonOption.ButtonLabel;
                    _button.ControlSize = NSControlSize.Regular;
                    _button.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
                    _button.TranslatesAutoresizingMaskIntoConstraints = false;

                    _button.WidthAnchor.ConstraintEqualToConstant(196f).Active = true;
                    //_button.HeightAnchor.ConstraintEqualToConstant(18f).Active = true;

                    string url = DocButtonOption.UrlProperty.Value;
                    _button.Activated += (o, args) => NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl(url));
                }

                return _button;
            }
        }

        /*
        public override void Dispose ()
        {
            linkButton.Activated -= LinkButton_Activated;
            base.Dispose ();
        }
        */
    }
}