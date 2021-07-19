using System;
using System.Collections.Generic;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class SwitchOption : ToggleButtonOption
    {
        public float Space = 10;
        public float Width = 640;

        private readonly List<Option> _childrenOptions = new List<Option>();

        public IReadOnlyList<Option> ChildrenOptions => _childrenOptions;

        /// <summary>
        /// Deprecated - use Property instead
        /// </summary>
        public ViewModelProperty<bool> IsOn => Property;

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

        public SwitchOption(ViewModelProperty<bool> isOn) : base(isOn)
        {
            ShowSpinner = new ViewModelProperty<bool>("showSpinner", false);

            isOn.Bind();
            ShowSpinner.Bind();
            Platform = OptionFactoryPlatform.Instance.CreateSwitchOptionPlatform(this);
        }
    }
}
