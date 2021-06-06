using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public class TextOptionVSMac : OptionWithLeftLabelVSMac
	{
		NSTextField _textField;

		public TextOptionVSMac(TextOption option) : base(option)
		{
		}

		protected override NSView Control
		{
			get
			{
				if (_textField == null)
				{
                    ViewModelProperty<string> property = ((TextOption) Option).Property;

					_textField = new AppKit.NSTextField();
					_textField.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
					_textField.StringValue = property.Value;
					_textField.TranslatesAutoresizingMaskIntoConstraints = false;

					property.PropertyChanged += delegate(object o, ViewModelPropertyChangedEventArgs args)
					{
						_textField.StringValue = ((string) args.NewValue) ?? string.Empty;
					};

					_textField.Changed += delegate
					{
						property.Value = _textField.StringValue;
					};
				}

				return _textField;
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
