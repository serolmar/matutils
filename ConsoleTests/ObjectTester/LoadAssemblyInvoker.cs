using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTests
{
    class LoadAssemblyInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            if (command.FunctionArgs.Length < 1 || string.IsNullOrWhiteSpace(command.FunctionArgs[0]))
            {
                command.Writer.WriteLine("Expecting the name of assembly.");
            }
            else
            {
                var assembly = command.Sender.LoadedAssemblies.FirstOrDefault(a => a.GetName().Name.ToLower().Contains(command.FunctionArgs[0].ToLower()));
                if (assembly == null)
                {
                    assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name.Equals(command.FunctionArgs[0], StringComparison.InvariantCultureIgnoreCase));
                    if (assembly == null)
                    {
                        command.Writer.WriteLine("Assembly {0} not found in application domain. Trying to load from some dll.", command.FunctionArgs[0]);
                        assembly = AppDomain.CurrentDomain.Load(command.FunctionArgs[0]);
                        command.Sender.LoadedAssemblies.Add(assembly);
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
