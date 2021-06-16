using System.Collections.Generic;

namespace Microsoft.VisualStudioUI.Options
{
    public class OptionCard
    {
        private readonly List<Option> _options = new List<Option>();

        public OptionCardPlatform Platform { get; }

        public OptionCard()
        {
            Platform = OptionFactoryPlatform.Instance.CreateOptionCardPlatform(this);
        }

        public IReadOnlyList<Option> Options => _options;

        public void AddOption(Option option) => _options.Add(option);

        /// <summary>
        /// This is the label for the category (used for grouping settings). If null, it's not shown.
        /// </summary>
        public string? Label { get; set; } = null;
    }
}