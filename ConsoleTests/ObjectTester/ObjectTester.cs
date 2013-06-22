namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// A stream manager that can be used to instantiate objects and call it's methods using reflexion.
    /// </summary>
    class ObjectTester
    {
        private Dictionary<string, Object> instances = new Dictionary<string, Object>();
        private Dictionary<string, Type> registeredTypes = new Dictionary<string, Type>();
        private Dictionary<string, IInvoker> customFunctions = new Dictionary<string, IInvoker>();
        private List<Assembly> loadedAssemblies = new List<Assembly>();

        public ObjectTester()
        {
            this.InitializeAssemblies();
            InitializeBuiltinTypes();
            InitializeCostumFuncs();
        }

        /// <summary>
        /// Gets the loaded assemblies.
        /// </summary>
        public List<Assembly> LoadedAssemblies
        {
            get
            {
                return this.loadedAssemblies;
            }
        }

        public void Run(TextReader reader, TextWriter writer)
        {
            writer.Write("Command> ");
            string commandLine = reader.ReadLine();
            while (commandLine != "exit")
            {
                try
                {
                    this.ProcessCommand(commandLine, reader, writer);
                }
                catch (Exception except)
                {
                    writer.WriteLine("There was an error. " + except.Message);
                }

                writer.Write("Command> ");
                commandLine = reader.ReadLine();
            }
        }

        /// <summary>
        /// Executes some command.
        /// </summary>
        /// <param name="commandString">The command string.</param>
        /// <param name="reader">The command's reader.</param>
        /// <param name="writer">The command's writer.</param>
        public void CallCommand(string commandString, TextReader reader, TextWriter writer)
        {
            ProcessCommand(commandString, reader, writer);
        }

        /// <summary>
        /// Adds an instantiated object to some variable.
        /// </summary>
        /// <param name="varName">The variable name.</param>
        /// <param name="objToAdd">The object to be added.</param>
        public void AddInstantiatedObject(string varName, Object objToAdd)
        {
            if (this.instances.ContainsKey(varName))
            {
                this.instances[varName] = objToAdd;
            }
            else
            {
                this.instances.Add(varName, objToAdd);
            }
        }

        /// <summary>
        /// Tries to get a type by its alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <param name="type">The type.</param>
        /// <returns>True if type exists and false otherwise.</returns>
        public bool TryGetTypeForAlias(string alias, out Type type)
        {
            return this.registeredTypes.TryGetValue(alias, out type);
        }

        /// <summary>
        /// Checks if type alias is defined.
        /// </summary>
        /// <param name="alias">The type alias.</param>
        /// <returns>True if type alias is defined and false otherwise.</returns>
        public bool ContainsTypeAlias(string alias)
        {
            return this.registeredTypes.ContainsKey(alias);
        }

        /// <summary>
        /// Registers a type for an alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <param name="type">The type.</param>
        public void RegisterType(string alias, Type type)
        {
            if (this.registeredTypes.ContainsKey(alias))
            {
                this.registeredTypes[alias] = type;
            }
            else
            {
                this.registeredTypes.Add(alias, type);
            }
        }

        /// <summary>
        /// Gets all the registered types.
        /// </summary>
        /// <returns>The registered types.</returns>
        public IEnumerable<KeyValuePair<string, Type>> GetRegisteredTypes()
        {
            return this.registeredTypes;
        }

        /// <summary>
        /// Gets a variable's stored value.
        /// </summary>
        /// <param name="variableName">The variable name.</param>
        /// <returns>The variable's value.</returns>
        public Object GetVariableValue(string variableName)
        {
            Object result = null;
            if (this.instances.ContainsKey(variableName))
            {
                result = instances[variableName];
            }
            else
            {
                return ReadVariableFromString(variableName);
            }

            return result;
        }

        /// <summary>
        /// Gets the registered commands.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, IInvoker>> GetRegisteredCommands()
        {
            return this.customFunctions;
        }

        /// <summary>
        /// Try get the variable value.
        /// </summary>
        /// <param name="variable">The variable name.</param>
        /// <param name="value">The ouput variable value.</param>
        /// <returns>True</returns>
        public bool TryGetVariableValue(string variable, out Object value)
        {
            return this.instances.TryGetValue(variable, out value);
        }

        /// <summary>
        /// Sets the variable value.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The value.</param>
        public void SetVariableValue(string variable, Object value)
        {
            if (this.instances.ContainsKey(variable))
            {
                this.instances[variable] = value;
            }
            else
            {
                this.instances.Add(variable, value);
            }
        }

        /// <summary>
        /// Initializes the loaded assemblies.
        /// </summary>
        private void InitializeAssemblies()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(List<int>));
            assembly = Assembly.Load("mscorlib");
            this.loadedAssemblies.Add(assembly);
        }

        /// <summary>
        /// Initializes all builtin types.
        /// </summary>
        private void InitializeBuiltinTypes()
        {
           this.registeredTypes.Add("int", typeof(int));
           this.registeredTypes.Add("Int16", typeof(Int16));
           this.registeredTypes.Add("Int32", typeof(Int32));
           this.registeredTypes.Add("Int64", typeof(Int64));
           this.registeredTypes.Add("long", typeof(long));
           this.registeredTypes.Add("ulong", typeof(ulong));
           this.registeredTypes.Add("double", typeof(double));
           this.registeredTypes.Add("float", typeof(float));
           this.registeredTypes.Add("decimal", typeof(decimal));
           this.registeredTypes.Add("Double", typeof(Double));
           this.registeredTypes.Add("Decimal", typeof(Decimal));
           this.registeredTypes.Add("bool", typeof(bool));
           this.registeredTypes.Add("Boolean", typeof(Boolean));
           this.registeredTypes.Add("string", typeof(string));
           this.registeredTypes.Add("String", typeof(String));
        }

        /// <summary>
        /// Initializes some initial custom functions.
        /// </summary>
        private void InitializeCostumFuncs()
        {
           this.customFunctions.Add("help", new PrintHelpInvoker());
           this.customFunctions.Add("methods", new PrintMethodListInvoker());
           this.customFunctions.Add("valueof", new PrintVariableInvoker());
           this.customFunctions.Add("instantiate", new CreateInstanceInvoker());
           this.customFunctions.Add("registeredtypes", new PrintRegisteredTypesInvoker());
           this.customFunctions.Add("setvariable", new SetVariableInvoker());
           this.customFunctions.Add("callmethod", new CallMethodInvoker());
           this.customFunctions.Add("interpretfile", new GetCommandsFromFileInvoker());
           this.customFunctions.Add("loadtype", new LoadTypeInvoker());
           this.customFunctions.Add("gettypes", new GetTypesInvoker());
           this.customFunctions.Add("loadcoretype", new LoadMsCoreTypeInvoker());
           this.customFunctions.Add("makearray", new MakeArrayInvoker());
           this.customFunctions.Add("loadassembly", new LoadAssemblyInvoker());
        }
        
        private void Caller(Command command, TextReader reader, TextWriter writer)
        {
            command.Reader = reader;
            command.Writer = writer;
            if (command.RetValueVar != string.Empty)
            {
                if (customFunctions.ContainsKey(command.Function))
                {
                    Object retVal = customFunctions[command.Function].Invoke(command);
                    if (retVal == null)
                    {
                    }
                    else
                    {
                        if (!instances.ContainsKey(command.RetValueVar))
                        {
                            instances.Add(command.RetValueVar, retVal);
                        }
                        else
                        {
                            instances[command.RetValueVar] = retVal;
                        }
                    }
                }
                else
                {
                    command.Writer.WriteLine("That's not an implemented function and object methods check is not implemented.");
                    //TODO: Check and invoke objects methods
                }
            }
            else
            {
                if (customFunctions.ContainsKey(command.Function))
                {
                    customFunctions[command.Function].Invoke(command);
                }
                else
                {
                    command.Writer.WriteLine("That's not an implemented function and object methods check is not implemented.");
                    //TODO: Check and invoke objects methods
                }
            }
        }

        private void ProcessCommand(string commandString, TextReader reader, TextWriter writer)
        {
            Command lineCommand = this.GetCommandLine(commandString);
            this.Caller(lineCommand, reader, writer);
        }

        /// <summary>
        /// Gets the command from command line.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <returns>The readed command.</returns>
        private Command GetCommandLine(string commandLine)
        {
            Command result = new Command() { Sender = this};
            string[] commands = commandLine.Trim().Split();
            if (commands.Length == 0)
            {
                throw new FormatException("Empty command.");
            }
            result.Function = commands[0];
            result.RetValueVar = "";
            result.FunctionArgs = null;
            List<string> readedArgs = new List<string>();
            int count = 1;
            int state = 0;
            while (count < commands.Length)
            {
                switch (state)
                {
                    case 0:
                        if (commands[count] == "-r")
                        {
                            state = 1;
                            ++count;
                        }
                        else if (commands[count] == "-a")
                        {
                            state = 2;
                            ++count;
                        }
                        else
                        {
                            state = 2;
                        }
                        break;
                    case 1:
                        if (commands[count] == "-a" || commands[count] == "-r")
                        {
                            throw new FormatException("Unnexpected " + commands[count] + " at position " + (count + 1) + ".");
                        }
                        result.RetValueVar = commands[count];
                        ++count;
                        state = 3;
                        break;
                    case 2:
                        if (commands[count] == "-a" || commands[count] == "-r")
                        {
                            throw new FormatException("Unnexpected " + commands[count] + " at position " + (count + 1) + ".");
                        }
                        readedArgs.Add(commands[count]);
                        state = 4;
                        ++count;
                        break;
                    case 3:
                        if (commands[count] == "-r")
                        {
                            throw new FormatException("Unnexpected -r at position " + (count + 1) + ".");
                        }
                        if (commands[count] == "-a")
                        {
                            state = 5;
                        }
                        else
                        {
                            readedArgs.Add(commands[count]);
                            state = 5;
                        }
                        ++count;
                        break;
                    case 4:
                        if (commands[count] == "-a")
                        {
                            throw new FormatException("Unnexpected -a at position " + (count + 1) + ".");
                        }
                        if (commands[count] == "-r")
                        {
                            state = 6;
                        }
                        else
                        {
                            readedArgs.Add(commands[count]);
                        }
                        ++count;
                        break;
                    case 5:
                        if (commands[count] == "-a")
                        {
                            throw new FormatException("Unnexpected -a at position " + (count + 1) + ".");
                        }
                        readedArgs.Add(commands[count]);
                        ++count;
                        break;
                    case 6:
                        result.RetValueVar = commands[count];
                        ++count;
                        state = 7;
                        break;
                    default:
                        throw new FormatException("Unexpected " + commands[count] + " at position " + (count + 1) + ".");
                }
            }
            result.FunctionArgs = readedArgs.ToArray();
            return result;
        }

        /// <summary>
        /// Reads a varible's value from its string representation.
        /// </summary>
        /// <param name="valueToRead">The value to be readed.</param>
        /// <returns>The variable vaule.</returns>
        private Object ReadVariableFromString(string valueToRead)
        {
            if (string.IsNullOrEmpty(valueToRead))
            {
                return null;
            }
            else
            {
                var integerValue = 0;
                if (int.TryParse(valueToRead, out integerValue))
                {
                    return integerValue;
                }
                else
                {
                    var doubleValue = 0.0;
                    if (double.TryParse(valueToRead, out doubleValue))
                    {
                        return doubleValue;
                    }
                    else
                    {
                        var boolValue = true;
                        if (bool.TryParse(valueToRead, out boolValue))
                        {
                            return boolValue;
                        }
                        else
                        {
                            return valueToRead;
                        }
                    }
                }
            }
        }
    }
}
