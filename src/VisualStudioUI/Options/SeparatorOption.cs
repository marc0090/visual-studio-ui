namespace Microsoft.VisualStudioUI.Options
{
	public class SeparatorOption : Option
	{
		public SeparatorOption()
		{
			Platform = OptionFactoryPlatform.Instance.CreateSeparatorOptionPlatform(this);
		}
	}
}
