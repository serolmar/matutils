using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleTests
{
    class GetCommandsFromFileInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            FileStream file = null;
            StreamReader reader = null;
            if (command.FunctionArgs.Length < 1)
            {
                command.Writer.WriteLine("Missing command file name.");
            }
            try
            {
                file = new FileStream(command.FunctionArgs[0], FileMode.Open);
                reader = new StreamReader(file);
                string line;
                int wellProcessedCommands = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        command.Sender.CallCommand(line, reader, command.Writer);
                        ++wellProcessedCommands;
                    }
                    catch (Exception except)
                    {
                        command.Writer.WriteLine("Error: " + except.Message);
                    }
                }

                command.Writer.WriteLine(wellProcessedCommands + " commands processed successfuly.");
                reader.Close();
                file.Close();
            }
            catch (FileNotFoundException)
            {
                command.Writer.WriteLine("Can't find specified file in program directory.");
            }
            catch (Exception except)
            {
                command.Writer.WriteLine("Error: " + except.Message);
            }
            if (reader != null)
            {
                reader.Close();
                file.Close();
            }

            return null;
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }
    }
}
