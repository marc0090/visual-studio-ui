using System;

namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// This class will go away, being replaced by RadioButtonOption, CheckBoxOption, and SwitchOption. 
    /// </summary>
    public class ButtonOption : Option
    {
        public string ButtonLabel { get; set; } = "";

        public event EventHandler Clicked;

        public void ButtonClicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
        }

        public ButtonOption()
        {
            Platform = OptionFactoryPlatform.Instance.CreateButtonOptionPlatform(this);
        }
    }
}
