using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// A DocButtonOption shows a help-style button that when pressed launches
    /// the browser for the specified URL. It normally points to web doc.
    /// </summary>
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
