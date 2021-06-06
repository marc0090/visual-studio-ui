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
		public abstract OptionPlatform CreateComboBoxOptionPlatform(ComboBoxOption comboBoxOption);
		public abstract OptionPlatform CreateTextBoxOptionPlatform(TextOption textOption);
	}
}
