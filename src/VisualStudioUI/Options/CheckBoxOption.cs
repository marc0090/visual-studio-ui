using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class CheckBoxOption : ToggleButtonOption
    {
        public CheckBoxOption(ViewModelProperty<bool> property) : base(property)
        {
            Platform = OptionFactoryPlatform.Instance.CreateCheckBoxOptionPlatform(this);
        }
    }
}
