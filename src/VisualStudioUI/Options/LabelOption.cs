namespace Microsoft.VisualStudioUI.Options
{
    public class LabelOption : Option
    {
        public bool IsBold { get; set; } = true;

        public LabelOption()
        {
            Platform = OptionFactoryPlatform.Instance.CreateLabelOptionPlatform(this);
        }
    }
}

