using AppKit;
using CoreGraphics;
using Microsoft.VisualStudioUI.Options;

//using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class OptionsPanelVSMac : NSScrollView {
        NSView _documentView;

        public OptionsPanelVSMac (OptionCards optionCards, CGRect frameRect) : base()
        {
            DrawsBackground = false;

            _documentView = new NSView();
            DocumentView = _documentView;

            NSView optionsView = ((OptionCardsVSMac) optionCards.Platform).View;
            _documentView.AddSubview(optionsView);
            optionsView.TopAnchor.ConstraintEqualToAnchor(_documentView.TopAnchor, 10f).Active = true;
            optionsView.LeftAnchor.ConstraintEqualToAnchor(_documentView.LeftAnchor, 10f).Active = true;
            optionsView.RightAnchor.ConstraintEqualToAnchor(_documentView.RightAnchor, 10f).Active = true;
            optionsView.BottomAnchor.ConstraintEqualToAnchor(_documentView.BottomAnchor, 10f).Active = true;
        }
        
        public override void SetFrameSize (CGSize newSize)
        {
            base.SetFrameSize (newSize);
            _documentView.Frame = new CGRect (0, 0, newSize.Width, newSize.Height);
        }
    }
}
