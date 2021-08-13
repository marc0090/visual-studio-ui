using System;
using System.IO;
using System.Linq;
using AppKit;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ScaledImageFileOptionVSMac : OptionVSMac
    {
        private NSStackView _frameView;
        private NSStackView _view;
        //private NSButton _imageView;
        private const float Space = 10f;

        public ScaledImageFileOptionVSMac(ScaledImageFileOption option) : base(option)
        {
        }

        public ScaledImageFileOption ImageOption => ((ScaledImageFileOption) Option);

        public override NSView View
        {
            get
            {
                if (_view == null)
                {
                    _view = new NSStackView(){ Orientation = NSUserInterfaceLayoutOrientation.Horizontal, Alignment = NSLayoutAttribute.CenterY };
                    _view.WantsLayer = true;
                    _view.TranslatesAutoresizingMaskIntoConstraints = false;
                    float viewWidth = (ImageOption.DrawSize + Space) * ImageOption.ImageArray.Value.Length;
                    _view.WidthAnchor.ConstraintEqualToConstant(viewWidth).Active = true;

                    foreach (var item in ImageOption.ImageArray.Value)
                    {
                        CreateImageView(item);
                    }
                }

                return _view!;
            }
        }

        private void CreateImageView(ScaledImageFile imageFile)
        {
            _frameView = new NSStackView()
            {
                Orientation = NSUserInterfaceLayoutOrientation.Vertical,
                Spacing = Space,
                Distribution = NSStackViewDistribution.Fill,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            // image viewer
            NSButton imageView = new NSButton ()
            {
                WantsLayer = true,
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            imageView.Layer.BorderColor = NSColor.LightGray.CGColor;
            imageView.Layer.BorderWidth = 1f;
            imageView.Layer.CornerRadius = 4f;
            imageView.Layer.BackgroundColor = NSColor.White.CGColor;
            imageView.Activated += OnImageViewerClicked;

            // dashed border
            CAShapeLayer border = new CAShapeLayer();
            border.Position = new CGPoint(imageView.Bounds.X, imageView.Bounds.Y);
            CGPath path = new CGPath();
            float start = 10f;
            float width = (ImageOption.DrawSize - start * 2);
            CGRect pathRect = new CGRect(start, start, width, width);
            path.AddRect(pathRect);
            border.Path = path;
            border.LineWidth = 2;
            border.LineDashPattern = new NSNumber[] { 10, 3 };
            border.FillColor = NSColor.Clear.CGColor;
            border.StrokeColor = NSColor.LightGray.CGColor;
            imageView.Layer.AddSublayer(border);

            // center label
            imageView.Title = ImageOption.GetImageTitle(imageFile);
            NSAttributedString attr = new NSAttributedString(imageView.Title, foregroundColor: NSColor.Gray);
            imageView.AttributedTitle = attr;

            _frameView.AddArrangedSubview(imageView);
            imageView.WidthAnchor.ConstraintEqualToConstant(ImageOption.DrawSize).Active = true;
            imageView.HeightAnchor.ConstraintEqualToConstant(ImageOption.DrawSize).Active = true;
            if (!string.IsNullOrWhiteSpace(imageFile.Path) && File.Exists(imageFile.Path))
            {
                var image = new NSImage(imageFile.Path);
                image.Size = new CGSize(ImageOption.DrawSize, ImageOption.DrawSize);
                imageView.Image = image;
                border?.RemoveFromSuperLayer();
            }

            if (!string.IsNullOrWhiteSpace(imageFile.Hint))
            {
                imageView.ToolTip = imageFile.Hint;
            }

            // bottom label
            var bottomLabel = new NSTextField();
            bottomLabel.Editable = false;
            bottomLabel.Bordered = false;
            bottomLabel.DrawsBackground = false;
            bottomLabel.StringValue = imageFile.Label ?? string.Empty;
            bottomLabel.Alignment = NSTextAlignment.Center;
            bottomLabel.Font = NSFont.SystemFontOfSize(NSFont.SmallSystemFontSize);
            bottomLabel.TextColor = NSColor.LabelColor;
            bottomLabel.TranslatesAutoresizingMaskIntoConstraints = false;

            _frameView.AddArrangedSubview(bottomLabel);
            
            _view.AddArrangedSubview(_frameView);
        }

        private void OnImageViewerClicked(object sender, EventArgs e)
        {
            NSButton imageViewer = (NSButton)sender;

            string path = ImageOption.RedrawImageViewer(sender, e);
            if (!string.IsNullOrWhiteSpace(path))
            {
                NSImage imageNew = new NSImage(path);
                imageNew.Size = new CGSize(ImageOption.DrawSize, ImageOption.DrawSize);
                imageViewer.Image = imageNew;

                var layer = imageViewer.Layer?.Sublayers;
                if (layer != null && layer.Any())
                {
                    layer.First().RemoveFromSuperLayer();
                }

                // Update property
                ScaledImageFile imageInArrary = ImageOption.ImageArray.Value.Where(x => ImageOption.GetImageTitle(x).Equals(imageViewer?.Title)).First();
                var arrayNew = ImageOption.ImageArray.Value.Remove(imageInArrary);
                imageInArrary.Path = path;
                arrayNew = arrayNew.Add(imageInArrary);
                ImageOption.ImageArray.Value = arrayNew;
            }
        }
    }
}
