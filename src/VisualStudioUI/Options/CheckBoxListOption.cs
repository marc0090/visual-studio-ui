using System;
using System.Collections.Immutable;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class CheckBoxListOption : Option
    {

        public event EventHandler? ListChanged;

        public ViewModelProperty<ImmutableArray<CheckBoxlistItem>> Property { get; }

        public void ListChangedInvoke(object sender, EventArgs e)
        {
            ListChanged?.Invoke(sender, e);
        }

        public CheckBoxListOption(ViewModelProperty<ImmutableArray<CheckBoxlistItem>> items)
        {
            Property = items;
            Platform = OptionFactoryPlatform.Instance.CreateCheckBoxListOptionPlatform(this);
        }
    }
}
