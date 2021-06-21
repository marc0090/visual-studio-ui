using System;
using System.Collections.Generic;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options {
	public class StringListOption : Option {
		public string DefaultValue;
		public string AddToolTip;
		public string RemoveToolTip;

		public ViewModelProperty<List<string>> Property { get; }

		public StringListOption (ViewModelProperty<List<string>> property, string label = "", string defaultListValue = "", string addToolTip = "", string removeToolTip = "")
		{
			Label = label;
			DefaultValue = defaultListValue;
			AddToolTip = addToolTip;
			RemoveToolTip = removeToolTip;
			Property = property;
			Platform = OptionFactoryPlatform.Instance.CreateStringListOptionPlatform (this);

		}
	}
}
