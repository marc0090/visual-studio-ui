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
        public string MenuLabel { get; }

        public ViewModelProperty<ImmutableArray<ScaledImageFile>> ImageArray { get; }

        public Action<object, EventArgs>? UnsetImage;

        public ScaledImageFileOption(ViewModelProperty<ImmutableArray<ScaledImageFile>> imageArray, string menuLabel = "")
        {
            ImageArray = imageArray;
            MenuLabel = menuLabel;
            Platform = OptionFactoryPlatform.Instance.CreateImageViewOptionPlatform(this);
        }

        public void UnsetImageViewer(object sender, EventArgs e)
        {
            UnsetImage?.Invoke(sender, e);
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
