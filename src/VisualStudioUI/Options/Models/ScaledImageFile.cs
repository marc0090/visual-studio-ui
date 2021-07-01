using System;
namespace Microsoft.VisualStudioUI.Options.Models
{
    public class ScaledImageFile
    {
        /// <summary>
        /// Image path of slected
        /// </summary>
        public string? Path { get; set; }

        public float Width { get; }
        public float Height { get; }

        /// <summary>
        /// The bottom label of image viewer 
        /// </summary>
        public string? Label { get; }

        public ScaledImageFile(float width, float height, string label)
        {
            Width = width;
            Height = height;
            Label = label;
        }
    }
}
