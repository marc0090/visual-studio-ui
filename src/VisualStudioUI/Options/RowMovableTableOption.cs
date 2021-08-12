using System;
using System.Collections.Immutable;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class RowMovableTableOption : Option
    {
        public float Space = 10.0f;
        public float Width = 354.0f;
        public float Height = 72.0f;

        public string UpToolTip = string.Empty;
        public string DownToolTip = string.Empty;

        public event EventHandler? ListChanged;

        public ViewModelProperty<ImmutableArray<CheckBoxlistItem>> Property { get; }

        public void ListChangedInvoke(object sender, EventArgs e)
        {
            ListChanged?.Invoke(sender, e);
        }

        public RowMovableTableOption(ViewModelProperty<ImmutableArray<CheckBoxlistItem>> items)
        {
            Property = items;
            Property.Bind();
            Platform = OptionFactoryPlatform.Instance.CreateRowMovableTableOptionPlatform(this);
        }
    }
}
