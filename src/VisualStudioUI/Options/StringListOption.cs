//
// SwitchableGroupOption.cs
//
// Author:
//       marcbookpro19 <v-marcguo@microsoft.com>
//
// Copyright (c) 2021 
//

using System;
using System.Collections.Immutable;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class StringListOption : Option
    {

        public string PrefixValue = string.Empty;
        public string DefaultValue = string.Empty;
        public string AddToolTip = string.Empty;
        public string RemoveToolTip = string.Empty;

        public event EventHandler? ListChanged;

        public ViewModelProperty<ImmutableArray<string>> Model { get; }

        public void ListChangedInvoke(object sender, EventArgs e)
        {
            ListChanged?.Invoke(sender, e);
        }

        public StringListOption(ViewModelProperty<ImmutableArray<string>> model, string addToolTip = "", string removeToolTip = "", string label = "", string defaultListValue = "")
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
