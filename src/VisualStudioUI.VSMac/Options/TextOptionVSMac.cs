using System;
using System.Drawing;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class TextOptionVSMac : OptionWithLeftLabelVSMac
    {
        NSView _optionView;
        private NSTextField? _textField;

        public TextOptionVSMac(TextOption option) : base(option)
        {
        }

        public TextOption TextOption => ((TextOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_optionView == null)
                {
                    _optionView = new NSView();
                    _optionView.WantsLayer = true;
                    _optionView.TranslatesAutoresizingMaskIntoConstraints = false;

                    ViewModelProperty<string> property = TextOption.Property;

                    _textField = new AppKit.NSTextField();
                    _textField.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
                    _textField.StringValue = property.Value ?? string.Empty;
                    _textField.TranslatesAutoresizingMaskIntoConstraints = false;
                    _textField.Editable = TextOption.Editable;
                    _textField.Bordered = TextOption.Bordered;
                    _textField.DrawsBackground = TextOption.DrawsBackground;
                    
                    _optionView.AddSubview(_textField);

                    _textField.WidthAnchor.ConstraintEqualToConstant(196f).Active = true;
                    _textField.LeftAnchor.ConstraintEqualToAnchor(_optionView.LeftAnchor).Active = true;
                    _textField.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor).Active = true;

                    _optionView.WidthAnchor.ConstraintEqualToConstant(600f).Active = true;
                    _optionView.HeightAnchor.ConstraintEqualToConstant(_textField.FittingSize.Height).Active = true;
                    
                    property.PropertyChanged += delegate(object o, ViewModelPropertyChangedEventArgs args)
                    {
                        _textField.StringValue = ((string)args.NewValue) ?? string.Empty;
                    };

                    _textField.Changed += delegate { property.Value = _textField.StringValue; };

                    if (TextOption.MacroMenuItems != null)
                    {
                        NSButton menuBtn = new NSButton() { Title = ">"};
                       // menuBtn.BezelStyle = NSBezelStyle.;
                        menuBtn.TranslatesAutoresizingMaskIntoConstraints = false;
                        menuBtn.Activated += (sender, e) => {

                            NSEvent events = NSApplication.SharedApplication.CurrentEvent;
                            NSMenu.PopUpContextMenu(CreateMenu(), events, events.Window.ContentView);

                            //var location = NSEvent.CurrentMouseLocation;
                            //nSMenu.PopUpMenu(nSMenu.ItemAt(0), location/*btn.Window.MouseLocationOutsideOfEventStream*/, btn.Window.ContentView);
                        };
                      
                        _optionView.AddSubview(menuBtn);

                        menuBtn.WidthAnchor.ConstraintEqualToConstant(19f).Active = true;
                        menuBtn.LeftAnchor.ConstraintEqualToAnchor(_textField.RightAnchor, 5f).Active = true;
                        menuBtn.BottomAnchor.ConstraintEqualToAnchor(_optionView.BottomAnchor).Active = true;
                    }
                }

                return _optionView;
            }
        }

        private NSMenu CreateMenu()
        {
            NSMenu groupMenu = new NSMenu();
            groupMenu.AutoEnablesItems = false;

            foreach (var item in TextOption.MacroMenuItems)
            {
                if (string.IsNullOrWhiteSpace(item.Label)) continue;

                if (item.Label.Equals("-"))
                {
                    groupMenu.AddItem(NSMenuItem.SeparatorItem);
                }
                else
                {
                    NSMenuItem menuItem = new NSMenuItem();
                    menuItem.Title = item.Label;
                    menuItem.Activated += (sender, e) => {
                        _textField.StringValue = item?.MacroName + _textField.StringValue; // New value insert to head
                    };

                    groupMenu.AddItem(menuItem);
                }
            }

            return groupMenu;
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);
            if (_textField != null)
                _textField.Enabled = enabled;
        }

        /*
        public override void Dispose ()
        {
            Property.PropertyChanged -= UpdatePopUpBtnValue;
            textField.Changed -= UpdatePropertyValue;

            base.Dispose ();
        }
        */
    }
}
