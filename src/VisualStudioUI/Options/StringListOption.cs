using System;
using System.Collections.Generic;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class StringListOption : Option
    {
        public string DefaultValue;
        public ViewModelProperty<List<string>> Property { get; }

        public StringListOption(ViewModelProperty<List<string>> property, string defaultListValue)
        {
            DefaultValue = defaultListValue;
            Property = property;
            Platform = OptionFactoryPlatform.Instance.CreateStringListOptionPlatform(this);
        }
    }
}