namespace Microsoft.VisualStudioUI.Options
{
    public class SeparatorOption : Option
    {
        public float Width { get; set; } = 600f;

        public SeparatorOption()
        {
            Platform = OptionFactoryPlatform.Instance.CreateSeparatorOptionPlatform(this);
        }
    }
}
