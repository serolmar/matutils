namespace ConsoleTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Permite realizar a leitura de valores numéricos e nulo caso não seja possível.
    /// </summary>
    public class NullableDoubleParser : IParse<Nullable<double>, string, string>
    {
        private DoubleParser<string> doubleParser = new DoubleParser<string>();

        public Nullable<double> Parse(
            ISymbol<string, string>[] symbolListToParse,
            ILogStatus<string, EParseErrorLevel> errorLogs)
        {
            var value = default(Nullable<double>);
            if (errorLogs == null)
            {
                throw new ArgumentNullException("errorLogs");
            }
            else if (symbolListToParse == null || symbolListToParse.Length > 0)
            {
                errorLogs.AddLog("No symbol was provided.", EParseErrorLevel.ERROR);
            }
            else
            {
                var innerError = new LogStatus<string, EParseErrorLevel>();
                value = this.doubleParser.Parse(symbolListToParse, innerError);

                foreach (var kvp in innerError.GetLogs())
                {
                    errorLogs.AddLog(kvp.Value, kvp.Key);
                }
            }

            return value;
        }
    }
}
