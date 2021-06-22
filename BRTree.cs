using System;
using System.Collections.Generic;
using System.IO;


namespace ASD___4
{
    /// <summary>
    /// Drzewo BR (Black-Red)
    /// </summary>
    public class BRTree
    {
        public class Tree
        {
            Node _root;
            int _count;
            public int count { get { return _count; } }


            public Node SearchAtLeastSimilar(Data item)
            {
                Node temp = _root;
                while (temp != null)
                {
                    if (temp.data.CompareTo(item) == 0)
                        break;
                    if (temp.data.CompareTo(item) > 0)
                    {
                        if (temp.left == null)
                            break;
                        temp = temp.left;
                    }
                    else
                    {
                        if (temp.right == null)
                            break;
                        temp = temp.right;
                    }
                }
                return temp;
            }

            public Node Search(Data item)
            {
                Node temp = _root;
                while (temp != null)
                {
                    if (temp.data.CompareTo(item) == 0)
                        break;
                    if (temp.data.CompareTo(item) > 0)
                        temp = temp.left;
                    else
                        temp = temp.right;
                }
                return temp;
            }

            // Wrapper.
            public void Insert(Data item)
            {
                _count++;
                Node new_item = new Node(item);
                if (this._root == null)
                {
                    new_item.color = Color.Black;
                    this._root = new_item;
                    return;
                }
                this.BasicInsert(_root, new_item);
                FixRedRed(new_item);
            }
            Node BasicInsert(Node root, Node new_item)
            {
                if (root == null)
                {
                    root = new_item;
                    return root;
                }
                if (root.data.CompareTo(new_item.data) > 0)
                {
                    root.left = BasicInsert(root.left, new_item);
                    root.left.parent = root;
                }
                else if (root.data.CompareTo(new_item.data) < 0)
                {
                    root.right = BasicInsert(root.right, new_item);
                    root.right.parent = root;
                }
                return root;
            }
            // Wrapper for it's private counterpart.
            public void Remove(Data item)
            {
                Node v = this.Search(item);
                if (v == null)
                    return;
                this.Remove(v);
                _count--;
            }
            void Remove(Node v)
            {
                Node u = BST_Replace(v);
                bool vu_black = v.color == Color.Black && (u == null || u.color == Color.Black);
                if (u == null)
                {
                    if (v.data.CompareTo(this._root.data) == 0)
                        this._root = null;
                    else
                    {
                        if (vu_black)
                            FixDoubleBlack(v);
                        else
                        {
                            if (v.Sibling != null)
                            {
                                v.Sibling.color = Color.Red;
                            }
                        }
                        if (v.IsRightChild)
                            v.parent.right = null;
                        else
                            v.parent.left = null;
                    }
                    return;
                }
                if (v.left == null || v.right == null)
                {
                    if (v == this._root)
                    {
                        v.data = u.data;
                        v.left = null;
                        v.right = null;
                    }
                    else
                    {
                        if (v.IsRightChild)
                            v.parent.right = u;
                        else
                            v.parent.left = u;
                        u.parent = v.parent;
                        if (vu_black)
                            FixDoubleBlack(u);
                        else
                            u.color = Color.Black;
                    }
                    return;
                }
                this.SwapData(u, v);
                this.Remove(u);
            }

            void FixDoubleBlack(Node v)
            {
                if (v == this._root)
                    return;
                Node sibling = v.Sibling, parent = v.parent;
                if (sibling == null)
                    FixDoubleBlack(parent);
                else
                {
                    if (sibling.color == Color.Red)
                    {
                        parent.color = Color.Red;
                        sibling.color = Color.Black;
                        if (sibling.IsRightChild)
                            RotateLeft(parent);
                        else
                            RotateRight(parent);
                        FixDoubleBlack(v);
                    }
                    else
                    {
                        if (sibling.HasRedChild)
                        {
                            if (sibling.right != null && sibling.right.color == Color.Red)
                            {
                                if (sibling.IsRightChild)
                                {
                                    sibling.right.color = sibling.color;
                                    sibling.color = parent.color;
                                    RotateLeft(parent);
                                }
                                else
                                {
                                    sibling.right.color = parent.color;
                                    RotateLeft(sibling);
                                    RotateRight(parent);
                                }
                            }
                            else
                            {
                                if (sibling.IsRightChild)
                                {
                                    sibling.left.color = parent.color;
                                    RotateRight(sibling);
                                    RotateLeft(parent);
                                }
                                else
                                {
                                    sibling.left.color = sibling.color;
                                    sibling.color = parent.color;
                                    RotateRight(parent);
                                }
                            }
                            parent.color = Color.Black;
                        }
                        else
                        {
                            sibling.color = Color.Red;
                            if (parent.color == Color.Black)
                                FixDoubleBlack(parent);
                            else
                                parent.color = Color.Black;
                        }
                    }
                }
            }

            Node BST_Replace(Node n)
            {
                if (n.left != null && n.right != null)
                    return this.GetSuccessor(n.left);
                if (n.left == null && n.right == null)
                    return null;
                if (n.right == null)
                    return n.right;
                else
                    return n.left;
            }

            Node GetSuccessor(Node n)
            {
                while (n.right != null)
                    n = n.right;
                return n;
            }

            void SwapData(Node n1, Node n2)
            {
                Data temp = n1.data;
                n1.data = n2.data;
                n2.data = temp;
            }

