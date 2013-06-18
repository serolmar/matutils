using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTests
{
    class PrintHelpInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            command.Writer.WriteLine("Usage: command [ -r <return>] [-a <arguments>]");
            command.Writer.WriteLine("Available commands are: ");
            foreach (var kvp in command.Sender.GetRegisteredCommands())
            {
                command.Writer.WriteLine(kvp.Key);
            }

            command.Writer.WriteLine("No further help is provided for time being.");
            return null;
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }
    }
}
