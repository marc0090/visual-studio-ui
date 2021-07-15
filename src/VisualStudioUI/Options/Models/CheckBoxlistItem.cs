//
// SwitchableGroupOption.cs
//
// Author:
//       marcbookpro19 <v-marcguo@microsoft.com>
//
// Copyright (c) 2021 
//

namespace Microsoft.VisualStudioUI.Options.Models
{
    public class CheckBoxlistItem
    {
        public string Title { get; }
        public bool Selected { get; set; }

        public CheckBoxlistItem(string title, bool selected)
        {
            Title = title;
            Selected = selected;
        }
    }
}
