using System;
using AppKit;

using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{

    public class CheckBoxListOptionVSMac : OptionVSMac
    {
        private NSView _optionView;
        private NSTableView _tableView;

        public CheckBoxListOptionVSMac(CheckBoxListOption option) : base(option)
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

        public CheckBoxListOption CheckBoxListOption => ((CheckBoxListOption)Option);

        public IntPtr Handle => throw new NotImplementedException();

        public void CreateView()
        {
            _optionView = new NSView();
                                     
            _optionView.TranslatesAutoresizingMaskIntoConstraints = false;

            _tableView = new NSTableView()
            {
                HeaderView = null,
                SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.None,
                Source = new CheckBoxSource(this),
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            _tableView.GridStyleMask = NSTableViewGridStyle.DashedHorizontalGridLine;
            _tableView.AddColumn(new NSTableColumn());

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

            _optionView.WidthAnchor.ConstraintEqualToConstant(640f - IndentValue()).Active = true;

            scrolledView.HeightAnchor.ConstraintEqualToConstant(CheckBoxListOption.Height).Active = true;
            scrolledView.WidthAnchor.ConstraintEqualToConstant(CheckBoxListOption.Width).Active = true;

            scrolledView.TopAnchor.ConstraintEqualToAnchor(_optionView.TopAnchor).Active = true;

            float leftSpace = Option.AllowSpaceForLabel ? 222f:20f;
            scrolledView.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, leftSpace + IndentValue()).Active = true;
            scrolledView.BottomAnchor.ConstraintEqualToAnchor(_optionView.BottomAnchor).Active = true;

            CheckBoxListOption.Property.PropertyChanged += OnCheckBoxListChanged;

        }

        public bool ShowDescriptions
        {
            get { return false; }
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);

            _tableView.Enabled = enabled;
        }

        private void OnCheckBoxListChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void OnItemToggled(object sender, EventArgs e)
        {

            var checkBoxButton = (NSButton)(sender);

            int row = (int)checkBoxButton.Tag;

            CheckBoxListOption.Property.Value[row].Selected = (checkBoxButton.State == NSCellStateValue.On);
            CheckBoxListOption.ListChangedInvoke(sender, e);
        }

        private void RefreshList()
        {
            _tableView.ReloadData();
        }
    }

    internal class CheckBoxSource : NSTableViewSource
    {
        private readonly CheckBoxListOptionVSMac _platform;

        public CheckBoxSource(CheckBoxListOptionVSMac platform)
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
            view.Title = _platform.CheckBoxListOption.Property.Value[(int)row].Title;
            view.State = _platform.CheckBoxListOption.Property.Value[(int)row].Selected ? NSCellStateValue.On : NSCellStateValue.Off;

            return view;
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return _platform.CheckBoxListOption.Property.Value.Length;
        }

        //public override nfloat GetRowHeight(NSTableView tableView, nint row)
        //{
        //	return 30;
        //}
    }

}
