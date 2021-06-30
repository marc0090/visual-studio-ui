using System;
using AppKit;

using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{

    public class CheckBoxListOptionVSMac : OptionVSMac
    {
        private NSStackView _optionView;
        private NSTableView _tableView;

        public CheckBoxListOptionVSMac(CheckBoxListOption option) : base(option)
        {
            option.Property.PropertyChanged += OnCheckBoxListChanged;
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
            _tableView = new NSTableView() { HeaderView = null, Source = new CheckBoxSource(this) };
            _tableView.GridStyleMask = NSTableViewGridStyle.DashedHorizontalGridLine;
            _tableView.SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.None;
            _tableView.AddColumn(new NSTableColumn());

            var scrolledView = new NSScrollView()
            {
                DocumentView = _tableView,
                BorderType = NSBorderType.LineBorder,
                HasVerticalScroller = true,
                HasHorizontalScroller = true,
                AutohidesScrollers = true,

            };

            _optionView = new NSStackView() { Orientation = NSUserInterfaceLayoutOrientation.Horizontal, Alignment = NSLayoutAttribute.Top };

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

                _optionView = new NSStackView() { Orientation = NSUserInterfaceLayoutOrientation.Horizontal, Alignment = NSLayoutAttribute.Top };
                _optionView.AddArrangedSubview(left);
            }

            _optionView.AddArrangedSubview(scrolledView);

            scrolledView.HeightAnchor.ConstraintEqualToConstant(72).Active = true;
            scrolledView.WidthAnchor.ConstraintEqualToConstant(450).Active = true;
            scrolledView.LeadingAnchor.ConstraintEqualToAnchor(_optionView.LeadingAnchor, 101).Active = true;

        }

        public bool ShowDescriptions
        {
            get { return false; }
        }

        private void OnCheckBoxListChanged(object sender, EventArgs e)
        {
            RefreshList();
        }

        public void OnItemToggled(object sender, EventArgs e)
        {

            var checkBoxButton = (NSButton)(sender);

            int row = (int)checkBoxButton.Tag;

            // CheckBoxListOption.Property.Value[row].Selected = (checkBoxButton.State == NSCellStateValue.On);
            // CheckBoxListOption.Items.Value = new bool[] { false, false, false, false, false, false };
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
