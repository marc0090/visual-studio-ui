using System.Collections.Immutable;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class ScaledImageFileOption : Option
    {
        /// <summary>
        /// The width and height of diplayed image viewer
        /// </summary>
        public float DrawSize { get; set; } = 200f;

        public ViewModelProperty<ImmutableArray<ScaledImageFile>> ImageArray { get; }

        public ScaledImageFileOption(ViewModelProperty<ImmutableArray<ScaledImageFile>> imageArray)
        {
            ImageArray = imageArray;
            Platform = OptionFactoryPlatform.Instance.CreateImageViewOptionPlatform(this);
        }
    }
}
