using System;
using System.IO;
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
            NSButton imageView = new NSButton
            {
                WantsLayer = true,
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            imageView.Layer.BorderColor = NSColor.LightGray.CGColor;
            imageView.Layer.BorderWidth = 1f;
            imageView.Layer.CornerRadius = 4f;
            imageView.Layer.BackgroundColor = NSColor.White.CGColor;
            imageView.Activated += OnAddClicked;
            
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
            imageView.Title = GetImageTitle(imageFile);
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

        private void OnAddClicked(object sender, EventArgs e)
        {
            NSButton imageViewer = (NSButton)sender;

            var openPanel = new NSOpenPanel();
            openPanel.CanChooseFiles = true;
            openPanel.ExtensionHidden = true;
            openPanel.AllowedFileTypes = new[] { "png" }; 
            var response = openPanel.RunModal();
            if (response == 1 && openPanel.Url != null)
            {
                NSImage? imageNew = null;
                if (!string.IsNullOrWhiteSpace(imageViewer.Title))
                {
                    var imageOld = GetImageFile(imageViewer.Title);

                    imageNew = new NSImage(openPanel.Url.Path);

                    if (imageNew.CGImage.Width != imageOld?.Width || imageNew.CGImage.Height != imageOld?.Height)
                    {
                        NSAlert alert = new NSAlert();
                        alert.AlertStyle = NSAlertStyle.Critical;
                        //alert.Icon = NSImage.GetSystemSymbol("xmark.circle", null);
                        alert.MessageText = "Incorrect image dimensions";
                        alert.InformativeText = string.Format("Only images with size {0}x{1} are allowed. Picture was {2}x{3}.", imageOld?.Width, imageOld?.Height, imageNew.CGImage.Width, imageNew.CGImage.Height);
                        alert.RunSheetModal(null);

                        return;
                    }
                }
                else
                {
                    //TODO: throw exception/ write log
                }

                if (imageViewer.Image != null)
                {
                    NSAlert alert = new NSAlert();
                    alert.AlertStyle = NSAlertStyle.Informational;
                    alert.AddButton("No");
                    alert.AddButton("Yes");
                    alert.Icon = NSImage.GetSystemSymbol("info.circle", null);
                    alert.MessageText = "Image already exists";
                    alert.InformativeText = string.Format("Should {0} be overwritten?", imageViewer.Title);
                    var result = alert.RunSheetModal(null);
                    if (result == (int)NSAlertButtonReturn.First)
                    {
                        // Don't need to overwrite
                        return;
                    }
                }

                if (imageNew != null && imageNew.IsValid)
                {
                    imageNew.Size = new CGSize(ImageOption.DrawSize, ImageOption.DrawSize);
                    imageViewer.Image = imageNew;
                }
                else
                {
                    //TODO: throw exception/ write log
                }
            }
        }

        private ScaledImageFile? GetImageFile(string title)
        {
            foreach (var item in ImageOption.ImageArray.Value)
            {
                if (title.Equals(GetImageTitle(item)))
                {
                    return item;
                }
            }

            return null;
        }

        private string GetImageTitle(ScaledImageFile imageFile)
        {
            return string.Format("({0}x{1})", imageFile?.Width, imageFile?.Height);
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
