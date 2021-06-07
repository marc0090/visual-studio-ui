using System;
using Foundation;
using AppKit;
using CoreGraphics;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public class HintPopover : NSPopover
	{
        const float DefaultMaxWidth = 330;
        const float PopOverVerticalMargin = 13;
        const float PopOverHorizontalMargin = 14;

        readonly NSTextField textField;
        readonly NSStackView container;

        public HintPopover (string text, WarningOrErrorSeverity? severity)
        {
            ContentViewController = new NSViewController (null, null);
            Animates = false;
            Behavior = NSPopoverBehavior.Transient;

            container = new NSStackView () {
                Orientation = NSUserInterfaceLayoutOrientation.Horizontal,
                Alignment = NSLayoutAttribute.CenterY,
                Spacing = 10,
                Distribution = NSStackViewDistribution.Fill,
                EdgeInsets = new NSEdgeInsets (0, PopOverHorizontalMargin, 0, PopOverHorizontalMargin)
            };
            ContentViewController.View = container;

            textField = new NSTextField {
                DrawsBackground = false,
                Bezeled = true,
                Editable = false,
                Cell = new VerticallyCenteredTextFieldCell (yOffset: -1),
                LineBreakMode = NSLineBreakMode.ByWordWrapping
            };
            container.AddArrangedSubview (textField);

            var fontColor = GetFontColor (severity);
            var attrString = new NSAttributedString (text, new NSStringAttributes {
                ForegroundColor = fontColor,
                Font = NSFont.SystemFontOfSize (NSFont.SystemFontSize - 1),
            });

            textField.AttributedStringValue = attrString;
            MaxWidth = DefaultMaxWidth;
        }

        public nfloat MaxWidth {
            get { return textField.PreferredMaxLayoutWidth; }
            set {
                textField.PreferredMaxLayoutWidth = value;

                var expectedHeight = textField.IntrinsicContentSize.Height;

                var expectedSize = new CGSize (0, expectedHeight);
                expectedSize.Width += PopOverHorizontalMargin * 2;
                expectedSize.Height += PopOverVerticalMargin * 2;
                container.SetFrameSize (expectedSize);
            }
        }

        NSColor GetFontColor (WarningOrErrorSeverity? severity)
        {
            //TODO: Handle this
            /*
            if (severity.HasValue) {
                switch (severity) {
                case WarningOrErrorSeverity.Error:
                    return MonoDevelop.Ide.Gui.Styles.PopoverWindow.ErrorBackgroundColor.ToNSColor ();
                case WarningOrErrorSeverity.Warning:
                    return MonoDevelop.Ide.Gui.Styles.PopoverWindow.WarningBackgroundColor.ToNSColor ();
                }
            }
            */
            return NSColor.LabelColor;
        }
    }

    /// <summary>
	/// Due to it protection level, below class copy form vsmac repo = > namespace MonoDevelop.Components.Mac
	/// </summary>
    class VerticallyCenteredTextFieldCell : NSTextFieldCell {
        nfloat offset;
        public VerticallyCenteredTextFieldCell (nfloat yOffset)
        {
            offset = yOffset;
        }

        // This is invoked from the `Copy (NSZone)` method. ObjC sometimes clones the native NSTextFieldCell so we need to be able
        // to create a new managed wrapper for it.
        protected VerticallyCenteredTextFieldCell (IntPtr ptr)
            : base (ptr)
        {
        }

        /// <summary>
        /// Like what happens for the ios designer, AppKit can sometimes clone the native `NSTextFieldCell` using the Copy (NSZone)
        /// method. We *need* to ensure we can create a new managed wrapper for the cloned native object so we need the IntPtr
        /// constructor. NOTE: By keeping this override in managed we ensure the new wrapper C# object is created ~immediately,
        /// which makes it easier to debug issues.
        /// </summary>
        /// <returns>The copy.</returns>
        /// <param name="zone">Zone.</param>
        public override NSObject Copy (NSZone zone)
        {
            // Don't remove this override because the comment on this explains why we need this!
            var newCell = (VerticallyCenteredTextFieldCell) base.Copy (zone);
            newCell.offset = offset;
            return newCell;
        }

        public override CGRect DrawingRectForBounds (CGRect theRect)
        {
            // Get the parent's idea of where we should draw.
            CGRect newRect = base.DrawingRectForBounds (theRect);

            // Ideal size for the text.
            CGSize textSize = CellSizeForBounds (theRect);

            // Center in the rect.
            nfloat heightDelta = newRect.Size.Height - textSize.Height;
            if (heightDelta > 0) {
                newRect.Size = new CGSize (newRect.Width, newRect.Height - heightDelta);
                newRect.Location = new CGPoint (newRect.X, newRect.Y + heightDelta / 2 + offset);
            }
            return newRect;
        }
    }
}
