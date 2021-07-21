﻿
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class FileEntryOption : Option
    {

        public bool Editable { get; set; } = true;
        public bool Bordered { get; set; } = true;
        public bool DrawsBackground { get; set; } = true;
        public string ButtonLabel { get; set; } = "Browse...";
        public FileEntryOption(ViewModelProperty<string> property)
        {
            Property = property;
            Platform = OptionFactoryPlatform.Instance.CreateCreateFileEntryOptionlatform(this);
        }

        public ViewModelProperty<string> Property { get; }
    }

}
