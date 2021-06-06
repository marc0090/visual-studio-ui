using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
#if LATER
	public class ComboBoxOptionVSMac : OptionWithLeftLabelVSMac
	{
		private HStack _hstack;
		private NSPopUpButton _popUpBtn;
		/*
		private HintPopover popover;
		private NSButton tipButton;
		*/

		public ViewModelProperty<string> Property { get; set; } = null;
		public ViewModelProperty<string[]> ItemsProperty { get; set; } = null;
		public bool NullIsDefault { get; set; } = false;

		public ComboBoxOptionVSMac (ComboBoxOption comboBoxOption) : base(comboBoxOption)
		{
		}

		internal override NSView GetDataUIElement(NSView labelUIElement)
		{
			if (_hstack == null)
			{
				_hstack = new HStack ();
				_popUpBtn = new NSPopUpButton ();
			}

			_popUpBtn.Activated += UpdatePropertyValue;

			if (Property != null) {
				Property.PropertyChanged += UpdatePopUpBtnValue;
			}

			if (ItemsProperty != null) {
				ItemsProperty.PropertyChanged += LoadPopUpBtnDataModel;
			}

			_hstack.Add (_popUpBtn);

			#if LATER
            if (!string.IsNullOrWhiteSpace(Option.Hint))
			{
				CreateHintIcon ();
            }
			#endif

			return _hstack;
		}

		/*
		public override void Dispose ()
		{
			popUpBtn.Activated -= UpdatePropertyValue;
			Property.PropertyChanged -= UpdatePopUpBtnValue;
			ItemsProperty.PropertyChanged -= LoadPopUpBtnDataModel;
			tipButton.Activated -= TipButtonActivated;

			base.Dispose ();
		}
		*/

		void UpdatePropertyValue (object sender, EventArgs e)
		{
			Property.Value = _popUpBtn.TitleOfSelectedItem;
		}

		void UpdatePopUpBtnValue (object sender, ViewModelPropertyChangedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace (Property.Value))
				return;

            if (_popUpBtn.SelectedItem == null)
			{
				_popUpBtn.Title = Property.Value;
			}
			else
			{
				_popUpBtn.SelectedItem.Title = Property.Value;
			}
		}

		void LoadPopUpBtnDataModel (object sender, EventArgs e)
		{
			_popUpBtn.RemoveAllItems ();

			// TODO: Handle
			/*
			if (NullIsDefault)
			{
				_popUpBtn.AddItem (TranslationCatalog.GetString("Default"));
			}
			*/

			_popUpBtn.AddItems (ItemsProperty.Value);

            if (!string.IsNullOrWhiteSpace(Property.Value))
			{
				_popUpBtn.SelectItem (Property.Value);
			}
		}
#endif

#if false
		void CreateHintIcon ()
		{
			var targetPopUpWidthConstraint = _popUpBtn.WidthAnchor.ConstraintEqualToConstant (3000);
			targetPopUpWidthConstraint.Priority = (System.Int32) AppKit.NSLayoutPriority.DefaultLow;
			targetPopUpWidthConstraint.Active = true;

			_popUpBtn.RightAnchor.ConstraintEqualToAnchor (_hstack.RightAnchor, 100).Active = true;
			_popUpBtn.TopAnchor.ConstraintEqualToAnchor (_hstack.TopAnchor, 0).Active = true;

			tipButton = new NSButton ();
			tipButton.BezelStyle = NSBezelStyle.HelpButton;
			tipButton.Title = "";
			tipButton.ControlSize = NSControlSize.Small;
			tipButton.Font = AppKit.NSFont.SystemFontOfSize (AppKit.NSFont.SmallSystemFontSize);
			tipButton.TranslatesAutoresizingMaskIntoConstraints = false;

			_hstack.Add (tipButton);

			var organizationIDHelpButtonWidthConstraint = tipButton.WidthAnchor.ConstraintEqualToConstant (18f);
			organizationIDHelpButtonWidthConstraint.Priority = (System.Int32) AppKit.NSLayoutPriority.DefaultLow;
			organizationIDHelpButtonWidthConstraint.Active = true;

			tipButton.RightAnchor.ConstraintEqualToAnchor (_popUpBtn.RightAnchor, 50f).Active = true;
			tipButton.TopAnchor.ConstraintEqualToAnchor (_popUpBtn.TopAnchor, 3f).Active = true;
			tipButton.Activated += TipButtonActivated;
        }

		void TipButtonActivated (object sender, EventArgs e)
		{
			ShowInfoPopover (Hint, tipButton);
		}

		void ShowInfoPopover (string message, NSButton button)
		{
			popover?.Close ();
			popover?.Dispose ();
			popover = new HintPopover (message, TaskSeverity.Information);
			popover.MaxWidth = 256;
			//TODO:
			//popover.SetAppearance (Appearance);

			var bounds = button.Bounds;
			popover.Show (bounds, button, NSRectEdge.MaxYEdge);
		}
    }
#endif
}
