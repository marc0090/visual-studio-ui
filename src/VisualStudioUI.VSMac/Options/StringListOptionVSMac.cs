using System;
using System.Collections.Generic;
using AppKit;
using Foundation;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options {

	public class StringListOptionVSMac : OptionVSMac {
		NSStackView _optionView;
		NSTableView _tableView;
		NSButton _addButton, _removeButton;
		string addToolTip, removeToolTip, defaultValue;
		internal List<string> StringsList;

		public StringListOptionVSMac (StringListOption option) : base (option)
		{
			StringsList = option.Property.Value;
			option.Property.PropertyChanged += OnStringsListChanged;
			defaultValue = option.DefaultValue;
			addToolTip = option.AddToolTip;
			removeToolTip = option.RemoveToolTip;
		}

		public override NSView View {
			get {
				if (_optionView == null) {
					CreateView ();
				}

				return _optionView;
			}
		}

		public IntPtr Handle => throw new NotImplementedException ();

		public void CreateView ()
		{
			_optionView = new NSStackView {
				Orientation = NSUserInterfaceLayoutOrientation.Vertical,
				Alignment = NSLayoutAttribute.Left
			};

			_tableView = new NSTableView () { HeaderView = null, Source = new ListSource (this) };
			_tableView.GridStyleMask = NSTableViewGridStyle.DashedHorizontalGridLine;
			_tableView.AddColumn (new NSTableColumn ());

			var scrolledView = new NSScrollView () {
				DocumentView = _tableView,
				BorderType = NSBorderType.LineBorder,
				HasVerticalScroller = true,
				HasHorizontalScroller = true,
				AutohidesScrollers = true,
			};
			scrolledView.HeightAnchor.ConstraintEqualToConstant (72).Active = true;
			scrolledView.WidthAnchor.ConstraintEqualToConstant (450).Active = true;

			if (!string.IsNullOrEmpty (Option.Label)) {
				NSStackView hStack = new NSStackView {
					Orientation = NSUserInterfaceLayoutOrientation.Horizontal,
					Alignment = NSLayoutAttribute.Right
				};
				NSTextField leftLabel = new NSTextField {
					StringValue = Option.Label
				};
				hStack.AddArrangedSubview (leftLabel);
				hStack.AddArrangedSubview (scrolledView);
			} else {
				_optionView.AddArrangedSubview (scrolledView);

			}

			_addButton = new NSButton {
				BezelStyle = NSBezelStyle.TexturedRounded,
				Bordered = false,
				Title = "",
				WantsLayer = true,
				Image = NSImage.GetSystemSymbol ("plus.circle", null),
				ContentTintColor = NSColor.SystemGreenColor,
				TranslatesAutoresizingMaskIntoConstraints = false,
				ToolTip = addToolTip
			};

			_addButton.WidthAnchor.ConstraintEqualToConstant (25).Active = true;
			_addButton.HeightAnchor.ConstraintEqualToConstant (21).Active = true;
			_addButton.Layer.BackgroundColor = NSColor.TextBackground.CGColor;
			_addButton.Layer.CornerRadius = 5;
			_addButton.Activated += OnAddClicked;

			_removeButton = new NSButton {
				BezelStyle = NSBezelStyle.Rounded,
				Bordered = false,
				WantsLayer = true,
				Title = "",
				Image = NSImage.GetSystemSymbol ("xmark.circle", null),
				ContentTintColor = NSColor.SystemPinkColor,
				TranslatesAutoresizingMaskIntoConstraints = false,
				ToolTip = removeToolTip
			};
			_removeButton.WidthAnchor.ConstraintEqualToConstant (25).Active = true;
			_removeButton.HeightAnchor.ConstraintEqualToConstant (21).Active = true;
			_removeButton.Activated += OnRemoveClicked;
			_removeButton.Layer.BackgroundColor = NSColor.TextBackground.CGColor;
			_removeButton.Layer.CornerRadius = 5;

			var h = new NSStackView ();
			h.AddArrangedSubview (_addButton);
			h.AddArrangedSubview (_removeButton);
			_optionView.AddArrangedSubview (h);
		}

		public string ValuePrefix {
			get; set;
		}

		public bool ShowDescriptions {
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

		void OnSelectionChanged (object sender, EventArgs e)
		{
			_removeButton.Enabled = (_tableView.SelectedCell != null);

		}

		void OnStringsListChanged (object sender, EventArgs e)
		{
			RefreshList ();
			//QueueDraw ();
		}

		public void OnValueEdited (object sender, EventArgs e)
		{
			var textField = ((NSTextField) (((NSNotification) sender).Object));
			int row = (int) textField.Tag;
			var newText = textField.StringValue;
			if (newText == null)
				return;

			var newValue = !string.IsNullOrEmpty (ValuePrefix) ? ValuePrefix + textField : newText;

			StringsList[row] = newValue;
		}

		void OnAddClicked (object sender, EventArgs e)
		{
			var defalutString = !string.IsNullOrEmpty (ValuePrefix) ? ValuePrefix + defaultValue : defaultValue;

			try {
				StringsList.Add (defalutString);
				RefreshList ();
			} catch {
				return;
			} finally {
				// _addButton.accessibil.MakeAccessibilityAnnouncement (TranslationCatalog.GetString ("Row added"));
			}
		}

		void OnRemoveClicked (object sender, EventArgs e)
		{
			int selectedRow = (int) _tableView.SelectedRow;

			if (selectedRow < 0 || selectedRow >= StringsList.Count) {
				return;
			}

			try {
				StringsList.RemoveAt (selectedRow);
				RefreshList ();

				if (StringsList.Count <= 0) {
					_removeButton.Enabled = false;
					return;
				}
			} catch {
				return;
			} finally {
				//this.remove.Accessible.MakeAccessibilityAnnouncement (TranslationCatalog.GetString ("Row removed"));
			}
		}

		void TableSelectLastItem ()
		{
			if (StringsList.Count <= 0) {
				return;
			}
			int selectRow = StringsList.Count - 1;
			_tableView.SelectRow (selectRow, false);
			_tableView.ScrollRowToVisible (selectRow);
		}

		void TableSelectFirstItem ()
		{
			if (StringsList.Count <= 0) {
				return;
			}
			_tableView.SelectRow (0, false);
		}

		void RefreshList ()
		{
			_tableView.ReloadData ();
			TableSelectLastItem ();

			_removeButton.Enabled = StringsList.Count > 0;

		}

	}

	class ListSource : NSTableViewSource {
		StringListOptionVSMac Option;

		public ListSource (StringListOptionVSMac option)
		{
			Option = option;
		}

		//public override void SelectionDidChange(NSNotification notification)
		//{
		//    var tableView = ((NSTableView)(notification.Object));
		//    int row = (int)tableView.SelectedRow;
		//    NSTableRowView rowView = tableView.GetRowView(row, false);
		//    rowView.Emphasized = false;
		//}

		//public override void SelectionIsChanging(NSNotification notification)
		//{
		//    var tableView = ((NSTableView)(notification.Object));
		//    int row = (int)tableView.SelectedRow;
		//    NSTableRowView rowView = tableView.GetRowView(row, false);
		//    rowView.Emphasized = false;
		//}

		public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			var view = (NSTableCellView) tableView.MakeView ("cell", this);
			if (view == null) {
				view = new NSTableCellView {
					TextField = new NSTextField {
						Frame = new CoreGraphics.CGRect (0, 6, tableColumn.Width, 18),
						Hidden = false,
						Bordered = false,
						DrawsBackground = false
					},
					Identifier = "cell"
				};

				view.AddSubview (view.TextField);

				view.TextField.EditingEnded += Option.OnValueEdited;
			}

			view.TextField.Tag = row;
			view.TextField.StringValue = Option.StringsList[(int) row];

			return view;
		}

		public override nint GetRowCount (NSTableView tableView)
		{
			return Option.StringsList.Count;
		}

		public override nfloat GetRowHeight (NSTableView tableView, nint row)
		{
			return 30;
		}
	}

}
