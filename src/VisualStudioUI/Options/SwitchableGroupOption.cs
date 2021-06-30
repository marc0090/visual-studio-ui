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
        private readonly List<Option> _childrenOptions = new List<Option>();
        public IReadOnlyList<Option> ChildrenOptions => _childrenOptions;

        public ViewModelProperty<bool> IsOn { get; }
        public ViewModelProperty<bool> ShowSpinner { get; }

        public event EventHandler? SwitchChanged;

        public void AddOption(Option option) => _childrenOptions.Add(option);

        public void RemoveOption(Option option)
        {
            if (_childrenOptions.Count <= 0)
                return;

            _childrenOptions.Remove(option);
        }

        public void SwitchChangedInvoke(object sender, EventArgs e)
        {
            SwitchChanged?.Invoke(sender, e);
        }

        public SwitchableGroupOption(ViewModelProperty<bool> isOn)
        {
            IsOn = isOn;
            ShowSpinner = new ViewModelProperty<bool>("showSpinner", false);

            IsOn.Bind();
            ShowSpinner.Bind();
            Platform = OptionFactoryPlatform.Instance.CreateSwitchableGroupOptionPlatform(this);
        }
    }
}
