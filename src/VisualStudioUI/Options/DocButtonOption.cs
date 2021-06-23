using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class DocButtonOption : Option
    {
        public ViewModelProperty<string> UrlProperty { get; }
        public string ButtonLabel { get; }

        public DocButtonOption(ViewModelProperty<string> urlProperty, string buttonLabel)
        {
            UrlProperty = urlProperty;
            ButtonLabel = buttonLabel;
            Platform = OptionFactoryPlatform.Instance.CreateDocButtonOptionPlatform(this);
        }
    }
}