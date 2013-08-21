namespace Mathematics.AlgebraicStructures.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa as operações sobre inteiros no que concerne a aritmética modular.
    /// </summary>
    public class ModularIntegerField : IField<int>
    {
        /// <summary>
        /// O módulo.
        /// </summary>
        private int module;

        /// <summary>
        /// Permite efectuar um passo intermédio para a obtenção da inversa.
        /// </summary>
        private LagrangeAlgorithm<int, IntegerDomain> inverseAlgorithm;

        public ModularIntegerField(int module)
        {
            if (module == 0 || module == 1 || module == -1)
            {
                throw new ArgumentException("Module can't neither 0, 1 nor -1.");
            }
            else
            {
                this.module = module;
                this.inverseAlgorithm = new LagrangeAlgorithm<int, IntegerDomain>(
                    new IntegerDomain());
            }
        }

        public int AdditiveUnity
        {
            get
            {
                return 0;
            }
        }

        public int MultiplicativeUnity
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Obtém a forma reduzida de um número relativamente ao módulo.
        /// </summary>
        /// <param name="number">O número.</param>
        /// <returns>O número reduzido.</returns>
        public int GetReduced(int number)
        {
            var innerNumber = number % this.module;
            if (innerNumber < 0)
            {
                innerNumber = this.module - innerNumber;
            }

            return innerNumber;
        }

        public int MultiplicativeInverse(int number)
        {
            var innerNumber = number % module;
            if (innerNumber != 0)
            {
                if (innerNumber < 0)
                {
                    innerNumber = this.module - innerNumber;
                }

                var bezoutResult = this.inverseAlgorithm.Run(this.module, innerNumber);
                if (bezoutResult.GreatestCommonDivisor != 1)
                {
                    return -bezoutResult.SecondFactor;
                }
                else
                {
                    throw new MathematicsException(string.Format("Number {0} mod {1} has no inverse.", number, this.module));
                }
            }
            else
            {
                throw new MathematicsException(string.Format("Number {0} mod {1} has no inverse.", number, this.module));
            }
        }

        public int AddRepeated(int element, int times)
        {
            if (times == 0)
            {
                return 0;
            }
            else
            {
                var innerElement = element % this.module;
                var innerTimes = times % this.module;
                var result = innerElement * innerTimes;
                result = result % this.module;
                if (result < 0)
                {
                    result = this.module - result;
                }

                return result;
            }
        }

        public int AdditiveInverse(int number)
        {
            var innerNumber = number % this.module;
            if (innerNumber < 0)
            {
                innerNumber = this.module - innerNumber;
            }

            return this.module - innerNumber;
        }

        public bool IsAdditiveUnity(int value)
        {
            return value % module == 0;
        }

        public bool Equals(int x, int y)
        {
            var innerX = x % this.module;
            var innerY = y % this.module;
            if (innerX < 0)
            {
                innerX = this.module - innerX;
            }

            if (innerY < 0)
            {
                innerY = this.module - innerY;
            }

            return innerX == innerY;
        }

        public int GetHashCode(int obj)
        {
            var innerObj = obj % this.module;
            if (innerObj < 0)
            {
                innerObj = this.module - innerObj;
            }

            return innerObj.GetHashCode();
        }

        public int Add(int left, int right)
        {
            var innerLeft = left % this.module;
            var innerRight = right % this.module;
            var result = innerLeft + innerRight;
            result = result % this.module;
            if (result < 0)
            {
                result = this.module - result;
            }

            return result;
        }

        public bool IsMultiplicativeUnity(int value)
        {
            return value % module == 1;
        }

        public int Multiply(int left, int right)
        {
            var innerLeft = left % this.module;
            var innerRight = right % this.module;
            var result = innerLeft * innerRight;
            result = result % this.module;
            if (result < 0)
            {
                result = this.module - result;
            }

            return result;
        }
    }
}
