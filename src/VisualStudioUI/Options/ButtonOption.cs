using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
	public class ButtonOption : Option
	{
		public enum ButtonType
        {
			Normal = 0,
			CheckBox = 3, // NSButtonType.Switch
			Radio = 4, // NSButtonType.Radio
		}
		public string Describetion { get; set; }
		public ButtonType Type { get; }
		//public string Describetion { get; set; }

		public ButtonOption (ButtonType btnType = ButtonType.Normal)
		{
			Type = btnType;
			Platform = OptionFactoryPlatform.Instance.CreateRadioButtonOptionPlatform (this);
		}
	}
}