using System;

namespace RedBlackTree
{
    class Program
    {
        static void Main(string[] args)
        {
            BinaryTree<int, int> tree = new BinaryTree<int, int>();
            tree.add(1, 3);
            tree.add(3, 6);
            tree.add(6, 7);
            tree.add(8, 2);
            tree.add(9, 3);
            tree.add(11, 2);
            tree.add(12, 3);
            tree.add(15, 6);
            tree.show();
            tree.remove(3);
            tree.show();
            Console.ReadLine();
        }
    }
}
