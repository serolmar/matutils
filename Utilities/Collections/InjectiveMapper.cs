using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    /// <summary>
    /// Estabelece uma correspondência biunívoca entre dois domínios.
    /// </summary>
    /// <typeparam name="ObjectSetType">O tipo de objectos de partida.</typeparam>
    /// <typeparam name="TargetSetType">O tipo dos objectos de chegada.</typeparam>
    public class InjectiveMapper<ObjectSetType, TargetSetType>
    {
        /// <summary>
        /// Realiza o mapeamento directo.
        /// </summary>
        private Dictionary<ObjectSetType, TargetSetType> mappObjectToTarget;

        /// <summary>
        /// Realiza o mapeamento inverso.
        /// </summary>
        private Dictionary<TargetSetType, ObjectSetType> mappTargetToObject;

        public InjectiveMapper()
        {
            this.mappObjectToTarget = new Dictionary<ObjectSetType, TargetSetType>();
            this.mappTargetToObject = new Dictionary<TargetSetType, ObjectSetType>();
        }

        public InjectiveMapper(
            IEqualityComparer<ObjectSetType> objectsEqualityComparer,
            IEqualityComparer<TargetSetType> targetsEqualityComparer)
        {
            this.mappObjectToTarget = new Dictionary<ObjectSetType, TargetSetType>(objectsEqualityComparer);
            this.mappTargetToObject = new Dictionary<TargetSetType, ObjectSetType>(targetsEqualityComparer);
        }

        /// <summary>
        /// Obtém a colecção dos objectos.
        /// </summary>
        public ICollection<ObjectSetType> Objects
        {
            get { return this.mappObjectToTarget.Keys; }
        }

        /// <summary>
        /// Obtém a colecção das imagens.
        /// </summary>
        public ICollection<TargetSetType> Targets
        {
            get { return this.mappTargetToObject.Keys; }
        }

        /// <summary>
        /// Adiciona um mapeamento.
        /// </summary>
        /// <param name="obj">O objecto do mapeamento.</param>
        /// <param name="target">A imagem do mapeamento.</param>
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
    }

    public class ObjectTargetPair<ObjectType, TargetType>
    {
        public ObjectTargetPair(ObjectType obj, TargetType tar)
        {
            this.ObjectValue = obj;
            this.TargetValue = tar;
        }

        public ObjectType ObjectValue { get; private set; }
        public TargetType TargetValue { get; private set; }
    }

    class ObjectTargetPairEnumerator<ObjectType, TargetType> : IEnumerator<ObjectTargetPair<ObjectType, TargetType>>
    {
        private Dictionary<ObjectType, TargetType>.Enumerator currentEnumerator;

        public ObjectTargetPairEnumerator(Dictionary<ObjectType, TargetType> mappObjectToTarget)
        {
            this.currentEnumerator = mappObjectToTarget.GetEnumerator();
        }

        #region IEnumerator<ObjectTargetPair<ObjectType,TargetType>> Members

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

        public void Dispose()
        {
            this.currentEnumerator.Dispose();
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return this.Current;
            }
        }

        public bool MoveNext()
        {
            return this.currentEnumerator.MoveNext();
        }

        public void Reset()
        {
            throw new CollectionsException("Operation is no supported.");
        }

        #endregion
    }
}
