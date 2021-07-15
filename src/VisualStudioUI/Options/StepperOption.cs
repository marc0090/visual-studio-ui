//
// StepperOption.cs
//
// Author:
//       marcbookpro19 <v-marcguo@microsoft.com>
//
// Copyright (c) 2021 
//

using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class StepperOption : Option
    {
        public int Increment = 1;
        public int Maximum = 1000;
        public int Minimum = 0;

        public ViewModelProperty<int> Property;

        public StepperOption(ViewModelProperty<int> property)
        {
            Property = property;
            Platform = OptionFactoryPlatform.Instance.CreateStepperOptionPlatform(this);

        }
    }
}
