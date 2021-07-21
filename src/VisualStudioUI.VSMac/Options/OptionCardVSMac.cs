using System.Collections.Generic;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

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

            var cardWidthConstraint = cardView.WidthAnchor.ConstraintEqualToConstant(640f);
            cardWidthConstraint.Active = true;
            //var cardHeightConstraint = cardView.HeightAnchor.ConstraintEqualToConstant (367f);
            //cardHeightConstraint.Active = true;

            /*
            cardView.TrailingAnchor.ConstraintEqualToAnchor (this.TrailingAnchor, 0f).Active = true;
            cardView.LeadingAnchor.ConstraintEqualToAnchor (this.LeadingAnchor, 0f).Active = true;
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

            cardView.AddSubview(background);
            /*
            var backgroundWidthConstraint = background.WidthAnchor.ConstraintEqualToConstant (640f);
            backgroundWidthConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
            backgroundWidthConstraint.Active = true;
            var backgroundHeightConstraint = background.HeightAnchor.ConstraintEqualToConstant (367f);
            backgroundHeightConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
            backgroundHeightConstraint.Active = true;
            */

            background.TrailingAnchor.ConstraintEqualToAnchor(cardView.TrailingAnchor, 0f).Active = true;
            background.LeadingAnchor.ConstraintEqualToAnchor(cardView.LeadingAnchor, 0f).Active = true;
            background.BottomAnchor.ConstraintEqualToAnchor(cardView.BottomAnchor, 0f).Active = true;
            background.TopAnchor.ConstraintEqualToAnchor(cardView.TopAnchor, 0f).Active = true;

            // View:     titleLabel
            var titleOffset = 0;
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

                titleLabel.LeadingAnchor.ConstraintEqualToAnchor(cardView.LeadingAnchor, 24f).Active = true;
                titleLabel.TopAnchor.ConstraintEqualToAnchor(cardView.TopAnchor, 17f).Active = true;

                //top position offset for card with a title
                titleOffset = 33;
            }

            // View:     optionsStackView
            var optionsStackView = new AppKit.NSStackView();
            optionsStackView.TranslatesAutoresizingMaskIntoConstraints = false;
            optionsStackView.EdgeInsets = new AppKit.NSEdgeInsets(0, 0, 0, 0);
            optionsStackView.Spacing = 0f;
            optionsStackView.Orientation = NSUserInterfaceLayoutOrientation.Vertical;
            optionsStackView.Distribution = NSStackViewDistribution.Fill;

            cardView.AddSubview(optionsStackView);
            optionsStackView.WidthAnchor.ConstraintEqualToConstant(600f).Active = true;

            optionsStackView.LeadingAnchor.ConstraintEqualToAnchor(cardView.LeadingAnchor, 20f).Active = true;
            optionsStackView.BottomAnchor.ConstraintEqualToAnchor(cardView.BottomAnchor, -20f).Active = true;
            optionsStackView.TopAnchor.ConstraintEqualToAnchor(cardView.TopAnchor, 20f + titleOffset).Active = true;

            foreach (Option option in OptionCard.Options)
            {
                NSView optionView = ((OptionVSMac)option.Platform).View;

                // If the option's visibility is dynamic, hide if it's not initially showing and update the hidden
                // property the desired visibility changes. For NSStackViews, hidden views are automatically detached,
                // excluded from the layout.

                UpdateOptionVisible(option);

                ToggleButtonOption? visibilityDependsOn = option.VisibilityDependsOn;
                if (visibilityDependsOn != null)
                {
                    visibilityDependsOn.Property.PropertyChanged += (sender, eventArgs) => UpdateOptionVisible(option);
                }

                ViewModelProperty<bool>? visible = option.Visible;
                if (visible != null)
                {
                    visible.PropertyChanged += (sender, eventArgs) => UpdateOptionVisible(option);
                }

                ToggleButtonOption? disablebilityDependsOn = option.DisablebilityDependsOn;
                if (disablebilityDependsOn != null)
                {
                    ViewModelProperty<bool> buttonProperty = disablebilityDependsOn.Property;

                    if (!buttonProperty.Value)
                        option.Platform.OnEnableChanged(buttonProperty.Value);

                    buttonProperty.PropertyChanged += delegate
                    {
                        option.Platform.OnEnableChanged(buttonProperty.Value);
                    };
                }

                optionsStackView.AddArrangedSubview(optionView);
            }

            return cardView;
        }

        private void UpdateOptionVisible(Option option)
        {
            NSView optionView = ((OptionVSMac)option.Platform).View;

            ViewModelProperty<bool>? toggleButtonProperty = option.VisibilityDependsOn?.Property;
            ViewModelProperty<bool>? visibleProperty = option.Visible;

            bool visible =
                (toggleButtonProperty == null || toggleButtonProperty.Value) &&
                (visibleProperty == null || visibleProperty.Value);

            optionView.Hidden = !visible;
        }
    }
}
