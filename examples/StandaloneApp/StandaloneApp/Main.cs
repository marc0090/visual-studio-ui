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
                    Hint = "This is the hint to show!!"
                }
            );
            card1.AddOption(
                new TextOption(StringProp("text2"))
                {
                    Label = "Text 2"
                }
            );
            card1.AddOption(
                new TextOption(StringProp("text3"))
                {
                    Label = "Text "
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
            card2.AddOption(new TextOption(StringProp("text4")));
            card2.AddOption(new TextOption(StringProp("text4")));

            OptionCards cards = new OptionCards();
            cards.AddCard(card1);
            cards.AddCard(card2);

            return cards;
        }

        public static ViewModelProperty<string> StringProp(string name) => new ViewModelProperty<string>(name);
    }
}
