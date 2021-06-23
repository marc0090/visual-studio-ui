using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using AppKit;
using Foundation;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{

    public class StringListOptionVSMac : OptionVSMac
    {
        NSStackView _optionView;
        NSTableView _tableView;
        NSButton _addButton, _removeButton;
        string addToolTip, removeToolTip, defaultValue;
        internal ViewModelProperty<ImmutableArray<string>> Model;
        List<string> StringList;

        public StringListOptionVSMac(StringListOption option) : base(option)
        {
            Model = option.Model;
            option.Model.PropertyChanged += OnStringsListChanged;
            StringList = new List<string>();
            defaultValue = option.DefaultValue;
            addToolTip = option.AddToolTip;
            removeToolTip = option.RemoveToolTip;
        }

        void UpdateStringListFromModel()
        {
            foreach (string item in StringListOption.Model.Value)
            {
                StringList.Add(item);
            }

        }

        void UpdateModelFromStringList()
        {
            StringListOption.Model.Value = StringList.ToImmutableArray();
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
                Title = "",
                WantsLayer = true,
                Image = NSImage.GetSystemSymbol("plus.circle", null),
                ContentTintColor = NSColor.SystemGreenColor,
                TranslatesAutoresizingMaskIntoConstraints = false,
                ToolTip = addToolTip
            };

            _addButton.WidthAnchor.ConstraintEqualToConstant(25).Active = true;
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
                ToolTip = removeToolTip
            };
            _removeButton.WidthAnchor.ConstraintEqualToConstant(25).Active = true;
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

            UpdateStringListFromModel();
            _tableView.ReloadData();
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
        //	if (!(container is PStringsList))
        //		throw new ArgumentException("The PList container must be a PStringsList.", nameof(container));

        //	if (StringsList != null)
        //		StringsList.Changed -= OnStringsListChanged;

        //	StringsList = (PStringsList)container;
        //	StringsList.Changed += OnStringsListChanged;
        //	RefreshList();
        //}

        void OnSelectionChanged(object sender, EventArgs e)
        {
            _removeButton.Enabled = (_tableView.SelectedCell != null);

        }

        void OnStringsListChanged(object sender, EventArgs e)
        {
            RefreshList();
            //QueueDraw ();
        }

        public void OnValueEdited(object sender, EventArgs e)
        {
            var textField = ((NSTextField)(((NSNotification)sender).Object));
            int row = (int)textField.Tag;
            var newText = textField.StringValue;
            if (newText == null)
                return;

            var newValue = !string.IsNullOrEmpty(ValuePrefix) ? ValuePrefix + textField : newText;

            StringList[row] = newValue;

            RefreshList();
        }

        void OnAddClicked(object sender, EventArgs e)
        {
            var defalutString = !string.IsNullOrEmpty(ValuePrefix) ? ValuePrefix + defaultValue : defaultValue;

            try
            {
                StringList.Add(defalutString);
                RefreshList();
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

        void OnRemoveClicked(object sender, EventArgs e)
        {
            int selectedRow = (int)_tableView.SelectedRow;

            if (selectedRow < 0 || selectedRow >= StringList.Count)
            {
                return;
            }

            try
            {
                StringList.RemoveAt(selectedRow);
                RefreshList();

                if (StringList.Count <= 0)
                {
                    _removeButton.Enabled = false;
                    return;
                }
            }
            catch
            {
                return;
            }
            finally
            {
                //this.remove.Accessible.MakeAccessibilityAnnouncement (TranslationCatalog.GetString ("Row removed"));
            }
        }

        void TableSelectLastItem()
        {
            if (StringList.Count <= 0)
            {
                return;
            }
            int selectRow = StringList.Count - 1;

            _tableView.SelectRow(selectRow, false);
            _tableView.ScrollRowToVisible(selectRow);
        }

        void TableSelectFirstItem()
        {
            if (StringList.Count <= 0)
            {
                return;
            }
            _tableView.SelectRow(0, false);
        }

        void RefreshList()
        {
            UpdateModelFromStringList();

            _tableView.ReloadData();

            TableSelectLastItem();

            _removeButton.Enabled = StringList.Count > 0;

        }
    }

    class ListSource : NSTableViewSource
    {
        StringListOptionVSMac Platform;

        public ListSource(StringListOptionVSMac platform)
        {
            Platform = platform;
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
                        Frame = new CoreGraphics.CGRect(4, 5, tableColumn.Width, 20),
                        Hidden = false,
                        Bordered = false,
                        DrawsBackground = false
                    },
                    Identifier = "cell"
                };

                view.AddSubview(view.TextField);

                view.TextField.EditingEnded += Platform.OnValueEdited;
            }

            view.TextField.Tag = row;
            view.TextField.StringValue = Platform.Model.Value[(int)row];

            return view;
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return Platform.Model.Value.Length;
        }

        public override nfloat GetRowHeight(NSTableView tableView, nint row)
        {
            return 30;
        }
    }

}
