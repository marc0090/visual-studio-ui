using System.Collections.Generic;

namespace Microsoft.VisualStudioUI.Options
{
    /// <summary>
    /// An OptionCard represents a group of options, with an optional label.
    /// In the UI, these may be represented visually by a "card" - a rounded rect border
    /// that surrounds the option group.
    /// </summary>
    public class OptionCard
    {
        private readonly List<Option> _options = new List<Option>();

        public float Width { get; set; } = 640f;

        public OptionCardPlatform Platform { get; }

        public OptionCard()
        {
            Platform = OptionFactoryPlatform.Instance.CreateOptionCardPlatform(this);
        }

        public IReadOnlyList<Option> Options => _options;

        public void AddOption(Option option) => _options.Add(option);

        public void RemoveOption(Option option)
        {
            if (_options.Count <= 0)
                return;

            _options.Remove(option);
        }

        /// <summary>
        /// The label for the card. If null, it's not shown.
        /// </summary>
        public string? Label { get; set; } = null;
    }
}
