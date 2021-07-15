using System;
namespace Microsoft.VisualStudioUI.Options.Models
{
    public class MacroMenuItem
    {
        public string? Label { get; }
        public string? MacroName { get; }

        public MacroMenuItem(string label, string name)
        {
            Label = label;
            MacroName = name;
        }
    }
}
