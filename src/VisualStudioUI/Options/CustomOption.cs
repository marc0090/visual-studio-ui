namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// A CustomOption can contain arbitrary (platform specific) UI, defined by the
    /// client. 
    /// </summary>
    public class CustomOption : Option
    {
        public CustomOption()
        {
        }

        /// <summary>
        /// Set the platform specific UI. To create a custom option, create a subclass of the
        /// platform's base class (e.g. OptionVSMac for VS Mac or OptionVSWin for VS Windows),
        /// which wraps the platform specific UI (e.g. wraps a Cocoa NSView for VS Mac UI or
        /// a WPF FrameworkElement for VS Win UI). Then set the instance of that here.
        /// </summary>
        /// <param name="optionPlatform">platform code for the custom option</param>
        public void InitPlatform(OptionPlatform optionPlatform)
        {
            Platform = optionPlatform;
        }
    }
}
