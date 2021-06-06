using System;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class OptionFactoryVSMac : OptionFactoryPlatform
    {
        public OptionFactoryVSMac()
        {
        }

        public override OptionCardPlatform CreateOptionCardPlatform(OptionCard optionCard) =>
            new OptionCardVSMac(optionCard);

        public override OptionCardsPlatform CreateOptionCardsPlatform(OptionCards optionCards) =>
            new OptionCardsVSMac(optionCards);

        public override OptionPlatform CreateTextBoxOptionPlatform(TextOption textOption) =>
            new TextOptionVSMac(textOption);
    
        public override OptionPlatform CreateComboBoxOptionPlatform(ComboBoxOption comboBoxOption) =>
            null; // new ComboBoxOptionVSMac(comboBoxOption);
    }
}
