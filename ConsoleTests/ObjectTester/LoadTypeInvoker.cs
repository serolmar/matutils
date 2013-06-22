using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTests
{
    class LoadTypeInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            if (command.FunctionArgs.Length < 2)
            {
                throw new Exception("This function takes at least a type and an alias as arguments.");
            }

            var found = false;
            foreach (var assembly in command.Sender.LoadedAssemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                Type foundType = null;
                var typeFullName = string.Empty;
                var genericTypeArgumentsNumber = 0;
                for (int i = 0; i < assemblyTypes.Length; ++i)
                {
                    var currentType = assemblyTypes[i];
                    var splited = currentType.FullName.Split('`');
                    typeFullName = splited[0];
                    if (typeFullName.Equals(command.FunctionArgs[0]))
                    {
                        if (splited.Length > 1)
                        {
                            int.TryParse(splited[1], out genericTypeArgumentsNumber);
                        }

                        foundType = currentType;
                        found = true;
                        i = assemblyTypes.Length;
                    }
                }

                if (found)
                {
                    if (genericTypeArgumentsNumber == 0)
                    {
                        command.Sender.RegisterType(command.FunctionArgs[1], foundType);
                    }
                    else
                    {
                        if (command.FunctionArgs.Length != genericTypeArgumentsNumber + 2)
                        {
                            command.Writer.WriteLine("Type {0} expects {1} generic type arguments.", typeFullName, genericTypeArgumentsNumber);
                        }
                        else
                        {
                            var readedTypes = new List<Type>();
                            for (int i = 1; i < command.FunctionArgs.Length - 1; ++i)
                            {
                                Type registeredType = null;
                                if (!command.Sender.TryGetTypeForAlias(command.FunctionArgs[i], out registeredType))
                                {
                                    command.Writer.WriteLine("Type with alias {0} isn't registered yet.", command.FunctionArgs[i]);
                                }
                                else
                                {
                                    readedTypes.Add(registeredType);
                                }
                            }

                            var constructedType = foundType.MakeGenericType(readedTypes.ToArray());
                            command.Sender.RegisterType(command.FunctionArgs[command.FunctionArgs.Length - 1], constructedType);
                        }
                    }

                    break;
                }
            }

            if (!found)
            {
                command.Writer.WriteLine("Type " + command.FunctionArgs[0] + " does not exist in loaded assemblies.");
            }

            return null;
        }

        public void PrintHelp(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
