using System;
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

        public Func<object, EventArgs, string> RedrawImage;

        public ScaledImageFileOption(ViewModelProperty<ImmutableArray<ScaledImageFile>> imageArray)
        {
            ImageArray = imageArray;
            Platform = OptionFactoryPlatform.Instance.CreateImageViewOptionPlatform(this);
        }

        public string RedrawImageViewer(object sender, EventArgs e)
        {
            return RedrawImage?.Invoke(sender, e);
        }

        public ScaledImageFile? GetImageFile(string title)
        {
            foreach (var item in ImageArray.Value)
            {
                if (title.Equals(GetImageTitle(item)))
                {
                    return item;
                }
            }

            return null;
        }

        public string GetImageTitle(ScaledImageFile imageFile)
        {
            return string.Format("({0}x{1})", imageFile?.Width, imageFile?.Height);
        }
    }
}
