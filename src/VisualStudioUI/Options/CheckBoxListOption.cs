using System;
using System.Collections.Immutable;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class CheckBoxListOption : Option
    {
        public float Space = 10.0f;
        public float Width = 354.0f;
        public float Height = 72.0f;

        public string UpToolTip = string.Empty;
        public string DownToolTip = string.Empty;

        public bool AllowReordering { get; }

        public event EventHandler? ListChanged;

        public ViewModelProperty<ImmutableArray<CheckBoxlistItem>> Property { get; }

        public void ListChangedInvoke(object sender, EventArgs e)
        {
            ListChanged?.Invoke(sender, e);
        }

        public CheckBoxListOption(ViewModelProperty<ImmutableArray<CheckBoxlistItem>> items, bool allowReording = false)
        {
            Property = items;
            Property.Bind();
            Platform = OptionFactoryPlatform.Instance.CreateCheckBoxListOptionPlatform(this);
            AllowReordering = allowReording;
        }
    }
}
