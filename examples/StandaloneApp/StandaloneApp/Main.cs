using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.StandaloneApp
{
    public static class Main
    {
        public static OptionCards CreateOptionCards()
        {
            //return CreateInfoPlistUI();
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
                new ComboBoxOption<string>(StringProp("option1"), StringArrayProp(new[] { "option1", "option2", "option3" }))
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

            var card3 = new OptionCard();
            card3.AddOption(
                new SwitchableGroupOption(BoolProp(true))
                {
                    Label = "Siri",
                    Name = "Allows your application to handle Siri requests.",
                    Hint = "Hint: Allows your application to handle Siri requests.",
                }
             );


            var card4 = new OptionCard();
            var switchableView = new SwitchableGroupOption(BoolProp(true))
            {
                Label = "iCloud",
                Name = "Allows your application to store data in the cloud and lets users share their data across devices.",
                Hint = "Hint: Allows your application to store data in the cloud and lets users share their data across devices.",
            };

            card4.AddOption(switchableView);


            ImmutableArray<string> list = ImmutableArray.Create("test1", "test2", "test3");

            card4.AddOption(new StringListOption(ListProp(list), "default string") { Label = "Containers" });

            //Signing 
            var signing = new OptionCard() { Label = "Signing" };
            bool isSelected = false;
            var manual = new ButtonOption(ButtonOption.ButtonType.Radio)

            {
                IsSelected = BoolProp(isSelected),
                Label = "Scheme",
                Name = "Manual Provisioning",
                Description = "Set provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioning"
            };
            manual.IsSelected.Bind();

            var auto = new ButtonOption(ButtonOption.ButtonType.Radio)
            {
                IsSelected = BoolProp(!isSelected),
                Name = "Automatic Provisioning",
                Description = "Set provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioning"
            };
            auto.IsSelected.Bind();

            manual.SelectionChanged += (sbye, e) =>
            {
                if (manual.IsSelected.Value == true)
                {
                    isSelected = !isSelected;
                    manual.IsSelected.Value = isSelected;
                    auto.IsSelected.Value = !isSelected;
                }
            };

            auto.SelectionChanged += (sbye, e) =>
            {
                if (auto.IsSelected.Value == true)
                {
                    isSelected = !isSelected;
                    manual.IsSelected.Value = isSelected;
                    auto.IsSelected.Value = !isSelected;
                }
            };

            signing.AddOption(manual);
            signing.AddOption(auto);

            signing.AddOption(new CheckBoxOption(BoolProp(false))
            {
                Label = "Orientations:",
                ButtonLabel = "Portrait",
                Description = "Set provisioningSet provisioningSet"
            });

            signing.AddOption(new CheckBoxOption(BoolProp(false))
            {
                Label = "Orientations:",
                ButtonLabel = "Portrait",
                Description = "Set provisioningSet provisioningSet. And here is more text, to be two lines."
            });

            OptionCards cards = new OptionCards();

            cards.AddCard(signing);
            cards.AddCard(card3);
            cards.AddCard(card4);
            cards.AddCard(card1);
            cards.AddCard(card2);

            return cards;
        }

        public static ViewModelProperty<bool> BoolProp(bool defaultValue) =>
            new ViewModelProperty<bool>("boolProp", defaultValue);
        public static ViewModelProperty<string> StringProp(string defaultValue) =>
            new ViewModelProperty<string>("stringProp", defaultValue);
        public static ViewModelProperty<ImmutableArray<string>> StringArrayProp(string[] defaultValue) =>
            new ViewModelProperty<ImmutableArray<string>>("stringArrayProp", ImmutableArray.Create(defaultValue));

        public static ViewModelProperty<ImmutableArray<string>> ListProp(ImmutableArray<string> defaultValue) => new ViewModelProperty<ImmutableArray<string>>("listProp", defaultValue);
    }
}
