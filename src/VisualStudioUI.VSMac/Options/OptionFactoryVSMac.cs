using System;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options {
	public class OptionFactoryVSMac : OptionFactoryPlatform {
		public OptionFactoryVSMac ()
		{
		}

		public override OptionCardPlatform CreateOptionCardPlatform (OptionCard optionCard) =>
			new OptionCardVSMac (optionCard);

		public override OptionCardsPlatform CreateOptionCardsPlatform (OptionCards optionCards) =>
			new OptionCardsVSMac (optionCards);

		public override OptionPlatform CreateTextOptionPlatform (TextOption textOption) =>
			new TextOptionVSMac (textOption);

		public override OptionPlatform CreateComboBoxOptionPlatform (ComboBoxOption comboBoxOption) =>
			new ComboBoxOptionVSMac (comboBoxOption);

		public override OptionPlatform CreateEditableComboBoxOptionPlatform (EditableComboBoxOption editableComboBoxOption) =>
			new EditableComboBoxOptionVSMac (editableComboBoxOption);

		public override OptionPlatform CreateDocButtonOptionPlatform (DocButtonOption docButtonOption) =>
			new DocButtonOptionVSMac (docButtonOption);

		public override OptionPlatform CreateSeparatorOptionPlatform (SeparatorOption separatorOption) =>
			new SeparatorOptionVSMac (separatorOption);
		public override OptionPlatform CreateSwitchableGroupOptionPlatform (SwitchableGroupOption switchableGroupOption) =>
			new SwitchableGroupOptionVSMac (switchableGroupOption);

		public override OptionPlatform CreateStringListOptionPlatform (StringListOption stringListOption) =>
			new StringListOptionVSMac (stringListOption);

		public override OptionPlatform CreateRadioButtonOptionPlatform (ButtonOption radioBtnOption) =>
			new ButtonOptionVSMac (radioBtnOption);

		public override OptionPlatform CreateImageViewOptionPlatform (ImageViewOption imageViewOption) =>
			new ImageViewOptionVSMac (imageViewOption);
	}
}
