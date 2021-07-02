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
            var switchableOption = new SwitchableGroupOption(BoolProp(true))
            {
                Label = "iCloud",
                Name = "Allows your application to store data in the cloud and lets users share their data across devices.Allows your application to store data in the cloud and lets users share their data across devices.Allows your application to store data in the cloud and lets users share their data across devices.Allows your application to store data in the cloud and lets users share their data across devices.",
                Hint = "Hint: Allows your application to store data in the cloud and lets users share their data across devices.",
            };

            card4.AddOption(switchableOption);

            ImmutableArray<string> list = ImmutableArray.Create("test1", "test2", "test3");

            var KeychainAccessGroupsList = new StringListOption(new ViewModelProperty<ImmutableArray<string>>("t", list))
            {
                AddToolTip = "Click to add an Keychain Access Group",
                RemoveToolTip = "Click to remove the selected Keychain Access Group",
                Label = "Keychain Groups",
                DefaultValue = "BundleIdentifier",
                PrefixValue = "AppIdentifierPrefix"
            };

            switchableOption.AddOption(KeychainAccessGroupsList);


            var card5 = new OptionCard();
            var walletSwitchableOption = new SwitchableGroupOption(BoolProp(true))
            {
                Label = "Wallet",
                Name = "Allows your application to manage passes, tickets, gift cards, and loyalty cards. It supports a variety of bar code formats."
            };
            var passTypeRadioGroup = new RadioButtonGroup();
            var alltype = new RadioButtonOption(passTypeRadioGroup, BoolProp(false))
            {
                Label = "Pass Types",
                ButtonLabel = "Allow all team pass types",
            };

            var subtype = new RadioButtonOption(passTypeRadioGroup, BoolProp(false))
            {
                ButtonLabel = "Allow subset of pass types",
            };


            ImmutableArray<CheckBoxlistItem> CheckBoxList = ImmutableArray.Create(
                new CheckBoxlistItem("Pass types1", false),
                new CheckBoxlistItem("Pass types2", false),
                new CheckBoxlistItem("Pass types3", false)

             );

            var testCheckBoxList = new CheckBoxListOption(new ViewModelProperty<ImmutableArray<CheckBoxlistItem>>("", CheckBoxList));

            walletSwitchableOption.AddOption(alltype);
            walletSwitchableOption.AddOption(subtype);
            walletSwitchableOption.AddOption(testCheckBoxList);
            card5.AddOption(walletSwitchableOption);

            // Signing 
            var signing = new OptionCard() { Label = "Signing" };

            ViewModelProperty<bool> automaticSigningEnabled = BoolProp(false);
            ViewModelProperty<bool> manualSigningEnabled = BoolProp(!automaticSigningEnabled.Value);

            var signingRadioGroup = new RadioButtonGroup();
            var manual = new RadioButtonOption(signingRadioGroup, manualSigningEnabled)
            {
                Label = "Scheme",
                ButtonLabel = "Manual Provisioning",
                Description = "Set provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioning"
            };

            var auto = new RadioButtonOption(signingRadioGroup, automaticSigningEnabled)
            {
                ButtonLabel = "Automatic Provisioning",
                Description = "Set provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioningSet provisioning"
            };

            signing.AddOption(manual);
            signing.AddOption(auto);

            var manualSigningOption1 = new CheckBoxOption(BoolProp(false))
            {
                ButtonLabel = "Some option for manual signing",
                VisibilityDependsOn = manual
            };
            signing.AddOption(manualSigningOption1);


            var autoSigningOption1 = new CheckBoxOption(BoolProp(false))
            {
                ButtonLabel = "Some option for automatic signing",
                VisibilityDependsOn = auto
            };
            var autoSigningOption2 = new CheckBoxOption(BoolProp(false))
            {
                ButtonLabel = "Another option for automatic signing",
                VisibilityDependsOn = auto
            };
            signing.AddOption(autoSigningOption1);
            signing.AddOption(autoSigningOption2);

            var iTunesArtwork = new OptionCard() { Label = "iTunes Artwork" };
            List<ScaledImageFile> imagelist = new List<ScaledImageFile>();
            imagelist.Add(new ScaledImageFile(512, 512, "1X"));
            imagelist.Add(new ScaledImageFile(1024, 1024, "2X"));
            ViewModelProperty<ImmutableArray<ScaledImageFile>> imageArray = new ViewModelProperty<ImmutableArray<ScaledImageFile>>("", imagelist.ToImmutableArray());
            var image = new ScaledImageFileOption(imageArray);
            iTunesArtwork.AddOption(image);

            var signInLabel = new LabelOption()
            {
                Label = ("Apple ID"),
                Name = "Sign in and select a team to enable Automatic Provisioning",
                IsBold = true
            };
            iTunesArtwork.AddOption(signInLabel);

            OptionCards cards = new OptionCards();
            cards.AddCard(iTunesArtwork);
            cards.AddCard(signing);
            cards.AddCard(card3);
            cards.AddCard(card4);
            cards.AddCard(card5);
            cards.AddCard(card1);
            cards.AddCard(card2);

            return cards;
        }

        public static ViewModelProperty<bool> BoolProp(bool defaultValue)
        {
            var prop = new ViewModelProperty<bool>("boolProp", defaultValue);
            prop.Bind();
            return prop;
        }

        public static ViewModelProperty<string> StringProp(string defaultValue) =>
            new ViewModelProperty<string>("stringProp", defaultValue);
        public static ViewModelProperty<ImmutableArray<string>> StringArrayProp(string[] defaultValue) =>
            new ViewModelProperty<ImmutableArray<string>>("stringArrayProp", ImmutableArray.Create(defaultValue));

        public static ViewModelProperty<bool[]> BoolArrayProp(bool[] defaultValue) =>
           new ViewModelProperty<bool[]>("boolArrayProp", defaultValue);

        public static ViewModelProperty<ImmutableArray<string>> ListProp(ImmutableArray<string> defaultValue) =>
            new ViewModelProperty<ImmutableArray<string>>("listProp", defaultValue);
    }
}
