using System;
using System.Collections.Generic;
using AppKit;
using Foundation;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class StringListOptionVSMac : OptionVSMac, INSTableViewDataSource, INSTableViewDelegate, INSTextFieldDelegate
    {
        NSStackView _optionView;
        NSTableView _tableView;
        NSScrollView _scrollView;
        NSButton _addButton, _removeButton;

        string defaultValue;
        List<string> _stringList;

        public StringListOptionVSMac(StringListOption option) : base(option)
        {
            _stringList = option.Property.Value;
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

        public IntPtr Handle => throw new NotImplementedException();

        public void CreateView()
        {
            _optionView = new NSStackView();
            _optionView.Orientation = NSUserInterfaceLayoutOrientation.Vertical;
            _optionView.Alignment = NSLayoutAttribute.Left;

            _tableView = new NSTableView() { HeaderView = null };
            _tableView.AddColumn(new NSTableColumn());
            _tableView.SelectionDidChange += OnSelectionChanged;

            var scrolledView = new NSScrollView()
            {
                DocumentView = _tableView,
                BorderType = NSBorderType.LineBorder,
                HasVerticalScroller = true,
                HasHorizontalScroller = true,
                AutohidesScrollers = true,
            };
            scrolledView.HeightAnchor.ConstraintEqualToConstant(72).Active = true;
            scrolledView.WidthAnchor.ConstraintEqualToConstant(450).Active = true;

            _optionView.AddArrangedSubview(scrolledView);

            _addButton = new AppKit.NSButton();
            _addButton.BezelStyle = NSBezelStyle.RegularSquare;
            _addButton.Title = "add";
            _addButton.ControlSize = NSControlSize.Large;
            _addButton.Font = AppKit.NSFont.SystemFontOfSize(AppKit.NSFont.SystemFontSize);
            _addButton.TranslatesAutoresizingMaskIntoConstraints = false;

            var addButtonWidthConstraint = _addButton.WidthAnchor.ConstraintEqualToConstant(21f);
            addButtonWidthConstraint.Priority = (System.Int32)AppKit.NSLayoutPriority.DefaultLow;
            addButtonWidthConstraint.Active = true;
            _addButton.Activated += OnAddClicked;

            _removeButton = new NSButton()
            {
                Title = "remove",

                Image = NSImage.ImageNamed("gtk-remove"),
                ToolTip = ""//removeTooltip
            };
            _removeButton.Activated += OnRemoveClicked;

            var h = new NSStackView();
            h.AddArrangedSubview(_addButton);
            h.AddArrangedSubview(_removeButton);
            _optionView.AddArrangedSubview(h);
        }

        [Export("tableView:viewForTableColumn:row:")]
        public NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {

            var view = (NSTextField)tableView.MakeView("cell", tableView);
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier = "cell";
                view.EditingEnded += OnValueEdited;
            }
            view.Tag = row;
            view.StringValue = _stringList[(int)row];
            return view;
        }

        [Export("numberOfRowsInTableView:")]
        public nint GetRowCount(NSTableView tableView)
        {
            return _stringList.Count;
        }

        public string ValuePrefix
        {
            get; set;
        }

        public bool ShowDescriptions
        {
            get { return false; }
        }



        //public void SetPListContainer(PObjectContainer container)
        //{
        //	if (!(container is P_stringList))
        //		throw new ArgumentException("The PList container must be a P_stringList.", nameof(container));

        //	if (_stringList != null)
        //		_stringList.Changed -= On_stringListChanged;

        //	_stringList = (P_stringList)container;
        //	_stringList.Changed += On_stringListChanged;
        //	RefreshList();
        //}

        void OnSelectionChanged(object sender, EventArgs e)
        {
            _removeButton.Enabled = (_tableView.SelectedCell != null);

        }

        //void On_stringListChanged(object sender, EventArgs e)
        //{
        //    RefreshList();
        //    //QueueDraw ();
        //}

        void OnValueEdited(object sender, EventArgs e)
        {
            var textField = (NSTextField)sender;
            int row = (int)textField.Tag;
            var newText = textField.StringValue;
            if (newText == null)
                return;

            var newValue = !string.IsNullOrEmpty(ValuePrefix) ? ValuePrefix + textField : newText;

            _stringList[row] = newValue;
        }

        void OnAddClicked(object sender, EventArgs e)
        {
            var defalutString = !string.IsNullOrEmpty(ValuePrefix) ? ValuePrefix + defaultValue : defaultValue;

            try
            {
                _stringList.Add(defalutString);
                _stringList.Add(defalutString);
                TableSelectLastItem();

            }
            catch
            {
                return;
            }
            finally
            {
                //this.add.Accessible.MakeAccessibilityAnnouncement (TranslationCatalog.GetString ("Row added"));
            }
        }

        void OnRemoveClicked(object sender, EventArgs e)
        {
            int selectedRow = (int)_tableView.SelectedRow;

            if (_tableView.SelectedCell == null)
                return;

            var str = _stringList[selectedRow];

            // _stringList.Changed -= On_stringListChanged;

            try
            {
                _stringList.RemoveAt(selectedRow);

                if (_stringList.Count <= 0)
                {
                    _removeButton.Enabled = false;
                    return;
                }
                TableSelectLastItem();
            }
            catch
            {
                return;
            }
            finally
            {
                // _stringList.Changed += On_stringListChanged;
                //this.remove.Accessible.MakeAccessibilityAnnouncement (TranslationCatalog.GetString ("Row removed"));
            }
        }

        void TableSelectLastItem()
        {
            if (_stringList.Count <= 0)
            {
                return;
            }
            _tableView.SelectRow(_stringList.Count - 1, false);
        }

        void TableSelectFirstItem()
        {
            if (_stringList.Count <= 0)
            {
                return;
            }
            _tableView.SelectRow(0, false);
        }

        void RefreshList()
        {
            _stringList.Clear();

            for (int i = 0; i < _stringList.Count; i++)
            {
                var str = _stringList[i];

                if (str == null)
                    continue;
                var value = str;
                if (!string.IsNullOrEmpty(ValuePrefix) && value.StartsWith(ValuePrefix, StringComparison.Ordinal))
                    value = value.Substring(ValuePrefix.Length);

                _stringList.Add(str);
            }

            _removeButton.Enabled = _stringList.Count > 0;
            _tableView.ReloadData();
            TableSelectLastItem();
        }

        void IDisposable.Dispose()
        {
            _tableView.SelectionDidChange -= OnSelectionChanged;
            _removeButton.Activated -= OnRemoveClicked;
            _addButton.Activated -= OnAddClicked;
        }
    }

}
