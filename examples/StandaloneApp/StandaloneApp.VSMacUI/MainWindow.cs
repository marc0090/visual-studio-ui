using AppKit;
using CoreGraphics;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.StandaloneApp;
using Microsoft.VisualStudioUI.VSMac;
using Microsoft.VisualStudioUI.VSMac.Options;

namespace Microsoft.VisualStudioUI.StandaloneApp.VSMacUI
{
    public class MainWindow : NSWindow
    {
        private int NumberOfTimesClicked = 0;

        public NSButton ClickMeButton { get; set; }
        public NSTextField ClickMeLabel { get; set; }

        public MainWindow(CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) :
            base(contentRect, aStyle, bufferingType, deferCreation)
        {
            Title = "Options Standalone App";

#if false
			// Create the content view for the window and make it fill the window
			ContentView = new NSView(Frame);

			// Add UI Elements to window
			ClickMeButton = new NSButton(new CGRect(10, Frame.Height - 70, 100, 30))
			{
				AutoresizingMask = NSViewResizingMask.MinYMargin
			;}
			ContentView.AddSubview(ClickMeButton);
#endif

            OptionCards optionCards = Main.CreateOptionCards();
            OptionsPanelVSMac optionsPanel = new OptionsPanelVSMac(optionCards);
            //ContentView.AddSubview(optionsPanel);
            ContentView = optionsPanel.View;

#if false
			ClickMeLabel = new NSTextField(new CGRect(120, Frame.Height - 65, Frame.Width - 130, 20))
			{
				BackgroundColor = NSColor.Red,
				TextColor = NSColor.Black,
				Editable = false,
				Bezeled = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.MinYMargin,
				StringValue = "Button has not been clicked yet."
			};
			ContentView.AddSubview(ClickMeLabel);
#endif
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            /*
            // Wireup events
            ClickMeButton.Activated += (sender, e) => {
                // Update count
                ClickMeLabel.StringValue = (++NumberOfTimesClicked == 1) ? "Button clicked one time." : string.Format("Button clicked {0} times.", NumberOfTimesClicked);
            };
            */
        }
    }
}