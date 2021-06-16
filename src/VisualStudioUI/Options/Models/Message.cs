//
// ViewModelProperty.cs
//
// Author:
//       Greg Munn <greg.munn@xamarin.com>
//
// Copyright (c) 2015 Xamarin Inc
//

using System;
using System.ComponentModel;

namespace Microsoft.VisualStudioUI.Options.Models
{
    public class Message
    {
        public string Text { get; }
        public MessageSeverity Severity { get; }

        public Message(string text, MessageSeverity severity)
        {
            Text = text;
            Severity = severity;
        }
    }
}