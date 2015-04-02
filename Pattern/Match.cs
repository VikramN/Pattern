/* The MIT License (MIT)

Copyright (c) Vikram (https://github.com/VikramN)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pattern
{
    /// <summary>
    /// Match an object to its type.
    /// </summary>
    public class Match
    {
        When[] _cases;

        private Match(When[] cases)
        {
            _cases = cases;
        }

        /// <summary>        
        /// The first macth will be executed (not the best)
        /// </summary>
        /// <param name="cases">Provide an array of actions to be executed. Type of action will be used for matching</param>        
        public static Match Switch(params When[] cases)
        {
            return new Match(cases);
        }

        /// <summary>
        /// Match the Switch cases against an instance
        /// </summary>        
        public void Against(object o)
        {
            // To break out
            var matched = false;

            // First match (not best)
            foreach (var item in _cases)
            {
                // Given type
                var instanceType = o.GetType();
                var instance = o;

                // Looper
                var t = instanceType;
                while (!matched && t != null)
                {
                    // Could be Same Type, Base Class, Interface
                    if (item.ActionType.IsAssignableFrom(t))
                    {
                        dynamic del = item.Action;
                        var exec = true;

                        if(item.Condition != null)
                        {
                            exec = item.Condition.DynamicInvoke(instance);
                        }

                        if(exec)
                        {
                            del.DynamicInvoke(instance);
                            matched = true;
                            break;
                        } 
                    }
                    
                    // Check against base class next
                    t = t.BaseType;
                }

                // Yup.. Done
                if (matched)
                {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Wrapper around Action    
    /// </summary>
    public class When
    {
        /// <summary>
        /// Use When.Its<> to create an action
        /// </summary>
        private When() { } 

        public dynamic Action { get; private set; }

        public Type ActionType { get; private set; }

        public dynamic Condition { get; private set; }

        /// <summary>
        /// Make a conditional match, based on the condition Func => bool
        /// </summary>
        public When And<T>(Func<T, bool> condition)
        {
            this.Condition = condition;
            return this;
        }

        /// <summary>
        /// Create a "case" to be executed
        /// </summary>
        /// <typeparam name="T">Type to match</typeparam>
        /// <param name="action">Action to execute</param>        
        public static When Its<T>(Action<T> action)
        {
            return new When() 
            {
                Action = action,
                ActionType = action.GetType().GenericTypeArguments[0]
            };
        }
    }
}
