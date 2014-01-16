using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Utilities.Collections;
using System.Collections.ObjectModel;

namespace Mathematics
{
    public abstract class ARangeReader<T, SymbValue, SymbType, InputReader>
    {
        #region Fields
        protected List<ISymbol<SymbValue, SymbType>> currentElementSymbols = new List<ISymbol<SymbValue, SymbType>>();

        protected GeneralMapper<SymbType, SymbType> mapInternalOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        protected GeneralMapper<SymbType, SymbType> mapExternalOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        protected GeneralMapper<SymbType, SymbType> mapOpenDelimitersToCloseDelimitersTypes = new GeneralMapper<SymbType, SymbType>();

        protected SymbType separatorSymb;

        /// <summary>
        /// Valor que indica se a leitura foi iniciada.
        /// </summary>
        protected bool hasStarted;

        /// <summary>
        /// Valor que indica se a leitura foi bem sucedida ou não.
        /// </summary>
        protected bool hasErrors;

        /// <summary>
        /// O conjunto de mensagens enviadas.
        /// </summary>
        protected List<string> errorMessages;

        /// <summary>
        /// Mantém a lista de símbolos que são ignorados.
        /// </summary>
        protected List<SymbType> blancks = new List<SymbType>();
        #endregion

        public ARangeReader()
        {
            this.hasErrors = false;
            this.errorMessages = new List<string>();
        }

        public void ReadRangeValues(
            MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser)
        {
            if (this.hasStarted)
            {
                throw new MathematicsException("Reader has already been started.");
            }
            else
            {
                try
                {
                    if (this.mapInternalOpenDelimitersToCloseDelimitersTypes.Objects.Count == 0)
                    {
                        throw new MathematicsException("No internal delimiter symbols were provided.");
                    }
                    else if (this.separatorSymb == null)
                    {
                        throw new MathematicsException("No separator symbol was provided.");
                    }
                    else
                    {
                        this.hasStarted = true;
                        this.errorMessages.Clear();
                        this.hasErrors = false;
                        this.InnerReadRangeValues(reader, parser);
                        this.hasStarted = false;
                    }
                }
                catch (Exception exception)
                {
                    this.hasStarted = false;
                    this.hasErrors = true;
                    throw exception;
                }
            }
        }

        public IEnumerable<int> Configuration
        {
            get
            {
                return this.GetFinalCofiguration();
            }
        }

        public ReadOnlyCollection<T> Elements
        {
            get
            {
                return this.GetElements();
            }
        }

        public bool HasErrors
        {
            get
            {
                return this.hasErrors;
            }
        }

        public ReadOnlyCollection<string> ErrorMessages
        {
            get
            {
                return this.errorMessages.AsReadOnly();
            }
        }

        public bool HasStarted
        {
            get
            {
                return this.hasStarted;
            }
        }

        public SymbType SeparatorSymbType
        {
            get
            {
                return this.separatorSymb;
            }
            set
            {
                this.separatorSymb = value;
            }
        }

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

        protected abstract void InnerReadRangeValues(MementoSymbolReader<InputReader, SymbValue, SymbType> reader,
            IParse<T, SymbValue, SymbType> parser);

        protected abstract IEnumerable<int> GetFinalCofiguration();

        protected abstract ReadOnlyCollection<T> GetElements();
    }
}
