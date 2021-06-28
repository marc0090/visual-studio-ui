using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class SwitchOption : ToggleButtonOption
    {
        public SwitchOption(ViewModelProperty<bool> property) : base(property)
        {
            Platform = OptionFactoryPlatform.Instance.CreateSwitchOptionPlatform(this);
        }
    }
}
