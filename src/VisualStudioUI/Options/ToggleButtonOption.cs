using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public abstract class ToggleButtonOption : Option
    {
        public ViewModelProperty<bool> Property { get; }

        /// <summary>
        /// The label for the toggle control itself. 
        /// </summary>
        public string ButtonLabel { get; set; } = "";

        protected ToggleButtonOption(ViewModelProperty<bool> property)
        {
            Property = property;
        }
    }
}
