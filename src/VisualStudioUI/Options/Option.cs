﻿using Microsoft.VisualStudioUI.Options.Models;

namespace Microsoft.VisualStudioUI.Options
{
	public abstract class Option
	{
		public OptionPlatform Platform { get; protected set; }

		public string? Name { get; set; } = null;

		public string? Label { get; set; } = null;

		public string? Hint { get; set; } = null;

		public ViewModelProperty<Message?>? ValidationMessage { get; set;  } = null;
	}
}
