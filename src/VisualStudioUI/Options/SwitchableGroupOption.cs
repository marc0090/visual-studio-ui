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
        public ViewModelProperty<bool> Property { get; }

        public readonly List<Option> _childOptions = new List<Option>();

        public IReadOnlyList<Option> ChildOptions => _childOptions;

        public void AddOption(Option option) => _childOptions.Add(option);

        public SwitchableGroupOption(ViewModelProperty<bool> property)
        {
            Property = property;
            Platform = OptionFactoryPlatform.Instance.CreateSwitchableGroupOptionPlatform(this);
        }
    }
}