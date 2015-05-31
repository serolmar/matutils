﻿namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa o algoritmo da média generalizada sobre um enumerável de elementos.
    /// </summary>
    /// <remarks>
    /// A implementação é segura quando usada em implementações paralelas desde que a colecção de entrada
    /// não seja alterada.
    /// </remarks>
    /// <typeparam name="T">O tipo dos objectos dos quais se pretende determinar a média.</typeparam>
    /// <typeparam name="P">O tipo dos objectos que irão constituir o resultado.</typeparam>
    /// <typeparam name="Q">O tipo dos objectos que reprsentam a contagem.</typeparam>
    public class EnumGeneralizedMeanAlgorithm<T, P, Q>
        : IAlgorithm<IEnumerable<T>, P>, IAlgorithm<IEnumerable<T>, Q, Func<P, P, P>, Func<Q,P,P>, P>
    {
        /// <summary>
        /// O objecto responsável pela sincronização de linhas de processamento.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// O objecto responsável pelo incremente do contador.
        /// </summary>
        private IIntegerNumber<Q> integerNumber;

        /// <summary>
        /// O objecto responsável pela adição dos elementos da média.
        /// </summary>
        private IAdditionOperation<T, T, T> additionOperation;

        /// <summary>
        /// A função que permite dividir os objectos pelo valor do contador.
        /// </summary>
        private Func<T, Q, P> integerDivisionFunction;

        /// <summary>
        /// A função directa aplicada na média generalizada.
        /// </summary>
        private Func<T, T> directFunction;

        /// <summary>
        /// A função inversa aplicada na média generalizada.
        /// </summary>
        private Func<P, P> inverseFunction;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="EnumGeneralizedMeanAlgorithm{T, P, Q}"/>.
        /// </summary>
        /// <param name="directFunction">A função directa aplicada na média generalizada.</param>
        /// <param name="inverseFunction">A função inversa aplicada na média generalizada.</param>
        /// <param name="integerDivisionFunction">A função que define a divisão entre os objectos e o contador.</param>
        /// <param name="additionOperation">O objecto responsável pela adição dos elementos.</param>
        /// <param name="integerNumber">O objecto responsável pelos incrementos do contador.</param>
        public EnumGeneralizedMeanAlgorithm(
            Func<T, T> directFunction,
            Func<P, P> inverseFunction,
            Func<T, Q, P> integerDivisionFunction,
            IAdditionOperation<T, T, T> additionOperation,
            IIntegerNumber<Q> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (additionOperation == null)
            {
                throw new ArgumentNullException("additionOperation");
            }
            else if (integerDivisionFunction == null)
            {
                throw new ArgumentNullException("integerDivisionFunction");
            }
            else if (directFunction == null)
            {
                throw new ArgumentException("Missing direct function for provided inverse.");
            }
            else if (inverseFunction == null)
            {
                throw new ArgumentException("Missing inverse function for provided direct.");
            }
            else
            {
                this.integerNumber = integerNumber;
                this.additionOperation = additionOperation;
                this.integerDivisionFunction = integerDivisionFunction;
                this.directFunction = directFunction;
                this.inverseFunction = inverseFunction;
                this.lockObject = new object();
            }
        }

        /// <summary>
        /// Obém ou atribui o objecto responsável pelo incremente do contador.
        /// </summary>
        public IIntegerNumber<Q> IntegerNumber
        {
            get
            {
                return this.integerNumber;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The integer number value can't be null.");
                }
                else
                {
                    this.integerNumber = value;
                }
            }
        }

        /// <summary>
        /// O objecto responsável pela adição dos elementos da média.
        /// </summary>
        public IAdditionOperation<T, T, T> AdditionOperation
        {
            get
            {
                return this.additionOperation;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The addition operation value can't be null");
                }
                else
                {
                    this.additionOperation = value;
                }
            }
        }

        /// <summary>
        ///  Obém ou atribui a função que permite dividir os objectos pelo valor do contador.
        /// </summary>
        public Func<T, Q, P> IntegerDivisionFunction
        {
            get
            {
                return this.integerDivisionFunction;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The integer division function value can't be null.");
                }
                else
                {
                    this.integerDivisionFunction = value;
                }
            }
        }

        /// <summary>
        ///  Obém ou atribui a função directa aplicada na média generalizada.
        /// </summary>
        public Func<T, T> DirectFunction
        {
            get
            {
                return this.directFunction;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The direct function value can't be null.");
                }
                else
                {
                    this.directFunction = value;
                }
            }
        }

        /// <summary>
        ///  Obém ou atribui a função inversa aplicada na média generalizada.
        /// </summary>
        public Func<P, P> InverseFunction
        {
            get
            {
                return this.inverseFunction;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The inverse function value can't be null.");
                }
                else
                {
                    this.inverseFunction = value;
                }
            }
        }

        /// <summary>
        /// Determina a média generalizada de um conjunto de valores.
        /// </summary>
        /// <param name="data">O conjunto de valores.</param>
        /// <returns>A média.</returns>
        public P Run(IEnumerable<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var innerIntegerNumber = default(IIntegerNumber<Q>);
                var innerAdditionOperation = default(IAdditionOperation<T, T, T>);
                var innerIntegerDivisionFunction = default(Func<T, Q, P>);
                var innerDirectFunction = default(Func<T, T>);
                var innerInverseFunction = default(Func<P, P>);

                // Para evitar colisões de argumentos em processamento paralelo.
                lock (this.lockObject)
                {
                    innerIntegerNumber = this.integerNumber;
                    innerAdditionOperation = this.additionOperation;
                    innerIntegerDivisionFunction = this.integerDivisionFunction;
                    innerDirectFunction = this.directFunction;
                    innerInverseFunction = this.inverseFunction;
                }

                var collectionEnum = data.GetEnumerator();
                if (collectionEnum.MoveNext())
                {
                    var count = innerIntegerNumber.MultiplicativeUnity;
                    var res = innerDirectFunction(collectionEnum.Current);
                    while (collectionEnum.MoveNext())
                    {
                        var current = innerDirectFunction(collectionEnum.Current);
                        res = innerAdditionOperation.Add(res, current);
                        count = innerIntegerNumber.Successor(count);
                    }

                    var result = innerIntegerDivisionFunction(res, count);
                    result = innerInverseFunction(result);
                    return result;
                }
                else
                {
                    throw new ArgumentException("No data was provided in collection. Can't compute the mean value.");
                }
            }
        }

        /// <summary>
        /// Determina a média de um conjunto de valores, utilizando blocos de tamanhos definidos.
        /// </summary>
        /// <remarks>
        /// A introdução de blocos no cálculo da média permite aumentar o número de itens, mantendo
        /// a precisão da estrutura de dados.
        /// </remarks>
        /// <param name="data">O conjunto de valores dos quais se pretende obter a média.</param>
        /// <param name="blockZise">O número de elementos a serem utilizados por bloco.</param>
        /// <param name="additionFunction">A função de adição para valores intermédios.</param>
        /// <param name="multiplicationFunction">A função de multiplicação para valores intermédios.</param>
        /// <returns>O resultado da média.</returns>
        public P Run(
            IEnumerable<T> data,
            Q blockZise,
            Func<P, P, P> additionFunction,
            Func<Q,P,P> multiplicationFunction)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else if (blockZise == null)
            {
                throw new ArgumentNullException("blockSize");
            }
            else if (additionFunction == null)
            {
                throw new ArgumentNullException("additionFunction");
            }
            else if (multiplicationFunction == null)
            {
                throw new ArgumentNullException("multiplicationFunction");
            }
            else if (this.integerNumber.Compare(blockZise, this.integerNumber.AdditiveUnity) <= 0)
            {
                throw new ArgumentException("The block size must be at least one.");
            }
            else
            {
                var innerIntegerNumber = default(IIntegerNumber<Q>);
                var innerAdditionOperation = default(IAdditionOperation<T, T, T>);
                var innerIntegerDivisionFunction = default(Func<T, Q, P>);
                var innerDirectFunction = default(Func<T, T>);
                var innerInverseFunction = default(Func<P, P>);

                // Para evitar colisões de argumentos em processamento paralelo.
                lock (this.lockObject)
                {
                    innerIntegerNumber = this.integerNumber;
                    innerAdditionOperation = this.additionOperation;
                    innerIntegerDivisionFunction = this.integerDivisionFunction;
                    innerDirectFunction = this.directFunction;
                    innerInverseFunction = this.inverseFunction;
                }

                var dataEnum = data.GetEnumerator();
                if (dataEnum.MoveNext())
                {
                    var numberOfBlocks = innerIntegerNumber.AdditiveUnity;
                    var blockCount = innerIntegerNumber.MultiplicativeUnity;
                    var blockResult = innerDirectFunction.Invoke(dataEnum.Current);
                    while (innerIntegerNumber.Compare(blockCount, blockZise) <= 0
                        && dataEnum.MoveNext())
                    {
                        var current = innerDirectFunction.Invoke(dataEnum.Current);
                        blockResult = innerAdditionOperation.Add(blockResult, current);
                        blockCount = innerIntegerNumber.Successor(blockCount);
                    }

                    var result = innerIntegerDivisionFunction.Invoke(blockResult, blockCount);
                    if (dataEnum.MoveNext())
                    {
                        // Um novo bloco é iniciado
                        numberOfBlocks = innerIntegerNumber.Successor(numberOfBlocks);
                        var blockMean = result;
                        blockCount = innerIntegerNumber.MultiplicativeUnity;
                        blockResult = dataEnum.Current;
                        while (dataEnum.MoveNext())
                        {
                            if (innerIntegerNumber.Compare(blockCount, blockZise) <= 0)
                            {
                                // Continua no mesmo bloco
                                var current = innerDirectFunction.Invoke(dataEnum.Current);
                                blockResult = innerAdditionOperation.Add(blockResult, current);
                                blockCount = innerIntegerNumber.Successor(blockCount);
                            }
                            else
                            {
                                // Um novo bloco é iniciado
                                numberOfBlocks = innerIntegerNumber.Successor(numberOfBlocks);
                                var auxiliary = innerIntegerDivisionFunction.Invoke(blockResult, blockCount);
                                result = additionFunction.Invoke(result, auxiliary);

                                // Actualização do novo bloco
                                blockCount = innerIntegerNumber.MultiplicativeUnity;
                                blockResult = dataEnum.Current;
                            }
                        }

                        if (innerIntegerNumber.IsMultiplicativeUnity(blockCount))
                        {

                        }
                        else if (innerIntegerNumber.Compare(blockCount, blockZise) == 0)
                        {
                            numberOfBlocks = innerIntegerNumber.Successor(numberOfBlocks);
                            var auxiliary = innerIntegerDivisionFunction.Invoke(blockResult, blockCount);
                            result = additionFunction.Invoke(result, auxiliary);
                        }
                        else
                        {

                        }

                        throw new NotImplementedException();
                    }
                    else
                    {
                        result = inverseFunction(result);
                        return result;
                    }
                }
                else
                {
                    throw new ArgumentException("No data was provided in collection. Can't compute the mean value.");
                }
            }
        }
    }

    /// <summary>
    /// Implementa o algoritmo da média generalizada sobre uma lista de elementos.
    /// </summary>
    /// <remarks>
    /// A implementação é segura quando usada em implementações paralelas desde que a colecção de entrada
    /// não seja alterada.
    /// </remarks>
    /// <typeparam name="T">O tipo dos objectos dos quais se pretende determinar a média.</typeparam>
    /// <typeparam name="P">O tipo dos objectos que irão constituir o resultado.</typeparam>
    public class ListGeneralizedMeanAlgorithm<T, P> : IAlgorithm<IList<T>, P>
    {
        /// <summary>
        /// O objecto responsável pela sincronização de linhas de processamento.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// O objecto responsável pela adição dos elementos da média.
        /// </summary>
        private IAdditionOperation<T, T, T> additionOperation;

        /// <summary>
        /// A função que permite dividir os objectos pelo valor do contador.
        /// </summary>
        private Func<T, int, P> integerDivisionFunction;

        /// <summary>
        /// A função directa aplicada na média generalizada.
        /// </summary>
        private Func<T, T> directFunction;

        /// <summary>
        /// A função inversa aplicada na média generalizada.
        /// </summary>
        private Func<P, P> inverseFunction;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="ListGeneralizedMeanAlgorithm{T, P}"/>.
        /// </summary>
        /// <param name="directFunction">A função directa aplicada na média generalizada.</param>
        /// <param name="inverseFunction">A função inversa aplicada na média generalizada.</param>
        /// <param name="integerDivisionFunction">A função que define a divisão entre os objectos e o contador.</param>
        /// <param name="additionOperation">O objecto responsável pela adição dos elementos.</param>
        public ListGeneralizedMeanAlgorithm(
            Func<T, T> directFunction,
            Func<P, P> inverseFunction,
            Func<T, int, P> integerDivisionFunction,
            IAdditionOperation<T, T, T> additionOperation)
        {
            if (additionOperation == null)
            {
                throw new ArgumentNullException("additionOperation");
            }
            else if (integerDivisionFunction == null)
            {
                throw new ArgumentNullException("integerDivisionFunction");
            }
            else if (directFunction == null)
            {
                throw new ArgumentException("Missing direct function for provided inverse.");
            }
            else if (inverseFunction == null)
            {
                throw new ArgumentException("Missing inverse function for provided direct.");
            }
            else
            {
                this.additionOperation = additionOperation;
                this.integerDivisionFunction = integerDivisionFunction;
                this.directFunction = directFunction;
                this.inverseFunction = inverseFunction;
                this.lockObject = new object();
            }
        }

        /// <summary>
        /// O objecto responsável pela adição dos elementos da média.
        /// </summary>
        public IAdditionOperation<T, T, T> AdditionOperation
        {
            get
            {
                return this.additionOperation;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The addition operation value can't be null");
                }
                else
                {
                    this.additionOperation = value;
                }
            }
        }

        /// <summary>
        ///  Obém ou atribui a função que permite dividir os objectos pelo valor do contador.
        /// </summary>
        public Func<T, int, P> IntegerDivisionFunction
        {
            get
            {
                return this.integerDivisionFunction;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The integer division function value can't be null.");
                }
                else
                {
                    this.integerDivisionFunction = value;
                }
            }
        }

        /// <summary>
        ///  Obém ou atribui a função directa aplicada na média generalizada.
        /// </summary>
        public Func<T, T> DirectFunction
        {
            get
            {
                return this.directFunction;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The direct function value can't be null.");
                }
                else
                {
                    this.directFunction = value;
                }
            }
        }

        /// <summary>
        ///  Obém ou atribui a função inversa aplicada na média generalizada.
        /// </summary>
        public Func<P, P> InverseFunction
        {
            get
            {
                return this.inverseFunction;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The inverse function value can't be null.");
                }
                else
                {
                    this.inverseFunction = value;
                }
            }
        }

        /// <summary>
        /// Determina a média generalizada de um conjunto de valores.
        /// </summary>
        /// <param name="data">O conjunto de valores.</param>
        /// <returns>A média.</returns>
        public P Run(IList<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var innerAdditionOperation = default(IAdditionOperation<T, T, T>);
                var innerIntegerDivisionFunction = default(Func<T, int, P>);
                var innerDirectFunction = default(Func<T, T>);
                var innerInverseFunction = default(Func<P, P>);

                // Para evitar colisões de argumentos em processamento paralelo.
                lock (this.lockObject)
                {
                    innerAdditionOperation = this.additionOperation;
                    innerIntegerDivisionFunction = this.integerDivisionFunction;
                    innerDirectFunction = this.directFunction;
                    innerInverseFunction = this.inverseFunction;
                }

                var dataLength = data.Count;
                if (dataLength > 0)
                {
                    var i = 0;
                    var res = innerDirectFunction(data[i++]);
                    while (i < dataLength)
                    {
                        res = innerAdditionOperation.Add(
                            res,
                             innerDirectFunction(data[i++]));
                    }

                    var result = innerIntegerDivisionFunction(res, dataLength);
                    result = innerInverseFunction(result);
                    return result;
                }
                else
                {
                    throw new ArgumentException("No data was provided in collection. Can't compute the mean value.");
                }
            }
        }
    }
}