using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ConsoleTests
{
    class PrintMethodListInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            if (command.FunctionArgs.Length != 1)
            {
                command.Writer.WriteLine("This function takes a type as an argument.");
                return null;
            }

            Type type = null;
            if (command.Sender.TryGetTypeForAlias(command.FunctionArgs[0], out type))
            {
                List<string> methodList = this.GetMethodList(type);
                if (methodList.Count > 0)
                {
                    command.Writer.WriteLine("These are the available methods in " + type.Name + " type:");
                    for (int i = 0; i < methodList.Count; ++i)
                    {
                        command.Writer.WriteLine(methodList[i]);
                    }
                    command.Writer.WriteLine("---End of method list---");
                }
                else
                {
                    command.Writer.WriteLine("Found no visible method in " + type.Name + " type.");
                }
            }
            else
            {
                command.Writer.WriteLine("Trying to inquire a non registered type.");
                return null;
            }

            return null;
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a list of methods from the specified type.
        /// </summary>
        /// <param name="fromObject">The type.</param>
        /// <returns>The list of methods.</returns>
        private List<string> GetMethodList(Type fromObject)
        {
            List<string> result = new List<string>();
            List<MethodInfo> methods = fromObject.GetMethods().ToList();
            for (int i = 0; i < methods.Count; ++i)
            {
                StringBuilder methodBuilder = new StringBuilder();
                string returnTypeName = methods[i].ReturnType.Name;
                if (returnTypeName == string.Empty)
                {
                    returnTypeName = "void";
                }
                methodBuilder.Append(returnTypeName);
                methodBuilder.Append(" ");
                methodBuilder.Append(methods[i].Name);
                methodBuilder.Append("(");
                ParameterInfo[] parameteres = methods[i].GetParameters();
                if (parameteres.Length > 0)
                {
                    methodBuilder.Append(parameteres[0].ParameterType.Name);
                    methodBuilder.Append(" ");
                    methodBuilder.Append(parameteres[0].Name);

                    for (int j = 1; j < parameteres.Length; j++)
                    {
                        methodBuilder.Append(parameteres[j].ParameterType.Name);
                        methodBuilder.Append(" ");
                        methodBuilder.Append(parameteres[j].Name);
                    }
                }
                methodBuilder.Append(")");
                result.Add(methodBuilder.ToString());
            }
            return result;
        }
    }
}
