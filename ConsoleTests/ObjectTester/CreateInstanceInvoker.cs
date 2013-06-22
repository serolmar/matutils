using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ConsoleTests
{
    class CreateInstanceInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            if (command.RetValueVar == string.Empty)
            {
                command.Writer.WriteLine("No variable will take the objects instance.");
                return null;
            }
            if (command.FunctionArgs.Length == 0)
            {
                command.Writer.WriteLine("Missing instance type.");
                return null;
            }
            Object retObject = null;
            Type[] types = GetTypesFromArgList(this.GetVariableValues(command.FunctionArgs, command.Sender));

            Type myType = null;
            if (command.Sender.TryGetTypeForAlias(command.FunctionArgs[0], out myType))
            {
                if (!myType.IsClass)
                {
                    command.Writer.WriteLine("To use builtin types, use setvariable instead.");
                    return null;
                }
                if (myType.ContainsGenericParameters)
                {
                    command.Writer.WriteLine("Error, trying to instantiate a non closed generic type.");
                    return null;
                }
                bool foudMatchConstructor = false;
                ConstructorInfo[] constrctsInfo = myType.GetConstructors();
                List<Object> realArguments = new List<Object>();
                foreach (ConstructorInfo cnstInfo in constrctsInfo)
                {
                    ParameterInfo[] parameters = cnstInfo.GetParameters();
                    if (parameters.Length + 1 == types.Length)
                    {
                        foudMatchConstructor = true;
                        for (int i = 0; i < parameters.Length; ++i)
                        {
                            if (!parameters[i].ParameterType.IsAssignableFrom(types[i + 1]))
                            {
                                foudMatchConstructor = false;
                                break;
                            }
                        }

                        if (foudMatchConstructor)
                        {
                            List<Type> temporaryTypes = types.ToList();
                            temporaryTypes.RemoveAt(0);
                            List<Object> temporary = this.GetVariableValues(command.FunctionArgs, command.Sender).ToList();
                            temporary.RemoveAt(0);
                            retObject = this.CreateInstance(myType, temporaryTypes.ToArray(), temporary.ToArray());
                            return retObject;
                        }
                    }
                }
                if (!foudMatchConstructor)
                {
                    command.Writer.WriteLine("No overload constructor has such argument signature.");
                    return null;
                }
            }
            else
            {
                command.Writer.WriteLine("Type " + command.FunctionArgs[0] + " do not exist in this assembly.");
            }

            return retObject;
        }

        public void PrintHelp(Command command)
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

        /// <summary>
        /// Gets the stored variables.
        /// </summary>
        /// <param name="variableNames">The variables names.</param>
        /// <param name="sender">The sender.</param>
        /// <returns>The variables values.</returns>
        private Object[] GetVariableValues(string[] variableNames, ObjectTester sender)
        {
            List<Object> listOfObjects = new List<Object>();
            foreach (string variableName in variableNames)
            {
                listOfObjects.Add(sender.GetVariableValue(variableName));
            }

            return listOfObjects.ToArray();
        }

        /// <summary>
        /// Gets all types from an object's array.
        /// </summary>
        /// <param name="arglist">The object's array.</param>
        /// <returns>The array of types.</returns>
        private Type[] GetTypesFromArgList(Object[] arglist)
        {
            if (arglist == null)
            {
                return Type.EmptyTypes;
            }
            if (arglist.Length == 0)
            {
                return Type.EmptyTypes;
            }
            Type[] result = new Type[arglist.Length];
            for (int i = 0; i < arglist.Length; ++i)
            {
                Type argType = arglist[i] == null ? typeof(Object) : arglist[i].GetType();
                result[i] = argType;
            }
            return result;
        }
    }
}
