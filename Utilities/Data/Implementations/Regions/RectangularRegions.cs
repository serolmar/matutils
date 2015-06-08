namespace Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Representa uma região rectangular
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem as coordenadas da região rectangular.</typeparam>
    public class RectangularRegion<T>
    {
        /// <summary>
        /// A coordenada x do canto superior esquerdo.
        /// </summary>
        private T topLeftX;

        /// <summary>
        /// A coordenada y do canto superior esquerdo.
        /// </summary>
        private T topLeftY;

        /// <summary>
        /// A coordenada x do canto inferiror direito.
        /// </summary>
        private T bottomRightX;

        /// <summary>
        /// A coordenada y do canto inferior direito.
        /// </summary>
        private T bottomRightY;

        /// <summary>
        /// Define o comparador de elementos utilizado nos algoritmos.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="RectangularRegion{T}"/>
        /// </summary>
        public RectangularRegion()
        {
            this.topLeftX = default(T);
            this.topLeftY = default(T);
            this.bottomRightX = default(T);
            this.bottomRightY = default(T);
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="RectangularRegion{T}"/>
        /// </summary>
        /// <param name="topLeftX">A coordenada x do canto superior esquerdo.</param>
        /// <param name="topLeftY">A coordenada y do canto superior esquerdo.</param>
        /// <param name="bottomRightX">A coordenada x do canto inferior direito.</param>
        /// <param name="bottomRightY">A coordenada y do canto inferior direito.</param>
        public RectangularRegion(
            T topLeftX,
            T topLeftY,
            T bottomRightX,
            T bottomRightY)
        {
            this.comparer = Comparer<T>.Default;
            if (this.comparer.Compare(topLeftX, bottomRightX) > 0)
            {
                throw new ArgumentException(
                    "The top left x coordinate must not be greater than the x coordinate of the bottom right corner.");
            }
            else if (this.comparer.Compare(topLeftY, bottomRightY) > 0)
            {
                throw new ArgumentException(
                    "The top left y coordinate must not be greater than the y coordinate of the bottom right corner.");
            }
            else
            {
                this.topLeftX = topLeftX;
                this.topLeftY = topLeftY;
                this.bottomRightX = bottomRightX;
                this.bottomRightY = bottomRightY;
            }
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="RectangularRegion{T}"/>
        /// </summary>
        /// <param name="topLeftX">A coordenada x do canto superior esquerdo.</param>
        /// <param name="topLeftY">A coordenada y do canto superior esquerdo.</param>
        /// <param name="bottomRightX">A coordenada x do canto inferior direito.</param>
        /// <param name="bottomRightY">A coordenada y do canto inferior direito.</param>
        /// <param name="comparer">O comparador responsável pela comparação das coordenadas.</param>
        public RectangularRegion(
            T topLeftX,
            T topLeftY,
            T bottomRightX,
            T bottomRightY,
            IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.comparer = comparer;
                if (this.comparer.Compare(topLeftX, bottomRightX) > 0)
                {
                    throw new ArgumentException(
                        "The top left x coordinate must not be greater than the x coordinate of the bottom right corner.");
                }
                else if (this.comparer.Compare(topLeftY, bottomRightY) > 0)
                {
                    throw new ArgumentException(
                        "The top left y coordinate must not be greater than the y coordinate of the bottom right corner.");
                }
                else
                {
                    this.topLeftX = topLeftX;
                    this.topLeftY = topLeftY;
                    this.bottomRightX = bottomRightX;
                    this.bottomRightY = bottomRightY;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da coordenada x do canto superior esquerdo.
        /// </summary>
        public T TopLeftX
        {
            get
            {
                return this.topLeftX;
            }
            set
            {
                if (this.comparer.Compare(value, bottomRightX) > 0)
                {
                    throw new ArgumentException(
                    "The top left x coordinate must not be greater than the x coordinate of the bottom right corner.");
                }
                else
                {
                    this.topLeftX = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui o valor da coordenda y do canto superior esquerdo.
        /// </summary>
        public T TopLeftY
        {
            get
            {
                return this.topLeftY;
            }
            set
            {
                if (this.comparer.Compare(value, this.bottomRightY) > 0)
                {
                    throw new ArgumentException(
                        "The top left y coordinate must not be greater than the y coordinate of the bottom right corner.");
                }
                else
                {
                    this.topLeftY = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui a coordenada x do canto inferior direito.
        /// </summary>
        public T BottomRightX
        {
            get
            {
                return this.bottomRightX;
            }
            set
            {
                if (this.comparer.Compare(value, this.topLeftX) < 0)
                {
                    throw new ArgumentException(
                       "The top left x coordinate must not be greater than the x coordinate of the bottom right corner.");
                }
                else
                {
                    this.bottomRightX = value;
                }
            }
        }

        /// <summary>
        /// Obtém ou atribui a coordenada y do canto inferior direito.
        /// </summary>
        public T BottomRightY
        {
            get
            {
                return this.bottomRightY;
            }
            set
            {
                if (this.comparer.Compare(value, this.topLeftY) < 0)
                {
                    throw new ArgumentException(
                        "The top left y coordinate must not be greater than the y coordinate of the bottom right corner.");
                }
                else
                {
                    this.bottomRightY = value;
                }
            }
        }

        /// <summary>
        /// Verifica se existe sobreposição das regiões rectangulares.
        /// </summary>
        /// <param name="other">A outra região rectangular.</param>
        /// <returns>Verdadeiro caso se dê sobreposição e falso caso contrário.</returns>
        public bool OverLaps(RectangularRegion<T> other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                if (this.comparer.Compare(this.topLeftX, other.topLeftX) < 0)
                {
                    if (this.comparer.Compare(this.topLeftY, other.bottomRightY) <= 0)
                    {
                        if (this.comparer.Compare(this.bottomRightX, other.topLeftX) >= 0 &&
                            this.comparer.Compare(this.bottomRightY, other.topLeftY) >= 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (this.comparer.Compare(this.topLeftX, other.bottomRightX) <= 0)
                {
                    if (this.comparer.Compare(this.topLeftY, other.bottomRightY) <= 0)
                    {
                        if (this.comparer.Compare(this.bottomRightX, other.topLeftX) >= 0 &&
                            this.comparer.Compare(this.bottomRightY, other.topLeftY) >= 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém a intersecção da região rectangular actual com a região rectangular proporcionada.
        /// </summary>
        /// <remarks>
        /// A função retorna um rectângulo sem área sempre que exista uma fronteira
        /// em comum. Se não se verificar sobreposição, a função irá retornar um nulo.
        /// </remarks>
        /// <param name="other">A região rectangular a ser intersectada.</param>
        /// <returns>O resultado da intersecção.</returns>
        public RectangularRegion<T> Intersect(RectangularRegion<T> other)
        {
            if (other == null)
            {
                return null;
            }
            else
            {
                if (this.comparer.Compare(this.topLeftX, other.topLeftX) <= 0)
                {
                    if (this.comparer.Compare(this.topLeftY, other.topLeftY) <= 0)
                    {
                        if (this.comparer.Compare(this.bottomRightX, other.topLeftX) >= 0)
                        {
                            if (this.comparer.Compare(this.bottomRightY, other.topLeftY) >= 0)
                            {
                                var rectTopLeftX = other.topLeftX;
                                var rectTopLeftY = other.topLeftY;
                                var rectBottomRightX = this.bottomRightX;
                                var rectBottomRightY = this.bottomRightY;

                                if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                                {
                                    rectBottomRightX = other.bottomRightX;
                                }

                                if (this.comparer.Compare(this.bottomRightY, other.bottomRightY) > 0)
                                {
                                    rectBottomRightY = other.bottomRightY;
                                }

                                return new RectangularRegion<T>(
                                    rectTopLeftX,
                                    rectTopLeftY,
                                    rectBottomRightX,
                                    rectBottomRightY);
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (this.comparer.Compare(this.topLeftY, other.bottomRightY) <= 0)
                    {
                        var rectTopLeftX = other.topLeftX;
                        var rectTopLeftY = this.topLeftY;
                        var rectBottomRightX = this.bottomRightX;
                        var rectBottomRightY = this.bottomRightY;

                        if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                        {
                            rectBottomRightX = other.bottomRightX;
                        }

                        if (this.comparer.Compare(this.bottomRightY, other.bottomRightY) > 0)
                        {
                            rectBottomRightY = other.bottomRightY;
                        }

                        return new RectangularRegion<T>(
                                    rectTopLeftX,
                                    rectTopLeftY,
                                    rectBottomRightX,
                                    rectBottomRightY);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (this.comparer.Compare(this.topLeftX, other.bottomRightX) <= 0)
                {
                    if (this.comparer.Compare(this.topLeftY, other.topLeftY) <= 0)
                    {
                        if (this.comparer.Compare(this.bottomRightY, other.topLeftY) >= 0)
                        {
                            var rectTopLeftX = this.topLeftX;
                            var rectTopLeftY = other.topLeftY;
                            var rectBottomRightX = this.bottomRightX;
                            var rectBottomRightY = this.bottomRightY;

                            if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                            {
                                rectBottomRightX = other.bottomRightX;
                            }

                            if (this.comparer.Compare(this.bottomRightY, other.bottomRightY) > 0)
                            {
                                rectBottomRightY = other.bottomRightY;
                            }

                            return new RectangularRegion<T>(
                                        rectTopLeftX,
                                        rectTopLeftY,
                                        rectBottomRightX,
                                        rectBottomRightY);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else if (this.comparer.Compare(this.topLeftY, other.bottomRightY) <= 0)
                    {
                        var rectTopLeftX = this.topLeftX;
                        var rectTopLeftY = this.topLeftY;
                        var rectBottomRightX = this.bottomRightX;
                        var rectBottomRightY = this.bottomRightY;

                        if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                        {
                            rectBottomRightX = other.bottomRightX;
                        }

                        if (this.comparer.Compare(this.bottomRightY, other.bottomRightY) > 0)
                        {
                            rectBottomRightY = other.bottomRightY;
                        }

                        return new RectangularRegion<T>(
                            rectTopLeftX,
                            rectTopLeftY,
                            rectBottomRightX,
                            rectBottomRightY);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Obtém uma lista de regiões rectangulares que resultam da subtracção
        /// da região proporcionada à região actual.
        /// </summary>
        /// <param name="other">A região a subtrair.</param>
        /// <param name="increment">A função que permite determinar o incremento.</param>
        /// <param name="decrement">A função que permite determinar o decremento.</param>
        /// <returns>
        /// O conjunto de regiões que cobrem a região actual com excepção da região definida.
        /// </returns>
        public List<RectangularRegion<T>> Subtract(
            RectangularRegion<T> other, 
            Func<T,T> increment,
            Func<T, T> decrement)
        {
            if (decrement == null)
            {
                throw new ArgumentNullException("decrement");
            }
            else
            {
                var result = new List<RectangularRegion<T>>();
                if (other == null)
                {
                    result.Add(this);
                }
                else
                {
                    if (this.comparer.Compare(this.topLeftX, other.topLeftX) < 0)
                    {
                        if (this.comparer.Compare(this.topLeftY, other.topLeftY) < 0)
                        {
                            if (this.comparer.Compare(this.bottomRightX, other.topLeftX) >= 0)
                            {
                                if (this.comparer.Compare(this.bottomRightY, other.topLeftY) >= 0)
                                {
                                    var otherTopLeftXDecremented = decrement.Invoke(other.topLeftX);
                                    var otherTopLeftYDecremented = decrement.Invoke(other.topLeftY);

                                    // Introdução do rectângulo horizontal superior
                                    result.Add(new RectangularRegion<T>(
                                        this.topLeftX,
                                        this.topLeftY,
                                        this.bottomRightX,
                                        otherTopLeftYDecremented));
                                    
                                    // Introdução do rectângulo vertical esquerdo
                                    result.Add(new RectangularRegion<T>(
                                        this.topLeftX,
                                        other.topLeftY,
                                        otherTopLeftXDecremented,
                                        this.bottomRightY));

                                    if (this.comparer.Compare(this.bottomRightY, other.bottomRightY) > 0)
                                    {
                                        // Introdução do rectângulo horizontal inferior
                                        var otherBottomRightYIncremented = increment.Invoke(other.bottomRightY);
                                        result.Add(new RectangularRegion<T>(
                                            other.topLeftX,
                                            otherBottomRightYIncremented,
                                            this.bottomRightX,
                                            this.bottomRightY));

                                        if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                                        {
                                            // Introdução do rectângulo vertical direito
                                            var otherBottomRightXIncremented = increment.Invoke(other.bottomRightX);
                                            result.Add(new RectangularRegion<T>(
                                                otherBottomRightXIncremented,
                                                other.topLeftY,
                                                this.bottomRightX,
                                                other.bottomRightY));
                                        }
                                    }
                                    else if(this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                                    {
                                        // Introdução do rectângulo vertical direito
                                        var otherBottomRightXIncremented = increment.Invoke(other.bottomRightX);
                                        result.Add(new RectangularRegion<T>(
                                            otherBottomRightXIncremented,
                                            other.topLeftY,
                                            this.bottomRightX,
                                            this.bottomRightY));
                                    }
                                }
                                else
                                {
                                    result.Add(this);
                                }
                            }
                            else
                            {
                                result.Add(this);
                            }
                        }
                        else if (this.comparer.Compare(this.topLeftY, other.bottomRightY) <= 0)
                        {
                            // Não há rectângulo horizontal superior
                            var otherTopLeftXDecremented = decrement.Invoke(other.topLeftX);
                            var otherTopLeftYDecremented = decrement.Invoke(other.topLeftY);

                            // Introdução do rectângulo vertical esquerdo
                            result.Add(new RectangularRegion<T>(
                                this.topLeftX,
                                this.topLeftY,
                                otherTopLeftXDecremented,
                                this.bottomRightY));

                            if (this.comparer.Compare(this.bottomRightY, other.bottomRightY) > 0)
                            {
                                // Introdução do rectângulo horizontal inferior
                                var otherBottomRightYIncremented = increment.Invoke(other.bottomRightY);
                                result.Add(new RectangularRegion<T>(
                                    other.topLeftX,
                                    otherBottomRightYIncremented,
                                    this.bottomRightX,
                                    this.bottomRightY));

                                if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                                {
                                    // Introdução do rectângulo vertical direito
                                    var otherBottomRightXIncremented = increment.Invoke(other.bottomRightX);
                                    result.Add(new RectangularRegion<T>(
                                        otherBottomRightXIncremented,
                                        this.topLeftY,
                                        this.bottomRightX,
                                        other.bottomRightY));
                                }
                            }
                            else if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                            {
                                // Introdução do rectângulo vertical direito
                                var otherBottomRightXIncremented = increment.Invoke(other.bottomRightX);
                                result.Add(new RectangularRegion<T>(
                                    otherBottomRightXIncremented,
                                    this.topLeftY,
                                    this.bottomRightX,
                                    this.bottomRightY));
                            }
                        }
                        else
                        {
                            result.Add(this);
                        }
                    }
                    else if (this.comparer.Compare(this.topLeftX, other.bottomRightX) <= 0)
                    {
                        // Não existe rectângulo horizontal superior
                        if (this.comparer.Compare(this.topLeftY, other.topLeftY) < 0)
                        {
                            if (this.comparer.Compare(this.bottomRightY, other.topLeftY) >= 0)
                            {
                                var otherTopLeftXDecremented = decrement.Invoke(other.topLeftX);
                                var otherTopLeftYDecremented = decrement.Invoke(other.topLeftY);

                                // Introdução do rectângulo vertical esquerdo
                                result.Add(new RectangularRegion<T>(
                                    this.topLeftX,
                                    this.topLeftY,
                                    otherTopLeftXDecremented,
                                    this.bottomRightY));

                                if (this.comparer.Compare(this.bottomRightY, other.bottomRightY) > 0)
                                {
                                    // Introdução do rectângulo horizontal inferior
                                    var otherBottomRightYIncremented = increment.Invoke(other.bottomRightY);
                                    result.Add(new RectangularRegion<T>(
                                        other.topLeftX,
                                        otherBottomRightYIncremented,
                                        this.bottomRightX,
                                        this.bottomRightY));

                                    if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                                    {
                                        // Introdução do rectângulo vertical direito
                                        var otherBottomRightXIncremented = increment.Invoke(other.bottomRightX);
                                        result.Add(new RectangularRegion<T>(
                                            otherBottomRightXIncremented,
                                            this.topLeftY,
                                            this.bottomRightX,
                                            other.bottomRightY));
                                    }
                                }
                                else if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                                {
                                    // Introdução do rectângulo vertical direito
                                    var otherBottomRightXIncremented = increment.Invoke(other.bottomRightX);
                                    result.Add(new RectangularRegion<T>(
                                        otherBottomRightXIncremented,
                                        this.topLeftY,
                                        this.bottomRightX,
                                        this.bottomRightY));
                                }
                            }
                            else
                            {
                                result.Add(this);
                            }
                        }
                        else if (this.comparer.Compare(this.topLeftY, other.bottomRightY) <= 0)
                        {
                            // Não há rectângulo horizontal superior nem rectângulo vertical esquerdo
                            if (this.comparer.Compare(this.bottomRightY, other.bottomRightY) > 0)
                            {
                                // Introdução do rectângulo horizontal inferior
                                var otherBottomRightYIncremented = increment.Invoke(other.bottomRightY);
                                result.Add(new RectangularRegion<T>(
                                    this.topLeftX,
                                    otherBottomRightYIncremented,
                                    this.bottomRightX,
                                    this.bottomRightY));

                                if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                                {
                                    // Introdução do rectângulo vertical direito
                                    var otherBottomRightXIncremented = increment.Invoke(other.bottomRightX);
                                    result.Add(new RectangularRegion<T>(
                                        otherBottomRightXIncremented,
                                        this.topLeftY,
                                        this.bottomRightX,
                                        other.bottomRightY));
                                }
                            }
                            else if (this.comparer.Compare(this.bottomRightX, other.bottomRightX) > 0)
                            {
                                // Introdução do rectângulo vertical direito
                                var otherBottomRightXIncremented = increment.Invoke(other.bottomRightX);
                                result.Add(new RectangularRegion<T>(
                                    otherBottomRightXIncremented,
                                    this.topLeftY,
                                    this.bottomRightX,
                                    this.bottomRightY));
                            }
                        }
                        else
                        {
                            result.Add(this);
                        }
                    }
                    else
                    {
                        result.Add(this);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Funde duas regiões rectangulares.
        /// </summary>
        /// <remarks>
        /// Duas regiões rectangulares podem ser fundidas caso sejam adjacentes
        /// e os lados de adjacência possuam o mesmo comprimento. A região actual
        /// absorve a região proporcionada caso se dê intersecção.
        /// </remarks>
        /// <param name="other">A região a ser fundida.</param>
        /// <returns>O para região fundida / parte restante da outra região.</returns>
        public Tuple<RectangularRegion<T>, RectangularRegion<T>> Union(
            RectangularRegion<T> other)
        {
            if (other == null)
            {
                return Tuple.Create<RectangularRegion<T>, RectangularRegion<T>>(this, null);
            }
            else
            {
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// Determina se o objecto proporcionado é igual ao objecto corrente.
        /// </summary>
        /// <param name="obj">O objecto a comparar.</param>
        /// <returns>Veradeiro se os objectos forem iguais e falso caso contrário.</returns>
        public override bool Equals(object obj)
        {
            var innerObj = obj as RectangularRegion<T>;
            if (innerObj == null)
            {
                return false;
            }
            else
            {
                var equalityComparer = EqualityComparer<T>.Default;
                if (equalityComparer.Equals(
                    this.topLeftX,
                    innerObj.topLeftX))
                {
                    if (equalityComparer.Equals(
                    this.topLeftY,
                    innerObj.topLeftY))
                    {
                        if (equalityComparer.Equals(
                            this.bottomRightX,
                            innerObj.bottomRightX))
                        {
                            return equalityComparer.Equals(
                                    this.bottomRightY,
                                    innerObj.bottomRightY);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Obtém o código confuso associado ao objecto.
        /// </summary>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode()
        {
            var comparer = EqualityComparer<T>.Default;
            var h1 = comparer.GetHashCode(this.topLeftX);
            var h2 = comparer.GetHashCode(this.topLeftY);
            var hres1 = (((h1 << 5) + h1) ^ h2);
            var h3 = comparer.GetHashCode(this.bottomRightX);
            var h4 = comparer.GetHashCode(this.bottomRightY);
            var hres2 = (((h3 << 5) + h3) ^ h4);
            return (((hres1 << 5) + hres1) ^ hres2);
        }

        /// <summary>
        /// Obtém a representação textual do objecto.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append("{");
            resultBuilder.AppendFormat(
                "top_left: ({0},{1}), bottom_right: ({2}, {3})",
                this.topLeftX,
                this.topLeftY,
                this.bottomRightX,
                this.bottomRightY);
            resultBuilder.Append("}");
            return resultBuilder.ToString();
        }
    }

    /// <summary>
    /// Permite determinar se duas regiões rectangulares são iguais.
    /// </summary>
    /// <typeparam name="T">O tipo de objectos que constituem as entradas das regiões rectangulares.</typeparam>
    public class RectangleEqualityComparer<T> : EqualityComparer<RectangularRegion<T>>
    {
        /// <summary>
        /// Mantém um comparador de coordenadas.
        /// </summary>
        private IEqualityComparer<T> equalityComparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="RectangleEqualityComparer{T}"/>.
        /// </summary>
        /// <remarks>
        /// As coordenadas serão comparadas com base no comparador por defeito.
        /// </remarks>
        public RectangleEqualityComparer()
        {
            this.equalityComparer = EqualityComparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="RectangleEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="equalityComparer">O comparador de coordenadas.</param>
        public RectangleEqualityComparer(IEqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null)
            {
                throw new ArgumentNullException("equalityComparer");
            }
            else
            {
                this.equalityComparer = equalityComparer;
            }
        }

        /// <summary>
        /// Determina se duas regiões rectangulares são iguais.
        /// </summary>
        /// <param name="x">O primeiro rectângulo.</param>
        /// <param name="y">O segundo rectângulo.</param>
        /// <returns>Verdadeiro caso as regiões rectangulares sejam iguais e falso caso contrário.</returns>
        public override bool Equals(RectangularRegion<T> x, RectangularRegion<T> y)
        {
            if (object.ReferenceEquals(x, y))
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
                return this.equalityComparer.Equals(x.TopLeftX, y.TopLeftX) &&
                    this.equalityComparer.Equals(x.TopLeftY, y.TopLeftY) &&
                    this.equalityComparer.Equals(x.BottomRightX, y.BottomRightX) &&
                    this.equalityComparer.Equals(x.BottomRightY, y.BottomRightY);
            }
        }

        /// <summary>
        /// Obtém o código confuso para a região rectangular.
        /// </summary>
        /// <param name="obj">A região rectangular.</param>
        /// <returns>O código confuso.</returns>
        public override int GetHashCode(RectangularRegion<T> obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                var h1 = this.equalityComparer.GetHashCode(obj.TopLeftX);
                var h2 = this.equalityComparer.GetHashCode(obj.TopLeftY);
                var hres1 = (((h1 << 5) + h1) ^ h2);
                var h3 = this.equalityComparer.GetHashCode(obj.BottomRightX);
                var h4 = this.equalityComparer.GetHashCode(obj.BottomRightY);
                var hres2 = (((h3 << 5) + h3) ^ h4);
                return (((hres1 << 5) + hres1) ^ hres2);
            }
        }
    }

    /// <summary>
    /// Permite comparar duas regiões rectangulares.
    /// </summary>
    /// <typeparam name="T">O tipo dos objectos que constituem as coordenadas das regiões rectangulares.</typeparam>
    public class RectangleComparer<T> : Comparer<RectangularRegion<T>>
    {
        /// <summary>
        /// Mantém o comparador das coordenadas.
        /// </summary>
        private IComparer<T> comparer;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="RectangleComparer{T}"/>.
        /// </summary>
        /// <remarks>
        /// A comparação das coordenadas será efectuada com o comparador definido por defeito.
        /// </remarks>
        public RectangleComparer()
        {
            this.comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="RectangleComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">O comparador para as coordenadas.</param>
        public RectangleComparer(IComparer<T> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            else
            {
                this.comparer = comparer;
            }
        }

        /// <summary>
        /// Compara duas regiões rectangulares.
        /// </summary>
        /// <param name="x">A primeira região rectangular.</param>
        /// <param name="y">A segunda região rectangular.</param>
        /// <returns>
        /// O valor -1 se a primeira região rectangular for inferior à segunda, 0 se ambas forem iguais e 
        /// 1 caso contrário.
        /// </returns>
        public override int Compare(RectangularRegion<T> x, RectangularRegion<T> y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return 0;
            }
            else if (x == null)
            {
                return -1;
            }
            else if (y == null)
            {
                return 1;
            }
            else
            {
                var comparision = this.comparer.Compare(x.TopLeftX, y.TopLeftX);
                if (comparision == -1 || comparision == 1)
                {
                    return comparision;
                }
                else
                {
                    comparision = this.comparer.Compare(x.TopLeftY, y.TopLeftY);
                    if (comparision == -1 || comparision == 1)
                    {
                        return comparision;
                    }
                    else
                    {
                        comparision = this.comparer.Compare(x.BottomRightX, y.BottomRightX);
                        if (comparision == -1 || comparision == 1)
                        {
                            return comparision;
                        }
                        else
                        {
                            comparision = this.comparer.Compare(x.BottomRightY, y.BottomRightY);
                            return comparision;
                        }
                    }
                }
            }
        }
    }
}
