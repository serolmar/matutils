namespace Mathematics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Permite somar, subtrair e comparar números máximos, à semelhança dos números complexos.
    /// </summary>
    class TransportationMaxDoubleNumberField
    {
        /// <summary>
        /// Obtém ou atribui a parte finita.
        /// </summary>
        /// <value>
        /// A parte finita.
        /// </value>
        double FiniteComponent { get; set; }

        /// <summary>
        /// Obtém ou atribui a parte inifinita.
        /// </summary>
        /// <value>
        /// A parte infinita.
        /// </value>
        double MaximumComponent { get; set; }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TransportationMaxDoubleNumberField"/>.
        /// </summary>
        public TransportationMaxDoubleNumberField() { }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="TransportationMaxDoubleNumberField"/>.
        /// </summary>
        /// <param name="enteringValue">O valor.</param>
        public TransportationMaxDoubleNumberField(double enteringValue)
        {
            if (enteringValue == double.MaxValue)
            {
                this.MaximumComponent = 1;
            }
            else if (enteringValue == double.MinValue)
            {
                this.MaximumComponent = -1;
            }
            else
            {
                this.FiniteComponent = enteringValue;
            }
        }

        /// <summary>
        /// Implementa o operador +.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static TransportationMaxDoubleNumberField operator +(
            TransportationMaxDoubleNumberField left, 
            TransportationMaxDoubleNumberField right)
        {
            return new TransportationMaxDoubleNumberField()
            {
                FiniteComponent = left.FiniteComponent + right.FiniteComponent,
                MaximumComponent = left.MaximumComponent + right.MaximumComponent
            };
        }

        /// <summary>
        /// Implementa o operador -.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static TransportationMaxDoubleNumberField operator -(
            TransportationMaxDoubleNumberField left, 
            TransportationMaxDoubleNumberField right)
        {
            return new TransportationMaxDoubleNumberField()
            {
                FiniteComponent = left.FiniteComponent - right.FiniteComponent,
                MaximumComponent = left.MaximumComponent - right.MaximumComponent
            };
        }

        /// <summary>
        /// Implementa o operador -.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static TransportationMaxDoubleNumberField operator -(
            double left, 
            TransportationMaxDoubleNumberField right)
        {
            if (left == double.MaxValue)
            {
                return new TransportationMaxDoubleNumberField()
                {
                    FiniteComponent = -right.FiniteComponent,
                    MaximumComponent = 1 - right.MaximumComponent
                };
            }
            else if (left == double.MinValue)
            {
                return new TransportationMaxDoubleNumberField()
                {
                    FiniteComponent = -right.FiniteComponent,
                    MaximumComponent = -1 - right.MaximumComponent
                };
            }
            else
            {
                return new TransportationMaxDoubleNumberField()
                {
                    FiniteComponent = left - right.FiniteComponent,
                    MaximumComponent = -right.MaximumComponent
                };
            }
        }

        /// <summary>
        /// Implementa o operador &gt;.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static bool operator >(
            TransportationMaxDoubleNumberField left, 
            TransportationMaxDoubleNumberField right)
        {
            return left.MaximumComponent > right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent > right.FiniteComponent);
        }

        /// <summary>
        /// Implementa o operador &gt;.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static bool operator >(TransportationMaxDoubleNumberField left, double right)
        {
            return (left.MaximumComponent > 1 && right == double.MaxValue) ||
                (left.MaximumComponent == 1 && right != double.MaxValue) ||
                (left.MaximumComponent > -1 && right == double.MinValue) ||
                (left.MaximumComponent == 0 && left.FiniteComponent > right);
        }

        /// <summary>
        /// Implementa o operador &gt;=.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static bool operator >=(
            TransportationMaxDoubleNumberField left, 
            TransportationMaxDoubleNumberField right)
        {
            return left.MaximumComponent >= right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent >= right.FiniteComponent);
        }

        /// <summary>
        /// Implementa o operador &lt;.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static bool operator <(
            TransportationMaxDoubleNumberField left, 
            TransportationMaxDoubleNumberField right)
        {
            return left.MaximumComponent < right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent < right.FiniteComponent);
        }

        /// <summary>
        /// Implementa o operador &lt;.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static bool operator <(TransportationMaxDoubleNumberField left, double right)
        {
            return (left.MaximumComponent < 1 && right == double.MaxValue) ||
                (left.MaximumComponent < -1 && right == double.MinValue) ||
                (left.MaximumComponent == -1 && right != double.MinValue) ||
                (left.MaximumComponent == 0 && left.FiniteComponent < right);
        }

        /// <summary>
        /// Implementa o operador &lt;=.
        /// </summary>
        /// <param name="left">O primeiro argumento do operador.</param>
        /// <param name="right">O segundo argumento do operador.</param>
        /// <returns>
        /// O resultado do operador.
        /// </returns>
        public static bool operator <=(
            TransportationMaxDoubleNumberField left, 
            TransportationMaxDoubleNumberField right)
        {
            return left.MaximumComponent <= right.MaximumComponent ||
                (left.MaximumComponent == right.MaximumComponent && left.FiniteComponent <= right.FiniteComponent);
        }

        /// <summary>
        /// Constrói uma repesentação textual da instância corrente.
        /// </summary>
        /// <returns>A representação textual.</returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Determina se o objecto proporcionado é igual à instância corrente.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>
        /// Verdadeiro caso o objecto seja igual e falso caso contrário.
        /// </returns>
        public override bool Equals(object obj)
        {
            TransportationMaxDoubleNumberField objToCompare = obj as TransportationMaxDoubleNumberField;
            if (objToCompare == null)
            {
                return base.Equals(obj);
            }
            else
            {
                return this.MaximumComponent == objToCompare.MaximumComponent && 
                    this.FiniteComponent == objToCompare.FiniteComponent;
            }
        }

        /// <summary>
        /// Retorna um código confuso para a instância corrente.
        /// </summary>
        /// <returns>
        /// O código confuso da instância corrente que é utilizado em alguns algoritmos.
        /// </returns>
        public override int GetHashCode()
        {
            return (this.MaximumComponent.GetHashCode() ^ this.FiniteComponent.GetHashCode()).GetHashCode();
        }
    }
}
