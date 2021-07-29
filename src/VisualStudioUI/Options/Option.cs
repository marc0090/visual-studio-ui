using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
    public enum OptionIndent
    {
        Normal = 0,
        SubOption = 30,
        SubSubOption = 60,
    }

    public abstract class Option
    {
        /// <summary>
        /// Platform holds the platform specific implementation of the option. It's
        /// typically not used by the client code.
        /// </summary>
        public OptionPlatform Platform { get; protected set; }

        /// <summary>
        /// Name is not shown to the user. We may remove this. 
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// Label is shown on the left. Sometimes it's set just on the first option (like a checkbox) in
        /// a sequence and meant to apply to all of them. 
        /// </summary>
        public string? Label { get; set; } = null;

        /// <summary>
        /// Description is optional extra text - often a sentence - typically shown below the option if
        /// present
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// Hint, when present, will cause a help button to show, pops up the hint message.
        /// </summary>
        public string? Hint { get; set; } = null;

        /// <summary>
        /// The ValidationMessage is used to show warnings/errors. If present, it's typically
        /// shown instead of the hint, taking its place in the UI.
        /// </summary>
        public ViewModelProperty<Message?>? ValidationMessage { get; set; } = null;

        /// <summary>
        /// When set, this option will only be shown if the specified ToggleButtonOption is
        /// toggled on.
        /// </summary>
        public ToggleButtonOption? VisibilityDependsOn { get; set; }

        /// <summary>
        /// When set, this option will only be shown if the specified property is true.
        /// If visibility just depends on a button state, then using VisibilityDependsOn
        /// instead is preferred. If VisibilityDependsOn and Visible are both set then
        /// both must be valid for the option to show.
        /// </summary>
        public ViewModelProperty<bool>? Visible { get; set; }

        /// <summary>
        /// When set, this option will only be enabled/disabled when the specified ToggleButtonOption is
        /// toggled on.
        /// </summary>
        public ToggleButtonOption? DisablebilityDependsOn { get; set; }

        /// <summary>
        /// When set, this option will only be enabled/disabled if the specified property is true.
        /// If visibility just depends on a button state, then using DisablebilityDependsOn
        /// instead is preferred. If DisablebilityDependsOn and Disable are both set then
        /// both must be valid for the option to show.
        /// </summary>
        public ViewModelProperty<bool>? Enable { get; set; }

        /// <summary>
        /// Option have three diffrent level of indent:OptionIndent.Normal,OptionIndent.SubOption,OptionIndent.SubSubOption
        /// </summary>
        public OptionIndent Indent { get; set; } = OptionIndent.Normal;

        /// <summary>
        /// For options that allow a Label (most of them), the label appears on the left.
        /// Even when the label isn't present, these options are normally indented so they
        /// are all left aligned with each other. Set this to false to not indent, which
        /// can be useful for options with long text and no need to align with other labeled
        /// options on the card. In practice, it's often used for checkboxes with long text after.
        /// </summary>
        public bool AllowSpaceForLabel { get; set; } = true;
    }
}
