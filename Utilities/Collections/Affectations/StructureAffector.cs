using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    /// <summary>
    /// Itera sobre todas as afectações recorrendo a uma estrutura na qual é possível identificar
    /// quais as posições possíveis para afectar os vários índices.
    /// </summary>
    public class StructureAffector : FastAfector
    {
        /// <summary>
        /// A matriz das afectações.
        /// </summary>
        protected int[][] affectorMatrix;

        /// <summary>
        /// Número de possíveis afectações por índice.
        /// </summary>
        private Dictionary<int, int> numberOfPossibleAffectationsByIndice;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="StructureAffector"/>.
        /// </summary>
        /// <param name="affectorStructure">A matriz de afectação.</param>
        /// <exception cref="System.ArgumentNullException">Se a matriz de afectação for nula.</exception>
        /// <exception cref="System.ArgumentException">Se a matriz de afectação for vazia.</exception>
        public StructureAffector(ICollection<ICollection<int>> affectorStructure)
        {
            if (affectorStructure == null)
            {
                throw new ArgumentNullException("affectorStructure");
            }

            if (affectorStructure.Count == 0)
            {
                throw new ArgumentException("Parameter collection affectorStructure must have elements to affect.");
            }

            this.affectorMatrix = new int[affectorStructure.Count][];
            int pointer = 0;
            InsertionSortedCollection<int> sorter = new InsertionSortedCollection<int>(Comparer<int>.Default);

            var counter = 0;
            foreach (var item in affectorStructure)
            {
                sorter.Clear();
                foreach (var innerItem in item)
                {
                    if (!sorter.HasElement(innerItem))
                    {
                        sorter.InsertSortElement(innerItem);
                    }
                }

                this.affectorMatrix[pointer++] = sorter.ToArray();
                counter += sorter.Count;
            }

            this.count = counter;
            this.numberOfPlaces = this.affectorMatrix.Length;
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="StructureAffector"/>.
        /// </summary>
        /// <param name="affectorStructure">A matriz de afectação.</param>
        /// <param name="possibleAffectionsByIndice">O número de afectações por índice.</param>
        /// <exception cref="System.ArgumentException">
        /// Se algum número de afectaçoes por índice não for positivo.
        /// </exception>
        public StructureAffector(
            ICollection<ICollection<int>> affectorStructure, 
            Dictionary<int, int> possibleAffectionsByIndice) : 
            this(affectorStructure)
        {
            if (possibleAffectionsByIndice != null)
            {
                this.numberOfPossibleAffectationsByIndice = new Dictionary<int,int>();
                foreach (var item in possibleAffectionsByIndice)
                {
                    if (item.Value < 0)
                    {
                        throw new ArgumentException("Every element in parameter possibleAffectationsByIndices must be greater than zero.");
                    }
                    else
                    {
                        this.numberOfPossibleAffectationsByIndice.Add(item.Key, item.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Obtém um enumerador para o afectador.
        /// </summary>
        /// <returns>
        /// O enumerador.
        /// </returns>
        public override IEnumerator<int[]> GetEnumerator()
        {
            return new StructureAffectorEnumerator(this);
        }

        /// <summary>
        /// Implementa um enumerador para o afectador.
        /// </summary>
        public class StructureAffectorEnumerator : FastAffectorEnumerator
        {
            /// <summary>
            /// Os índices afectados.
            /// </summary>
            private Dictionary<int, int> affectedIndices = new Dictionary<int, int>();

            /// <summary>
            /// Instancia um novo objecto do tipo <see cref="StructureAffectorEnumerator"/>.
            /// </summary>
            /// <param name="structureAffector">O afectador.</param>
            public StructureAffectorEnumerator(StructureAffector structureAffector) : base(structureAffector)
            {
                this.currentAffectationIndices = new int[structureAffector.NumberOfPlaces];
            }

            /// <summary>
            /// Inicializa o índice actual.
            /// </summary>
            protected override void ResetPointedIndex()
            {
                this.currentAffectationIndices[this.currentPointer] = 0;
            }

            /// <summary>
            /// Verifica a validade da afectação actual.
            /// </summary>
            /// <returns>
            /// Verdadeiro caso a afectação seja vaálida e falso caso contrário.
            /// </returns>
            protected override bool CheckForCurrAffectIndicesValidity()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                return this.currentAffectationIndices[this.currentPointer] != affector.affectorMatrix[this.currentPointer].Length;
            }

            /// <summary>
            /// Verifica se o elemento corrente constitui ou não uma repetição.
            /// </summary>
            /// <returns>
            /// Verdadeiro caso existam repetições e falso caso contrário.
            /// </returns>
            protected override bool VerifyRepetitions()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                int indexBeingAffected = affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]];

                if (affector.numberOfPossibleAffectationsByIndice != null &&
                    affector.numberOfPossibleAffectationsByIndice.ContainsKey(indexBeingAffected))
                {
                    if (!this.affectedIndices.ContainsKey(indexBeingAffected))
                    {
                        if (affector.numberOfPossibleAffectationsByIndice[indexBeingAffected] == 0)
                        {
                            return false;
                        }
                        else
                        {
                            this.affectedIndices.Add(indexBeingAffected, 0);
                        }
                    }

                    if (this.affectedIndices[indexBeingAffected] == affector.numberOfPossibleAffectationsByIndice[indexBeingAffected])
                    {
                        return false;
                    }
                }
                else
                {
                    if (this.affectedIndices.ContainsKey(indexBeingAffected))
                    {
                        if (this.affectedIndices[indexBeingAffected] == 1)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            /// <summary>
            /// Incremente o índice actual.
            /// </summary>
            /// <returns>
            /// Verdadeiro se o índice pode ser incrementado e falso caso contrário.
            /// </returns>
            protected override bool IncrementCurrent()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                ++this.currentAffectationIndices[this.currentPointer];
                if (this.currentAffectationIndices[this.currentPointer] == affector.affectorMatrix[this.currentPointer].Length) return false;
                return true;
            }

            /// <summary>
            /// Incrementa o contador para a verificação de repetições.
            /// </summary>
            protected override void IncrementAffectations()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                if (this.affectedIndices.ContainsKey(affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]]))
                {
                    ++this.affectedIndices[affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]]];
                }
                else
                {
                    this.affectedIndices.Add(affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]], 1);
                }
            }

            /// <summary>
            /// Decremente o contador para verificação de repetições.
            /// </summary>
            protected override void DecrementAffectations()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                if (this.affectedIndices.ContainsKey(affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]]))
                {
                    --this.affectedIndices[affector.affectorMatrix[this.currentPointer][this.currentAffectationIndices[this.currentPointer]]];
                }
            }

            /// <summary>
            /// Obtém a afectação actual.
            /// </summary>
            /// <returns>
            /// A afectação.
            /// </returns>
            /// <exception cref="System.Exception">
            /// Se o enumerador se encontrar antes do início ao após o final da colecção.
            /// </exception>
            protected override int[] GetCurrent()
            {
                StructureAffector affector = this.thisFastAffector as StructureAffector;
                if (this.isBeforeStart)
                {
                    throw new CollectionsException("Enumerator is in \"IsBeforeStart\" status.");
                }

                if (this.isAfterEnd)
                {
                    throw new CollectionsException("Enumerator is in \"IsAfterEnd\" status.");
                }

                int[] result = new int[this.currentAffectationIndices.Length];
                for (int i = 0; i < this.currentAffectationIndices.Length; ++i)
                {
                    result[i] =  affector.affectorMatrix[i][this.currentAffectationIndices[i]];
                }

                return result;
            }
        }
    }
}
