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
                    if (command.Sender.TryGetVariableValue(command.FunctionArgs[i], out variable))
                    {
                        argValues.Add(variable);
                        argTypes.Add(variable.GetType());
                    }
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

        public string GetHelp()
        {
            throw new NotImplementedException();
        }
    }
}
