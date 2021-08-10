using System;
using System.IO;
using AppKit;
using Microsoft.VisualStudioUI.Options;
using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.VSMac.Options
{
    public class ProjectFileOptionVSMac : OptionWithLeftLabelVSMac
    {
        private NSStackView _controlView;
        private NSTextField _textField;
        private NSButton _button;

        public ProjectFileOptionVSMac(ProjectFileOption option) : base(option)
        {
        }

        public ProjectFileOption ProjectFileOption => ((ProjectFileOption)Option);

        private string DirProject()
        {
            string DirDebug = System.IO.Directory.GetCurrentDirectory();
            string DirProject = DirDebug;

            for (int counter_slash = 0; counter_slash < 4; counter_slash++)
            {
                DirProject = DirProject.Substring(0, DirProject.LastIndexOf(@"/"));
            }

            return DirProject;
        }

        protected override NSView ControlView
        {
            get
            {
                if (_controlView == null)
                {
                    _controlView = new NSStackView();
                    _controlView.Orientation = NSUserInterfaceLayoutOrientation.Horizontal;
                    _controlView.TranslatesAutoresizingMaskIntoConstraints = false;

                    ViewModelProperty<string> property = ProjectFileOption.Property;

                    _textField = new NSTextField
                    {
                        Font = NSFont.SystemFontOfSize(NSFont.SystemFontSize),
                        StringValue = property.Value ?? string.Empty,
                        TranslatesAutoresizingMaskIntoConstraints = false,
                        Editable = true,
                        Bordered = true,
                        DrawsBackground = true,
                    };

                    _controlView.AddArrangedSubview(_textField);

                    property.PropertyChanged += delegate
                    {
                        var fullPath = property.Value;
                        if (string.IsNullOrEmpty(fullPath))
                        {
                            _textField.StringValue = "";
                            return;
                        }
                        int i = fullPath.LastIndexOf(@"/")+1;
                        _textField.StringValue = fullPath.Substring(i, fullPath.Length - i);
                    };

                    _textField.Changed += delegate { property.Value = _textField.StringValue; };

                    _button = new NSButton
                    {
                        BezelStyle = NSBezelStyle.RoundRect,
                        Bordered = true,
                        LineBreakMode = NSLineBreakMode.TruncatingTail,
                        Title = ProjectFileOption.Name ?? "···"
                    };

                    _button.Activated += (s, e) =>
                    {
                        var openPanel = new NSOpenPanel();
                        string p = (AppDomain.CurrentDomain.BaseDirectory);
                        string path = p;
                        int i = 0;
                        for (int counter_slash = 0; counter_slash < 6; counter_slash++)
                        {
                            i = path.LastIndexOf(@"/");
                            if (i < 0) break;
                            path = path.Substring(0, i);
                        }

                        openPanel.Directory = path;
                        openPanel.CanChooseDirectories = false;
                        openPanel.CanChooseFiles = true;
                        openPanel.AllowedFileTypes =new string[] { "plist"};
                        var response = openPanel.RunModal();
                        if (response == 1 && openPanel.Url != null)
                        {
                            property.Value = openPanel.Filename;
                        }
                       // ProjectFileOption.ButtonClicked(s, e);
                    };

                    _controlView.AddArrangedSubview(_button);
                    _controlView.HeightAnchor.ConstraintEqualToConstant(21f).Active = true;

                    _textField.WidthAnchor.ConstraintEqualToConstant(196f).Active = true;
                    _textField.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
                    _textField.LeadingAnchor.ConstraintEqualToAnchor(_controlView.LeadingAnchor).Active = true;
                    _textField.TopAnchor.ConstraintEqualToAnchor(_controlView.TopAnchor).Active = true;
                    _button.HeightAnchor.ConstraintEqualToConstant(21).Active = true;
                    _button.WidthAnchor.ConstraintEqualToConstant(24).Active = true;
                    _button.TrailingAnchor.ConstraintEqualToAnchor(_controlView.TrailingAnchor).Active = true;
                }
                return _controlView;
            }
        }

        public override void OnEnableChanged(bool enabled)
        {
            base.OnEnableChanged(enabled);

            _textField.Enabled = enabled;

            _button.Enabled = enabled;
        }
    }
}
