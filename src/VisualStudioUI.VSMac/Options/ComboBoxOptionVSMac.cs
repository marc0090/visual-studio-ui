using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public class ComboBoxOptionVSMac : OptionWithLeftLabelVSMac
	{
		NSPopUpButton _popUpButton;

		public ViewModelProperty<string> SdkWarning { get; set; } = null;
		public bool NullIsDefault { get; set; } = false;

		public ComboBoxOptionVSMac(ComboBoxOption option) : base(option)
		{
		}

		public ComboBoxOption ComboBoxOption => ((ComboBoxOption)Option);

		protected override NSView Control
		{
			get
			{
				if (_popUpButton == null)
				{
					ViewModelProperty<string> property = ComboBoxOption.Property;
					ViewModelProperty<string[]> itemsProperty = ComboBoxOption.ItemsProperty;
					
					// View:     popUpButton
					_popUpButton = new AppKit.NSPopUpButton();
					_popUpButton.BezelStyle = NSBezelStyle.Rounded;
					_popUpButton.ControlSize = NSControlSize.Regular;
					_popUpButton.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
					//_popUpButton.AddItem ("Default");
					_popUpButton.TranslatesAutoresizingMaskIntoConstraints = false;

					_popUpButton.WidthAnchor.ConstraintEqualToConstant(198f).Active = true;

					_popUpButton.Activated += UpdatePropertyFromUI;
					property.PropertyChanged += UpdateUIFromProperty;
					itemsProperty.PropertyChanged += delegate { UpdateItemChoices(); };
					
					UpdateItemChoices();

					// TODO: Handle this
					/*
					if (SdkWarning != null) {
						SdkWarning.PropertyChanged += UpdateSdkWarning;
					}
					*/
				}

				return _popUpButton;
			}
		}

		/*
		public override void Dispose ()
		{
			_popUpButton.Activated -= UpdatePropertyValue;
			Property.PropertyChanged -= UpdatePopUpBtnValue;
			ItemsProperty.PropertyChanged -= LoadPopUpBtnDataModel;
			tipButton.Activated -= TipButtonActivated;

			base.Dispose ();
		}
		*/

		void UpdatePropertyFromUI (object sender, EventArgs e)
		{
			ComboBoxOption.Property.Value = _popUpButton.TitleOfSelectedItem;
		}

		void UpdateUIFromProperty (object sender, ViewModelPropertyChangedEventArgs e)
		{
            string value = ComboBoxOption.Property.Value;

			if (string.IsNullOrWhiteSpace (value))
				return;

            if (_popUpButton.SelectedItem == null)
			{
				_popUpButton.Title = value;
			}
			else
			{
				_popUpButton.SelectedItem.Title = value;
			}
		}

		void UpdateItemChoices ()
		{
			_popUpButton.RemoveAllItems ();

			/*
			if (NullIsDefault)
			{
				_popUpButton.AddItem (TranslationCatalog.GetString("Default"));
			}
			*/

			string[] items = ComboBoxOption.ItemsProperty.Value;
			if (items != null)
			{
				_popUpButton.AddItems (items);
			}

			var value = ComboBoxOption.Property.Value;
			if (!string.IsNullOrWhiteSpace(value))
			{
				_popUpButton.SelectItem(value);
			}
		}

		// TODO: Handle this
		/*
		void UpdateSdkWarning (object sender, ViewModelPropertyChangedEventArgs e)
		{
			if (e.NewValue != null) {
				// update error icon
				UpdateHintIcon (NSBezelStyle.Circular);
			}
			else {
				// update image hit icon
				UpdateHintIcon (NSBezelStyle.HelpButton);

			}
		}

		void UpdateHintIcon (NSBezelStyle styple )
		{
			tipButton.BezelStyle = styple;
        }
		*/
    }
}
