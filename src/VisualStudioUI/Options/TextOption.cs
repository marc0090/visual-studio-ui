using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
	public class TextOption : Option
	{
		public bool Editable { get; set; } = true;
		public bool Bordered { get; set; } = true;
		public bool DrawsBackground { get; set; } = true;

		public TextOption(ViewModelProperty<string> property)
		{
			Property = property;
			Platform = OptionFactoryPlatform.Instance.CreateTextOptionPlatform(this);
		}
	
		public ViewModelProperty<string> Property { get; }
	}
}
