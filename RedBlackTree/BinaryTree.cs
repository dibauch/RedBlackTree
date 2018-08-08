using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBlackTree
{
    class BinaryTree<T,V> where T : IComparable<T>
    {
        Node<T,V> root;

        public void add(T value, V reference)
        {
            if (root == null)
            {
                root = new Node<T,V>(value, null, reference);
                root.Color = Color.black;
            }
            else
            {
                Position positionForInsert;
                Node<T,V> parent = findParent(value, out positionForInsert);
                Node<T,V> newNode = new Node<T,V>(value, parent, reference);
                if (positionForInsert == Position.left)
                    parent.Left = newNode;
                else
                    parent.Right = newNode;
                rebalanceAfterInsert(newNode);
            }
        }

        public List<V> getAllElements()
        {
            return getAllElementsFromNode(root);
        }

        public List<T> getAllElementKeys()
        {
            return getAllElementKeysFromNode(root);
        }

        List<T> getAllElementKeysFromNode(Node<T,V> n)
        {
            List<T> retList = new List<T>();
            if (n != null)
            {
                if (n.Left != null)
                    retList.InsertRange(0, getAllElementKeysFromNode(n.Left));
                if (n.Right != null)
                    retList.InsertRange(0, getAllElementKeysFromNode(n.Right));
                retList.Add(n.Value);
            }
            return retList;
        }

        List<V> getAllElementsFromNode(Node<T,V> n)
        {
            List<V> retList = new List<V>();
            if (n != null)
            {
                if (n.Left != null)
                    retList.InsertRange(0, getAllElementsFromNode(n.Left));
                if (n.Right != null)
                    retList.InsertRange(0, getAllElementsFromNode(n.Right));
                retList.Add(n.Reference);
            }
            return retList;
        }

        public void show()
        {
            List<Node<T,V>> nodes = new List<Node<T,V>> { root };
            while (nodes.Count != 0)
            {
                List<Node<T,V>> newNodes = new List<Node<T,V>>();
                nodes.ForEach(n =>
                {
                    Console.ForegroundColor = n.Color == Color.red ? ConsoleColor.Red : ConsoleColor.DarkGray;
                    string parent = n.Parent == null ? "" : n.Parent.Value.ToString();
                    Console.Write($"{parent}.{n.Value}.{n.Reference} ");
                    if (n.Left != null)
                        newNodes.Add(n.Left);
                    if (n.Right != null)
                        newNodes.Add(n.Right);
                });
                Console.WriteLine();
                nodes = newNodes;
            }
            Console.WriteLine();
        }

        public Node<T,V> find(T value)
        {
            Node<T,V> curNode = root;
            while (curNode != null && curNode.Value.CompareTo(value) != 0)
            {
                if (curNode.Value.CompareTo(value) > 0)
                    curNode = curNode.Left;
                else
                    curNode = curNode.Right;
            }
            return curNode;
        }

        public void remove(T key)
        {
            Node<T, V> node = find(key);
            if (node != null)
            {
                Node<T, V> parent = node.Parent;
                if (node.Left != null && node.Right != null)
                {
                    Node<T, V> child = node.Right;
                    while (child.Left != null)
                        child = child.Left;
                    node.Value = child.Value;
                    node.Reference = child.Reference;
                    Node<T, V> childParent = child.Parent;
                    if (childParent.Left == child)
                        childParent.Left = null;
                    else
                        childParent.Right = null;
                }
                else
                {
                    Node<T, V> child = null;
                    if (node.Left == null && node.Right == null)
                    {
                        if (parent == null)
                            root = null;
                        else if (parent.Left == node)
                            parent.Left = null;
                        else
                            parent.Right = null;
                    }
                    else
                    {
                        child = node.Left == null ? node.Right : node.Left;
                        if (parent == null)
                            root = child;
                        else if (parent.Left == node)
                            parent.Left = child;
                        else
                            parent.Right = child;
                        child.Parent = parent;
                    }
                    if (node.Color == Color.black)
                        if (child != null && child.Color == Color.red)
                            child.Color = Color.black;
                        else rebalanceAfterRemove(child);
                }
            }
        }

        void rebalanceAfterRemove(Node<T,V> n)
        {
            if (n != null && n.Parent != null)
                deleteCase2(n);
        }

        void deleteCase2(Node<T,V> n)
        {
            Node<T,V> brother = n.getBrother();
            Node<T,V> parent = n.Parent;

            if (brother.Color == Color.red)
            {
                parent.Color = Color.red;
                brother.Color = Color.black;
                if (n == parent.Left)
                    leftRotate(parent);
                else
                    rightRotate(parent);
                deleteCase3(n);
            }
        }

        void deleteCase3(Node<T,V> n)
        {
            Node<T,V> brother = n.getBrother();
            Node<T,V> parent = n.Parent;
            if (parent.Color == Color.black && brother.Color == Color.black && brother.Left.Color == Color.black && brother.Right.Color == Color.black)
            {
                brother.Color = Color.red;
                rebalanceAfterRemove(parent);
            }
            else
                deleteCase4(n);
        }

        void deleteCase4(Node<T,V> n)
        {
            Node<T,V> brother = n.getBrother();
            Node<T,V> parent = n.Parent;
            if (parent.Color == Color.red && brother.Color == Color.black && brother.Left.Color == Color.black && brother.Right.Color == Color.black)
            {
                brother.Color = Color.red;
                parent.Color = Color.black;
            }
            else
                deleteCase5(n);
        }

        void deleteCase5(Node<T,V> n)
        {
            Node<T,V> brother = n.getBrother();
            Node<T,V> parent = n.Parent;
            if (brother.Color == Color.black)
            {
                if (n == parent.Left && brother.Right.Color == Color.black && brother.Left.Color == Color.red)
                {
                    brother.Color = Color.red;
                    brother.Left.Color = Color.black;
                    rightRotate(brother);
                }
                else if (n == parent.Right && brother.Left.Color == Color.black && brother.Right.Color == Color.red)
                {
                    brother.Color = Color.red;
                    brother.Right.Color = Color.black;
                    leftRotate(brother);
                }
            }
            deleteCase6(n);
        }

        void deleteCase6(Node<T,V> n)
        {
            Node<T,V> brother = n.getBrother();
            Node<T,V> parent = n.Parent;
            brother.Color = parent.Color;
            parent.Color = Color.black;
            if (n == n.Parent.Left)
            {
                brother.Right.Color = Color.black;
                leftRotate(parent);
            }
            else
            {
                brother.Left.Color = Color.black;
                rightRotate(parent);
            }
        }

        void rebalanceAfterInsert(Node<T,V> node)
        {
            Node<T,V> parent = node.Parent;
            Node<T,V> uncle = node.getUncle();
            Node<T,V> grandFather = node.getGrandFather();
            if (parent == null)
                node.Color = Color.black;
            else
            {
                if (parent.Color == Color.black)
                    return;
                else
                {
                    if (uncle != null && uncle.Color == Color.red)
                    {
                        parent.Color = Color.black;
                        uncle.Color = Color.black;
                        grandFather.Color = Color.red;
                        rebalanceAfterInsert(grandFather);
                    }
                    else
                    {
                        if (grandFather != null)
                        {
                            if (parent.Right == node && parent == grandFather.Left)
                                leftRotate(parent);
                            else if (parent.Left == node && parent == grandFather.Right)
                                rightRotate(parent);
                            parent.Color = Color.black;
                            grandFather.Color = Color.red;
                            if (node == parent.Left && parent == grandFather.Left)
                                rightRotate(grandFather);
                            else if (node == parent.Right && parent == grandFather.Right)
                                leftRotate(grandFather);
                        }
                    }
                }
            }
        }
        
        void  leftRotate(Node<T,V> node)
        {
            Node<T,V> newRoot = node.Right;
            node.Right = newRoot.Left;
            if (newRoot.Left != null)
                newRoot.Left.Parent = node;
            newRoot.Left = node;
            newRoot.Parent = node.Parent;
            node.Parent = newRoot;
            if (newRoot.Parent == null)
                root = newRoot;
            else
            {
                Node<T,V> parent = newRoot.Parent;
                if (parent.Right == node)
                    parent.Right = newRoot;
                else
                    parent.Left = newRoot;
            }
        }

        void rightRotate(Node<T,V> node)
        {
            Node<T,V> newRoot = node.Left;
            node.Left = newRoot.Right;
            if (newRoot.Right != null)
                newRoot.Right.Parent = node;
            newRoot.Right = node;
            newRoot.Parent = node.Parent;
            node.Parent = newRoot;
            if (newRoot.Parent == null)
                root = newRoot;
            else
            {
                Node<T,V> parent = newRoot.Parent;
                if (parent.Right == node)
                    parent.Right = newRoot;
                else
                    parent.Left = newRoot;
            }
        }

        Node<T,V> findParent(T value, out Position position)
        {
            Node<T,V> curRoot = root;
            while(true)
            { 
                if (curRoot.Value.CompareTo(value) > 0)
                {
                    if (curRoot.Left == null)
                    {
                        position = Position.left;
                        break;
                    }
                    else
                        curRoot = curRoot.Left;
                }
                else if (curRoot.Value.CompareTo(value) < 0)
                {
                    if (curRoot.Right == null)
                    {
                        position = Position.right;
                        break;
                    }
                    else
                        curRoot = curRoot.Right;
                }
                else
                {
                    position = Position.here;
                    break;
                }
            }
            return curRoot;
        }

    }
}
