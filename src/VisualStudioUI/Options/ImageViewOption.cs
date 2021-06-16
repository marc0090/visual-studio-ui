using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class ImageViewOption : Option
    {
        public ImageViewOption()
        {
            Platform = OptionFactoryPlatform.Instance.CreateImageViewOptionPlatform(this);
        }
    }
}