using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.VSMac.Options;

namespace Microsoft.VisualStudioUI.StandaloneApp.VSMacUI
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            OptionFactoryPlatform.Initialize(new OptionFactoryVSMac());
            
            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}
    