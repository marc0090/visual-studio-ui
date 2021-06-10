using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
	public class RadioButtonOption : Option
	{
		public string Title { get; }

		public RadioButtonOption (string title)
		{
			Title = title;
			Platform = OptionFactoryPlatform.Instance.CreateRadioButtonOptionPlatform (this);
		}
	}
}