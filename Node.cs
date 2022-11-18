using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    class Node
    {
        public Node Next;
        public Node Prev;
        public int element { get; set; }

        public Node(Node next, Node prev, int element)
        {
            Next = next;
            Prev = prev;
            this.element = element;
        }
    }
}