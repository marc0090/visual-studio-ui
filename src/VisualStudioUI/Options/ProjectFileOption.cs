using System;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class ProjectFileOption : Option
    {
        public event EventHandler Clicked;

        public ProjectFileOption(ViewModelProperty<string> property)
        {
            Property = property;
            Platform = OptionFactoryPlatform.Instance.CreateCreateProjectFileOptionlatform(this);
        }

        public ViewModelProperty<string> Property { get; }
        public void ButtonClicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
        }
    }
}
