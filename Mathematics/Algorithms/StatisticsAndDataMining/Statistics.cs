// -----------------------------------------------------------------------
// <copyright file="Statistics.cs" company="Sérgio O. Marques">
// Ver licença do projecto.
// </copyright>
// -----------------------------------------------------------------------

namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;

    /// <summary>
    /// Implementa o algoritmo da média generalizada sobre um enumerável de elementos.
    /// </summary>
    /// <remarks>
    /// A implementação é segura quando usada em implementações paralelas desde que a colecção de entrada
    /// não seja alterada.
    /// Trata-se de um algoritmo que permite calcular médias generalizadas como é o caso da média
    /// harmónica e da variância.
    /// </remarks>
    /// <typeparam name="T">O tipo dos objectos dos quais se pretende determinar a média.</typeparam>
    /// <typeparam name="P">O tipo dos objectos que irão constituir o resultado.</typeparam>
    /// <typeparam name="Q">O tipo dos objectos que reprsentam a contagem.</typeparam>
    public class EnumGeneralizedMeanAlgorithm<T, P, Q>
        : IAlgorithm<IEnumerable<T>, P>
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
        private IAdditionOperation<P, P, P> additionOperation;

        /// <summary>
        /// A função que permite dividir os objectos pelo valor do contador.
        /// </summary>
        private Func<P, Q, P> integerDivisionFunction;

        /// <summary>
        /// A função directa aplicada na média generalizada.
        /// </summary>
        private Func<T, P> directFunction;

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
            Func<T, P> directFunction,
            Func<P, P> inverseFunction,
            Func<P, Q, P> integerDivisionFunction,
            IAdditionOperation<P, P, P> additionOperation,
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
        public IAdditionOperation<P, P, P> AdditionOperation
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
        public Func<P, Q, P> IntegerDivisionFunction
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
        public Func<T, P> DirectFunction
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
                var innerAdditionOperation = default(IAdditionOperation<P, P, P>);
                var innerIntegerDivisionFunction = default(Func<P, Q, P>);
                var innerDirectFunction = default(Func<T, P>);
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
        /// <param name="weightDivisionFuntion">
        /// A função que permite calcular o quociente entre dois números inteiros, retornando o peso
        /// percentual de cada bloco.
        /// </param>
        /// <param name="weightMultiplicationFunction">
        /// A função que permite multiplicar o quociente pelo resultado da média.
        /// </param>
        /// <returns>O resultado da média.</returns>
        /// <typeparam name="W">O tipo dos objectos que resultam da divisão dos números inteiros.</typeparam>
        public P Run<W>(
            IEnumerable<T> data,
            Q blockZise,
            Func<Q, Q, W> weightDivisionFuntion,
            Func<W, P, P> weightMultiplicationFunction)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else if (blockZise == null)
            {
                throw new ArgumentNullException("blockSize");
            }
            else if (weightDivisionFuntion == null)
            {
                throw new ArgumentNullException("weightDivisionFunction");
            }
            else if (weightMultiplicationFunction == null)
            {
                throw new ArgumentNullException("weightMultiplicationFunction");
            }
            else if (this.integerNumber.Compare(blockZise, this.integerNumber.AdditiveUnity) <= 0)
            {
                throw new ArgumentException("The block size must be at least one.");
            }
            else
            {
                var innerIntegerNumber = default(IIntegerNumber<Q>);
                var innerAdditionOperation = default(IAdditionOperation<P, P, P>);
                var innerIntegerDivisionFunction = default(Func<P, Q, P>);
                var innerDirectFunction = default(Func<T, P>);
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
                    while (innerIntegerNumber.Compare(blockCount, blockZise) < 0
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
                        blockResult = innerDirectFunction.Invoke(dataEnum.Current);
                        while (dataEnum.MoveNext())
                        {
                            if (innerIntegerNumber.Compare(blockCount, blockZise) < 0)
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
                                result = innerAdditionOperation.Add(result, auxiliary);

                                // Actualização do novo bloco
                                blockCount = innerIntegerNumber.MultiplicativeUnity;
                                blockResult = innerDirectFunction.Invoke(dataEnum.Current);
                            }
                        }

                        if (innerIntegerNumber.Compare(blockCount, blockZise) == 0)
                        {
                            numberOfBlocks = innerIntegerNumber.Successor(numberOfBlocks);
                            var auxiliary = innerIntegerDivisionFunction.Invoke(blockResult, blockCount);
                            result = innerAdditionOperation.Add(result, auxiliary);
                            result = innerIntegerDivisionFunction.Invoke(result, numberOfBlocks);
                        }
                        else
                        {
                            var auxCount = innerIntegerNumber.Multiply(numberOfBlocks, blockZise);
                            var totalItems = innerIntegerNumber.Add(auxCount, blockCount);
                            var percentage = weightDivisionFuntion.Invoke(blockZise, totalItems);
                            result = weightMultiplicationFunction.Invoke(percentage, result);
                            var auxiliary = innerIntegerDivisionFunction.Invoke(blockResult, totalItems);
                            result = innerAdditionOperation.Add(result, auxiliary);
                        }

                        result = innerInverseFunction.Invoke(result);
                        return result;
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
    /// Trata-se de um algoritmo que permite calcular médias generalizadas como é o caso da média
    /// harmónica e da variância.
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
        private IAdditionOperation<P, P, P> additionOperation;

        /// <summary>
        /// A função que permite dividir os objectos pelo valor do contador.
        /// </summary>
        private Func<P, int, P> integerDivisionFunction;

        /// <summary>
        /// A função directa aplicada na média generalizada.
        /// </summary>
        private Func<T, P> directFunction;

        /// <summary>
        /// Mantém a função inversa.
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
            Func<T, P> directFunction,
            Func<P, P> inverseFunction,
            Func<P, int, P> integerDivisionFunction,
            IAdditionOperation<P, P, P> additionOperation)
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
        public IAdditionOperation<P, P, P> AdditionOperation
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
        public Func<P, int, P> IntegerDivisionFunction
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
        public Func<T, P> DirectFunction
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
        ///  Obém ou atribui a função directa aplicada na média generalizada.
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
                var innerAdditionOperation = default(IAdditionOperation<P, P, P>);
                var innerIntegerDivisionFunction = default(Func<P, int, P>);
                var innerDirectFunction = default(Func<T, P>);
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
                    return innerInverseFunction.Invoke(result);
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
        /// <param name="weightDivisionFuntion">A função que permite dividir inteiros.</param>
        /// <param name="weightMultiplicationFunction">
        /// A função que permite multiplicar o quociente pelo resultado da média.
        /// </param>
        /// <returns>O resultado da média.</returns>
        public P Run<W>(
            IList<T> data,
            int blockZise,
            Func<int, int, W> weightDivisionFuntion,
            Func<W, P, P> weightMultiplicationFunction)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else if (weightDivisionFuntion == null)
            {
                throw new ArgumentNullException("weightDivisionFuntion");
            }
            else if (weightMultiplicationFunction == null)
            {
                throw new ArgumentNullException("weightMultiplicationFunction");
            }
            else if (blockZise <= 0)
            {
                throw new ArgumentException("The block size must be at least one.");
            }
            else
            {
                var innerAdditionOperation = default(IAdditionOperation<P, P, P>);
                var innerIntegerDivisionFunction = default(Func<P, int, P>);
                var innerDirectFunction = default(Func<T, P>);
                var innerInverseFunction = default(Func<P, P>);

                // Para evitar colisões de argumentos em processamento paralelo.
                lock (this.lockObject)
                {
                    innerAdditionOperation = this.additionOperation;
                    innerIntegerDivisionFunction = this.integerDivisionFunction;
                    innerDirectFunction = this.directFunction;
                    innerInverseFunction = this.inverseFunction;
                }

                var listCount = data.Count;
                if (listCount == 0)
                {
                    throw new ArgumentException("No data was provided in collection. Can't compute the mean value.");
                }
                else
                {
                    var nextIndex = blockZise;
                    if (nextIndex < listCount)
                    {
                        var blockResult = innerDirectFunction.Invoke(data[0]);
                        for (int i = 1; i < nextIndex; ++i)
                        {
                            var aux = innerDirectFunction.Invoke(data[i]);
                            blockResult = innerAdditionOperation.Add(blockResult, aux);
                        }

                        // Determinação da primeira média
                        var result = innerIntegerDivisionFunction.Invoke(blockResult, blockZise);
                        var index = nextIndex;
                        nextIndex += blockZise;
                        while (nextIndex < listCount)
                        {
                            blockResult = innerDirectFunction.Invoke(data[index]);
                            ++index;
                            for (; index < nextIndex; ++index)
                            {
                                var aux = innerDirectFunction.Invoke(data[index]);
                                blockResult = innerAdditionOperation.Add(blockResult, aux);
                            }

                            blockResult = innerIntegerDivisionFunction.Invoke(blockResult, blockZise);
                            result = innerAdditionOperation.Add(result, blockResult);
                            index = nextIndex;
                            nextIndex += blockZise;
                        }

                        // Tratamento do último bloco
                        blockResult = innerDirectFunction.Invoke(data[index]);
                        ++index;
                        for (; index < listCount; ++index)
                        {
                            var aux = innerDirectFunction.Invoke(data[index]);
                            blockResult = innerAdditionOperation.Add(blockResult, aux);
                        }

                        var blocksNumber = listCount / blockZise;
                        var count = listCount - index;
                        if (count == blockZise)
                        {
                            // O resultado é igual à média das médias
                            blockResult = innerIntegerDivisionFunction.Invoke(blockResult, blockZise);
                            result = innerAdditionOperation.Add(result, blockResult);
                            result = innerIntegerDivisionFunction.Invoke(result, blocksNumber);
                            return result;
                        }
                        else
                        {
                            // O resultado é igual à média ponderada dos primeiros blocos com o último
                            var weight = weightDivisionFuntion.Invoke(blockZise, listCount);
                            result = weightMultiplicationFunction.Invoke(weight, result);
                            blockResult = innerIntegerDivisionFunction.Invoke(blockResult, listCount);
                            result = innerAdditionOperation.Add(result, blockResult);
                            return result;
                        }
                    }
                    else
                    {
                        // Retornar apneas a média do primeiro bloco
                        var result = innerDirectFunction.Invoke(data[0]);
                        for (int i = 1; i < listCount; ++i)
                        {
                            var aux = innerDirectFunction.Invoke(data[i]);
                            result = innerAdditionOperation.Add(result, aux);
                        }

                        result = innerIntegerDivisionFunction.Invoke(result, listCount);
                        return innerInverseFunction.Invoke(result);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Determina o coeficiente de correlação para pares de valores.
    /// </summary>
    /// <typeparam name="First">O tipo dos objectos que constituem a primeira coluna.</typeparam>
    /// <typeparam name="Second">O tipo dos objectos que constituem a segunda coluna.</typeparam>
    /// <typeparam name="Res">O tipo de objectos que constitui o resultado do coeficiente.</typeparam>
    public class TauCorrelation<First, Second, Res>
        : IAlgorithm<IList<First>, IList<Second>, IComparer<First>, IComparer<Second>, Res>
    {
        /// <summary>
        /// Função que permite ordenar uma lista de longos, dado
        /// o comparador de termos.
        /// </summary>
        private Action<int[], IComparer<int>> generalSoter;

        /// <summary>
        /// Função que permite order uma lista de longos dado
        /// o comparador de termos e que retorna o número de trocas
        /// efectuadas caso a ordenação fosse realizada pelo método
        /// do borbulhamento.
        /// </summary>
        private Func<int[], IComparer<int>, ulong> countSorter;

        /// <summary>
        /// Função responsável pela divisão.
        /// </summary>
        private Func<ulong, Res, Res> divideFunction;

        /// <summary>
        /// Função que permite o cálculo da raiz quadrada.
        /// </summary>
        private Func<ulong, Res> squareRootFunction;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="TauCorrelation{First, Second, Res, CollA, CollB}"/>.
        /// </summary>
        /// <param name="generalSoter">A função reponsável pela ordenação inicial.</param>
        /// <param name="countSorter">
        /// A função responsável pela ordenação que retorne o número de trocas
        /// efectuadas caso fosse usado o método do borbulhamento.</param>
        /// <param name="divideFunction">A função responsável pela divisão de coeficientes.</param>
        /// <param name="squareRootFunction">A função responsável pela determinação da raiz quadrada.</param>
        public TauCorrelation(
            Action<int[], IComparer<int>> generalSoter,
            Func<int[], IComparer<int>, ulong> countSorter,
            Func<ulong, Res, Res> divideFunction,
            Func<ulong, Res> squareRootFunction
            )
        {
            if (generalSoter == null)
            {
                throw new ArgumentNullException("genralSoter");
            }
            else if (countSorter == null)
            {
                throw new ArgumentNullException("countSorter");
            }
            else if (divideFunction == null)
            {
                throw new ArgumentNullException("divideFuntion");
            }
            else if (squareRootFunction == null)
            {
                throw new ArgumentNullException("squareRootFunction");
            }
            else
            {
                this.generalSoter = generalSoter;
                this.countSorter = countSorter;
                this.divideFunction = divideFunction;
                this.squareRootFunction = squareRootFunction;
            }
        }

        /// <summary>
        /// Determina o coeficiente tau entre as listas de valores.
        /// </summary>
        /// <param name="first">A primeira lista.</param>
        /// <param name="second">A segunda lista.</param>
        /// <param name="firstComparer">O comparador de elementos da primeira coluna.</param>
        /// <param name="secondComparer">O comparador de elementos da segunda coluna.</param>
        /// <returns>O valor do coeficiente.</returns>
        public Res Run(
            IList<First> first,
            IList<Second> second,
            IComparer<First> firstComparer,
            IComparer<Second> secondComparer)
        {
            if (first == null)
            {
                throw new ArgumentNullException("first");
            }
            else if (second == null)
            {
                throw new ArgumentNullException("second");
            }
            else if (firstComparer == null)
            {
                throw new ArgumentNullException("firstComparer");
            }
            else if (secondComparer == null)
            {
                throw new ArgumentNullException("secondComparer");
            }
            else
            {
                var firstLength = first.LongCount();
                var secondLength = second.LongCount();
                if (firstLength == secondLength)
                {
                    if (firstLength == 0)
                    {
                        var squareRoot = this.squareRootFunction.Invoke(1UL);
                        return this.divideFunction.Invoke(1, squareRoot);
                    }
                    else
                    {
                        return this.ComputeTau(first, second, firstComparer, secondComparer);
                    }
                }
                else
                {
                    throw new ArgumentException("The number of items in both lists doesn't match.");
                }
            }
        }

        /// <summary>
        /// Determina o coeficiente tau entre as listas de valores.
        /// </summary>
        /// <param name="first">A primeira lista.</param>
        /// <param name="second">A segunda lista.</param>
        /// <param name="firstComparer">O comparador de elementos da primeira coluna.</param>
        /// <param name="secondComparer">O comparador de elementos da segunda coluna.</param>
        /// <returns>O valor do coeficiente.</returns>
        private Res ComputeTau(
            IList<First> first,
            IList<Second> second,
            IComparer<First> firstComparer,
            IComparer<Second> secondComparer)
        {
            var elementsComparer = new ElementsComparer(
                first,
                second,
                firstComparer,
                secondComparer);
            var count = first.Count;
            var indexes = new int[count];
            for (var i = 0; i < count; ++i)
            {
                indexes[i] = i;
            }

            this.generalSoter.Invoke(indexes, elementsComparer);

            var tiedX = 0UL;
            var tiedXy = 0UL;
            var consecutiveTiedX = 1UL;
            var consecutiveTiedXy = 1UL;

            var prevX = first[0];
            var prevY = second[0];

            for (var i = 1; i < count; ++i)
            {
                var currX = first[i];
                var currY = second[i];
                if (firstComparer.Compare(currX, prevX) == 0)
                {
                    ++consecutiveTiedX;
                    if (secondComparer.Compare(currY, prevY) == 0)
                    {
                        ++consecutiveTiedXy;
                    }
                    else
                    {
                        if (consecutiveTiedXy > 1)
                        {
                            tiedXy += consecutiveTiedXy * (consecutiveTiedXy - 1);
                        }

                        consecutiveTiedXy = 1UL;
                        prevY = currY;
                    }
                }
                else
                {
                    if (consecutiveTiedX > 1)
                    {
                        tiedX += consecutiveTiedX * (consecutiveTiedX - 1);
                    }

                    if (consecutiveTiedXy > 1)
                    {
                        tiedXy += consecutiveTiedXy * (consecutiveTiedXy - 1);
                    }

                    consecutiveTiedX = 1UL;
                    consecutiveTiedXy = 1UL;

                    prevY = currY;
                    prevX = currX;
                }
            }

            if (consecutiveTiedX > 1)
            {
                tiedX += consecutiveTiedX * (consecutiveTiedX - 1);
            }

            if (consecutiveTiedXy > 1)
            {
                tiedXy += consecutiveTiedXy * (consecutiveTiedXy - 1);
            }

            var columnComparer = new ColumnComparer(
                second,
                secondComparer);
            var swaps = this.countSorter.Invoke(
                indexes,
                columnComparer);

            var tiedY = 0UL;
            var consecutiveTiedY = 1UL;
            prevY = second[0];
            for (var i = 1; i < count; ++i)
            {
                var currY = second[i];
                if (secondComparer.Compare(currY, prevY) == 0)
                {
                    ++consecutiveTiedY;
                }
                else
                {
                    if (consecutiveTiedY > 1)
                    {
                        tiedY += consecutiveTiedY * (consecutiveTiedY - 1);
                    }

                    prevY = currY;
                }
            }

            if (consecutiveTiedY > 1)
            {
                tiedY += consecutiveTiedY * (consecutiveTiedY - 1);
            }

            var numPairs = 1UL;
            var n = (ulong)count;
            if (count > 1)
            {
                numPairs = n * (n - 1);
            }

            var temp = ((numPairs - tiedX - tiedY + tiedXy) >> 1) - swaps;
            var sqrt = this.squareRootFunction.Invoke(
                ((numPairs >> 1) - tiedX) * ((numPairs >> 1) - tiedY));
            return this.divideFunction.Invoke(
                temp,
                sqrt);
        }

        /// <summary>
        /// Permite comparar os índices dos vectores de acordo
        /// com os respectivos elementos.
        /// </summary>
        private class ElementsComparer : IComparer<int>
        {
            /// <summary>
            /// Mantém a primeira lista de valores.
            /// </summary>
            private IList<First> firstList;

            /// <summary>
            /// Mantém a segunda lista de valores.
            /// </summary>
            private IList<Second> secondList;

            /// <summary>
            /// Mantém o comparador dos elementos da primeira lista.
            /// </summary>
            private IComparer<First> firstComparer;

            /// <summary>
            /// Mantém o compaorador para os elementso da segunda lista.
            /// </summary>
            private IComparer<Second> secondComparer;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="ElementsComparer"/>.
            /// </summary>
            /// <param name="firstList">A primeira lista de valores.</param>
            /// <param name="secondList">A segunda lista de valores.</param>
            /// <param name="firstComparer">O comparador dos elementos da primeira lista.</param>
            /// <param name="secondComparer">O comparador dos elementos da segunda lista.</param>
            public ElementsComparer(
                IList<First> firstList,
                IList<Second> secondList,
                IComparer<First> firstComparer,
                IComparer<Second> secondComparer)
            {
                this.firstList = firstList;
                this.secondList = secondList;
                this.firstComparer = firstComparer;
                this.secondComparer = secondComparer;
            }

            /// <summary>
            /// Compara dois valores.
            /// </summary>
            /// <param name="x">O primeiro valor a ser comparado.</param>
            /// <param name="y">O segundo valor a ser comparado.</param>
            /// <returns>
            /// O valor 1 se <paramref name="x"/> for superior a <paramref name="y"/>,
            /// 0 se ambos os valores forem iguais e -1 se <paramref name="x"/> for
            /// inferior a <paramref name="y"/>.
            /// </returns>
            public int Compare(int x, int y)
            {
                var compValue = this.firstComparer.Compare(
                    this.firstList[x],
                    this.firstList[y]);
                if (compValue == 0)
                {
                    compValue = this.secondComparer.Compare(
                        this.secondList[x],
                        this.secondList[y]);
                }

                return compValue;
            }
        }

        /// <summary>
        /// Permite comparar os índices dos vectores de acordo com
        /// os elementos da segunda coluna.
        /// </summary>
        private class ColumnComparer : IComparer<int>
        {
            /// <summary>
            /// Mantém a segunda lista de valores.
            /// </summary>
            private IList<Second> secondList;

            /// <summary>
            /// Mantém o compaorador para os elementso da segunda lista.
            /// </summary>
            private IComparer<Second> secondComparer;

            /// <summary>
            /// Instancia uma nova instância de objectos do tipo <see cref="ElementsComparer"/>.
            /// </summary>
            /// <param name="secondList">A segunda lista de valores.</param>
            /// <param name="secondComparer">O comparador dos elementos da segunda lista.</param>
            public ColumnComparer(
                IList<Second> secondList,
                IComparer<Second> secondComparer)
            {
                this.secondList = secondList;
                this.secondComparer = secondComparer;
            }

            /// <summary>
            /// Compara dois valores.
            /// </summary>
            /// <param name="x">O primeiro valor a ser comparado.</param>
            /// <param name="y">O segundo valor a ser comparado.</param>
            /// <returns>
            /// O valor 1 se <paramref name="x"/> for superior a <paramref name="y"/>,
            /// 0 se ambos os valores forem iguais e -1 se <paramref name="x"/> for
            /// inferior a <paramref name="y"/>.
            /// </returns>
            public int Compare(int x, int y)
            {
                return this.secondComparer.Compare(
                    this.secondList[x],
                    this.secondList[y]);
            }
        }
    }

    /// <summary>
    /// Implementa a determinação da mediana, utilizando um dicionário cujas
    /// chaves são ordenadas.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as entradas da amostra.</typeparam>
    public class DicMedianAlgorithm<T> : IAlgorithm<IEnumerable<T>, Tuple<T, T>>
    {
        /// <summary>
        /// O objecto responsável pela sincronização de linhas de processamento.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// Mantém a fábrica responsável pela criação de dicionários ordenados.
        /// </summary>
        private Func<ISortedDictionary<T, MutableTuple<ulong>>> dictionaryFactory;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="DicMedianAlgorithm{T}"/>.
        /// </summary>
        public DicMedianAlgorithm()
        {
            this.lockObject = new object();
            this.dictionaryFactory = () => new SortedDictionaryWrapper<T, MutableTuple<ulong>>();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="DicMedianAlgorithm{T}"/>.
        /// </summary>
        /// <param name="dictionaryFactory">
        /// A fábrica responsável pela criação de dicionários ordenados.
        /// </param>
        public DicMedianAlgorithm(Func<ISortedDictionary<T, MutableTuple<ulong>>> dictionaryFactory)
        {
            if (dictionaryFactory == null)
            {
                throw new ArgumentNullException("dictionaryFactory");
            }
            else
            {
                this.lockObject = new object();
                this.dictionaryFactory = dictionaryFactory;
            }
        }

        /// <summary>
        /// Obtém ou atribui a fábrica responsável pela criação de dicionários ordenados..
        /// </summary>
        public Func<ISortedDictionary<T, MutableTuple<ulong>>> DictionaryFactory
        {
            get
            {
                return this.dictionaryFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Dictionary factory value can't be null.");
                }
                else
                {
                    this.dictionaryFactory = value;
                }
            }
        }

        /// <summary>
        /// Determina a mediana.
        /// </summary>
        /// <param name="data">O enumerador para os dados.</param>
        /// <returns>O valor da mediana.</returns>
        public Tuple<T, T> Run(IEnumerable<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            else
            {
                var innerDicationaryFactory = default(Func<ISortedDictionary<T, MutableTuple<ulong>>>);
                lock (this.lockObject)
                {
                    innerDicationaryFactory = this.dictionaryFactory;
                }

                var sortedDictionary = innerDicationaryFactory.Invoke();
                var dataEnumerator = data.GetEnumerator();
                if (dataEnumerator.MoveNext())
                {
                    sortedDictionary.Add(
                        dataEnumerator.Current,
                        MutableTuple.Create(1UL));
                    while (dataEnumerator.MoveNext())
                    {
                        var current = dataEnumerator.Current;
                        var value = default(MutableTuple<ulong>);
                        if (sortedDictionary.TryGetValue(current, out value))
                        {
                            value.Item1++;
                        }
                        else
                        {
                            sortedDictionary.Add(
                                current,
                                MutableTuple.Create(1UL));
                        }
                    }

                    var dicEnum = sortedDictionary.GetEnumerator();
                    dicEnum.MoveNext();
                    var sum = dicEnum.Current.Value.Item1;
                    while (dicEnum.MoveNext())
                    {
                        sum += dicEnum.Current.Value.Item1;
                    }

                    dicEnum.Reset();
                    var semiCount = (sum + 1) >> 1;
                    dicEnum.MoveNext();
                    var curr = dicEnum.Current;
                    var acc = curr.Value.Item1;
                    while (acc < semiCount)
                    {
                        dicEnum.MoveNext();
                        curr = dicEnum.Current;
                        acc += curr.Value.Item1;
                    }



                    if (acc == semiCount)
                    {
                        dicEnum.MoveNext();
                        if ((sum & 1) == 0)
                        {
                            return Tuple.Create(curr.Key, dicEnum.Current.Key);
                        }
                        else
                        {
                            return Tuple.Create(curr.Key, curr.Key);
                        }
                    }
                    else
                    {
                        return Tuple.Create(curr.Key, curr.Key);
                    }
                }
                else
                {
                    throw new MathematicsException("Can't compute median on empty collections.");
                }
            }
        }
    }

    /// <summary>
    /// Implementa o algoritmo que permite determinar a moda, utilizando um dicionário.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as amostras.</typeparam>
    public class DicModeAlgorithm<T> : IAlgorithm<IEnumerable<T>, List<T>>
    {
        /// <summary>
        /// Mantém o objecto responsável pela sincronização.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// Mantém a fábrica dos dicionários.
        /// </summary>
        private Func<IDictionary<T, MutableTuple<ulong>>> dicFactory;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="DicModeAlgorithm{T}"/>.
        /// </summary>
        public DicModeAlgorithm()
        {
            this.lockObject = new object();
            this.dicFactory = () => new Dictionary<T, MutableTuple<ulong>>();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="DicModeAlgorithm{T}"/>.
        /// </summary>
        /// <param name="dicFactory">A fábrica responsável pela criação do dicionário.</param>
        public DicModeAlgorithm(Func<IDictionary<T, MutableTuple<ulong>>> dicFactory)
        {
            if (dicFactory == null)
            {
                throw new ArgumentNullException("dicFactory");
            }
            else
            {
                this.lockObject = new object();
                this.dicFactory = dicFactory;
            }
        }

        /// <summary>
        /// Obtém ou atribui a fábrica responsável pela criação
        /// dos dicionários.
        /// </summary>
        public Func<IDictionary<T, MutableTuple<ulong>>> DicationaryFactory
        {
            get
            {
                return this.dicFactory;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("Dictionary factory can't be null.");
                }
                else
                {
                    this.dicFactory = value;
                }
            }
        }

        /// <summary>
        /// Determina a moda de uma amostra.
        /// </summary>
        /// <param name="data">A amostra.</param>
        /// <returns>As modas.</returns>
        public List<T> Run(IEnumerable<T> data)
        {
            var innerDicFactory = default(Func<IDictionary<T, MutableTuple<ulong>>>);
            lock (this.lockObject)
            {
                innerDicFactory = this.dicFactory;
            }

            var dictionary = innerDicFactory.Invoke();
            var dataEnum = data.GetEnumerator();
            if (dataEnum.MoveNext())
            {
                dictionary.Add(dataEnum.Current, MutableTuple.Create(1UL));
                while (dataEnum.MoveNext())
                {
                    var current = dataEnum.Current;
                    var value = default(MutableTuple<ulong>);
                    if (dictionary.TryGetValue(current, out value))
                    {
                        value.Item1++;
                    }
                    else
                    {
                        dictionary.Add(
                            current,
                            MutableTuple.Create(1UL));
                    }
                }

                var dicEnum = dictionary.GetEnumerator();
                dicEnum.MoveNext();
                var curr = dicEnum.Current;
                var max = curr.Value.Item1;
                var result = new List<T>() { curr.Key };
                while (dicEnum.MoveNext())
                {
                    curr = dicEnum.Current;
                    if (curr.Value.Item1 == max)
                    {
                        result.Add(curr.Key);
                    }
                    else if (curr.Value.Item1 > max)
                    {
                        result.Clear();
                        max = curr.Value.Item1;
                        result.Add(curr.Key);
                    }
                }

                return result;
            }
            else
            {
                throw new MathematicsException("Can't compute mode on empty collections.");
            }
        }
    }
}
