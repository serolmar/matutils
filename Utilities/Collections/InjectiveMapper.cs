using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public class InjectiveMapper<ObjectSetType, TargetSetType>
    {
        private Dictionary<ObjectSetType, TargetSetType> mappObjectToTarget;
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

        public ICollection<ObjectSetType> Objects
        {
            get { return this.mappObjectToTarget.Keys; }
        }

        public ICollection<TargetSetType> Targets
        {
            get { return this.mappTargetToObject.Keys; }
        }

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

        public TargetSetType TargetFor(ObjectSetType obj)
        {
            if (!this.mappObjectToTarget.ContainsKey(obj))
            {
                throw new KeyNotFoundException("Object isn't mapped to any target.");
            }
            return this.mappObjectToTarget[obj];
        }

        public ObjectSetType ObjectFor(TargetSetType target)
        {
            if (!this.mappTargetToObject.ContainsKey(target))
            {
                throw new KeyNotFoundException("Object isn't mapped to any target.");
            }
            return this.mappTargetToObject[target];
        }

        public bool ContainsObject(ObjectSetType obj)
        {
            return this.mappObjectToTarget.ContainsKey(obj);
        }

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
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
