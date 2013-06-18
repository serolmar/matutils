
namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// A class that represents a stream command.
    /// </summary>
    class Command
    {
        /// <summary>
        /// Gets and sets the command sender.
        /// </summary>
        public ObjectTester Sender { get; set; }

        /// <summary>
        /// Gets and sets the function name.
        /// </summary>
        public string Function
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the function return value string representation.
        /// </summary>
        public string RetValueVar
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the function arguments string representation.
        /// </summary>
        public string[] FunctionArgs
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the current writer.
        /// </summary>
        public TextWriter Writer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the current reader.
        /// </summary>
        public TextReader Reader
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the function arguments as typed objects.
        /// </summary>
        /// <returns>The typed objects array.</returns>
        public Object[] GetTypedFunctionArgs()
        {
            List<Object> result = new List<Object>();
            for (int position = 0; position < FunctionArgs.Length; ++position)
            {
                if (FunctionArgs[position].Contains(".") || FunctionArgs[position].Contains("e") || FunctionArgs[position].Contains("E"))
                {
                    try
                    {
                        decimal value = Convert.ToDecimal(FunctionArgs[position]);
                        result.Add(value);
                    }
                    catch (Exception)
                    {
                        result.Add(FunctionArgs[position]);
                    }
                }
                else
                {
                    try
                    {
                        long value = Convert.ToInt64(FunctionArgs[position]);
                        result.Add(value);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            bool value = Convert.ToBoolean(FunctionArgs[position]);
                            result.Add(value);
                        }
                        catch (Exception)
                        {
                            result.Add(FunctionArgs[position]);
                        }
                    }
                }
            }
            return result.ToArray();
        }
    }
}
