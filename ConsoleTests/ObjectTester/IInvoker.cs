namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal interface IInvoker
    {
        Object Invoke(Command command);

        void PrintHelp(Command command);
    }
}
