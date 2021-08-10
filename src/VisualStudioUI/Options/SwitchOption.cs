
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class SwitchOption : ToggleButtonOption
    {
        public ViewModelProperty<bool> ShowSpinner { get; }

        public SwitchOption(ViewModelProperty<bool> isOn) : base(isOn)
        {
            ShowSpinner = new ViewModelProperty<bool>("showSpinner", false);
            Property.Bind();
            ShowSpinner.Bind();
            Platform = OptionFactoryPlatform.Instance.CreateSwitchOptionPlatform(this);
        }
    }
}
