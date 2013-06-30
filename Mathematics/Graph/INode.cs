using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface INode<NodeObjectType>
    {
        NodeObjectType NodeObject { get; set; }

        IEnumerable<NodeObjectType> Childs { get; }

        void Add(INode<NodeObjectType> child);

        void Remove(INode<NodeObjectType> child);
    }
}
