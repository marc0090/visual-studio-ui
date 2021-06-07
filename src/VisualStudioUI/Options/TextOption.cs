using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
	public class TextOption : Option
	{
		public TextOption(ViewModelProperty<string> property)
		{
			Property = property;
			Platform = OptionFactoryPlatform.Instance.CreateTextOptionPlatform(this);
		}
	
		public ViewModelProperty<string> Property { get; }
	}
}
