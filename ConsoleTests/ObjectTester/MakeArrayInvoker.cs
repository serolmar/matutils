using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ConsoleTests
{
    class MakeArrayInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            if (command.FunctionArgs.Length < 1)
            {
                command.Writer.WriteLine("Missing type.");
                return null;
            }
            if (command.FunctionArgs.Length < 2)
            {
                command.Writer.WriteLine("Missing rank.");
                return null;
            }

            Type initialType = null;
            if (!command.Sender.TryGetTypeForAlias(command.FunctionArgs[0], out initialType))
            {
                command.Writer.WriteLine("Type {0} is not registered.", command.FunctionArgs[0]);
                return null;
            }

            Object rank = command.Sender.GetVariableValue(command.FunctionArgs[1]);
            if (rank == null)
            {
                command.Writer.WriteLine("Invalid rank. Expected an integer value.");
                return null;
            }
            if (((Int32)rank) <= 0)
            {
                command.Writer.WriteLine("Rank is too small.");
                return null;
            }

            var arrayType = initialType.MakeArrayType((Int32)rank);
            Type[] argumentTypes = new Type[(Int32)rank];
            for (int i = 0; i < argumentTypes.Length; ++i)
            {
                argumentTypes[i] = typeof(int);
            }
            List<Object> args = new List<object>();
            for (int i = 2; i < command.FunctionArgs.Length; ++i)
            {
                args.Add(command.Sender.GetVariableValue(command.FunctionArgs[i]));
            }

            return this.CreateInstance(arrayType, argumentTypes, args.ToArray());
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates an instance and attaches it to a variable name.
        /// </summary>
        /// <param name="variableName">The variable name.</param>
        private Object CreateInstance(Type typeToCreate, Type[] types, Object[] arguments)
        {
            Type myType = typeToCreate;
            ConstructorInfo constructInstanciator = myType.GetConstructor(types);
            if (constructInstanciator != null)
            {
                return constructInstanciator.Invoke(arguments);
            }
            else
            {
                return null;
            }
        }
    }
}
