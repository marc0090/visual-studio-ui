using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class LabelOption : Option
    {
        public bool IsBold { get; set; } = true;
        public ViewModelProperty<bool> Hidden { get; set; }

        public LabelOption()
        {
            Platform = OptionFactoryPlatform.Instance.CreateLabelOptionPlatform(this);
        }
    }
}

