using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Utilities.Collections
{
    public class GeneralMapper<ObjectSetType, TargetSetType>
    {
        private Dictionary<ObjectSetType, List<TargetSetType>> mappObjectToTarget;
        private Dictionary<TargetSetType, List<ObjectSetType>> mappTargetToObject;

        public GeneralMapper()
        {
            this.mappObjectToTarget = new Dictionary<ObjectSetType, List<TargetSetType>>();
            this.mappTargetToObject = new Dictionary<TargetSetType, List<ObjectSetType>>();
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

        public  ReadOnlyCollection<TargetSetType> TargetFor(ObjectSetType obj)
        {
            List<TargetSetType> result = null;
            if (!this.mappObjectToTarget.TryGetValue(obj, out result))
            {
                result = new List<TargetSetType>();
            }

            return result.AsReadOnly();
        }

        public ReadOnlyCollection<ObjectSetType> ObjectFor(TargetSetType target)
        {
            List<ObjectSetType> result = null;
            if (!this.mappTargetToObject.TryGetValue(target, out result))
            {
                result = new List<ObjectSetType>();
            }

            return result.AsReadOnly();
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
}
