//
// OptionsComboBoxWidget.cs
//
// Author:
//       vstester <v-tianlz@microsoft.com>
//
// Copyright (c) 2021 ${CopyrightHolder}
//

using System.Collections.Immutable;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// An EditableComboBoxOption shows a list of items, where the user must choose one or
    /// the user can provide an arbitrary value, not in the list.
    /// </summary>
    public class EditableComboBoxOption : Option
    {
        public ViewModelProperty<string> Property { get; }
        public ViewModelProperty<ImmutableArray<string>> ItemsProperty { get; }

        /// <summary>
        /// Create an EditableComboBox.
        /// </summary>
        /// <param name="property">current value; it can be the empty string to indicate nothing is chosen</param>
        /// <param name="itemsProperty">list of items to show</param>
        public EditableComboBoxOption(ViewModelProperty<string> property, ViewModelProperty<ImmutableArray<string>> itemsProperty)
        {
            Property = property;
            ItemsProperty = itemsProperty;
            Platform = OptionFactoryPlatform.Instance.CreateEditableComboBoxOptionPlatform(this);
        }
    }
}
