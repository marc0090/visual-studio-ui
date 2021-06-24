using System.Collections.Immutable;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class StringListOption : Option
    {
        public string DefaultValue;
        public string AddToolTip;
        public string RemoveToolTip;

        public ViewModelProperty<ImmutableArray<string>> Model { get; }

        public StringListOption(ViewModelProperty<ImmutableArray<string>> model, string label = "",
            string defaultListValue = "", string addToolTip = "", string removeToolTip = "")
        {
            Label = label;
            DefaultValue = defaultListValue;
            AddToolTip = addToolTip;
            RemoveToolTip = removeToolTip;
            Model = model;
            Platform = OptionFactoryPlatform.Instance.CreateStringListOptionPlatform(this);
        }
    }
}
