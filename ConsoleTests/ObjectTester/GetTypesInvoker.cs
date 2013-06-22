using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTests
{
    class GetTypesInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            Dictionary<string, bool> exists = new Dictionary<string, bool>();
            var firstArgument = string.Empty;
            if (command.FunctionArgs.Length > 0)
            {
                firstArgument = command.FunctionArgs[0];
            }

            foreach (var assembly in command.Sender.LoadedAssemblies)
            {
                if (string.IsNullOrEmpty(firstArgument) ||
                    assembly.GetName().Name.Equals(firstArgument, StringComparison.CurrentCultureIgnoreCase))
                {
                    foreach (Type t in assembly.GetTypes())
                    {
                        if (!exists.ContainsKey(t.FullName))
                        {
                            command.Writer.WriteLine(t.FullName);
                            exists.Add(t.FullName, true);
                        }
                    }
                }
            }

            return null;
        }

        public void PrintHelp(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
