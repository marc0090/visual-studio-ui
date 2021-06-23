using System;
using System.Diagnostics;
using System.Collections.Immutable;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ComboBoxOptionVSMac<TItem> : OptionWithLeftLabelVSMac where TItem : class
    {
        private NSPopUpButton? _popUpButton;

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
                    ViewModelProperty<TItem?> property = ComboBoxOption.Property;
                    ViewModelProperty<ImmutableArray<TItem>> itemsProperty = ComboBoxOption.ItemsProperty;

                    // View:     popUpButton
                    _popUpButton = new AppKit.NSPopUpButton();
                    _popUpButton.BezelStyle = NSBezelStyle.Rounded;
                    _popUpButton.ControlSize = NSControlSize.Regular;
                    _popUpButton.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
                    //_popUpButton.AddItem ("Default");
                    _popUpButton.TranslatesAutoresizingMaskIntoConstraints = false;

                    _popUpButton.WidthAnchor.ConstraintEqualToConstant(198f).Active = true;

                    _popUpButton.Activated += UpdatePropertyFromUI;
                    property.PropertyChanged += delegate { UpdateSelectedItemUIFromProperty(); };
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
                _popUpButton!.TitleOfSelectedItem,
                ComboBoxOption.ItemDisplayStringFunc);
            ComboBoxOption.Property.Value = match;
        }

        void UpdateItemChoices()
        {
            _popUpButton!.RemoveAllItems();

            /*
            if (NullIsDefault)
            {
                _popUpButton.AddItem (TranslationCatalog.GetString("Default"));
            }
            */
            
            ImmutableArray<TItem> items = ComboBoxOption.ItemsProperty.Value;

            // The intention is that null items aren't allowed - no items should be an empty list.
            // But handle this case just in case, to be safe.
            if ((ImmutableArray<TItem>?) items == null)
                return;

            foreach (TItem item in items)
            {
                string itemDisplayString = ComboBoxOption.ItemDisplayStringFunc(item);
                _popUpButton.AddItem(itemDisplayString);
            }

            UpdateSelectedItemUIFromProperty();

        }

        void UpdateSelectedItemUIFromProperty()
        {
            TItem? currentValue = ComboBoxOption.Property.Value;
            if (currentValue == null)
                _popUpButton!.SelectItem ((NSMenuItem?) null);
            else {
                string currenValueDisplayString = ComboBoxOption.ItemDisplayStringFunc (currentValue);
                _popUpButton!.SelectItem (currenValueDisplayString);
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