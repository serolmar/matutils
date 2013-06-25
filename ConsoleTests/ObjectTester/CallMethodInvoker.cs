namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    class CallMethodInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            if (command.FunctionArgs.Length < 2)
            {
                command.Writer.WriteLine("Function takes object and member name.");
                return null;
            }

            Object thInstance = null;
            if (command.Sender.TryGetVariableValue(command.FunctionArgs[1], out thInstance))
            {
                Type objectType = thInstance.GetType();
                List<Object> argValues = new List<Object>();
                List<Type> argTypes = new List<Type>();
                for (int i = 2; i < command.FunctionArgs.Length; ++i)
                {
                    Object variable = null;
                    if (!command.Sender.TryGetVariableValue(command.FunctionArgs[i], out variable))
                    {
                        variable = this.ReadVariableFromString(command.FunctionArgs[i]);
                    }

                    argValues.Add(variable);
                    argTypes.Add(variable.GetType());
                }

                MethodInfo methodInfo = objectType.GetMethod(command.FunctionArgs[0], argTypes.ToArray());
                if (methodInfo == null)
                {
                    command.Writer.WriteLine("No method exists in object type with specified signature.");
                    return null;
                }

                return methodInfo.Invoke(thInstance, argValues.ToArray());
            }
            else
            {
                command.Writer.WriteLine("Can't call methods on null variables.");
                return null;
            }
        }

        public void PrintHelp(Command command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a variable from the string value.
        /// </summary>
        /// <param name="strValue">The string value.</param>
        /// <returns>The variable value.</returns>
        private object ReadVariableFromString(string strValue)
        {
            object result = null;
            if (!string.IsNullOrWhiteSpace(strValue))
            {
                if (strValue.Trim().ToLower() != "null")
                {
                    var integerValue = 0;
                    if (int.TryParse(strValue, out integerValue))
                    {
                        result = integerValue;
                    }
                    else
                    {
                        var doubleValue = 0.0;
                        if (double.TryParse(strValue, out doubleValue))
                        {
                            result = doubleValue;
                        }
                        else
                        {
                            var boolValue = false;
                            if (bool.TryParse(strValue, out boolValue))
                            {
                                result = boolValue;
                            }
                            else
                            {
                                result = strValue;
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
