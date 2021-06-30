using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using AppKit;
using Foundation;
using Microsoft.VisualStudioUI.Options;

namespace Microsoft.VisualStudioUI.VSMac.Options
{

    public class StringListOptionVSMac : OptionVSMac
    {
        private NSStackView _optionView;
        private NSTableView _tableView;
        private NSButton _addButton, _removeButton;

        private readonly List<string> _stringList = new List<string>();

        public StringListOptionVSMac(StringListOption option) : base(option)
        {
        }

        private void UpdateStringListFromModel()
        {
            _stringList.Clear();
            foreach (string item in StringListOption.Model.Value)
            {
                _stringList.Add(item);
            }
        }

        private void UpdateModelFromStringList()
        {
            StringListOption.Model.PropertyChanged -= OnStringsListChanged;
            StringListOption.Model.Value = _stringList.ToImmutableArray();
            StringListOption.Model.PropertyChanged += OnStringsListChanged;
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

        public StringListOption StringListOption => ((StringListOption)Option);

        public IntPtr Handle => throw new NotImplementedException();

        public void CreateView()
        {

            var vContainer = new NSStackView
            {
                Orientation = NSUserInterfaceLayoutOrientation.Vertical,
                Alignment = NSLayoutAttribute.Left

            };

            _tableView = new NSTableView() { HeaderView = null, Source = new ListSource(this) };
            _tableView.GridStyleMask = NSTableViewGridStyle.DashedHorizontalGridLine;
            _tableView.AddColumn(new NSTableColumn());

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

            vContainer.AddArrangedSubview(scrolledView);

            _addButton = new NSButton
            {
                BezelStyle = NSBezelStyle.TexturedRounded,
                Bordered = false,
                Title = string.Empty,
                WantsLayer = true,
                Image = NSImage.GetSystemSymbol("plus.circle", null),
                ContentTintColor = NSColor.SystemGreenColor,
                TranslatesAutoresizingMaskIntoConstraints = false,
                ToolTip = StringListOption.AddToolTip
            };

            _addButton.WidthAnchor.ConstraintEqualToConstant(28).Active = true;
            _addButton.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
            _addButton.Layer.BackgroundColor = NSColor.TextBackground.CGColor;
            _addButton.Layer.CornerRadius = 5;
            _addButton.Activated += OnAddClicked;

            _removeButton = new NSButton
            {
                BezelStyle = NSBezelStyle.Rounded,
                Bordered = false,
                WantsLayer = true,
                Title = "",
                Image = NSImage.GetSystemSymbol("xmark.circle", null),
                ContentTintColor = NSColor.SystemPinkColor,
                TranslatesAutoresizingMaskIntoConstraints = false,
                ToolTip = StringListOption.RemoveToolTip
            };
            _removeButton.WidthAnchor.ConstraintEqualToConstant(28).Active = true;
            _removeButton.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
            _removeButton.Activated += OnRemoveClicked;
            _removeButton.Layer.BackgroundColor = NSColor.TextBackground.CGColor;
            _removeButton.Layer.CornerRadius = 5;

            var h = new NSStackView() { Orientation = NSUserInterfaceLayoutOrientation.Horizontal };
            h.AddArrangedSubview(_addButton);
            h.AddArrangedSubview(_removeButton);

            vContainer.AddArrangedSubview(h);

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
                _optionView.AddArrangedSubview(vContainer);
            }
            else
            {
                _optionView = vContainer;

            }
            StringListOption.Model.PropertyChanged += OnStringsListChanged;

            UpdateStringListFromModel();
        }

        public bool ShowDescriptions
        {
            get { return false; }
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            _removeButton.Enabled = (_tableView.SelectedCell != null);

        }

        private void OnStringsListChanged(object sender, EventArgs e)
        {
            UpdateStringListFromModel();

            _tableView.ReloadData();

            TableSelectLastItem();

            _removeButton.Enabled = _stringList.Count > 0;

        }

