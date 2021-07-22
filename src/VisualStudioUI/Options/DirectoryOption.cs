using System;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class DirectoryOption : Option
    {

        public event EventHandler Clicked;

        public DirectoryOption(ViewModelProperty<string> property)
        {
            Property = property;
            Platform = OptionFactoryPlatform.Instance.CreateCreateDirectoryOptionlatform(this);
        }

        public ViewModelProperty<string> Property { get; }
        public void ButtonClicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
        }
    }

}
