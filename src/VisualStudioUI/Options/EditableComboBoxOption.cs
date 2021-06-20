//
// OptionsComboBoxWidget.cs
//
// Author:
//       vstester <v-tianlz@microsoft.com>
//
// Copyright (c) 2021 ${CopyrightHolder}
//

using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class EditableComboBoxOption<TItem> : Option where TItem : class, IDisplayable
    {
        public ViewModelProperty<IDisplayable?> Property { get; }
        public ViewModelProperty<TItem[]> ItemsProperty { get; }

        public EditableComboBoxOption(ViewModelProperty<IDisplayable?> property, ViewModelProperty<TItem[]> itemsProperty)
        {
            Property = property;
            ItemsProperty = itemsProperty;
            Platform = OptionFactoryPlatform.Instance.CreateEditableComboBoxOptionPlatform(this);
        }
    }
}