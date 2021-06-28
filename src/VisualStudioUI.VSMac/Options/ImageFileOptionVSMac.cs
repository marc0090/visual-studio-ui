using AppKit;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ImageFileOptionVSMac : OptionWithLeftLabelVSMac
    {
        NSView _frameView;
        NSImageView _imageView;

        public ImageFileOptionVSMac(ImageFileOption option) : base(option)
        {
        }

        public ImageFileOption TextOption => ((ImageFileOption) Option);

        protected override NSView ControlView
        {
            get
            {
                if (_imageView == null)
                {
                    //TODO: 
                    _imageView = new AppKit.NSImageView();
                    _imageView.WantsLayer = true;
                    _imageView.TranslatesAutoresizingMaskIntoConstraints = false;
                    _imageView.Layer.BorderColor = NSColor.FromRgba(0f, 0f, 0f, 1f).CGColor;
                    _imageView.Layer.BorderWidth = 1;

                    var imageViewWidthConstraint = _imageView.WidthAnchor.ConstraintEqualToConstant(80f);
                    imageViewWidthConstraint.Priority = (System.Int32) AppKit.NSLayoutPriority.DefaultLow;
                    imageViewWidthConstraint.Active = true;
                    var imageViewHeightConstraint = _imageView.HeightAnchor.ConstraintEqualToConstant(50f);
                    imageViewHeightConstraint.Active = true;
                }

                return _imageView;
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
