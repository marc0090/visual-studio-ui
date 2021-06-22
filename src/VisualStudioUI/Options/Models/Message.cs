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
        
        protected bool Equals(Message other)
        {
            return Text == other.Text && Severity == other.Severity;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Message) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Text.GetHashCode() * 397) ^ (int) Severity;
            }
        }
    }
}