using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
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

    }
}
