using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Parsers;
using Utilities.Collections;

namespace Mathematics
{
    public abstract class AMultiDimensionalRangeParser<T, SymbValue, SymbType, InputReader>
    {
        #region Fields
        protected List<ISymbol<SymbValue, SymbType>> currentElementSymbols = new List<ISymbol<SymbValue, SymbType>>();

        protected GeneralMapper<SymbType, SymbType> mapInternalOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        protected GeneralMapper<SymbType, SymbType> mapExternalOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        protected GeneralMapper<SymbType, SymbType> mapOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        protected SymbType separatorSymb;

        protected List<SymbType> blancks = new List<SymbType>();
        #endregion

        public AMultiDimensionalRangeParser()
        {
        }

        public abstract MultiDimensionalRange<T> ParseRange(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser);

        #region Public Methods
        public void MapInternalDelimiters(SymbType openSymbolType, SymbType closeSymbType)
        {
            if (this.blancks.Contains(openSymbolType) || this.blancks.Contains(closeSymbType))
            {
                throw new ExpressionReaderException("Can't mark a blanck symbol as a delimiter type. Please remove symbol from blancks before mark it as a delimiter.");
            }
            else
            {
                this.mapInternalOpenDelimitersToCloseDelimitersTypes.Add(openSymbolType, closeSymbType);
            }
        }

        public void MapExternalDelimiters(SymbType openSymbType, SymbType closeSymbType)
        {
            if (this.blancks.Contains(openSymbType) || this.blancks.Contains(closeSymbType))
            {
                throw new ExpressionReaderException("Can't mark a blanck symbol as a delimiter type. Please remove symbol from blancks before mark it as a delimiter.");
            }
            else
            {
                this.mapExternalOpenDelimitersToCloseDelimitersTypes.Add(openSymbType, closeSymbType);
            }
        }

        public void AddBlanckSymbolType(SymbType symbolType)
        {
            if (symbolType != null)
            {
                if (symbolType.Equals(this.separatorSymb))
                {
                    throw new ExpressionReaderException("Can't mark the separator as a blank symbol.");
                }
                else if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(symbolType) || this.mapInternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(symbolType))
                {
                    throw new ExpressionReaderException("Can't mark a delimiter as a blank symbol.");
                }
                else if (this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsObject(symbolType) || this.mapExternalOpenDelimitersToCloseDelimitersTypes.ContainsTarget(symbolType))
                {
                    throw new ExpressionReaderException("Can't mark a delimiter as a blank symbol.");
                }

                if (!this.blancks.Contains(symbolType))
                {
                    this.blancks.Add(symbolType);
                }
            }
        }

        public void RemoveBlanckSymbolType(SymbType symbolType)
        {
            this.blancks.Remove(symbolType);
        }

        public void ClearBlanckSymbols()
        {
            this.blancks.Clear();
        }
        #endregion
    }
}
