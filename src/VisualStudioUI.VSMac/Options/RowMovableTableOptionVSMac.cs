using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AppKit;

using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class RowMovableTableOptionVSMac : OptionVSMac
    {
        private NSView _optionView;
        private NSTableView _tableView;
        private NSButton _upButton, _downButton;

        public RowMovableTableOptionVSMac(RowMovableTableOption option) : base(option)
        {
        }

        public override NSView View
        {
            get
            {
                if (_optionView == null)
                {
                    CreateView();
                }

                return _optionView;
            }
        }

        public RowMovableTableOption RowMovableTableOption => ((RowMovableTableOption)Option);

        public void CreateView()
        {
            _optionView = new NSView();

            _optionView.TranslatesAutoresizingMaskIntoConstraints = false;

            _tableView = new NSTableView()
            {
                HeaderView = null,
                Source = new SourceList(this),
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            _tableView.GridStyleMask = NSTableViewGridStyle.DashedHorizontalGridLine;
            _tableView.AddColumn(new NSTableColumn());
            _tableView.Activated += OnSelectionChanged;

            var scrolledView = new NSScrollView()
            {
                DocumentView = _tableView,
                BorderType = NSBorderType.LineBorder,
                HasVerticalScroller = true,
                HasHorizontalScroller = true,
                AutohidesScrollers = true,
                TranslatesAutoresizingMaskIntoConstraints = false
            };

            _optionView.AddSubview(scrolledView);
            _upButton = new NSButton
            {
                BezelStyle = NSBezelStyle.Inline,
                Bordered = false,
                Title = string.Empty,
                WantsLayer = true,
                Image = NSImage.GetSystemSymbol("arrow.up.circle", null),
                ContentTintColor = NSColor.SystemBlueColor,
                TranslatesAutoresizingMaskIntoConstraints = false,
                ToolTip = RowMovableTableOption.UpToolTip,
                Enabled = false,
            };
            _upButton.Layer.BackgroundColor = NSColor.ControlBackground.CGColor;
            _upButton.Layer.CornerRadius = 5;
            _upButton.Activated += OnMoveUpClicked;

            _downButton = new NSButton
            {
                BezelStyle = NSBezelStyle.Inline,
                Bordered = false,
                WantsLayer = true,
                Title = "",
                Image = NSImage.GetSystemSymbol("arrow.down.circle", null),
                ContentTintColor = NSColor.SystemBlueColor,
                TranslatesAutoresizingMaskIntoConstraints = false,
                ToolTip = RowMovableTableOption.DownToolTip,
                Enabled = false,
            };
            _downButton.Activated += OnMoveDownClicked;
            _downButton.Layer.BackgroundColor = NSColor.ControlBackground.CGColor;
            _downButton.Layer.CornerRadius = 5;

            _optionView.AddSubview(_upButton);
            _optionView.AddSubview(_downButton);

            if (!string.IsNullOrEmpty(Option.Label))
            {
                var left = new NSTextField();
                left.Editable = false;
                left.Bordered = false;
                left.DrawsBackground = false;
                left.StringValue = Option.Label + ":";
                left.Alignment = NSTextAlignment.Right;
                left.Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
                left.TextColor = NSColor.LabelColor;
                left.TranslatesAutoresizingMaskIntoConstraints = false;
                left.SizeToFit();
                _optionView.AddSubview(left);
                left.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor).Active = true;
                left.WidthAnchor.ConstraintEqualToConstant(205).Active = true;
                left.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor).Active = true;
            }

            _upButton.WidthAnchor.ConstraintEqualToConstant(30).Active = true;
            _upButton.HeightAnchor.ConstraintEqualToConstant(25).Active = true;
            _downButton.WidthAnchor.ConstraintEqualToConstant(30).Active = true;
            _downButton.HeightAnchor.ConstraintEqualToConstant(25).Active = true;
            _optionView.WidthAnchor.ConstraintEqualToConstant(640f - IndentValue()).Active = true;
            scrolledView.HeightAnchor.ConstraintEqualToConstant(RowMovableTableOption.Height).Active = true;
            scrolledView.WidthAnchor.ConstraintEqualToConstant(RowMovableTableOption.Width).Active = true;

            _upButton.TopAnchor.ConstraintEqualToAnchor(scrolledView.BottomAnchor, 10).Active = true;
            _upButton.LeadingAnchor.ConstraintEqualToAnchor(scrolledView.LeadingAnchor).Active = true;
            _downButton.TopAnchor.ConstraintEqualToAnchor(_upButton.TopAnchor).Active = true;
            _downButton.LeadingAnchor.ConstraintEqualToAnchor(_upButton.TrailingAnchor, 10).Active = true;
            _optionView.BottomAnchor.ConstraintEqualToAnchor(_upButton.BottomAnchor, 2).Active = true;
            scrolledView.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor).Active = true;

            float leftSpace = Option.AllowSpaceForLabel ? 222f : 20f;
            scrolledView.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, leftSpace + IndentValue()).Active = true;

            RowMovableTableOption.Property.PropertyChanged += OnCheckBoxListChanged;
        }

        public bool ShowDescriptions
        {
            get { return false; }
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);

            _upButton.Enabled = enabled;
            _downButton.Enabled = enabled;
            _tableView.Enabled = enabled;
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonsStatus();
        }

        private void UpdateButtonsStatus()
        {
            _upButton.Enabled = (_tableView.SelectedRow > 0);
            _downButton.Enabled = (_tableView.SelectedRow < RowMovableTableOption.Property.Value.Length - 1);
        }

        private void OnCheckBoxListChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void OnItemToggled(object sender, EventArgs e)
        {
            var checkBoxButton = (NSButton)(sender);

            int row = (int)checkBoxButton.Tag;

            RowMovableTableOption.Property.Value[row].Selected = (checkBoxButton.State == NSCellStateValue.On);
            RowMovableTableOption.ListChangedInvoke(sender, e);
        }

        private void RefreshList(int selectIndex = -1)
        {
            _tableView.ReloadData();

            if (selectIndex > -1)
            {
                _tableView.SelectRow(selectIndex, false);
            }

            UpdateButtonsStatus();
        }

        private void OnMoveUpClicked(object sender, EventArgs e)
        {
            try
            {
                if (RowMovableTableOption.Property.Value.Any())
                {
                    int selectedIndex = (int)_tableView.SelectedRow;
                    if (selectedIndex <= 0) return;// selected the first one

                    List<CheckBoxlistItem> list = new List<CheckBoxlistItem>(RowMovableTableOption.Property.Value);

                    var old = list.ElementAt(selectedIndex - 1);
                    list[selectedIndex - 1] = list.ElementAt(selectedIndex);
                    list[selectedIndex] = old;

                    RowMovableTableOption.Property.Value = RowMovableTableOption.Property.Value.Clear();
                    RowMovableTableOption.Property.Value = ImmutableArray.CreateRange(list);

                    RefreshList(selectedIndex - 1);

                    RowMovableTableOption.ListChangedInvoke(sender, e);
                }
            }
            catch
            { }
        }

        private void OnMoveDownClicked(object sender, EventArgs e)
        {
            try
            {
                if (RowMovableTableOption.Property.Value.Any())
                {
                    int selectedIndex = (int)_tableView.SelectedRow;
                    List<CheckBoxlistItem> list = new List<CheckBoxlistItem>(RowMovableTableOption.Property.Value);
                    if (selectedIndex >= list.Count - 1) return; // selected the last one

                    var old = list.ElementAt(selectedIndex + 1);
                    list[selectedIndex + 1] = list.ElementAt(selectedIndex);
                    list[selectedIndex] = old;

                    RowMovableTableOption.Property.Value = RowMovableTableOption.Property.Value.Clear();
                    RowMovableTableOption.Property.Value = ImmutableArray.CreateRange(list);

                    RefreshList(selectedIndex + 1);

                    RowMovableTableOption.ListChangedInvoke(sender, e);
                }
            }
            catch
            { }
        }
    }

    internal class SourceList : NSTableViewSource
    {
        private readonly RowMovableTableOptionVSMac _platform;

        public SourceList(RowMovableTableOptionVSMac platform)
        {
            _platform = platform;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            const string identifer = "cell";

            var view = (NSButton)tableView.MakeView(identifer, this);
            if (view == null)
            {
                view = new NSButton();
                view.Bordered = false;
                view.SetButtonType(NSButtonType.Switch);
                view.Activated += _platform.OnItemToggled;
                view.Identifier = identifer;
            }

            view.Tag = row;
            view.Title = _platform.RowMovableTableOption.Property.Value[(int)row].Title;
            view.State = _platform.RowMovableTableOption.Property.Value[(int)row].Selected ? NSCellStateValue.On : NSCellStateValue.Off;

            return view;
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return _platform.RowMovableTableOption.Property.Value.Length;
        }
    }
}

