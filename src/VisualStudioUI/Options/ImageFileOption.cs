using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class ImageFileOption : Option
    {
        public float Size { get; set; } = 200f;
        public string CenterLable { get; set; } = string.Empty;

        public ImageFileOption()
        {
            Platform = OptionFactoryPlatform.Instance.CreateImageViewOptionPlatform(this);
        }
    }
}
