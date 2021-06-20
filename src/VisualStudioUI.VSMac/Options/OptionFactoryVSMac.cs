using System;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

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

        public override OptionPlatform CreateTextOptionPlatform(TextOption textOption) =>
            new TextOptionVSMac(textOption);

        public override OptionPlatform CreateComboBoxOptionPlatform<TItem>(ComboBoxOption<TItem> comboBoxOption) =>
            new ComboBoxOptionVSMac<TItem>(comboBoxOption);

        public override OptionPlatform CreateEditableComboBoxOptionPlatform<TItem>(
            EditableComboBoxOption<TItem> editableComboBoxOption) =>
            new EditableComboBoxOptionVSMac<TItem>(editableComboBoxOption);

        public override OptionPlatform CreateDocButtonOptionPlatform(DocButtonOption docButtonOption) =>
            new DocButtonOptionVSMac(docButtonOption);

        public override OptionPlatform CreateSeparatorOptionPlatform(SeparatorOption separatorOption) =>
            new SeparatorOptionVSMac(separatorOption);

        public override OptionPlatform
            CreateSwitchableGroupOptionPlatform(SwitchableGroupOption switchableGroupOption) =>
            new SwitchableGroupOptionVSMac(switchableGroupOption);

        public override OptionPlatform CreateStringListOptionPlatform(StringListOption stringListOption) =>
            new StringListOptionVSMac(stringListOption);

        public override OptionPlatform CreateRadioButtonOptionPlatform(ButtonOption radioBtnOption) =>
            new ButtonOptionVSMac(radioBtnOption);

        public override OptionPlatform CreateImageViewOptionPlatform(ImageFileOption imageFileOption) =>
            new ImageFileOptionVSMac(imageFileOption);
    }
}