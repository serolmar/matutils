using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ConsoleTests
{
    class LoadMsCoreTypeInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            if (command.FunctionArgs.Length < 1)
            {
                command.Writer.WriteLine("Missing type name.");
                return null;
            }
            if (command.FunctionArgs.Length < 2)
            {
                command.Writer.WriteLine("Missing type alias.");
                return null;
            }

            Assembly assembly = command.Sender.LoadedAssemblies.FirstOrDefault(a => a.GetName().Name.Equals("mscorlib", StringComparison.InvariantCultureIgnoreCase));
            if (assembly == null)
            {
                command.Writer.WriteLine("Core lib was not properly loaded.");
            }
            else
            {
                assembly = Assembly.Load("mscorlib");
                try
                {
                    var q = from t in assembly.GetTypes()
                            where t.FullName.Equals(command.FunctionArgs[0])
                            select t;
                    if (q.Count() != 0)
                    {
                        command.Sender.RegisterType(command.FunctionArgs[1], q.First());
                    }
                    else
                    {
                        throw new Exception("Inexistent type.");
                    }
                }
                catch (Exception exception)
                {
                    throw exception;
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
