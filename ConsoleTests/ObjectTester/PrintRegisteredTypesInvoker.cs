using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTests
{
    class PrintRegisteredTypesInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            command.Writer.WriteLine("The registered types are: ");
            foreach (var registeredTypes in command.Sender.GetRegisteredTypes())
            {
                command.Writer.Write(registeredTypes.Key);
                command.Writer.Write(":");
                if (registeredTypes.Value.IsClass)
                {
                    command.Writer.WriteLine("class");
                }
                else
                {
                    command.Writer.WriteLine("built-in/struct");
                }
            }

            return null;
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }
    }
}
