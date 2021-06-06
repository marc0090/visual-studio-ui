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
		public ViewModelProperty<string> Property { get; set; }
		public ViewModelProperty<string[]> ItemsProperty { get; set; } = null;

		public ComboBoxOption(ViewModelProperty<string> property)
		{
			Property = property;
			Platform = OptionFactoryPlatform.Instance.CreateComboBoxOptionPlatform(this);
		}
	}
}
