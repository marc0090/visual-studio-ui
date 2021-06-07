using System;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.StandaloneApp
{
    public static class Main
    {
        public static OptionCards CreateOptionCards()
        {
            var card1 = new OptionCard()
            {
                Label = "My Card"
            };

            card1.AddOption(
                new TextOption(StringProp("text1"))
                {
                    Label = "Text 1",
                    Hint = "This is the hint to show!"
                }
            );
            card1.AddOption(
                new TextOption(StringProp("text2"))
                {
                    Label = "Text 2"
                }
            );
            card1.AddOption(new SeparatorOption());
            card1.AddOption(
                new TextOption(StringProp("text3"))
                {
                    Label = "Text"
                }
            );
            card1.AddOption(
                new ComboBoxOption(StringProp("option1"), StringArrayProp(new[] { "option1", "option2", "option3" }))
                {
                    Label = "Choices",
                    Hint = "This is the hint for the combo box"
                }
            );
            card1.AddOption(
                new EditableComboBoxOption(StringProp("option1"), StringArrayProp(new[] { "option1", "option2", "option3" }))
                {
                    Label = "Editable choices",
                    Hint = "This is the hint for the editable combo box"
                }
            );

            var card2 = new OptionCard()
            {
                Label = "Second Card"
            };
            card2.AddOption(new TextOption(StringProp("text1")));
            card2.AddOption(new TextOption(StringProp("text2")));
            card2.AddOption(new TextOption(StringProp("text3")));
            card2.AddOption(new TextOption(StringProp("text4")));
            card2.AddOption(new TextOption(StringProp("text4")));
            card2.AddOption(
                new DocButtonOption(StringProp("https://www.microsoft.com"), "Launch Browser for Doc")
                {
                    Label = "Learn more"
                }
            );

            OptionCards cards = new OptionCards();
            cards.AddCard(card1);
            cards.AddCard(card2);

            return cards;
        }

        public static ViewModelProperty<string> StringProp(string defaultValue) => new ViewModelProperty<string>("stringProp", defaultValue);
        public static ViewModelProperty<string[]> StringArrayProp(string[] defaultValue) =>
            new ViewModelProperty<string[]>("stringArrayProp", defaultValue);
    }
}
