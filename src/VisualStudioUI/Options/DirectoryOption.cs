using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class DirectoryOption : Option
    {
        public DirectoryOption(ViewModelProperty<string> property)
        {
            Property = property;
            Platform = OptionFactoryPlatform.Instance.CreateCreateDirectoryOptionlatform(this);
        }

        public ViewModelProperty<string> Property { get; }

    }

}
