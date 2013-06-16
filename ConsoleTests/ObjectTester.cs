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
        private Dictionary<string, CostumFunction> customFunctions = new Dictionary<string, CostumFunction>();
        private List<Assembly> loadedAssemblies = new List<Assembly>();

        public ObjectTester()
        {
            this.InitializeAssemblies();
            InitializeBuiltinTypes();
            InitializeCostumFuncs();
        }

        /// <summary>
        /// Defines a delegate for user custom functions.
        /// </summary>
        /// <param name="command">The readed command.</param>
        /// <returns></returns>
        delegate Object CostumFunction(Command command);

        public void Run(TextReader reader, TextWriter writer)
        {
            writer.Write("Command> ");
            string commandLine = reader.ReadLine();
            while (commandLine != "exit")
            {
                try
                {
                    ProcessCommand(commandLine, reader, writer);
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
           this.customFunctions.Add("help", PrintHelp);
           this.customFunctions.Add("methods", PrintMethodList);
           this.customFunctions.Add("valueof", PrintVariable);
           this.customFunctions.Add("instantiate", CreateInstance);
           this.customFunctions.Add("registeredtypes", PrintRegisteredTypes);
           this.customFunctions.Add("setvariable", SetVariableValue);
           this.customFunctions.Add("callmethod", CallMethod);
           this.customFunctions.Add("interpretfile", GetCommandsFromFile);
           this.customFunctions.Add("loadtype", LoadType);
           this.customFunctions.Add("gettypes", GetTypes);
           this.customFunctions.Add("loadcoretype", LoadMsCoreType);
           this.customFunctions.Add("makearray", MakeArray);
           this.customFunctions.Add("loadassembly", this.LoadAssembly);
        }

        /// <summary>
        /// Creates an instance and attaches it to a variable name.
        /// </summary>
        /// <param name="variableName">The variable name.</param>
        private Object CreateInstance(Type typeToCreate, Type[] types, Object[] arguments)
        {
            Type myType = typeToCreate;
            //Type[] types = GetTypesFromArgList(arguments);
            ConstructorInfo constructInstanciator = myType.GetConstructor(types);
            if (constructInstanciator != null)
            {
                return constructInstanciator.Invoke(arguments);
            }
            else
            {
                return null;
            }
        }

        private Object CreateInstance(Command command)
        {
            if (command.RetValueVar == string.Empty)
            {
                command.Writer.WriteLine("No variable will take the objects instance.");
                return null;
            }
            if (command.FunctionArgs.Length == 0)
            {
                command.Writer.WriteLine("Missing instance type.");
                return null;
            }
            Object retObject = null;
            Type[] types = GetTypesFromArgList(this.GetVariableValues(command.FunctionArgs));

            Type myType = null;
            if (registeredTypes.ContainsKey(command.FunctionArgs[0]))
            {
                myType = registeredTypes[command.FunctionArgs[0]];
            }
            if (myType != null)
            {
                if (!myType.IsClass)
                {
                    command.Writer.WriteLine("To use builtin types, use setvariable instead.");
                    return null;
                }
                if (myType.ContainsGenericParameters)
                {
                    command.Writer.WriteLine("Error, trying to instantiate a non closed generic type.");
                    return null;
                }
                bool foudMatchConstructor = false;
                ConstructorInfo[] constrctsInfo = myType.GetConstructors();
                List<Object> realArguments = new List<Object>();
                foreach (ConstructorInfo cnstInfo in constrctsInfo)
                {
                    ParameterInfo[] parameters = cnstInfo.GetParameters();
                    if (parameters.Length + 1 == types.Length)
                    {
                        foudMatchConstructor = true;
                        for (int i = 0; i < parameters.Length; ++i)
                        {
                            if (!parameters[i].ParameterType.IsAssignableFrom(types[i + 1]))
                            {
                                foudMatchConstructor = false;
                                break;
                            }
                        }
                        if (foudMatchConstructor)
                        {
                            List<Type> temporaryTypes = types.ToList();
                            temporaryTypes.RemoveAt(0);
                            List<Object> temporary = this.GetVariableValues(command.FunctionArgs).ToList();
                            temporary.RemoveAt(0);
                            retObject = CreateInstance(myType, temporaryTypes.ToArray(), temporary.ToArray());
                        }
                    }
                }
                if (!foudMatchConstructor)
                {
                    command.Writer.WriteLine("No overload constructor has such argument signature.");
                    return null;
                }
            }
            else
            {
                command.Writer.WriteLine("Type " + command.FunctionArgs[0] + " do not exist in this assembly.");
            }
            return retObject;
        }

        public void RegisterType(string typeName, Type type)
        {
            if (!registeredTypes.ContainsKey(typeName))
            {
                registeredTypes[typeName] = type;
            }
        }

        private Object PrintRegisteredTypes(Command command)
        {
            command.Writer.WriteLine("The registered types are: ");
            foreach (string types in registeredTypes.Keys)
            {
                command.Writer.Write(types);
                command.Writer.Write(":");
                if (registeredTypes[types].IsClass)
                {
                    command.Writer.WriteLine("class");
                }
                else
                {
                    command.Writer.WriteLine("built-in");
                }
            }
            return null;
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
        /// Gets a list of commands from file.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Nothing.</returns>
        private Object GetCommandsFromFile(Command command)
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
                    Command fileCommand = null;
                    try
                    {
                        fileCommand = GetCommandLine(line);
                    }
                    catch (Exception except)
                    {
                        command.Writer.WriteLine("Error: " + except.Message);
                    }
                    if (fileCommand != null)
                    {
                        ++wellProcessedCommands;
                        Caller(fileCommand, reader, command.Writer);
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

        /// <summary>
        /// Constructs an array using reflexion.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The array.</returns>
        private Object MakeArray(Command command)
        {
            if (command.FunctionArgs.Length < 1)
            {
                command.Writer.WriteLine("Missing type.");
                return null;
            }
            if (command.FunctionArgs.Length < 2)
            {
                command.Writer.WriteLine("Missing rank.");
                return null;
            }
            if (!this.registeredTypes.ContainsKey(command.FunctionArgs[0]))
            {
                command.Writer.WriteLine("Type is not registered.");
                return null;
            }
            Object rank = GetVariableValue(command.FunctionArgs[1]);
            if (rank == null)
            {
                command.Writer.WriteLine("Invalid rank. Expected an integer value.");
                return null;
            }
            if (((Int32)rank) <= 0)
            {
                command.Writer.WriteLine("Rank is too small.");
                return null;
            }
            Type arrayType = this.registeredTypes[command.FunctionArgs[0]].MakeArrayType((Int32)rank);
            Type[] argumentTypes = new Type[(Int32)rank];
            for (int i = 0; i < argumentTypes.Length; ++i)
            {
                argumentTypes[i] = typeof(int);
            }
            List<Object> args = new List<object>();
            for (int i = 2; i < command.FunctionArgs.Length; ++i)
            {
                args.Add(GetVariableValue(command.FunctionArgs[i]));
            }
            return CreateInstance(arrayType, argumentTypes, args.ToArray());
        }

        /// <summary>
        /// Prints help for commands.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Nothing.</returns>
        private Object PrintHelp(Command command)
        {
            command.Writer.WriteLine("Usage: command [ -r <return>] [-a <arguments>]");
            command.Writer.WriteLine("Available commands are: ");
            foreach (string str in customFunctions.Keys)
            {
                command.Writer.WriteLine(str);
            }
            command.Writer.WriteLine("No further help is provided for time being.");
            return null;
        }

        /// <summary>
        /// Loads some assembly in current domain.
        /// </summary>
        /// <param name="command">The assembly to be loaded.</param>
        /// <returns>Nothing.</returns>
        private Object LoadAssembly(Command command)
        {
            if (command.FunctionArgs.Length < 1 || string.IsNullOrWhiteSpace(command.FunctionArgs[0]))
            {
                command.Writer.WriteLine("Expecting the name of assembly.");
            }
            else
            {
                var assembly = this.loadedAssemblies.FirstOrDefault(a => a.GetName().Name.ToLower().Contains(command.FunctionArgs[0].ToLower()));
                if (assembly == null)
                {
                    assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name.Equals(command.FunctionArgs[0], StringComparison.InvariantCultureIgnoreCase));
                    if (assembly == null)
                    {
                        command.Writer.WriteLine("Assembly {0} not found in application domain. Trying to load from some dll.", command.FunctionArgs[0]);
                        assembly = AppDomain.CurrentDomain.Load(command.FunctionArgs[0]);
                        this.loadedAssemblies.Add(assembly);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Loads a C# core type for usage.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Nothing.</returns>
        private Object LoadMsCoreType(Command command)
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

            Assembly assembly = this.loadedAssemblies.FirstOrDefault(a => a.GetName().Name.Equals("mscorlib", StringComparison.InvariantCultureIgnoreCase));
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
                        if (!this.registeredTypes.ContainsKey(command.FunctionArgs[1]))
                        {
                            this.registeredTypes[command.FunctionArgs[1]] = q.First();
                        }
                        else
                        {
                            this.registeredTypes.Add(command.FunctionArgs[1], q.First());
                        }
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

        /// <summary>
        /// Gets the command from command line.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <returns>The readed command.</returns>
        private Command GetCommandLine(string commandLine)
        {
            Command result = new ObjectTester.Command();
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
        /// Gets a list of methods from the specified type.
        /// </summary>
        /// <param name="fromObject">The type.</param>
        /// <returns>The list of methods.</returns>
        private List<string> GetMethodList(Type fromObject)
        {
            List<string> result = new List<string>();
            List<MethodInfo> methods = fromObject.GetMethods().ToList();
            for (int i = 0; i < methods.Count; ++i)
            {
                StringBuilder methodBuilder = new StringBuilder();
                string returnTypeName = methods[i].ReturnType.Name;
                if (returnTypeName == string.Empty)
                {
                    returnTypeName = "void";
                }
                methodBuilder.Append(returnTypeName);
                methodBuilder.Append(" ");
                methodBuilder.Append(methods[i].Name);
                methodBuilder.Append("(");
                ParameterInfo[] parameteres = methods[i].GetParameters();
                if (parameteres.Length > 0)
                {
                    methodBuilder.Append(parameteres[0].ParameterType.Name);
                    methodBuilder.Append(" ");
                    methodBuilder.Append(parameteres[0].Name);

                    for (int j = 1; j < parameteres.Length; j++)
                    {
                        methodBuilder.Append(parameteres[j].ParameterType.Name);
                        methodBuilder.Append(" ");
                        methodBuilder.Append(parameteres[j].Name);
                    }
                }
                methodBuilder.Append(")");
                result.Add(methodBuilder.ToString());
            }
            return result;
        }

        /// <summary>
        /// Gets all types from an object's array.
        /// </summary>
        /// <param name="arglist">The object's array.</param>
        /// <returns>The array of types.</returns>
        private Type[] GetTypesFromArgList(Object[] arglist)
        {
            if (arglist == null)
            {
                return Type.EmptyTypes;
            }
            if (arglist.Length == 0)
            {
                return Type.EmptyTypes;
            }
            Type[] result = new Type[arglist.Length];
            for (int i = 0; i < arglist.Length; ++i)
            {
                Type argType = arglist[i] == null ? typeof(Object) : arglist[i].GetType();
                result[i] = argType;
            }
            return result;
        }

        /// <summary>
        /// Prints the lsit of methods.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Nothing.</returns>
        private Object PrintMethodList(Command command)
        {
            if (command.FunctionArgs.Length != 1)
            {
                command.Writer.WriteLine("This function takes a type as an argument.");
                return null;
            }
            if (!registeredTypes.ContainsKey(command.FunctionArgs[0]))
            {
                command.Writer.WriteLine("Trying to inquire a non registered type.");
                return null;
            }
            Type type = registeredTypes[command.FunctionArgs[0]];
            List<string> methodList = this.GetMethodList(type);
            if (methodList.Count > 0)
            {
                command.Writer.WriteLine("These are the available methods in " + type.Name + " type:");
                for (int i = 0; i < methodList.Count; ++i)
                {
                    command.Writer.WriteLine(methodList[i]);
                }
                command.Writer.WriteLine("---End of method list---");
            }
            else
            {
                command.Writer.WriteLine("Found no visible method in " + type.Name + " type.");
            }

            return null;
        }

        /// <summary>
        /// Prints the variable value to the command's output.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Nothing.</returns>
        private Object PrintVariable(Command command)
        {
            if (command.FunctionArgs.Length != 1)
            {
                command.Writer.WriteLine("Expected only one argument as variable name.");
                return null;
            }
            if (this.instances.ContainsKey(command.FunctionArgs[0]))
            {
                this.Print(command, this.instances[command.FunctionArgs[0]]);
            }
            else
            {
                command.Writer.WriteLine("Variable not set.");
            }

            return null;
        }

        /// <summary>
        /// Sets a variable's value.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The variable's value.</returns>
        private Object SetVariableValue(Command command)
        {
            if (!registeredTypes.ContainsKey(command.FunctionArgs[0]))
            {
                command.Writer.WriteLine("The type " + command.FunctionArgs[0] + " is not registered.");
            }

            Object obj = ReadVariableFromString(registeredTypes[command.FunctionArgs[0]], command.FunctionArgs[1]);
            if (obj == null)
            {
                command.Writer.WriteLine("Value isn't of Built in type.");
            }
            else
            {
                if (this.instances.ContainsKey(command.FunctionArgs[2]))
                {
                    this.instances[command.FunctionArgs[2]] = obj;
                }
                else
                {
                    this.instances.Add(command.FunctionArgs[2], obj);
                }
            }

            return obj;
        }

        /// <summary>
        /// Loads the type specified by the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Nothing.</returns>
        private Object LoadType(Command command)
        {
            if (command.FunctionArgs.Length < 2)
            {
                throw new Exception("This function takes at least a type and an alias as arguments.");
            }

            var found = false;
            foreach (var assembly in this.loadedAssemblies)
            {
                var assemblyTypes = assembly.GetTypes();
                Type foundType = null;
                var typeFullName = string.Empty;
                var genericTypeArgumentsNumber = 0;
                for (int i = 0; i < assemblyTypes.Length; ++i)
                {
                    var currentType = assemblyTypes[i];
                    var splited = currentType.FullName.Split('`');
                    typeFullName = splited[0];
                    if (typeFullName.Equals(command.FunctionArgs[0]))
                    {
                        if (splited.Length > 1)
                        {
                            int.TryParse(splited[1], out genericTypeArgumentsNumber);
                        }

                        foundType = currentType;
                        found = true;
                        i = assemblyTypes.Length;
                    }
                }

                if (found)
                {
                    if (genericTypeArgumentsNumber == 0)
                    {
                        if (!this.registeredTypes.ContainsKey(command.FunctionArgs[1]))
                        {
                            this.registeredTypes[command.FunctionArgs[1]] = foundType;
                        }
                        else
                        {
                            this.registeredTypes.Add(command.FunctionArgs[1], foundType);
                        }
                    }
                    else
                    {
                        if (command.FunctionArgs.Length != genericTypeArgumentsNumber + 2)
                        {
                            command.Writer.WriteLine("Type {0} expects {1} generic type arguments.", typeFullName, genericTypeArgumentsNumber);
                        }
                        else
                        {
                            var readedTypes = new List<Type>();
                            for (int i = 1; i < command.FunctionArgs.Length - 1; ++i)
                            {
                                Type registeredType = null;
                                if (!this.registeredTypes.TryGetValue(command.FunctionArgs[i], out registeredType))
                                {
                                    command.Writer.WriteLine("Type with alias {0} isn't registered yet.", command.FunctionArgs[1]);
                                }
                                else
                                {
                                    readedTypes.Add(registeredType);
                                }
                            }

                            var constructedType = foundType.MakeGenericType(readedTypes.ToArray());
                            if (!this.registeredTypes.ContainsKey(command.FunctionArgs[command.FunctionArgs.Length - 1]))
                            {
                                this.registeredTypes[command.FunctionArgs[command.FunctionArgs.Length - 1]] = constructedType;
                            }
                            else
                            {
                                this.registeredTypes.Add(command.FunctionArgs[command.FunctionArgs.Length - 1], constructedType);
                            }
                        }
                    }

                    break;
                }
            }

            if (!found)
            {
                command.Writer.WriteLine("Type " + command.FunctionArgs[0] + " does not exist in loaded assemblies.");
            }

            return null;
        }

        /// <summary>
        /// Loads all types specified by the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>Nothing.</returns>
        private Object GetTypes(Command command)
        {
            Dictionary<string, bool> exists = new Dictionary<string, bool>();
            foreach (Assembly assembly in this.loadedAssemblies)
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

        /// <summary>
        /// Gets a variable's stored value.
        /// </summary>
        /// <param name="variableName">The variable name.</param>
        /// <returns>The variable's value.</returns>
        private Object GetVariableValue(string variableName)
        {
            Object result = null;
            if (this.instances.ContainsKey(variableName))
            {
                result = instances[variableName];
            }
            else
            {
                return ReadVariableFromString(typeof(int), variableName);
            }

            return result;
        }

        /// <summary>
        /// Gets the stored variables.
        /// </summary>
        /// <param name="variableNames">The variables names.</param>
        /// <returns>The variables values.</returns>
        private Object[] GetVariableValues(string[] variableNames)
        {
            List<Object> listOfObjects = new List<Object>();
            foreach (string variableName in variableNames)
            {
                listOfObjects.Add(this.GetVariableValue(variableName));
            }

            return listOfObjects.ToArray();
        }

        /// <summary>
        /// Calls the method invoked by some command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The result of invocation.</returns>
        private Object CallMethod(Command command)
        {
            if (command.FunctionArgs.Length < 2)
            {
                command.Writer.WriteLine("Function takes object and member name.");
                return null;
            }

            Object thInstance = GetVariableValue(command.FunctionArgs[1]);
            if (thInstance == null)
            {
                command.Writer.WriteLine("Can't call methods on null variables.");
            }

            Type objectType = thInstance.GetType();
            List<Object> argValues = new List<Object>();
            List<Type> argTypes = new List<Type>();
            for (int i = 2; i < command.FunctionArgs.Length; ++i)
            {
                Object variable = GetVariableValue(command.FunctionArgs[i]);
                argValues.Add(variable);
                argTypes.Add(variable.GetType());
            }

            MethodInfo methodInfo = objectType.GetMethod(command.FunctionArgs[0], argTypes.ToArray());
            if (methodInfo == null)
            {
                command.Writer.WriteLine("No method exists in object type with specified signature.");
                return null;
            }

            return methodInfo.Invoke(thInstance, argValues.ToArray());
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
            else if(rank > 1)
            {
                int[] pointers = new int[rank];
                int pointer = 0;
                short state = 0;
                while(state != 3)
                {
                    if(state == 0)
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
                    else if(state == 1)
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

        /// <summary>
        /// A class that represents a stream command.
        /// </summary>
        private class Command
        {
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
}
