using System;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public abstract class OptionFactoryPlatform
    {
        private static OptionFactoryPlatform? _instance;

		public static void Initialize (OptionFactoryPlatform factory)
		{
			_instance = factory;
		}

		public static bool IsInitialized => _instance != null;

		public static OptionFactoryPlatform Instance {
			get {
				if (_instance == null)
					throw new InvalidOperationException ("OptionsPlatformFactory needs to be initialized");
				return _instance;
			}
		}

        public abstract OptionCardsPlatform CreateOptionCardsPlatform(OptionCards optionCards);
        public abstract OptionCardPlatform CreateOptionCardPlatform(OptionCard optionCard);
        
        public abstract OptionPlatform CreateTextOptionPlatform(TextOption textOption);
        public abstract OptionPlatform CreateComboBoxOptionPlatform<TItem>(ComboBoxOption<TItem> comboBoxOption)
            where TItem : class;
        public abstract OptionPlatform CreateEditableComboBoxOptionPlatform(EditableComboBoxOption editableComboBoxOption);
        public abstract OptionPlatform CreateDocButtonOptionPlatform(DocButtonOption docButtonOption);
        public abstract OptionPlatform CreateSeparatorOptionPlatform(SeparatorOption separatorOption);
        public abstract OptionPlatform CreateSwitchableGroupOptionPlatform(SwitchableGroupOption switchableGroupOption);
        public abstract OptionPlatform CreateStringListOptionPlatform(StringListOption stringListOption);
        public abstract OptionPlatform CreateRadioButtonOptionPlatform(ButtonOption radioBtnOption);
        public abstract OptionPlatform CreateImageViewOptionPlatform(ImageFileOption imageFileOption);
    }
}
