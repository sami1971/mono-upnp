﻿//
// Argument.cs
//
// Author:
//   Scott Peterson <lunchtimemama@gmail.com>
//
// Copyright (C) 2008 S&S Black Ltd.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Reflection;
using System.Xml;

namespace Mono.Upnp.Server
{
    public class Argument
	{
        private readonly Action action;
        private readonly bool is_return_value;
        private readonly string name;
        private readonly StateVariable related_state_variable;
        private readonly ArgumentDirection direction;

        protected internal Argument (Action action, string name, bool isReturnValue, StateVariable relatedStateVariable, ArgumentDirection direction)
        {
            if (action == null) {
                throw new ArgumentNullException ("action");
            }
            if (name == null) {
                throw new ArgumentNullException ("name");
            }
            if (relatedStateVariable == null) {
                throw new ArgumentNullException ("relatedStateVariable");
            }
            if (isReturnValue && direction == ArgumentDirection.In) {
                throw new ArgumentException ("If the argument is a return value, it must have an 'Out' direction.");
            }

            this.name = name;
            this.is_return_value = isReturnValue;
            this.related_state_variable = relatedStateVariable;
            this.direction = direction;
        }

        private object value;
        public object Value {
            get { return value; }
            set { this.value = value; }
        }

        public string Name {
            get { return name; }
        }

        public ArgumentDirection Direction {
            get { return direction; }
        }

        public StateVariable RelatedStateVariable {
            get { return related_state_variable; }
        }

        private Type type;

        protected internal void Serialize (XmlWriter writer)
        {
            writer.WriteStartElement ("argument");
            writer.WriteStartElement ("name");
            writer.WriteValue (name);
            writer.WriteEndElement ();
            writer.WriteStartElement ("direction");
            writer.WriteValue (direction == ArgumentDirection.In ? "in" : "out");
            writer.WriteEndElement ();
            if (is_return_value) {
                writer.WriteStartElement ("retval");
                writer.WriteEndElement ();
            }
            writer.WriteStartElement ("relatedStateVariable");
            writer.WriteValue (related_state_variable.Name);
            writer.WriteEndElement ();
            writer.WriteEndElement ();
        }
	}
}
