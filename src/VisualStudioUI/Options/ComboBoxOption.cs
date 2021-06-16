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
    public class ComboBoxOption : Option
    {
        public ViewModelProperty<string> Property { get; }
        public ViewModelProperty<string[]> ItemsProperty { get; }

        public ComboBoxOption(ViewModelProperty<string> property, ViewModelProperty<string[]> itemsProperty)
        {
            Property = property;
            ItemsProperty = itemsProperty;
            Platform = OptionFactoryPlatform.Instance.CreateComboBoxOptionPlatform(this);
        }
    }
}