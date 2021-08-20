// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Microsoft.VisualStudioUI.Options.Models
{
    public class ScaledImageFile
    {
        /// <summary>
        /// Image path of slected
        /// </summary>
        private string _path;
        public string? Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value ?? string.Empty;
            }
        }

        public float Width { get; }
        public float Height { get; }

        /// <summary>
        /// The bottom label of image viewer 
        /// </summary>
        public string? Label { get; }
        public string Hint { get; set; }

        public ScaledImageFile(float width, float height, string label)
        {
            Width = width;
            Height = height;
            Label = label;
        }
    }
}
