using System;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// This class will go away, being replaced by RadioButtonOption, CheckBoxOption, and SwitchOption. 
    /// </summary>
    public class ButtonOption : Option
    {
        public enum ButtonType
        {
            Normal = -1,
            CheckBox = 3, // NSButtonType.Switch
            Radio = 4, // NSButtonType.Radio
        }

        public string Description { get; set; }
        public ButtonType Type { get; }

        public ViewModelProperty<bool> IsSelected { get; set; } = null;

        public event EventHandler SelectionChanged;
        public event EventHandler Clicked;

        public void UpdateStatus(object sender, EventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);
        }

        public void ButtonClicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
        }

        public ButtonOption(ButtonType btnType = ButtonType.Normal)
        {
            Type = btnType;
            Platform = OptionFactoryPlatform.Instance.CreateButtonOptionPlatform(this);
        }
    }
}
