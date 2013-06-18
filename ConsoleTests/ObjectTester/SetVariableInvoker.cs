using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTests
{
    class SetVariableInvoker : IInvoker
    {
        public object Invoke(Command command)
        {
            Type objType = null;
            if (command.Sender.TryGetTypeForAlias(command.FunctionArgs[0], out objType))
            {
                Object obj = this.ReadVariableFromString(objType, command.FunctionArgs[1]);
                if (obj == null)
                {
                    command.Writer.WriteLine("Value isn't of Built in type.");
                }
                else
                {
                    command.Sender.SetVariableValue(command.FunctionArgs[2], obj);
                }

                return obj;
            }
            else
            {
                command.Writer.WriteLine("The type " + command.FunctionArgs[0] + " is not registered.");
                return null;
            }
        }

        public string GetHelp()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a varible's value from its string representation.
        /// </summary>
        /// <param name="type">The variable's type.</param>
        /// <param name="valueToRead">The value to be readed.</param>
        /// <returns>The variable vaule.</returns>
        private Object ReadVariableFromString(Type type, string valueToRead)
        {
            Object obj = null;
            switch (type.Name)
            {
                case "int":
                case "Int32":
                    try
                    {
                        obj = Convert.ToInt32(valueToRead);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
                case "Int16":
                    try
                    {
                        obj = Convert.ToInt16(valueToRead);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
                case "long":
                case "Int64":
                    try
                    {
                        obj = Convert.ToInt64(valueToRead);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
                case "ulong":
                    try
                    {
                        obj = Convert.ToUInt64(valueToRead);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
                case "double":
                case "Double":
                case "float":
                    try
                    {
                        obj = Convert.ToDouble(valueToRead);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
                case "decimal":
                case "Decimal":
                    try
                    {
                        obj = Convert.ToDecimal(valueToRead);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
                case "bool":
                case "Boolean":
                    try
                    {
                        obj = Convert.ToBoolean(valueToRead);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
                case "string":
                case "String":
                    return valueToRead;
            }

            return obj;
        }
    }
}
