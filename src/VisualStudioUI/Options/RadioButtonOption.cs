// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// RadioButtonOptions allows the user to choose between multiple, mutually exclusive, choices,
    /// typically shown in the UI as radio buttons. RadioButtonOptions are associated with a RadioButtonGroup,
    /// with at most one button in the group having a true value for its Property.
    /// </summary>
    public class RadioButtonOption : ToggleButtonOption
    {
        public RadioButtonGroup RadioButtonGroup { get; }
 
        /// <summary>
        /// Construct a RadioButtonOption, for the specified group and property.
        /// </summary>
        /// <param name="radioButtonGroup">group, used to define mutually exclusive options</param>
        /// <param name="property">bool property for this radio button</param>
        public RadioButtonOption(RadioButtonGroup radioButtonGroup, ViewModelProperty<bool> property) : base(property)
        {
            RadioButtonGroup = radioButtonGroup;
            radioButtonGroup.AddRadioButton(this);
            
            Platform = OptionFactoryPlatform.Instance.CreateRadioButtonOptionPlatform(this);
        }
    }
}
