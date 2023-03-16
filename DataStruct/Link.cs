using System;

namespace DataStruct
{
    public class Link<T>
    {
        public Link()
        {
            head = new Node<T>();
            tail = head;
        }
        public Node<T> head;
        public Node<T> tail;

        public void Append(T val)
        {
            Node<T> n = new Node<T>();
            n.Value = val;
            n.Prev = tail;
            tail.Next = n;
            tail = n;
        }
    }

    public class Node<T>
    {
        public Node<T> Prev;
        public Node<T> Next;
        public T Value;

    }
}
