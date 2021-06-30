using System;
using AppKit;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ImageFileOptionVSMac : OptionVSMac
    {
        NSView _frameView;
        /*NSImageView*/NSButton _imageView;

        public ImageFileOptionVSMac(ImageFileOption option) : base(option)
        {
        }

        public ImageFileOption ImageOption => ((ImageFileOption) Option);

        public override NSView View
        {
            get
            {
                if (_frameView == null)
                {
                    //TODO: 
                    _frameView = new AppKit.NSView();
                    
                    var optionHeightConstraint = _frameView.HeightAnchor.ConstraintEqualToConstant(ImageOption.Size + 20f);
                    optionHeightConstraint.Active = true;

                    // image viewer
                    _imageView = new AppKit.NSButton(); 
                    _imageView.WantsLayer = true;
                    _imageView.TranslatesAutoresizingMaskIntoConstraints = false;
                    _imageView.Layer.BorderColor = NSColor.LightGray.CGColor;
                    _imageView.Layer.BorderWidth = 1f;
                    _imageView.Layer.CornerRadius = 4f;
                    _imageView.Layer.BackgroundColor = NSColor.White.CGColor;
                    _imageView.Activated += OnAddClicked;
                    CAShapeLayer border = new CAShapeLayer();

                    // dashed border
                    border.Position = new  CGPoint(_imageView.Bounds.X, _imageView.Bounds.Y);
                    CGPath path = new CGPath();
                    float start = 10f;
                    float width = (ImageOption.Size - start * 2);
                    CGRect pathRect = new CGRect(start, start, width, width);
                    path.AddRect(pathRect);
                    border.Path = path;
                    border.LineWidth = 2;
                    border.LineDashPattern = new NSNumber[] { 10,3};
                    border.FillColor = NSColor.Clear.CGColor;
                    border.StrokeColor = NSColor.LightGray.CGColor;
                    _imageView.Layer.AddSublayer(border);

                    // center label
                    _imageView.Title = ImageOption.CenterLable;
                    NSAttributedString attr =  new NSAttributedString(_imageView.Title, foregroundColor: NSColor.Gray);
                    _imageView.AttributedTitle = attr;

                    _frameView.AddSubview(_imageView);
                    _imageView.WidthAnchor.ConstraintEqualToConstant(ImageOption.Size).Active = true;
                    _imageView.HeightAnchor.ConstraintEqualToConstant(ImageOption.Size).Active = true;
                    _imageView.TopAnchor.ConstraintEqualToAnchor(_frameView.TopAnchor, 0f).Active = true;

                    // bottom label
                    var bottomLabel = new NSTextField();
                    bottomLabel.Editable = false;
                    bottomLabel.Bordered = false;
                    bottomLabel.DrawsBackground = false;
                    bottomLabel.StringValue = ImageOption.Label ?? string.Empty;
                    bottomLabel.Alignment = NSTextAlignment.Center;
                    bottomLabel.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                    bottomLabel.TextColor = NSColor.LabelColor;
                    bottomLabel.TranslatesAutoresizingMaskIntoConstraints = false;

                    bottomLabel.WidthAnchor.ConstraintEqualToConstant(ImageOption.Size).Active = true;
                    _frameView.AddSubview(bottomLabel);
                }

                return _frameView;
            }
        }
        
        private void OnAddClicked(object sender, EventArgs e)
        {
            var openPanel = new NSOpenPanel();
            openPanel.CanChooseFiles = true;
            var response = openPanel.RunModal();
            if (response == 1 && openPanel.Url != null)
            {
                _imageView.Image = new NSImage(openPanel.Url.Path);
            }
        }

        /*
		public override void Dispose ()
		{
			Property.PropertyChanged -= UpdatePopUpBtnValue;
			textField.Changed -= UpdatePropertyValue;

			base.Dispose ();
		}
		*/
    }
}
