namespace Mathematics
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Utilities;
    using Utilities.Collections;

    /// <summary>
    /// Um contentor de pontos bidimensionais.
    /// </summary>
    /// <typeparam name="FirstCoeffType">
    /// O tipo de coeficientes que constituem as primeiras coordenadas dos pontos.
    /// </typeparam>
    /// <typeparam name="SecondCoeffType">
    /// O tipo de coeficientes que constituem as segundas coordenadas dos pontos.
    /// </typeparam>
    public class PointContainer2D<FirstCoeffType, SecondCoeffType> : IIndexed<int, Tuple<FirstCoeffType, SecondCoeffType>>
    {
        /// <summary>
        /// A lista de pontos.
        /// </summary>
        private List<Tuple<FirstCoeffType, SecondCoeffType>> points;

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PointContainer2D{FirstCoeffType, SecondCoeffType}"/>.
        /// </summary>
        public PointContainer2D()
        {
            this.points = new List<Tuple<FirstCoeffType, SecondCoeffType>>();
        }

        /// <summary>
        /// Instancia uma nova instância de objectos do tipo <see cref="PointContainer2D{FirstCoeffType, SecondCoeffType}"/>
        /// </summary>
        /// <param name="points">O conjunto de pontos.</param>
        public PointContainer2D(IEnumerable<Tuple<FirstCoeffType, SecondCoeffType>> points)
        {
            if (points == null)
            {
                throw new ArgumentNullException("points");
            }
            else
            {
                this.points = new List<Tuple<FirstCoeffType, SecondCoeffType>>();
                foreach (var point in points)
                {
                    if (point == null)
                    {
                        throw new ArgumentException("Null points aren't allowed in container.");
                    }
                    else if (point.Item1 == null || point.Item2 == null)
                    {
                        throw new ArgumentException("Null coordinates aren't allowed for points in container.");
                    }
                    else
                    {
                        this.points.Add(point);
                    }
                }
            }
        }

        /// <summary>
        /// Ocorre antes do elemento ser adicionado ao contentor.
        /// </summary>
        public event AddDeleteEventHandler<Tuple<FirstCoeffType, SecondCoeffType>> BeforeAddEvent;

        /// <summary>
        /// Ocorre depois do elemento ser adicionado ao contentor.
        /// </summary>
        public event AddDeleteEventHandler<Tuple<FirstCoeffType, SecondCoeffType>> AfterAddEvent;

        /// <summary>
        /// Ocorre antes do elemento ser removido do contentor.
        /// </summary>
        public event AddDeleteEventHandler<int> BeforeDeleteEvent;

        /// <summary>
        /// Ocorre depois do elemento ser removido do contentor.
        /// </summary>
        public event AddDeleteEventHandler<int> AfterDeleteEvent;

        /// <summary>
        /// Obtém o ponto que está associado ao índice.
        /// </summary>
        /// <param name="index">O índice.</param>
        /// <returns>O ponto.</returns>
        public Tuple<FirstCoeffType, SecondCoeffType> this[int index]
        {
            get
            {
                return this.points[index];
            }
        }

        /// <summary>
        /// Obtém o número de pontos contido no contentor.
        /// </summary>
        public int Count
        {
            get
            {
                return this.Count;
            }
        }

        /// <summary>
        /// Permite determinar o índice da primeira ocorrência de um ponto com as coordenadas especificadas.
        /// </summary>
        /// <param name="firstCoord">A primeira coordenada do ponto a ser pesquisado.</param>
        /// <param name="secondCoord">A segunda coordenada do ponto a ser pesquisado.</param>
        /// <returns>O índice do ponto no contentor e -1 se o ponto não for encontrado.</returns>
        public int IndexOf(FirstCoeffType firstCoord, SecondCoeffType secondCoord)
        {
            if (firstCoord == null)
            {
                throw new ArgumentNullException("firstCoord");
            }
            else if (secondCoord == null)
            {
                throw new ArgumentNullException("secondCoord");
            }
            else
            {
                return this.points.IndexOf(Tuple.Create(firstCoord, secondCoord));
            }
        }

        /// <summary>
        /// Determina o índice da primeira ocorrência de um ponto cujas coordenadas são comparadas por intermédio
        /// de um comparador personalizado.
        /// </summary>
        /// <param name="firstCoord">A primeira coordenada do ponto a ser pesquisado.</param>
        /// <param name="secondCoord">A segunda coordenada do ponto a ser pesquisado.</param>
        /// <param name="firstCoordsComparer">O comparador das primeiras coordenadas.</param>
        /// <returns>O índice do ponto no contentor e -1 se o ponto não for encontrado.</returns>
        public int IndexOf(
            FirstCoeffType firstCoord,
            FirstCoeffType secondCoord,
            IEqualityComparer<FirstCoeffType> firstCoordsComparer)
        {
            if (firstCoord == null)
            {
                throw new ArgumentNullException("firstCoord");
            }
            else if (secondCoord == null)
            {
                throw new ArgumentNullException("secondCoord");
            }
            else if (firstCoordsComparer == null)
            {
                throw new ArgumentNullException("coordsComparer");
            }
            else
            {
                var index = -1;
                var length = this.points.Count;
                for (int i = 0; i < length; ++i)
                {
                    var point = this.points[i];
                    if (firstCoordsComparer.Equals(point.Item1, firstCoord))
                    {
                        index = i;
                        i = length;
                    }
                }

                return index;
            }
        }

        /// <summary>
        /// Adiciona um ponto ao contentor.
        /// </summary>
        /// <param name="firstCoord">A primeira coordenada do ponto.</param>
        /// <param name="secondCoord">A segunda coordenada do ponto.</param>
        public void Add(FirstCoeffType firstCoord, SecondCoeffType secondCoord)
        {
            if (firstCoord == null)
            {
                throw new ArgumentNullException("firstCoord");
            }
            else if (secondCoord == null)
            {
                throw new ArgumentNullException("secondCoord");
            }
            else
            {
                var point = Tuple.Create(firstCoord, secondCoord);
                if (this.BeforeAddEvent == null)
                {
                    this.points.Add(Tuple.Create(firstCoord, secondCoord));
                    if (this.AfterAddEvent != null)
                    {
                        this.AfterAddEvent.Invoke(
                            this,
                            new AddDeleteEventArgs<Tuple<FirstCoeffType, SecondCoeffType>>(point));
                    }
                }
                else
                {
                    var eventArg = new AddDeleteEventArgs<Tuple<FirstCoeffType, SecondCoeffType>>(point);
                    this.BeforeAddEvent.Invoke(
                        this,
                        eventArg);
                    this.points.Add(point);
                    if (this.AfterAddEvent != null)
                    {
                        this.AfterAddEvent.Invoke(
                            this,
                            eventArg);
                    }
                }
            }
        }

        /// <summary>
        /// Remove o elemneto especificado pelo índice.
        /// </summary>
        /// <param name="index">O índice do elemento a ser removido.</param>
        public void RemoveAt(int index)
        {
            if (index >= 0 && index < this.points.Count)
            {
                if (this.BeforeDeleteEvent == null)
                {
                    this.points.RemoveAt(index);
                }
                else
                {
                    var eventArg = new AddDeleteEventArgs<int>(index);
                    this.BeforeDeleteEvent.Invoke(
                        this,
                        eventArg);
                    this.points.RemoveAt(index);
                    if (this.AfterAddEvent != null)
                    {
                        this.AfterDeleteEvent.Invoke(
                            this,
                            eventArg);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador para os pontos do contentor.
        /// </summary>
        /// <returns>O enumerador para os pontos do contentor.</returns>
        public IEnumerator<Tuple<FirstCoeffType, SecondCoeffType>> GetEnumerator()
        {
            return this.points.GetEnumerator();
        }

        /// <summary>
        /// Obtém o enumerador não genérico para os pontos do contentor.
        /// </summary>
        /// <returns>O enumerador não genérico para os pontos do contentor.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
