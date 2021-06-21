//
// SwitchableGroupOption.cs
//
// Author:
//       marcbookpro19 <v-marcguo@microsoft.com>
//
// Copyright (c) 2021 
//
using System;
using System.Collections.Generic;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class SwitchableGroupOption : Option
    {

        public ViewModelProperty<bool> State { get; }

        public event EventHandler? SwitchChanged;

        public readonly List<Option> _childOptions = new List<Option>();

        public IReadOnlyList<Option> ChildOptions => _childOptions;

        public void AddOption(Option option) => _childOptions.Add(option);

        public void SwitchChangedInvoke(object sender, EventArgs e)
        {
            SwitchChanged?.Invoke(sender, e);
        }

        public SwitchableGroupOption(ViewModelProperty<bool> state)
        {
            State = state;
            Platform = OptionFactoryPlatform.Instance.CreateSwitchableGroupOptionPlatform(this);
        }
    }
}
