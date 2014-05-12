namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Implementa as operações de anel sobre o polinómio de grau generalizado.
    /// </summary>
    /// <remarks>
    /// Uma definição comum de polinómios que se adapta à ciência de computadores consiste em considerá-los
    /// como sendo funçlões do conjunto de inteiros (graus) num conjunto de valores (coeficientes). Porém, esta
    /// definição admite uma generalização óbvia que consiste em considerá-los como sendo funções de um conjunto de
    /// objectos (grau) sobre o qual está definida uma estrutura de monóide sobre um conjunto de valores (coeficientes).
    /// No presente caso, considera-se que essa estrutura seja a de um número inteiro por ser mais útil na
    /// implementação dos vários algoritmos.
    /// </remarks>
    /// <typeparam name="CoeffType">O tipo de coeficiente do polinómio.</typeparam>
    /// <typeparam name="DegreeType">O tipo de grau do polinómio.</typeparam>
    public class GeneralDegUnivarPolynomRing<CoeffType, DegreeType>
        : IRing<GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>>
    {
        /// <summary>
        /// O objecto responsável pelas operações de inteiros sobre os graus.
        /// </summary>
        protected IIntegerNumber<DegreeType> integerNumber;

        /// <summary>
        /// O anel responsável pelas operações sobre os coeficientes.
        /// </summary>
        protected IRing<CoeffType> ring;

        /// <summary>
        /// O nome da variável envolvida nos cálculos.
        /// </summary>
        protected string variableName;

        /// <summary>
        /// Cria uma instância de objectos do tipo <see cref="GeneralDegUnivarPolynomRing{CoeffType, DegreeType}"/>.
        /// </summary>
        /// <param name="variableName">O nome da variável.</param>
        /// <param name="ring">O anel responsável pelas operações sobre os coeficientes.</param>
        /// <param name="integerNumber">O objecto responsável pelas operações sobre os graus.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Se algum dos argumentos form nulo.
        /// </exception>
        /// <exception cref="System.ArgumentException">Se a variável for vazia.</exception>
        public GeneralDegUnivarPolynomRing(
            string variableName, 
            IRing<CoeffType> ring,
            IIntegerNumber<DegreeType> integerNumber)
        {
            if (integerNumber == null)
            {
                throw new ArgumentNullException("integerNumber");
            }
            else if (ring == null)
            {
                throw new ArgumentNullException("ring");
            }
            else if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentException("Variable must not be empty.");
            }
            else
            {
                this.variableName = variableName;
                this.ring = ring;
            }
        }

        /// <summary>
        /// Obtém a unidade aditiva.
        /// </summary>
        /// <value>
        /// A unidade aditiva.
        /// </value>
        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> AdditiveUnity
        {
            get
            {
                return new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.variableName,
                    this.integerNumber);
            }
        }

        /// <summary>
        /// Obtém a unidade multiplicativa.
        /// </summary>
        /// <value>
        /// A unidade multiplicativa.
        /// </value>
        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> MultiplicativeUnity
        {
            get
            {
                return new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.ring.MultiplicativeUnity, 
                    this.integerNumber.AdditiveUnity, 
                    this.variableName,
                    this.ring,
                    this.integerNumber);
            }
        }

        /// <summary>
        /// Determina o inverso aditivo de um polinómio geral.
        /// </summary>
        /// <param name="number">O polinómio.</param>
        /// <returns>O inverso aditivo.</returns>
        /// <exception cref="System.ArgumentNullException">Caso o argumento seja nulo.</exception>
        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> AdditiveInverse(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> number)
        {
            if (number == null)
            {
                throw new ArgumentNullException("number");
            }
            else
            {
                return number.GetSymmetric(this.ring);
            }
        }

        /// <summary>
        /// Determina se o polinómio geral é uma unidade aditiva.
        /// </summary>
        /// <param name="value">O polinómio a ser analisado.</param>
        /// <returns>Veradeiro caso o polinómio seja uma unidade aditiva e falso caso contrário.</returns>
        /// <exception cref="System.ArgumentNullException">Se o arugmento for nulo.</exception>
        public virtual bool IsAdditiveUnity(GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return value.IsZero;
            }
        }

        /// <summary>
        /// Determina o resultado da adição de dois polinómios gerais.
        /// </summary>
        /// <param name="left">O primeiro polinómio a ser adicionado.</param>
        /// <param name="right">O segundo polinómio a ser adicionado.</param>
        /// <returns>O resultado da soma dos dois polinómios.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Se pelo menos um dos argumentos for nulo.
        /// </exception>
        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Add(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> left, 
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> right)
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
                return left.Add(right, this.ring);
            }
        }

        /// <summary>
        /// Determina quando dois números de precisão dupla são iguais.
        /// </summary>
        /// <param name="x">O primeiro polinómio geral a ser comparado.</param>
        /// <param name="y">O segundo polinómio geral a ser comparado.</param>
        /// <returns>
        /// Verdadeiro caso ambos os objectos sejam iguais e falso no caso contrário.
        /// </returns>
        public virtual bool Equals(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> x, 
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null)
            {
                return false;
            }
            else if (y == null)
            {
                return false;
            }
            else
            {
                return x.Equals(y);
            }
        }

        /// <summary>
        /// Retorna um código confuso para a instância.
        /// </summary>
        /// <param name="obj">A instância.</param>
        /// <returns>
        /// Um código confuso para a instância que pode ser usado em vários algoritmos habituais.
        /// </returns>
        public virtual int GetHashCode(GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return obj.GetHashCode();
            }
        }

        /// <summary>
        /// Calcula o produto de dois polinomio gerais.
        /// </summary>
        /// <param name="left">O primeiro polinómio geral a ser multiplicado.</param>
        /// <param name="right">O segundo polinómio geral a ser multiplicado.</param>
        /// <returns>O resultado do produto dos polinómios.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// Se pelo menos um dos argumentos for nulo.
        /// </exception>
        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> Multiply(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> left, 
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> right)
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
                return left.Multiply(right, this.ring);
            }
        }

        /// <summary>
        /// Determina se o polinómio geral é uma unidade multiplicativa.
        /// </summary>
        /// <param name="value">O polinómio a ser analisado.</param>
        /// <returns>Verdadeiro caso o polinómios seja uma unidade multiplicativa e falso caso contrário.</returns>
        /// <exception cref="System.ArgumentNullException">Se o argumento for nulo.</exception>
        public virtual bool IsMultiplicativeUnity(GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            else
            {
                return value.IsUnity(this.ring);
            }
        }

        /// <summary>
        /// Calcula a soma repetida de um polinómio.
        /// </summary>
        /// <param name="element">O polinómio a ser somado.</param>
        /// <param name="times">O número de vezes que a soma é aplicada.</param>
        /// <returns>O resultado da soma repetida.</returns>
        /// <exception cref="System.ArgumentNullException">Se o argumento for nulo.</exception>
        public virtual GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> AddRepeated(
            GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType> element, 
            int times)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            else
            {
                var result = new GeneralDegUnivarPolynomNormalForm<CoeffType, DegreeType>(
                    this.variableName,
                    this.integerNumber);
                foreach (var termsKvp in element)
                {
                    result = result.Add(
                        this.ring.AddRepeated(termsKvp.Value, times),
                        termsKvp.Key, this.ring);
                }

                return result;
            }
        }
    }
}