            void FixRedRed(Node x)
            {
                if (x.data.CompareTo(this._root.data) == 0)
                {
                    x.color = Color.Black;
                    return;
                }
                Node parent = x.parent, grandfather = parent.parent, uncle = x.GetUncle();
                if (parent.color == Color.Red)
                {
                    if (uncle != null && uncle.color == Color.Red)
                    {
                        parent.color = Color.Black;
                        uncle.color = Color.Black;
                        grandfather.color = Color.Red;
                        FixRedRed(grandfather);
                    }
                    else
                    {

                        if (parent.IsRightChild)
                        {
                            if (x.IsRightChild)
                            {
                                this.ColorSwap(grandfather, parent);
                            }
                            else
                            {
                                this.RotateRight(parent);
                                this.ColorSwap(grandfather, x);
                            }
                            this.RotateLeft(grandfather);
                        }
                        else
                        {
                            if (!x.IsRightChild)
                                this.ColorSwap(grandfather, parent);
                            else
                            {
                                this.RotateLeft(parent);
                                this.ColorSwap(grandfather, x);
                            }
                            this.RotateRight(grandfather);
                        }

                    }
                }
            }

            void RotateLeft(Node r)
            {
                Node new_parent = r.right, temp = new_parent.left;
                if (r == this._root)
                    this._root = new_parent;
                r.MoveDown(new_parent);
                r.right = temp;
                if (temp != null)
                    temp.parent = r;
                new_parent.left = r;
            }

            void RotateRight(Node r)
            {
                Node new_parent = r.left, temp = new_parent.right;
                if (r == this._root)
                    this._root = new_parent;
                r.MoveDown(new_parent);
                r.left = temp;
                if (temp != null)
                    temp.parent = r;
                new_parent.right = r;
            }

            void ColorSwap(Node a, Node b)
            {
                Color temp = a.color;
                a.color = b.color;
                b.color = temp;
            }


            // Works on simple insert soo any file inputed to this method should be fine.
            public void LoadFromFile(string filePath)
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"There is no file named ({filePath})");
                using (var binReader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    int dataCount = binReader.ReadInt32();
                    Data_FreeAccess newData;
                    for (int i = 0; i < dataCount; i++)
                    {
                        newData.surname = binReader.ReadString();
                        newData.name = binReader.ReadString();
                        newData.address = binReader.ReadString();
                        int phoneNumberCount = binReader.ReadByte();
                        newData.phoneNumbers = new int[phoneNumberCount];
                        for (int ii = 0; ii < phoneNumberCount; ii++)
                            newData.phoneNumbers[ii] = binReader.ReadInt32();
                        Insert(new Data(newData));
                    }
                }
            }

            public void SaveToFile_ByLevel(string filePath)
            {
                if (_root == null)
                    throw new NullReferenceException("tree is empty...");
                if (!File.Exists(filePath))
                    File.WriteAllText(filePath, "");
                using (var binWriter = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                {
                    binWriter.Write(count);

                    Node node = _root;
                    var toPrint = new Queue<Node>();
                    toPrint.Enqueue(node);
                    while (toPrint.Count > 0)
                    {
                        node = toPrint.Dequeue();
                        if (node.left != null)
                            toPrint.Enqueue(node.left);
                        if (node.right != null)
                            toPrint.Enqueue(node.right);

                        // Writing to file:
                        binWriter.Write(node.data.surname);
                        binWriter.Write(node.data.name);
                        binWriter.Write(node.data.address);
                        binWriter.Write((byte)node.data.phoneNumbers.Length);
                        foreach (int phoneNumber in node.data.phoneNumbers)
                            binWriter.Write(phoneNumber);
                    }
                }
            }
        }


        public enum Color { Red, Black }

        public class Node
        {
            public Data data;
            // Generalnie to te deklaracje można wywalić i zastąpić je zwyczajnie niżej robiąc np. right {get;set;}
            //  chyba to zostawiłem tylko po to by później można było zmienić accessory bez zmieniania api.
            Node _left, _right, _parent;
            Color _color;

            public Node right  { get => this._right;  set => this._right = value; }
            public Node left   { get => this._left;   set => this._left = value; }
            public Node parent { get => this._parent; set => this._parent = value; }
            public Color color { get => this._color;  set => this._color = value; }

            public Node Sibling
            {
                get
                {
                    if (this._parent == null)
                        return null;
                    if (this.IsRightChild)
                        return this._parent.left;
                    return this._parent._right;
                }
            }

            public bool HasRedChild
                => (   (this._right != null && this._right.color == Color.Red)
                    || (this._left != null && this._left.color == Color.Red) );
            public bool IsRightChild { get { return this == this._parent._right; } }

            public Node(Data data)
            {
                this.data = data;
                _left = null;
                _right = null;
                _parent = null;
                _color = Color.Red;
            }

            public Node GetUncle()
            {
                if (this._parent == null || this._parent._parent == null)
                    return null;
                else if (this._parent.IsRightChild)
                    return this._parent._parent._left;
                return this._parent._parent._right;
            }

            public void MoveDown(Node newParent)
            {
                if (this._parent != null)
                {
                    if (this.IsRightChild)
                        this._parent.right = newParent;
                    this._parent.left = newParent;
                }
                newParent.parent = this._parent;
                this._parent = newParent;
            }


            public static bool operator ==(Node n1, Node n2)
            {
                if (n1 is null && n2 is null)
                    return true;
                if (n1 is null || n2 is null)
                    return false;
                if (n1.data.CompareTo(n2.data) == 0)
                    return true;
                return false;
            }
            public static bool operator !=(Node n1, Node n2) => !(n1 == n2);

            // For debug.
            public new string ToString
                => this.data + " " + this._color +
                    " => L: " + (this._left != null ? this._left.data.ToString() : "-") +
                    " R: "    + (this._right != null ? this._right.data.ToString() : "-") +
                    " P: "    + (this._parent != null ? this._parent.data.ToString() : "-");
        }
    }
}
/*
  Rules:
    1. Node is either red or black.
    2. The root and leaves (NILs also) are black.
    3. If node is red it's children are black.
    4. All paths to it's leaves contain same number of black nodes.
*/