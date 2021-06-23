using AppKit;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public abstract class OptionVSMac : OptionPlatform
    {
        public OptionVSMac(Option option) : base(option)
        {
        }

        public abstract NSView View { get; }

        public override void OnPropertiesChanged()
        {
        }
    }
}