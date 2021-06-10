using System;

namespace Microsoft.VisualStudioUI.Options
{
    public abstract class OptionFactoryPlatform
	{
		private static OptionFactoryPlatform? _instance;

		public static void Initialize(OptionFactoryPlatform factory)
        {
			_instance = factory;
        }

		public static bool IsInitialized => _instance != null;

		public static OptionFactoryPlatform Instance
        {
			get
            {
				if (_instance == null)
					throw new InvalidOperationException("OptionsPlatformFactory needs to be initialized");
				return _instance;
            }
		}

		public abstract OptionCardsPlatform CreateOptionCardsPlatform(OptionCards optionCards);
		public abstract OptionCardPlatform CreateOptionCardPlatform(OptionCard optionCard);
		public abstract OptionPlatform CreateTextOptionPlatform(TextOption textOption);
		public abstract OptionPlatform CreateComboBoxOptionPlatform(ComboBoxOption comboBoxOption);
		public abstract OptionPlatform CreateEditableComboBoxOptionPlatform(EditableComboBoxOption editableComboBoxOption);
		public abstract OptionPlatform CreateDocButtonOptionPlatform(DocButtonOption docButtonOption);
		public abstract OptionPlatform CreateSeparatorOptionPlatform(SeparatorOption separatorOption);
		public abstract OptionPlatform CreateRadioButtonOptionPlatform (RadioButtonOption radioBtnOption);
	}
}
