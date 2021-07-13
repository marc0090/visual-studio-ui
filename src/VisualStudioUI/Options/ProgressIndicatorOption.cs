using System;
using System.Collections.Generic;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public class ProgressIndicatorOption : Option
    {
        public Option Element { get; }
        public ViewModelProperty<bool> ShowSpinner { get; }

        public ProgressIndicatorOption(Option element)
        {
            Element = element;
            ShowSpinner = new ViewModelProperty<bool>("showSpinner", false);
            Platform = OptionFactoryPlatform.Instance.CreateProgressIndicatorOptionPlatform(this);
        }
    }
}
