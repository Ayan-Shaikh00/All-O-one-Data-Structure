
public class AllOne
{
    private class Bucket
    {
        public int Count;
        public HashSet<string> Keys = new HashSet<string>();
        public Bucket Prev, Next;

        public Bucket(int count)
        {
            Count = count;
        }
    }

    private Bucket head, tail;
    private Dictionary<string, Bucket> map;

    public AllOne()
    {
        head = new Bucket(0);
        tail = new Bucket(0);

        head.Next = tail;
        tail.Prev = head;

        map = new Dictionary<string, Bucket>();
    }

    public void Inc(string key)
    {
        if (!map.ContainsKey(key))
        {
            Bucket first = head.Next;

            if (first == tail || first.Count != 1)
            {
                Bucket newBucket = new Bucket(1);
                AddAfter(head, newBucket);
                first = newBucket;
            }

            first.Keys.Add(key);
            map[key] = first;
        }
        else
        {
            Bucket current = map[key];
            Bucket next = current.Next;

            if (next == tail || next.Count != current.Count + 1)
            {
                Bucket newBucket = new Bucket(current.Count + 1);
                AddAfter(current, newBucket);
                next = newBucket;
            }

            next.Keys.Add(key);
            map[key] = next;

            current.Keys.Remove(key);

            if (current.Keys.Count == 0)
                Remove(current);
        }
    }

    public void Dec(string key)
    {
        Bucket current = map[key];

        if (current.Count == 1)
        {
            current.Keys.Remove(key);
            map.Remove(key);
        }
        else
        {
            Bucket prev = current.Prev;

            if (prev == head || prev.Count != current.Count - 1)
            {
                Bucket newBucket = new Bucket(current.Count - 1);
                AddAfter(prev, newBucket);
                prev = newBucket;
            }

            prev.Keys.Add(key);
            map[key] = prev;

            current.Keys.Remove(key);
        }

        if (current.Keys.Count == 0)
            Remove(current);
    }

    public string GetMaxKey()
    {
        if (tail.Prev == head)
            return "";

        return GetAnyKey(tail.Prev);
    }

    public string GetMinKey()
    {
        if (head.Next == tail)
            return "";

        return GetAnyKey(head.Next);
    }

    private void AddAfter(Bucket node, Bucket newNode)
    {
        newNode.Prev = node;
        newNode.Next = node.Next;

        node.Next.Prev = newNode;
        node.Next = newNode;
    }

    private void Remove(Bucket node)
    {
        node.Prev.Next = node.Next;
        node.Next.Prev = node.Prev;
    }

    private string GetAnyKey(Bucket bucket)
    {
        foreach (string key in bucket.Keys)
            return key;

        return "";
    }
}
