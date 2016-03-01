namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Aplica o produto escalar a dois vectores.
    /// </summary>
    /// <typeparam name="CoeffType">O tipo de coeficientes que constituem as entradas do vector.</typeparam>
    public class OrthoVectorScalarProduct<CoeffType> : IScalarProductSpace<IMathVector<CoeffType>, CoeffType>
    {
        /// <summary>
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </summary>
        private IRing<CoeffType> ring;

        /// <summary>
        /// O comparador de coeficientes.
        /// </summary>
        private IComparer<CoeffType> comparer;

        /// <summary>
        /// Cria instâncias de objectos do tipo <see cref="OrthoVectorScalarProduct{CoeffType}"/>
        /// </summary>
        /// <param name="comparer">O comparador de coeficientes.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <exception cref="ArgumentNullException">Se algum dos coeficientes for nulo.</exception>
        public OrthoVectorScalarProduct(IComparer<CoeffType> comparer, IRing<CoeffType> ring)
        {
            if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else
            {
                this.ring = ring;
                if (comparer == null)
                {
                    this.comparer = Comparer<CoeffType>.Default;
                }
                else
                {
                    this.comparer = comparer;
                }
            }
        }

        /// <summary>
        /// Obtém o anel responsável pelas operações sobre as entradas dos vectores.
        /// </summary>
        /// <value>
        /// O anel responsável pelas operações sobre as entradas dos vectores.
        /// </value>
        public IRing<CoeffType> Ring
        {
            get
            {
                return this.ring;
            }
        }

        /// <summary>
        /// Multiplica escalarmente dois vectores linha ou coluna.
        /// </summary>
        /// <remarks>
        /// Dois vectores admitem multiplicação caso sejam ambos vectores linha ou ambos vectores coluna e 
        /// tenham o mesmo número de elementos. O produto tensorial invariante não é suportado. Esta função não
        /// tem em linha de conta para os coeficientes métricos a matriz identidade.
        /// </remarks>
        /// <param name="left">O primeiro vector a ser multiplicado.</param>
        /// <param name="right">O segundo vector a ser multiplicado.</param>
        /// <returns>O valor da multiplicação escalar.</returns>
        public CoeffType Multiply(IMathVector<CoeffType> left, IMathVector<CoeffType> right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }
            else if (right == null)
            {
                throw new ArgumentNullException("right");
            }
            else
            {
                if (left.Length == right.Length)
                {
                    var result = this.ring.AdditiveUnity;
                    for (int i = 0; i < left.Length; ++i)
                    {
                        var value = this.ring.Multiply(left[i], right[i]);
                        result = this.ring.Add(result, value);
                    }

                    return result;
                }
                else
                {
                    throw new MathematicsException("Can only apply scalar product to vectors of the same dimension.");
                }
            }
        }

        /// <summary>
        /// Compara dois valores para averiguar se são iguais ou se um é maior do que o outro.
        /// </summary>
        /// <param name="x">O primeiro valor a ser comparado.</param>
        /// <param name="y">O segundo valor a ser comparado.</param>
        /// <returns>
        /// O valor 1 caso o primeiro seja maior do que o segundo, 0 caso sejam iguais e -1 caso o segundo
        /// seja menor que o primeiro.
        /// </returns>
        public int Compare(CoeffType x, CoeffType y)
        {
            return this.comparer.Compare(x, y);
        }
    }
}
