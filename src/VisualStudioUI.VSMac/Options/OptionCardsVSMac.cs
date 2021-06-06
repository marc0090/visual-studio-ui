using AppKit;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public class OptionCardsVSMac : OptionCardsPlatform
	{
		NSStackView _cardsStack;

		public OptionCardsVSMac(OptionCards optionCards)  : base(optionCards)
		{
		}

		public NSView View
		{
			get
			{
				if (_cardsStack == null)
					_cardsStack = CreateView();
				return _cardsStack;
			}
		}

		private NSStackView CreateView()
		{
			var cardsStack = new NSStackView();
			cardsStack.TranslatesAutoresizingMaskIntoConstraints = false;
			cardsStack.EdgeInsets = new NSEdgeInsets (0, 0, 0, 0);
			cardsStack.Alignment = NSLayoutAttribute.Leading;
			cardsStack.Spacing = 10f;
			cardsStack.Orientation = NSUserInterfaceLayoutOrientation.Vertical;
			cardsStack.Distribution = NSStackViewDistribution.Fill;

			var optionsStackWidthConstraint = cardsStack.WidthAnchor.ConstraintEqualToConstant (600f);
			optionsStackWidthConstraint.Active = true;

			foreach (OptionCard card in OptionCards.Cards)
			{
				NSView cardView = ((OptionCardVSMac) card.Platform).View;
				cardsStack.AddArrangedSubview(cardView);
			}

			// The dummy view at the end grows/shrinks for the extra space
			var dummyView = new NSView()
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			cardsStack.AddArrangedSubview(dummyView);

			return cardsStack;
		}
	}
}
