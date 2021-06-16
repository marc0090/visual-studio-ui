using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class SeparatorOptionVSMac : OptionVSMac
    {
        NSView _separatorView;

        public SeparatorOptionVSMac(SeparatorOption option) : base(option)
        {
        }

        public override NSView View
        {
            get
            {
                if (_separatorView == null)
                {
                    _separatorView = CreateView();
                }

                return _separatorView;
            }
        }

        private NSView CreateView()
        {
            // View:     separatorView
            var separatorView = new NSView();
            separatorView.WantsLayer = true;
            separatorView.TranslatesAutoresizingMaskIntoConstraints = false;

            var separatorViewWidthConstraint = separatorView.WidthAnchor.ConstraintEqualToConstant(600f);
            separatorViewWidthConstraint.Active = true;
            var separatorViewHeightConstraint = separatorView.HeightAnchor.ConstraintEqualToConstant(31f);
            separatorViewHeightConstraint.Active = true;

            // View:     boxView
            var boxView = new NSBox();
            boxView.BoxType = NSBoxType.NSBoxSeparator;
            boxView.Title = "";
            boxView.TranslatesAutoresizingMaskIntoConstraints = false;

            separatorView.AddSubview(boxView);
            var boxViewWidthConstraint = boxView.WidthAnchor.ConstraintEqualToConstant(600f);
            boxViewWidthConstraint.Priority = (System.Int32) AppKit.NSLayoutPriority.DefaultLow;
            boxViewWidthConstraint.Active = true;
            var boxViewHeightConstraint = boxView.HeightAnchor.ConstraintEqualToConstant(1f);
            boxViewHeightConstraint.Active = true;

            boxView.RightAnchor.ConstraintEqualToAnchor(separatorView.RightAnchor, 0f).Active = true;
            boxView.LeftAnchor.ConstraintEqualToAnchor(separatorView.LeftAnchor, 0f).Active = true;
            boxView.TopAnchor.ConstraintEqualToAnchor(separatorView.CenterYAnchor, -0.5f).Active = true;

            return separatorView;
        }
    }
}