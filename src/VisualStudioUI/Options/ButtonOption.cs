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

        public ButtonType Type { get; }

        public ViewModelProperty<bool> Active { get; set; }
        public ViewModelProperty<bool> Hidden { get; set; }
        public ViewModelProperty<bool> Enable { get; set; }

        public event EventHandler Actived;
        public event EventHandler Clicked;

        public void UpdateStatus(object sender, EventArgs e)
        {
            Actived?.Invoke(sender, e);
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
