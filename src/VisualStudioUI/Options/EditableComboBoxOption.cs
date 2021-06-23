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
    public class EditableComboBoxOption : Option
    {
        public ViewModelProperty<string> Property { get; }
        public ViewModelProperty<ImmutableArray<string>> ItemsProperty { get; }

        public EditableComboBoxOption(ViewModelProperty<string> property, ViewModelProperty<ImmutableArray<string>> itemsProperty)
        {
            Property = property;
            ItemsProperty = itemsProperty;
            Platform = OptionFactoryPlatform.Instance.CreateEditableComboBoxOptionPlatform(this);
        }
    }
}