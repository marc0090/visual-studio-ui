using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class ImageFileOption : Option
    {
        public ImageFileOption()
        {
            Platform = OptionFactoryPlatform.Instance.CreateImageViewOptionPlatform(this);
        }
    }
}