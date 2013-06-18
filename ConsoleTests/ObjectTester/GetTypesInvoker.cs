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
            foreach (var assembly in command.Sender.LoadedAssemblies)
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

            return null;
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }
    }
}
