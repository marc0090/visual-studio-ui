//
// OptionsComboBoxWidget.cs
//
// Author:
//       vstester <v-tianlz@microsoft.com>
//
// Copyright (c) 2021 ${CopyrightHolder}
//

using Microsoft.VisualStudioUI.Options.Models;
using System.Collections.Immutable;

namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// A ComboBoxOption shows a list of items, where the user must choose one.
    /// Not selecting anything is also supported, indicated by Property.Value being
    /// null.
    /// </summary>
    /// <typeparam name="TItem">item type; items can be any type, with ItemDisplayStringFunc
    /// used to get the display string
    /// </typeparam>
    public class ComboBoxOption<TItem> : Option where TItem : class
    {
        public ViewModelProperty<TItem?> Property { get; }
        public ViewModelProperty<ImmutableArray<TItem>> ItemsProperty { get; }
        public ItemDisplayStringFunc<TItem> ItemDisplayStringFunc { get; }

        /// <summary>
        /// Create a ComboBoxOption. 
        /// </summary>
        /// <param name="property">current value; if the property Value is null, it means nothing is selected</param>
        /// <param name="itemsProperty">list of items to show</param>
        /// <param name="itemDisplayStringFunc">function to map item to display string shown in UI; if null ToString is used</param>
        public ComboBoxOption(ViewModelProperty<TItem?> property,
            ViewModelProperty<ImmutableArray<TItem>> itemsProperty,
            ItemDisplayStringFunc<TItem>? itemDisplayStringFunc = null)
        {
            Property = property;
            ItemsProperty = itemsProperty;
            Platform = OptionFactoryPlatform.Instance.CreateComboBoxOptionPlatform(this);

            if (itemDisplayStringFunc == null)
                ItemDisplayStringFunc = DisplayableItemsUtil.ItemDisplayStringFromToString;
            else ItemDisplayStringFunc = itemDisplayStringFunc;
        }
    }
}