        public void OnValueEdited(object sender, EventArgs e)
        {
            var sObject = ((NSNotification)sender).Object;

            if (sObject == null)
            {
                return;
            }

            var textField = (NSTextField)(sObject);

            int row = (int)textField.Tag;
            string? newText = textField.StringValue;
            if (string.IsNullOrEmpty(newText) || newText.Equals(_stringList[row]))
            {
                return;
            }

            string? newValue = !string.IsNullOrEmpty(StringListOption.PrefixValue) ? StringListOption.PrefixValue + textField.StringValue : newText;

            _stringList[row] = newValue;

            UpdateModelFromStringList();
            RefreshList();

            StringListOption.ListChangedInvoke(sender, e);

        }

        private void OnAddClicked(object sender, EventArgs e)
        {
            var defalutString = !string.IsNullOrEmpty(StringListOption.PrefixValue) ? StringListOption.PrefixValue + StringListOption.DefaultValue : StringListOption.DefaultValue;

            try
            {
                _stringList.Add(defalutString);

                UpdateModelFromStringList();
                RefreshList();

                StringListOption.ListChangedInvoke(sender, e);

            }
            catch
            {
                return;
            }
            finally
            {
                // _addButton.accessibil.MakeAccessibilityAnnouncement (TranslationCatalog.GetString ("Row added"));
            }
        }

        private void OnRemoveClicked(object sender, EventArgs e)
        {
            int selectedRow = (int)_tableView.SelectedRow;

            if (selectedRow < 0 || selectedRow >= _stringList.Count)
            {
                return;
            }
            //fix reference bug when selected row in textfield editing state
            NSTableRowView? view = _tableView.GetRowView(selectedRow, false);
            view.Window.MakeFirstResponder(null);

            try
            {
                _stringList.RemoveAt(selectedRow);
                UpdateModelFromStringList();
                RefreshList();
                StringListOption.ListChangedInvoke(sender, e);
            }
            catch
            {
                return;
            }
            finally
            {
                // this.remove.Accessible.MakeAccessibilityAnnouncement (TranslationCatalog.GetString ("Row removed"));
            }
        }

        private void TableSelectLastItem()
        {
            if (_stringList.Count <= 0)
            {
                return;
            }
            int selectRow = _stringList.Count - 1;

            _tableView.SelectRow(selectRow, false);
            _tableView.ScrollRowToVisible(selectRow);
        }

        private void TableSelectFirstItem()
        {
            if (_stringList.Count <= 0)
            {
                return;
            }
            _tableView.SelectRow(0, false);
        }

        private void RefreshList()
        {
            _tableView.ReloadData();

            TableSelectLastItem();

            _removeButton.Enabled = _stringList.Count > 0;

        }
    }

    internal class ListSource : NSTableViewSource
    {
        private readonly StringListOptionVSMac _platform;

        public ListSource(StringListOptionVSMac platform)
        {
            _platform = platform;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            var view = (NSTableCellView)tableView.MakeView("cell", this);
            if (view == null)
            {
                view = new NSTableCellView
                {
                    TextField = new NSTextField
                    {
                        Frame = new CoreGraphics.CGRect(5, 2, tableColumn.Width, 20),
                        Hidden = false,
                        Bordered = false,
                        DrawsBackground = false,
                        Highlighted = false,
                    },
                    Identifier = "cell"
                };

                view.AddSubview(view.TextField);

                view.TextField.EditingEnded += _platform.OnValueEdited;
            }

            view.TextField.Tag = row;
            string value = _platform.StringListOption.Model.Value[(int)row];
            string prefix = _platform.StringListOption.PrefixValue;

            if (!string.IsNullOrEmpty(prefix) && value.StartsWith(prefix, StringComparison.Ordinal))
                value = value.Substring(prefix.Length);

            view.TextField.StringValue = value;

            return view;
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return _platform.StringListOption.Model.Value.Length;
        }

        //public override nfloat GetRowHeight(NSTableView tableView, nint row)
        //{
        //	return 30;
        //}
    }

}
