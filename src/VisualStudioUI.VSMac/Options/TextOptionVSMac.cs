// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

using AppKit;
using Foundation;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class TextOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSView? _controlView;
        private MacDebuggerTextField? _textField;
        private NSButton? _menuBtn;

        public TextOptionVSMac(TextOption option) : base(option)
        {
        }

        public TextOption TextOption => ((TextOption)Option);

        protected override NSView ControlView
        {
            get
            {
                if (_controlView == null)
                {
                    _controlView = new NSView
                    {
                        WantsLayer = true,
                        TranslatesAutoresizingMaskIntoConstraints = false
                    };

                    ViewModelProperty<string> property = TextOption.Property;

                    _textField = new MacDebuggerTextField
                    {
                        Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
                        StringValue = property.Value ?? string.Empty,
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        PlaceholderString = TextOption.PlaceholderText ?? string.Empty,
                    };
                    _textField.LineBreakMode = NSLineBreakMode.TruncatingTail;
                    SetAccessibilityTitleToLabel(_textField);

                    _controlView.AddSubview(_textField);

                    property.PropertyChanged += delegate
                    {
                        _textField.StringValue = TextOption.Property.Value;
                    };

                    _textField.Changed += delegate { property.Value = _textField.StringValue; };

                    if (TextOption.IsOnlyDigital)
                    {
                        var format = new NumberFormatter()
                        {
                            NumberStyle = NSNumberFormatterStyle.None,
                        };
                        format.RoundingMode = NSNumberFormatterRoundingMode.Up;//.PartialStringValidationEnabled = false;
                        format.MaximumIntegerDigits = 256;
                        //_textField.Cell.AllowedInputSourceLocales = NSAll new string[] { "0","1","2", "3", "4", "5", "6", "7", "8", "9" };
                        //_textField.Formatter = format;


                        _textField.Changed += (s, e) => {


                        };
                    }

                    if (TextOption.MacroMenuItems != null)
                    {
                        _menuBtn = new NSButton() {
                            BezelStyle = NSBezelStyle.RoundRect,
                            Image = NSImage.ImageNamed("NSGoRightTemplate"),
                            TranslatesAutoresizingMaskIntoConstraints = false
                        };

                        _menuBtn.Activated += (sender, e) =>
                        {
                            NSEvent events = NSApplication.SharedApplication.CurrentEvent;
                            NSMenu.PopUpContextMenu(CreateMenu(), events, events.Window.ContentView);
                        };
                        _controlView.AddSubview(_menuBtn);

                        _menuBtn.WidthAnchor.ConstraintEqualToConstant(24f).Active = true;
                        _menuBtn.HeightAnchor.ConstraintEqualToConstant(21f).Active = true;
                        _menuBtn.TrailingAnchor.ConstraintEqualToAnchor(_controlView.TrailingAnchor).Active = true;
                        _menuBtn.CenterYAnchor.ConstraintEqualToAnchor(_controlView.CenterYAnchor).Active = true;
                        _controlView.WidthAnchor.ConstraintEqualToConstant(228f).Active = true;
                    }
                    else
                    {
                        _controlView.WidthAnchor.ConstraintEqualToConstant(196f).Active = true;
                    }

                    _controlView.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
                    _textField.WidthAnchor.ConstraintEqualToConstant(196f).Active = true;
                    _textField.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
                    _textField.LeadingAnchor.ConstraintEqualToAnchor(_controlView.LeadingAnchor).Active = true;
                    _textField.TopAnchor.ConstraintEqualToAnchor(_controlView.TopAnchor).Active = true;
                }

                return _controlView;
            }
        }

        private NSMenu CreateMenu()
        {
            NSMenu groupMenu = new NSMenu
            {
                AutoEnablesItems = false
            };

            foreach (var item in TextOption.MacroMenuItems)
            {
                if (string.IsNullOrWhiteSpace(item.Label)) continue;

                if (!"-".Equals(item.Label))
                {
                    NSMenuItem menuItem = new NSMenuItem
                    {
                        Title = item.Label
                    };
                    menuItem.Activated += (sender, e) =>
                    {
                        if (_textField != null)
                        {
                            _textField.StringValue = item?.MacroName + _textField.StringValue; // New value insert to head
                            TextOption.Property.Value = _textField.StringValue;
                        }
                    };

                    groupMenu.AddItem(menuItem);
                }
                else
                {
                    groupMenu.AddItem(NSMenuItem.SeparatorItem);
                }
            }

            return groupMenu;
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);
            if (_textField != null)
                _textField.Enabled = enabled;

            if (_menuBtn!= null)
                _menuBtn.Enabled = enabled;
        }

    }

    internal class NumberFormatter : NSNumberFormatter
    {
        public override bool IsPartialStringValid(ref string partialString, out NSRange proposedSelRange, string origString, NSRange origSelRange, out string error)
        {
            var newChar = partialString;
            if (!string.IsNullOrWhiteSpace(partialString) &&
                !string.IsNullOrWhiteSpace(origString) &&
                partialString.Length > origString.Length)
            {
                newChar = newChar.Replace(origString, "");

                if (!int.TryParse(newChar, out int n))
                {
                    partialString = origString;
                    proposedSelRange = origSelRange;
                    error = "";
                    return false;
                }
            }
            
            return base.IsPartialStringValid(ref partialString, out proposedSelRange, origString, origSelRange, out error);
        }
    }

    class MacDebuggerTextField : NSTextField
    {
        string oldValue, newValue;
        bool editing;

        public override bool BecomeFirstResponder()
        {
           //if (Superview is MacDebuggerObjectNameView nameView)
            {
             //   nameView.BeginEditing();
              //  return true;
            }

            return base.BecomeFirstResponder();
        }

        public override void DidBeginEditing(NSNotification notification)
        {
            base.DidBeginEditing(notification);

           // if (Superview is MacDebuggerObjectCellViewBase cellView)
            {
             //   cellView.TreeView.OnBeginEditing();
                oldValue = newValue = StringValue.Trim();
                editing = true;
            }
        }

        public override void DidChange(NSNotification notification)
        {
            var str = StringValue.Trim()[StringValue.Length - 1];
            newValue = StringValue.Trim();
            base.DidChange(notification);
        }

        public override void DidEndEditing(NSNotification notification)
        {
            base.DidEndEditing(notification);

            if (!editing)
                return;

            editing = false;

            /*var cellView =Superview;
            cellView.TreeView.OnEndEditing();

            if (cellView is MacDebuggerObjectValueView)
            {
                var node = cellView.Node;

                if (node != null && newValue != oldValue && cellView.TreeView.GetEditValue(node, newValue))
                {
                    var metadata = new Dictionary<string, object>();
                    metadata["UIElementName"] = cellView.TreeView.UIElementName;
                    metadata["ObjectValue.Type"] = node.TypeName;

                    Counters.EditedValue.Inc(1, null, metadata);
                    cellView.Refresh();
                }
            }*/

            oldValue = newValue = null;
        }
    }
}
