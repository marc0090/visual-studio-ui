namespace Microsoft.VisualStudioUI.Options
{
    public class CustomOption : Option
    {
        public CustomOption()
        {
        }

        public void InitPlatform(OptionPlatform optionPlatform)
        {
            Platform = optionPlatform;
        }
    }
}