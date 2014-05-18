namespace Utilities.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;

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
        /// <param name="objectsEqualityComparer">O comparador para os objectos do conjunto de partida.</param>
        /// <param name="targetsEqualityComparer">O comparador para os objectos do conjunto de chegada.</param>
        public GeneralMapper()
        {
            this.mappObjectToTarget = new Dictionary<ObjectSetType, List<TargetSetType>>();
            this.mappTargetToObject = new Dictionary<TargetSetType, List<ObjectSetType>>();
        }

        /// <summary>
        /// Instancia um novo objecto do tipo <see cref="GeneralMapper{ObjectSetType, TargetSetType}"/>.
        /// </summary>
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
        /// <param name="obj">O objecto..</param>
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
        /// <param name="obj">O objecto.</param>
        /// <returns>Verdadeiro caso o objecto se encontre no conjunto de chegada e falso caso contrário.</returns>
        public bool ContainsTarget(TargetSetType tar)
        {
            return this.mappTargetToObject.ContainsKey(tar);
        }
    }
}
