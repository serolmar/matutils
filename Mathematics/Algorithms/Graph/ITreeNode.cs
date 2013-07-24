using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mathematics
{
    public interface ITreeNode<NodeObjectType>
    {
        NodeObjectType NodeObject { get; set; }

        IEnumerable<ITreeNode<NodeObjectType>> Childs { get; }
    }
}
