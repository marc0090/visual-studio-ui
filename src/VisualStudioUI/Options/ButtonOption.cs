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
		public string Title { get; }
		public ButtonType Type { get; }

		public ButtonOption (string title, ButtonType btnType = ButtonType.Normal)
		{
			Title = title;
			Type = btnType;
			Platform = OptionFactoryPlatform.Instance.CreateRadioButtonOptionPlatform (this);
		}
	}
}