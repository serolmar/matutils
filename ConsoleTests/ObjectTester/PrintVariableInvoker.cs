using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ConsoleTests
{
    class PrintVariableInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            if (command.FunctionArgs.Length != 1)
            {
                command.Writer.WriteLine("Expected only one argument as variable name.");
                return null;
            }

            Object variableValue = null;
            if (command.Sender.TryGetVariableValue(command.FunctionArgs[0], out variableValue))
            {
                this.Print(command, variableValue);
            }
            else
            {
                command.Writer.WriteLine("Variable not set.");
            }

            return null;
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prints an object's value to command's writer.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="objToPrint">The object to be printed.</param>
        private void Print(Command command, Object objToPrint)
        {
            command.Writer.Write(command.FunctionArgs[0]);
            command.Writer.Write(" = ");
            Type objType = objToPrint.GetType();
            if (objType.IsArray)
            {
                command.Writer.WriteLine(PrintArray(objToPrint, objType));
            }
            else
            {
                command.Writer.WriteLine(objToPrint.ToString());
            }
        }

        /// <summary>
        /// Prints an array of values.
        /// </summary>
        /// <param name="objToPrint">The array to be printed.</param>
        /// <param name="objType">The type of objects.</param>
        /// <returns>The array string representation.</returns>
        private string PrintArray(Object objToPrint, Type objType)
        {
            StringBuilder resultBuilder = new StringBuilder();
            resultBuilder.Append("[");
            int rank = objType.GetArrayRank();
            if (rank == 1)
            {
                int length = (int)objType.InvokeMember(
                    "Length",
                    BindingFlags.GetProperty,
                    null,
                    objToPrint, null);
                if (length > 0)
                {
                    resultBuilder.Append(objType.InvokeMember(
                        "GetValue",
                        BindingFlags.InvokeMethod,
                        null,
                        objToPrint,
                        new object[] { 0 }));
                    for (int i = 1; i < length; ++i)
                    {
                        resultBuilder.Append(",");
                        resultBuilder.Append(objType.InvokeMember(
                        "GetValue",
                        BindingFlags.InvokeMethod,
                        null,
                        objToPrint,
                        new object[] { new int[] { i } }));
                    }
                }

                resultBuilder.Append("]");
            }
            else if (rank > 1)
            {
                int[] pointers = new int[rank];
                int pointer = 0;
                short state = 0;
                while (state != 3)
                {
                    if (state == 0)
                    {
                        if (pointer >= rank)
                        {
                            resultBuilder.Append(objType.InvokeMember(
                                    "GetValue",
                                    BindingFlags.InvokeMethod,
                                    null,
                                    objToPrint,
                                    new object[] { pointers }));
                            state = 1;
                            --pointer;
                        }
                        else
                        {
                            int dimensionLength = (int)objType.InvokeMember(
                                "GetLength",
                                BindingFlags.InvokeMethod,
                                null,
                                objToPrint,
                                new object[] { pointer });
                            if (pointers[pointer] > dimensionLength)
                            {
                                resultBuilder.Append("]");
                                --pointer;
                            }
                            else
                            {
                                if (pointer < rank - 1)
                                {
                                    resultBuilder.Append("[");
                                }
                                ++pointer;
                            }
                        }
                    }
                    else if (state == 1)
                    {
                        if (pointer == -1)
                        {
                            //resultBuilder.Append("]");
                            state = 3;
                        }
                        else
                        {
                            pointers[pointer]++;
                            int dimensionLength = (int)objType.InvokeMember(
                                "GetLength",
                                BindingFlags.InvokeMethod,
                                null,
                                objToPrint,
                                new object[] { pointer });
                            if (pointers[pointer] >= dimensionLength)
                            {
                                resultBuilder.Append("]");
                                pointers[pointer] = 0;
                                --pointer;
                            }
                            else
                            {
                                state = 2;
                            }
                        }
                    }
                    else if (state == 2)
                    {
                        resultBuilder.Append(",");
                        state = 0;
                    }
                }
            }

            return resultBuilder.ToString();
        }
    }
}
