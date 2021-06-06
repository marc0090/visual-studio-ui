namespace Microsoft.VisualStudioUI.Options
{
	public abstract class OptionPlatform
	{
		public Option Option { get; }

		public OptionPlatform(Option option)
		{
			Option = option;
		}

		public abstract void OnPropertiesChanged();
	}
}
