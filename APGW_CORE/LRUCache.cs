using System.Collections.Generic;
using System.Collections.Concurrent;

namespace APGW
{
    public class LRUCache<K,V>
    {
        #region Fields

        private readonly ConcurrentDictionary<K, V> entries;
        private LinkedList<NodeVal> list;
     
        private readonly int capacity;

        #endregion

        private class Node
        {
            public NodeVal Val { get; set; }
        }

        private class NodeVal 
        {
            public K Key { get; set; }
            public V Value { get; set; }

            public NodeVal(K key, V value) {
                Key = key;
                Value = value;
            }
        }

        public LRUCache(int capacity = 32) {
            this.capacity = capacity;
            entries = new ConcurrentDictionary<K, V>();

            list = new LinkedList<NodeVal>();
        }

        public bool GetVal(K key, out V value)
        {
            value = default(V);
            Node entry = new Node();
            entry.Val = new NodeVal(key, value);
       
            if (!entries.TryGetValue(key, out value)) return false;

            LinkedListNode<NodeVal> foundNode = list.Find(entry.Val);

            // Move to head
            if (foundNode != null)
            {
                list.Remove(foundNode);
                list.AddFirst(foundNode);
            }

            return true;
        }

        public void Set(K key, V value)
        {
            V entry = default(V);
            if (!entries.TryGetValue(key, out entry))
            {
                if (entries.Count == capacity)
                {
                    list.RemoveLast();
                }
                entry = value;
                entries.TryAdd(key, entry);
            }

            Node newNode = new Node { Val = new NodeVal(key, entry) };

            list.AddFirst(newNode.Val);
        }
    }
}
