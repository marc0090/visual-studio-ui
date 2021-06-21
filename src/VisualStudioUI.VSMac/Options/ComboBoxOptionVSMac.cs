using System;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ComboBoxOptionVSMac<TItem> : OptionWithLeftLabelVSMac where TItem : class
    {
        private NSPopUpButton _popUpButton;

        public ViewModelProperty<string> SdkWarning { get; set; } = null;
        public bool NullIsDefault { get; set; } = false;

        public ComboBoxOptionVSMac(ComboBoxOption<TItem> option) : base(option)
        {
        }

        public ComboBoxOption<TItem> ComboBoxOption => ((ComboBoxOption<TItem>) Option);

        protected override NSView Control
        {
            get
            {
                if (_popUpButton == null)
                {
                    ViewModelProperty<TItem> property = ComboBoxOption.Property;
                    ViewModelProperty<TItem[]> itemsProperty = ComboBoxOption.ItemsProperty;

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

        void UpdatePropertyFromUI(object sender, EventArgs e)
        {
            TItem? match = DisplayableItemsUtil.FindMatch(ComboBoxOption.ItemsProperty.Value,
                _popUpButton.TitleOfSelectedItem,
                ComboBoxOption.ItemDisplayStringFunc);
            ComboBoxOption.Property.Value = match!;
        }

        void UpdateUIFromProperty(object sender, ViewModelPropertyChangedEventArgs e)
        {
            TItem value = ComboBoxOption.Property.Value;

            string displayString = ComboBoxOption.ItemDisplayStringFunc(value);
            if (_popUpButton.SelectedItem == null)
            {
                _popUpButton.Title = displayString;
            }
            else
            {
                _popUpButton.SelectedItem.Title = displayString;
            }
        }

        void UpdateItemChoices()
        {
            _popUpButton.RemoveAllItems();

            /*
            if (NullIsDefault)
            {
                _popUpButton.AddItem (TranslationCatalog.GetString("Default"));
            }
            */

            TItem[] items = ComboBoxOption.ItemsProperty.Value;
            foreach (TItem item in items)
            {
                string itemDisplayString = ComboBoxOption.ItemDisplayStringFunc(item);
                _popUpButton.AddItem(itemDisplayString);
            }

            string currentItemDisplayString = ComboBoxOption.ItemDisplayStringFunc(ComboBoxOption.Property.Value);
            _popUpButton.SelectItem(currentItemDisplayString);
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