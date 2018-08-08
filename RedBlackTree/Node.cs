using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    enum Color { red, black }
    enum Position { left, right, here}

    class Node<T, V>
    {
        public Color Color { get; set; }
        public Node<T,V> Parent { get; set; }
        public Node<T,V> Left { get; set; }
        public Node<T,V> Right { get; set; }
        public T Value { get; set; }
        public V Reference { get; set; }

        public Node<T,V> getUncle()
        {
            Node<T,V> grandFather = getGrandFather();
            if (grandFather == null)
                return null;
            if (grandFather.Left == Parent)
                return grandFather.Right;
            else if (grandFather.Right == Parent)
                return grandFather.Left;
            return null;
        }

        public Node<T,V> getGrandFather()
        {
            if (Parent != null)
                return Parent.Parent;
            else
                return null;
        }

        public Node<T,V> getBrother()
        {
            if (Parent != null)
            {
                if (Parent.Left == this)
                    return Parent.Right;
                else
                    return Parent.Left;
            }
            else
                return null;
        }

        public Node(T value, Node<T,V> parent, V reference)
        {
            Value = value;
            Color = Color.red;
            Parent = parent;
            Reference = reference;
        }
    }
}
