using System.Collections.Generic;
using AppKit;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
	public class OptionCardVSMac : OptionCardPlatform
	{
		private NSView _cardView;

		public OptionCardVSMac(OptionCard optionCard) : base(optionCard)
		{
		}

		public NSView View
		{
			get
			{
				if (_cardView == null)
					_cardView = CreateView();
				return _cardView;
			}
		}

		NSView CreateView()
		{
			// View:     card
			var cardView = new AppKit.NSView();
			cardView.WantsLayer = true;
			cardView.TranslatesAutoresizingMaskIntoConstraints = false;

			var cardWidthConstraint = cardView.WidthAnchor.ConstraintEqualToConstant (640f);
			cardWidthConstraint.Active = true;
			//var cardHeightConstraint = cardView.HeightAnchor.ConstraintEqualToConstant (367f);
			//cardHeightConstraint.Active = true;

			/*
			cardView.RightAnchor.ConstraintEqualToAnchor (this.RightAnchor, 0f).Active = true;
			cardView.LeftAnchor.ConstraintEqualToAnchor (this.LeftAnchor, 0f).Active = true;
			cardView.BottomAnchor.ConstraintEqualToAnchor (this.BottomAnchor, 0f).Active = true;
			cardView.TopAnchor.ConstraintEqualToAnchor (this.TopAnchor, 0f).Active = true;
			*/

			// View:     background
			var background = new AppKit.NSBox();
			background.BoxType = NSBoxType.NSBoxCustom;
			background.FillColor = NSColor.AlternatingContentBackgroundColors[1];
			background.BorderColor = NSColor.SeparatorColor;
			background.BorderWidth = 1;
			background.CornerRadius = 8;
			background.Title = "";
			background.TranslatesAutoresizingMaskIntoConstraints = false;

			cardView.AddSubview (background);
			/*
			var backgroundWidthConstraint = background.WidthAnchor.ConstraintEqualToConstant (640f);
			backgroundWidthConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
			backgroundWidthConstraint.Active = true;
			var backgroundHeightConstraint = background.HeightAnchor.ConstraintEqualToConstant (367f);
			backgroundHeightConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
			backgroundHeightConstraint.Active = true;
			*/

			background.RightAnchor.ConstraintEqualToAnchor (cardView.RightAnchor, 0f).Active = true;
			background.LeftAnchor.ConstraintEqualToAnchor (cardView.LeftAnchor, 0f).Active = true;
			background.BottomAnchor.ConstraintEqualToAnchor (cardView.BottomAnchor, 0f).Active = true;
			background.TopAnchor.ConstraintEqualToAnchor (cardView.TopAnchor, 0f).Active = true;

			// View:     titleLabel
			if (!string.IsNullOrEmpty(OptionCard.Label))
			{
				var titleLabel = new AppKit.NSTextField();
				titleLabel.Editable = false;
				titleLabel.Bordered = false;
				titleLabel.DrawsBackground = false;
				titleLabel.PreferredMaxLayoutWidth = 1;
				titleLabel.StringValue = OptionCard.Label;
				titleLabel.Alignment = NSTextAlignment.Left;
				titleLabel.Font =
					AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize, AppKit.NSFontWeight.Bold);
				titleLabel.TextColor = NSColor.LabelColor;
				titleLabel.TranslatesAutoresizingMaskIntoConstraints = false;

				cardView.AddSubview(titleLabel);
				var titleLabelWidthConstraint = titleLabel.WidthAnchor.ConstraintEqualToConstant(370f);
				titleLabelWidthConstraint.Active = true;
				var titleLabelHeightConstraint = titleLabel.HeightAnchor.ConstraintEqualToConstant(16f);
				titleLabelHeightConstraint.Active = true;

				titleLabel.LeftAnchor.ConstraintEqualToAnchor(cardView.LeftAnchor, 24f).Active = true;
				titleLabel.TopAnchor.ConstraintEqualToAnchor(cardView.TopAnchor, 17f).Active = true;
			}

			// View:     optionsStackView
			var optionsStackView = new AppKit.NSStackView();
			optionsStackView.TranslatesAutoresizingMaskIntoConstraints = false;
			optionsStackView.EdgeInsets = new AppKit.NSEdgeInsets (0, 0, 0, 0);
			optionsStackView.Spacing = 0f;
			optionsStackView.Orientation = NSUserInterfaceLayoutOrientation.Vertical;
			optionsStackView.Distribution = NSStackViewDistribution.EqualSpacing;

			cardView.AddSubview (optionsStackView);
			var optionsStackWidthConstraint = optionsStackView.WidthAnchor.ConstraintEqualToConstant (600f);
			optionsStackWidthConstraint.Active = true;

			optionsStackView.LeftAnchor.ConstraintEqualToAnchor (cardView.LeftAnchor, 20f).Active = true;
			optionsStackView.TopAnchor.ConstraintEqualToAnchor (cardView.TopAnchor, 54f).Active = true;
			optionsStackView.BottomAnchor.ConstraintEqualToAnchor(cardView.BottomAnchor, -20f).Active = true;

			foreach (Option option in OptionCard.Options)
			{
				var optionVSMac = (OptionVSMac) option.Platform;
				optionsStackView.AddArrangedSubview(optionVSMac.View);
			}

			return cardView;

#if OLD
			contentGrid = new NSGridView
			{
				RowSpacing = (int)Alignments.Spacing.Row,
				ColumnSpacing = (int)Alignments.Spacing.Column,
				X = NSGridCellPlacement.Trailing
			};

			foreach (OptionsWidget optionsWidget in children)
			{
				var label = new NSTextField
				{
					StringValue = optionsWidget.Label,
					DrawsBackground = false,
					Bordered = false,
					Editable = false,
					Selectable = false,
					Alignment = NSTextAlignment.Right
				};

				label.SizeToFit();

				NSView dataUIElement = optionsWidget.GetDataUIElement(label);
				contentGrid.AddRow(new NSView[]
				{
					label, dataUIElement
				});
				
			}
#endif
		}
	}
}
