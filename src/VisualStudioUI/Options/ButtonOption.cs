using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
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

		public ButtonOption (ButtonType btnType = ButtonType.Normal)
		{
			Type = btnType;
			Platform = OptionFactoryPlatform.Instance.CreateRadioButtonOptionPlatform (this);
		}
	}
}