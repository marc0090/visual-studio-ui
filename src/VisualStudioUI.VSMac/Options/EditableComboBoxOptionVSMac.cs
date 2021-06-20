using System;
using AppKit;
using Foundation;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class EditableComboBoxOptionVSMac<TItem> : OptionWithLeftLabelVSMac where TItem : class, IDisplayable
    {
        NSComboBox _comboBox;

        public EditableComboBoxOptionVSMac(EditableComboBoxOption<TItem> option) : base(option)
        {
        }

        public EditableComboBoxOption<TItem> EditableComboBoxOption => ((EditableComboBoxOption<TItem>) Option);

        protected override NSView Control
        {
            get
            {
                if (_comboBox == null)
                {
                    ViewModelProperty<IDisplayable> property = EditableComboBoxOption.Property;
                    ViewModelProperty<TItem[]> itemsProperty = EditableComboBoxOption.ItemsProperty;

                    _comboBox = new AppKit.NSComboBox();
                    _comboBox.ControlSize = NSControlSize.Regular;
                    _comboBox.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
                    _comboBox.TranslatesAutoresizingMaskIntoConstraints = false;

                    _comboBox.WidthAnchor.ConstraintEqualToConstant(198f).Active = true;

                    _comboBox.SelectionChanged += UpdatePropertyFromSelection;
                    _comboBox.Changed += UpdatePropertyFromUIEdit;
                    EditableComboBoxOption.Property.PropertyChanged += UpdateUIFromProperty;

                    EditableComboBoxOption.ItemsProperty.PropertyChanged += delegate { UpdateItemChoices(); };

                    UpdateItemChoices();
                }

                return _comboBox;
            }
        }

        /*
        public override void Dispose ()
        {
            entryComBox.SelectionChanged -= UpdatePropertyValue;
            Property.PropertyChanged -= UpdateUIFromProperty;
            ItemsProperty.PropertyChanged -= LoadComBoxDataModel;
            entryComBox.Changed -= UpdatePropertyValue;

            base.Dispose ();
        }
        */

        void UpdatePropertyFromSelection(object sender, EventArgs e)
        {
            TItem? match = DisplayableUtil.FindMatch(EditableComboBoxOption.ItemsProperty.Value, _comboBox.SelectedValue.ToString());
            EditableComboBoxOption.Property.Value = match;
        }

        void UpdatePropertyFromUIEdit(object sender, EventArgs e)
        {
            string newValue = _comboBox.StringValue;

            IDisplayable? newPropertyValue;
            TItem? match = DisplayableUtil.FindMatch(EditableComboBoxOption.ItemsProperty.Value, newValue);
            if (match != null)
                newPropertyValue = match;
            else if (!string.IsNullOrWhiteSpace(newValue))
                newPropertyValue = new DisplayableString(newValue);
            else newPropertyValue = null;

            EditableComboBoxOption.Property.Value = newPropertyValue;
        }

        void UpdateUIFromProperty(object sender, ViewModelPropertyChangedEventArgs e)
        {
            string? value = EditableComboBoxOption.Property.Value?.ToDisplayString();
            if (string.IsNullOrWhiteSpace(value))
                _comboBox.StringValue = string.Empty;
            else _comboBox.StringValue = value;
        }

        void UpdateItemChoices()
        {
            _comboBox.RemoveAll();

            TItem[] items = EditableComboBoxOption.ItemsProperty.Value;
            foreach (TItem item in EditableComboBoxOption.ItemsProperty.Value)
            {
                _comboBox.Add(new NSString(item.ToDisplayString()));
            }

            string? value = EditableComboBoxOption.Property.Value?.ToDisplayString();
            if (!string.IsNullOrWhiteSpace(value) &&
                Array.IndexOf(EditableComboBoxOption.ItemsProperty.Value, value) != -1)
            {
                _comboBox.StringValue = value;
            }
        }
    }
}