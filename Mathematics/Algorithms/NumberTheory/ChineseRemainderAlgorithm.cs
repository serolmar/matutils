namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Representa uma congruência onde 
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objecto que representa a congruência.</typeparam>
    public class Congruence<ObjectType>
    {
        /// <summary>
        /// O módulo da congruência.
        /// </summary>
        private ObjectType modulus;

        /// <summary>
        /// O valor associado à congruência.
        /// </summary>
        private ObjectType value;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="Congruence{ObjectType}"/>.
        /// </summary>
        /// <param name="modulus">O módulo da congruência.</param>
        /// <param name="congruence">O valor da congruência.</param>
        /// <exception cref="ArgumentNullException">
        /// Se pelo menos um dos argumentos for nulo.
        /// </exception>
        public Congruence(ObjectType modulus, ObjectType congruence)
        {
            if (modulus == null)
            {
                throw new ArgumentNullException("modulus");
            }
            else if (congruence == null)
            {
                throw new ArgumentNullException("congruence");
            }
            else
            {
                this.modulus = modulus;
                this.value = congruence;
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor do módulo.
        /// </summary>
        /// <value>
        /// O valor do módulo.
        /// </value>
        /// <exception cref="MathematicsException">Se o valor do módulo for nulo.</exception>
        public ObjectType Modulus
        {
            get
            {
                return this.modulus;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The modulus value can't be null.");
                }
                else
                {
                    this.modulus = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da congruência.
        /// </summary>
        /// <value>
        /// O valor da congruência.
        /// </value>
        /// <exception cref="MathematicsException">Se o valor da congruência for nulo.</exception>
        public ObjectType Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (value == null)
                {
                    throw new MathematicsException("The value can't be null.");
                }
                else
                {
                    this.value = value;
                }
            }
        }

        /// <summary>
        /// Obtém a congruência reduzida dado o domínio.
        /// </summary>
        /// <param name="domain">O domínio.</param>
        /// <returns>A congruência reduzida.</returns>
        /// <exception cref="ArgumentNullException">Se o domínio for nulo.</exception>
        public Congruence<ObjectType> GetReduced(IEuclidenDomain<ObjectType> domain)
        {
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            else
            {
                var reduced = domain.Rem(this.value, this.modulus);
                return new Congruence<ObjectType>(this.modulus, reduced);
            }
        }

        /// <summary>
        /// Constrói uma representação textual da congruência.
        /// </summary>
        /// <returns>A representação textual da congruência.</returns>
        public override string ToString()
        {
            return string.Format("{0} mod {1}", this.value, this.modulus);
        }
    }

    /// <summary>
    /// Permite aplicar o teorema dos restos chinês a um conjunto de números.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo de objectos que constituem os números.</typeparam>
    public class ChineseRemainderAlgorithm<ObjectType>
        : IAlgorithm<List<Congruence<ObjectType>>, IEuclidenDomain<ObjectType>, Congruence<ObjectType>>
    {
        /// <summary>
        /// Mantém o objecto que permite sincronizar tarefas independentes.
        /// </summary>
        private object lockObject = new object();

        /// <summary>
        /// Mabtém a lista de congruências que se encontra em fase de processamento.
        /// </summary>
        private List<Congruence<ObjectType>> processingCongruences = new List<Congruence<ObjectType>>();

        /// <summary>
        /// O número de tarefas requeridas para o processo actual.
        /// </summary>
        private int numberOfTasks;

        /// <summary>
        /// Mantém um valor que indica se o algoritmo actual se encontra em execução.
        /// </summary>
        private bool running;

        /// <summary>
        /// Mantém um valor que indica se um erro foi encontrado.
        /// </summary>
        private bool foundError;

        /// <summary>
        /// O algoritmo para determinar inversas.
        /// </summary>
        private IBachetBezoutAlgorithm<ObjectType> extendedAlgorithm;

        /// <summary>
        /// Mantém o domínio associado ao algoritmo.
        /// </summary>
        private IEuclidenDomain<ObjectType> domain;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ChineseRemainderAlgorithm{ObjectType}"/>.
        /// </summary>
        /// <param name="numberOfTasks">O número de tarefas de execução paralela usadas no processamento.</param>
        /// <exception cref="ArgumentOutOfRangeException">Se o número de tarefas for não positivo.</exception>
        public ChineseRemainderAlgorithm(int numberOfTasks = 1)
        {
            if (numberOfTasks <= 0)
            {
                throw new ArgumentOutOfRangeException("numberOfTasks");
            }
            else
            {
                this.numberOfTasks = numberOfTasks;
                this.running = false;
            }
        }

        /// <summary>
        /// Obtém o número máximo de tarefas a serem usadas pelo algoritmo.
        /// </summary>
        public int NumberOfTasks
        {
            get
            {
                return this.numberOfTasks;
            }
        }

        /// <summary>
        /// Obtém um valor que indica se o algoritmo actual se encontra em execução.
        /// </summary>
        public bool Running
        {
            get
            {
                return this.running;
            }
        }

        /// <summary>
        /// Obtém a congruência que soluciona o problema do resto chinês associado à lista de congruências
        /// proporcionada.
        /// </summary>
        /// <param name="congruences">A lista de congruências.</param>
        /// <param name="domain">O domínio.</param>
        /// <returns>A solução do problema caso exista e nulo caso contrário.</returns>
        /// <exception cref="MathematicsException">
        /// Se o algoritmo já se encontrar em execução ou não forem providenciadas quaisquer congruências.
        /// </exception>
        /// <exception cref="ArgumentNullException">Se o domínio for nulo.</exception>
        public Congruence<ObjectType> Run(
            List<Congruence<ObjectType>> congruences,
            IEuclidenDomain<ObjectType> domain)
        {
            lock (this.lockObject)
            {
                if (this.running)
                {
                    throw new MathematicsException("The chinese remainder algorithm is running.");
                }
                else if (domain == null)
                {
                    throw new ArgumentNullException("domain");
                }
                else if (congruences == null || congruences.Count == 0)
                {
                    throw new MathematicsException("No congruence was provided.");
                }
                else
                {
                    this.running = true;
                }
            }

            try
            {
                this.foundError = false;
                if (this.SetupCongruences(congruences, domain))
                {
                    this.domain = domain;
                    this.extendedAlgorithm = new LagrangeAlgorithm<ObjectType>(domain);
                    this.SolveTwo();

                    lock (this.lockObject)
                    {
                        this.running = false;
                        if (this.foundError)
                        {
                            return null;
                        }
                        else
                        {
                            return this.processingCongruences[0];
                        }
                    }
                }
                else
                {
                    lock (this.lockObject)
                    {
                        this.running = false;
                    }

                    return null;
                }
            }
            catch (Exception exception)
            {
                lock (this.lockObject)
                {
                    this.running = false;
                }

                throw exception;
            }
        }

        /// <summary>
        /// Resolve o problema relativamente a duas congruências tiradas da lista.
        /// </summary>
        private void SolveTwo()
        {
            var state = true;
            lock (this.lockObject)
            {
                if (this.foundError)
                {
                    state = false;
                }
            }

            while (state)
            {
                var firstListCongruence = default(Congruence<ObjectType>);
                var secondListCongruence = default(Congruence<ObjectType>);
                lock (this.lockObject)
                {
                    if (this.processingCongruences.Count < 2)
                    {
                        state = false;
                    }
                    else
                    {
                        firstListCongruence = this.processingCongruences[0];
                        secondListCongruence = this.processingCongruences[1];
                        this.processingCongruences.RemoveAt(0);
                        this.processingCongruences.RemoveAt(0);
                    }
                }

                if (state)
                {
                    var extendedResult = this.extendedAlgorithm.Run(
                        firstListCongruence.Modulus,
                        secondListCongruence.Modulus);
                    var difference = domain.Add(
                        secondListCongruence.Value,
                        domain.AdditiveInverse(firstListCongruence.Value));
                    var quoAndRemanider = domain.GetQuotientAndRemainder(
                        difference,
                        extendedResult.GreatestCommonDivisor);
                    if (domain.IsAdditiveUnity(quoAndRemanider.Remainder))
                    {
                        var mk = domain.Multiply(extendedResult.FirstFactor, quoAndRemanider.Quotient);

                        // Constitui o valor do resultado
                        var value = domain.Multiply(firstListCongruence.Modulus, mk);
                        value = domain.Add(value, firstListCongruence.Value);

                        // Constitui o módulo do resultado
                        var modulus = domain.Multiply(extendedResult.FirstCofactor, extendedResult.SecondCofactor);
                        modulus = domain.Multiply(modulus, extendedResult.GreatestCommonDivisor);
                        value = domain.Rem(value, modulus);
                        var resultCongruence = new Congruence<ObjectType>(modulus, value);
                        lock (this.lockObject)
                        {
                            this.processingCongruences.Add(resultCongruence);
                        }
                    }
                    else
                    {
                        lock (this.lockObject)
                        {
                            this.foundError = true;
                            state = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Estabelece o cenário para as congruências.
        /// </summary>
        /// <param name="congruences">As congruências.</param>
        /// <param name="domain">O domínio.</param>
        /// <returns>Verdadeiro caso o problema possa ter uma solução e falso caso contrário.</returns>
        /// <exception cref="MathematicsException">Se alguma congruência for zero.</exception>
        private bool SetupCongruences(
            List<Congruence<ObjectType>> congruences,
            IEuclidenDomain<ObjectType> domain)
        {
            // Para averiguar se algum módulo já foi processado.
            var processedModulus = new Dictionary<ObjectType, Congruence<ObjectType>>(
                domain);

            for (int i = 0; i < congruences.Count; ++i)
            {
                var currentCongruence = congruences[i];
                if (domain.IsAdditiveUnity(currentCongruence.Modulus))
                {
                    throw new MathematicsException("Congruence modulus is an additive unity.");
                }
                else
                {
                    var processed = default(Congruence<ObjectType>);
                    if (processedModulus.TryGetValue(currentCongruence.Modulus, out processed))
                    {
                        var currentValue = currentCongruence.Value;
                        var difference = domain.Add(currentValue, domain.AdditiveInverse(processed.Value));
                        difference = domain.Rem(difference, processed.Modulus);
                        if (!domain.IsAdditiveUnity(difference))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        this.processingCongruences.Add(currentCongruence);
                        processedModulus.Add(currentCongruence.Modulus, currentCongruence);
                    }
                }
            }

            return true;
        }
    }
}
