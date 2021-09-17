using System;
using AppKit;
using CoreGraphics;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ButtonOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSButton _button;

        public ButtonOptionVSMac(ButtonOption option) : base(option)
        {
        }

        public ButtonOption ButtonOption => ((ButtonOption)Option);


        protected override NSView ControlView
        {
            get
            {
                if (_button == null)
                {
                    _button = new NSButton();
                    _button.BezelStyle = NSBezelStyle.RoundRect;
                    _button.ControlSize = NSControlSize.Regular;
                    _button.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                    _button.Title = ButtonOption.ButtonLabel;
                    _button.TranslatesAutoresizingMaskIntoConstraints = false;
                    _button.SizeToFit();

                    if (!string.IsNullOrWhiteSpace(ButtonOption.PopoverMessage))
                        _button.Activated += (o, args) => ShowHintPopover(ButtonOption.PopoverMessage, _button, 500);
                    else
                    {

                        NSButton addBtn = new NSButton
                        {
                            BezelStyle = NSBezelStyle.RoundRect,
                            ControlSize = NSControlSize.Regular,
                            Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
                            Title = "Add",
                            TranslatesAutoresizingMaskIntoConstraints = false,
                        };

                        NSWindow wind = new NSWindow();

                        // _button.Activated += ButtonOption.ButtonClicked;
                        _button.Activated += (s, e) => {
                            NSComboBox _comboBox = new NSComboBox
                            {
                                ControlSize = NSControlSize.Regular,
                                Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
                                TranslatesAutoresizingMaskIntoConstraints = false
                            };
                            _comboBox.WidthAnchor.ConstraintEqualToConstant(198f).Active = true;

                            /*var scrollview = new NSScrollView(new CGRect(0, 0, 1000, 200))
                            {
                                DocumentView = _comboBox,
                                HasHorizontalScroller = true,
                                HasVerticalScroller = true
                            };*/

                            var scrollview = new NSStackView()
                            {
                               // Orientation = NSUserInterfaceLayoutOrientation.Vertical,
                                Spacing = 10,
                                Distribution = NSStackViewDistribution.Fill,
                                TranslatesAutoresizingMaskIntoConstraints = false
                            };
                            scrollview.Alignment = NSLayoutAttribute.CenterX;
                            //scrollview.Alignment = NSLayoutAttribute.CenterY;
                            //scrollview.Layer.BackgroundColor = NSColor.Red.CGColor;
                            // scrollview.HorizontalScroller.ControlSize = NSControlSize.Small;
                            // scrollview.VerticalScroller.ControlSize = NSControlSize.Small;

                            var comboBoxView = new NSStackView()
                            {
                                Orientation = NSUserInterfaceLayoutOrientation.Horizontal,
                                Spacing = 10,
                                // Distribution = NSStackViewDistribution.Fill,
                                TranslatesAutoresizingMaskIntoConstraints = false
                            };

                            var left = new NSTextField();
                            left.Editable = false;
                            left.Bordered = false;
                            left.DrawsBackground = false;
                            left.StringValue = Option.Label + ":";
                            //left.Alignment = NSTextAlignment.Right;
                            //left.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                            // left.TextColor = NSColor.LabelColor;
                            left.TranslatesAutoresizingMaskIntoConstraints = false;
                            //left.SizeToFit();
                            /*comboBoxView.AddSubview(left);
                            left.LeadingAnchor.ConstraintEqualToAnchor(comboBoxView.LeadingAnchor, 10).Active = true;
                            left.WidthAnchor.ConstraintEqualToConstant(205).Active = true;
                            left.TopAnchor.ConstraintEqualToAnchor(comboBoxView.TopAnchor).Active = true;
                            */
                            // comboBoxView.AddSubview(left);
                            comboBoxView.AddSubview(_comboBox);
                            //_comboBox.TopAnchor.ConstraintEqualToAnchor(comboBoxView.TopAnchor).Active = true;
                            scrollview.AddArrangedSubview(left);
                            scrollview.AddArrangedSubview(_comboBox);

                            var buttonsView = new NSStackView()
                            {
                                Orientation = NSUserInterfaceLayoutOrientation.Horizontal,
                                Spacing = 10,
                                //  Distribution = NSStackViewDistribution.Fill,
                                TranslatesAutoresizingMaskIntoConstraints = false
                            };

                            //addBtn.SizeToFit();

                            NSButton removeBtn = new NSButton()
                            {
                                BezelStyle = NSBezelStyle.RoundRect,
                                ControlSize = NSControlSize.Regular,
                                Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
                                Title = "remove",
                                TranslatesAutoresizingMaskIntoConstraints = false,
                            };
                            buttonsView.AddArrangedSubview(addBtn);
                            buttonsView.AddArrangedSubview(removeBtn);
                            scrollview.AddArrangedSubview(buttonsView);



                            //scrollview.HeightAnchor.ConstraintEqualToConstant(500).Active = true;
                            //scrollview.WidthAnchor.ConstraintEqualToConstant(500).Active = true;



                            //scrollview.Frame = new CGRect(0, 0, 400, 300);

                            var dlg = new NSAlert();
                            dlg.Window.Title = "category";
                            dlg.AlertStyle = NSAlertStyle.Informational;
                            dlg.AccessoryView = scrollview;
                            dlg.Icon = new NSImage();
                            dlg.Icon.Size = new CGSize(0, 0);
                            // dlg.RunModal();

                            wind.Title = "category";
                            wind.ContentView.AddSubview(scrollview);
                            //CGRect frame = wind.Frame;
                            //frame.Size = new CGSize(1000, 200);
                            wind.ContentView.WidthAnchor.ConstraintEqualToConstant(500).Active = true;
                            wind.ContentView.HeightAnchor.ConstraintEqualToConstant(200).Active = true;

                            scrollview.HeightAnchor.ConstraintEqualToAnchor(wind.ContentView.HeightAnchor).Active = true;
                            scrollview.WidthAnchor.ConstraintEqualToAnchor(wind.ContentView.WidthAnchor).Active = true;

                            //scrollview.TopAnchor.ConstraintEqualToAnchor(wind.ContentView.TopAnchor).Active = true;
                            var response = NSApplication.SharedApplication.RunModalForWindow(wind);

                            /* NSWindowController controller = new NSWindowController();
                             controller.Window.Title = "tst";
                             NSApplication application = new NSApplication(null);
                             application.RunModalForWindow(wind);*/


                        };



                        addBtn.Activated += (s, e) => {
                            wind.Close();
                            wind.Dispose();
                            NSApplication.SharedApplication.StopModal();
                        };
                    }
                       
                }

                return _button!;

            }
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);

            if (_button != null)
                _button.Enabled = enabled;
        }
    }
}
