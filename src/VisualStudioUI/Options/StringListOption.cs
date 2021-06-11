using System;
using System.Collections.Generic;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class StringListOption : Option
    {
        public ViewModelProperty<List<string>> Property { get; }

        public StringListOption(ViewModelProperty<List<string>> property)
        {
            Property = property;
            Platform = OptionFactoryPlatform.Instance.CreateStringListOptionPlatform(this);

        }
    }
}
