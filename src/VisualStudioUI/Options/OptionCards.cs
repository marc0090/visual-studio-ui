using System.Collections.Generic;

namespace Microsoft.VisualStudioUI.Options
{
    public class OptionCards
    {
        private readonly List<OptionCard> _cards = new List<OptionCard>();

        public OptionCardsPlatform Platform { get; }

        public OptionCards()
        {
            Platform = OptionFactoryPlatform.Instance.CreateOptionCardsPlatform(this);
        }

        public IReadOnlyList<OptionCard> Cards => _cards;

        public void AddCard(OptionCard card)
        {
            _cards.Add(card);
        }

        public void RemoveAllCards()
        {
            _cards.Clear();
        }
    }
}
