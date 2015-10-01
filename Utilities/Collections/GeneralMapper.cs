namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Repesenta um mapeamento de objecto para imagem.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos.</typeparam>
    /// <typeparam name="TargetType">O tipo das imagens.</typeparam>
    public class ObjectTargetPair<ObjectType, TargetType>
    {
        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ObjectTargetPair{ObjectType, TargetType}"/>.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <param name="tar">A imagem.</param>
        public ObjectTargetPair(ObjectType obj, TargetType tar)
        {
            this.ObjectValue = obj;
            this.TargetValue = tar;
        }

        /// <summary>
        /// Obtém ou atribui o objecto.
        /// </summary>
        /// <value>
        /// O objecto.
        /// </value>
        public ObjectType ObjectValue { get; private set; }

        /// <summary>
        /// Obtém ou atribui a imagem.
        /// </summary>
        /// <value>
        /// A imagem.
        /// </value>
        public TargetType TargetValue { get; private set; }
    }

    /// <summary>
    /// Implementa um mapeador geral entre vários objectos.
    /// </summary>
    /// <remarks>
    /// O mapeador geral permite generalizar o conceito de dicionário uma vez que o mapeamento,
    /// neste caso, é bi-direccional e a cada elemento de um dos conjuntos pode corresponder um ou mais
    /// elementos do outro conjunto.
    /// </remarks>
    /// <typeparam name="ObjectSetType">O tipo dos objectos do conjunto de partida.</typeparam>
    /// <typeparam name="TargetSetType">O tipo de objectos do conjunto de chegada.</typeparam>
    public class GeneralMapper<ObjectSetType, TargetSetType>
    {
        /// <summary>
        /// Permite mapear os objectos do conjunto de partida aos do conjunto de chegada.
        /// </summary>
        private Dictionary<ObjectSetType, List<TargetSetType>> mappObjectToTarget;

        /// <summary>
        /// Permite mapear os objectos do conjunto de chegada aos do conjunto de partida.
        /// </summary>
        private Dictionary<TargetSetType, List<ObjectSetType>> mappTargetToObject;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GeneralMapper{ObjectSetType, TargetSetType}"/>.
        /// </summary>
        public GeneralMapper()
        {
            this.mappObjectToTarget = new Dictionary<ObjectSetType, List<TargetSetType>>();
            this.mappTargetToObject = new Dictionary<TargetSetType, List<ObjectSetType>>();
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GeneralMapper{ObjectSetType, TargetSetType}"/>.
        /// </summary>
        /// <param name="objectsEqualityComparer">O comparador para os objectos do conjunto de partida.</param>
        /// <param name="targetsEqualityComparer">O comparador para os objectos do conjunto de chegada.</param>
        public GeneralMapper(
            IEqualityComparer<ObjectSetType> objectsEqualityComparer,
            IEqualityComparer<TargetSetType> targetsEqualityComparer)
        {
            if (objectsEqualityComparer == null)
            {
                this.mappObjectToTarget = new Dictionary<ObjectSetType, List<TargetSetType>>();
            }
            else
            {
                this.mappObjectToTarget = new Dictionary<ObjectSetType, List<TargetSetType>>(
                    objectsEqualityComparer);
            }

            if (targetsEqualityComparer == null)
            {
                this.mappTargetToObject = new Dictionary<TargetSetType, List<ObjectSetType>>();
            }
            else
            {
                this.mappTargetToObject = new Dictionary<TargetSetType, List<ObjectSetType>>(
                    targetsEqualityComparer);
            }
        }

        /// <summary>
        /// Obtém os objectos do conjunto de partida.
        /// </summary>
        /// <value>
        /// Os objectos do conjunto de partida.
        /// </value>
        public ICollection<ObjectSetType> Objects
        {
            get { return this.mappObjectToTarget.Keys; }
        }

        /// <summary>
        /// Obtém os objectos do conjunto de chegada.
        /// </summary>
        /// <value>
        /// Os objectos do conjunto de chegada.
        /// </value>
        public ICollection<TargetSetType> Targets
        {
            get { return this.mappTargetToObject.Keys; }
        }

        /// <summary>
        /// Adiciona um mapeamento.
        /// </summary>
        /// <param name="obj">O objecto de partida.</param>
        /// <param name="target">O objecto de chegada.</param>
        public void Add(ObjectSetType obj, TargetSetType target)
        {
            List<TargetSetType> targets = null;
            if (this.mappObjectToTarget.TryGetValue(obj, out targets))
            {
                if (!targets.Contains(target))
                {
                    targets.Add(target);
                    List<ObjectSetType> objects = null;
                    if (this.mappTargetToObject.TryGetValue(target, out objects))
                    {
                        objects.Add(obj);
                    }
                    else
                    {
                        this.mappTargetToObject.Add(target, new List<ObjectSetType>() { obj });
                    }
                }
            }
            else
            {
                this.mappObjectToTarget.Add(obj, new List<TargetSetType>() { target });
                List<ObjectSetType> objects = null;
                if (this.mappTargetToObject.TryGetValue(target, out objects))
                {
                    objects.Add(obj);
                }
                else
                {
                    this.mappTargetToObject.Add(target, new List<ObjectSetType>() { obj });
                }
            }
        }

        /// <summary>
        /// Determina o conjunto de objectos do conjunto de chegada que estão mapeados pelo objecto especificado.
        /// </summary>
        /// <param name="obj">O objecto..</param>
        /// <returns>O conjunto de objectos mapeados.</returns>
        public  ReadOnlyCollection<TargetSetType> TargetFor(ObjectSetType obj)
        {
            List<TargetSetType> result = null;
            if (!this.mappObjectToTarget.TryGetValue(obj, out result))
            {
                result = new List<TargetSetType>();
            }

            return result.AsReadOnly();
        }

        /// <summary>
        /// Determina o conjunto de objectos do conjunto de partida que estão mapeados pelo objecto especificado.
        /// </summary>
        /// <param name="target">O objecto..</param>
        /// <returns>O conjunto de objectos mapeados.</returns>
        public ReadOnlyCollection<ObjectSetType> ObjectFor(TargetSetType target)
        {
            List<ObjectSetType> result = null;
            if (!this.mappTargetToObject.TryGetValue(target, out result))
            {
                result = new List<ObjectSetType>();
            }

            return result.AsReadOnly();
        }

        /// <summary>
        /// Determina se um objecto se encontra no conjunto de partida.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro caso o objecto se encontre no conjunto de partida e falso caso contrário.</returns>
        public bool ContainsObject(ObjectSetType obj)
        {
            return this.mappObjectToTarget.ContainsKey(obj);
        }

        /// <summary>
        /// Determina se um objecto se encontra no conjunto de chegada.
        /// </summary>
        /// <param name="target">O objecto.</param>
        /// <returns>Verdadeiro caso o objecto se encontre no conjunto de chegada e falso caso contrário.</returns>
        public bool ContainsTarget(TargetSetType target)
        {
            return this.mappTargetToObject.ContainsKey(target);
        }
    }

    /// <summary>
    /// Estabelece uma correspondência biunívoca entre dois domínios.
    /// </summary>
    /// <typeparam name="ObjectSetType">O tipo de objectos de partida.</typeparam>
    /// <typeparam name="TargetSetType">O tipo dos objectos de chegada.</typeparam>
    public class InjectiveMapper<ObjectSetType, TargetSetType>
        : IEnumerable<ObjectTargetPair<ObjectSetType, TargetSetType>>
    {
        /// <summary>
        /// Realiza o mapeamento directo.
        /// </summary>
        private Dictionary<ObjectSetType, TargetSetType> mappObjectToTarget;

        /// <summary>
        /// Realiza o mapeamento inverso.
        /// </summary>
        private Dictionary<TargetSetType, ObjectSetType> mappTargetToObject;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="InjectiveMapper{ObjectSetType, TargetSetType}"/>.
        /// </summary>
        public InjectiveMapper()
        {
            this.mappObjectToTarget = new Dictionary<ObjectSetType, TargetSetType>();
            this.mappTargetToObject = new Dictionary<TargetSetType, ObjectSetType>();
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="InjectiveMapper{ObjectSetType, TargetSetType}"/>.
        /// </summary>
        /// <param name="objectsEqualityComparer">O comparador para os objectos do conjunto de partida.</param>
        /// <param name="targetsEqualityComparer">O comparador para os objectos do conjunto de chegada.</param>
        public InjectiveMapper(
            IEqualityComparer<ObjectSetType> objectsEqualityComparer,
            IEqualityComparer<TargetSetType> targetsEqualityComparer)
        {
            if (objectsEqualityComparer == null)
            {
                this.mappObjectToTarget = new Dictionary<ObjectSetType, TargetSetType>();
            }
            else
            {
                this.mappObjectToTarget = new Dictionary<ObjectSetType, TargetSetType>(objectsEqualityComparer);
            }

            if (targetsEqualityComparer == null)
            {
                this.mappTargetToObject = new Dictionary<TargetSetType, ObjectSetType>();
            }
            else
            {
                this.mappTargetToObject = new Dictionary<TargetSetType, ObjectSetType>(targetsEqualityComparer);
            }
        }

        /// <summary>
        /// Obtém a colecção dos objectos.
        /// </summary>
        /// <value>
        /// Os objectos.
        /// </value>
        public ICollection<ObjectSetType> Objects
        {
            get { return this.mappObjectToTarget.Keys; }
        }

        /// <summary>
        /// Obtém a colecção das imagens.
        /// </summary>
        /// <value>
        /// As imagens.
        /// </value>
        public ICollection<TargetSetType> Targets
        {
            get { return this.mappTargetToObject.Keys; }
        }

        /// <summary>
        /// Adiciona um mapeamento.
        /// </summary>
        /// <param name="obj">O objecto do mapeamento.</param>
        /// <param name="target">A imagem do mapeamento.</param>
        /// <exception cref="ArgumentException">Se o mapeamento existir.</exception>
        public void Add(ObjectSetType obj, TargetSetType target)
        {
            if (this.mappObjectToTarget.ContainsKey(obj))
            {
                throw new ArgumentException("Object was already mapped to a target");
            }
            if (this.mappTargetToObject.ContainsKey(target))
            {
                throw new ArgumentException("Target was already mapped by an object");
            }
            this.mappObjectToTarget.Add(obj, target);
            this.mappTargetToObject.Add(target, obj);
        }

        /// <summary>
        /// Obtém a imagem do objecto especificado.
        /// </summary>
        /// <param name="obj">O objecto.</param>
        /// <returns>A imagem.</returns>
        /// <exception cref="KeyNotFoundException">Se o mapeamento não existir.</exception>
        public TargetSetType TargetFor(ObjectSetType obj)
        {
            if (!this.mappObjectToTarget.ContainsKey(obj))
            {
                throw new KeyNotFoundException("Object isn't mapped to any target.");
            }
            return this.mappObjectToTarget[obj];
        }

        /// <summary>
        /// Obtém o objecto que está associado à imagem especificada.
        /// </summary>
        /// <param name="target">A imagem.</param>
        /// <returns>O objecto.</returns>
        /// <exception cref="KeyNotFoundException">Se o mapeamento não existir.</exception>
        public ObjectSetType ObjectFor(TargetSetType target)
        {
            if (!this.mappTargetToObject.ContainsKey(target))
            {
                throw new KeyNotFoundException("Object isn't mapped to any target.");
            }
            return this.mappTargetToObject[target];
        }

        /// <summary>
        /// Verifica se o objecto está contido no mapeamento.
        /// </summary>
        /// <param name="obj">O obejcto a ser verificado.</param>
        /// <returns>Verdadeiro caso o objecto esteja contido no mapeamento e falso caso contrário.</returns>
        public bool ContainsObject(ObjectSetType obj)
        {
            return this.mappObjectToTarget.ContainsKey(obj);
        }

        /// <summary>
        /// Verifica se a imagem está contida no mapeamento.
        /// </summary>
        /// <param name="tar">A imagem-</param>
        /// <returns>Verdadeiro caso a imagem esteja no mapeamento e falso caso contrário.</returns>
        public bool ContainsTarget(TargetSetType tar)
        {
            return this.mappTargetToObject.ContainsKey(tar);
        }

        /// <summary>
        /// Obtém um enumerador para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        public IEnumerator<ObjectTargetPair<ObjectSetType, TargetSetType>> GetEnumerator()
        {
            return new ObjectTargetPairEnumerator<ObjectSetType, TargetSetType>(
                this.mappObjectToTarget);
        }

        /// <summary>
        /// Obtém um enumerador não genérico para a colecção.
        /// </summary>
        /// <returns>O enumerador.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Implementa um enumerador para os mapeamentos.
    /// </summary>
    /// <typeparam name="ObjectType">O tipo dos objectos.</typeparam>
    /// <typeparam name="TargetType">O tipo das imagens.</typeparam>
    internal class ObjectTargetPairEnumerator<ObjectType, TargetType>
        : IEnumerator<ObjectTargetPair<ObjectType, TargetType>>
    {
        /// <summary>
        /// O eumerador para o contentor.
        /// </summary>
        private Dictionary<ObjectType, TargetType>.Enumerator currentEnumerator;

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="ObjectTargetPairEnumerator{ObjectType, TargetType}"/>.
        /// </summary>
        /// <param name="mappObjectToTarget">O contentor dos mapeamentos.</param>
        public ObjectTargetPairEnumerator(Dictionary<ObjectType, TargetType> mappObjectToTarget)
        {
            this.currentEnumerator = mappObjectToTarget.GetEnumerator();
        }

        #region IEnumerator<ObjectTargetPair<ObjectType,TargetType>> Members

        /// <summary>
        /// Obtém o elemento da colecção apontado pelo enumerador.
        /// </summary>
        /// <returns>O elemento da colecção apontado pelo enumerador.</returns>
        public ObjectTargetPair<ObjectType, TargetType> Current
        {
            get
            {
                return new ObjectTargetPair<ObjectType, TargetType>(
                                this.currentEnumerator.Current.Key,
                                this.currentEnumerator.Current.Value);
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Descarta o enumerador.
        /// </summary>
        public void Dispose()
        {
            this.currentEnumerator.Dispose();
        }

        #endregion

        #region IEnumerator Members

        /// <summary>
        /// Obtém o elemento da colecção apontado pelo enumerador.
        /// </summary>
        /// <returns>O elemento da colecção apontado pelo enumerador.</returns>
        object System.Collections.IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        /// <summary>
        /// Avança o enumerador para o próximo elemento da colecção.
        /// </summary>
        /// <returns>
        /// Verdadeiro caso o enumerador avance e falso caso se encontre no final da colecção.
        /// </returns>
        public bool MoveNext()
        {
            return this.currentEnumerator.MoveNext();
        }

        /// <summary>
        /// Operação não suportada.
        /// </summary>
        /// <exception cref="CollectionsException">Sempre.</exception>
        public void Reset()
        {
            throw new CollectionsException("Operation is no supported.");
        }

        #endregion
    }
}
