using System.Collections;
using System.Text;

namespace SaberInteractive
{
    public class ListRandom : IEnumerable<ListNode>
    {
        private ListNode? _head;
        public ListNode? Head => _head;

        private ListNode? _tail;
        public ListNode? Tail => _tail;

        private int _count = 0;
        public int Count => _count;

        private readonly Random _random = new();

        public void Serialize(Stream s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            try
            {
                Dictionary<ListNode, int> listNodeDictionary = this.Select((listNode, index) => (listNode, index))
                    .ToDictionary(keySelector => keySelector.listNode, elementSelector => elementSelector.index);

                using BinaryWriter binaryWriter = new(s, Encoding.UTF8);

                foreach (ListNode listNode in this)
                {
                    binaryWriter.Write(listNode.Data);
                    binaryWriter.Write(listNodeDictionary[listNode.Random]);
                }
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Serialize error:", exception);
            }
        }

        public void Deserialize(Stream s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            try
            {
                _head = null;
                _tail = null;
                _count = 0;

                Dictionary<ListNode, int> listNodeDictionary = new();

                using BinaryReader binaryReader = new(s, Encoding.UTF8);

                while (binaryReader.BaseStream.Position != s.Length)
                {
                    ListNode listNodeRead = new(binaryReader.ReadString()) { Previous = this.LastOrDefault() };

                    listNodeDictionary.Add(listNodeRead, binaryReader.ReadInt32());

                    AddListNode(listNodeRead);
                }

                foreach (ListNode listNode in this)
                    listNode.Random = this.ElementAt(listNodeDictionary[listNode]);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException("Deserialize error:", exception);
            }
        }

        private void AddListNode(ListNode listNode, bool random = false)
        {
            _count++;

            if (_tail == null)
            {
                _head = listNode;
                _head.Random = Head;
                _tail = listNode;
            }
            else
            {
                _tail.Next = listNode;
                listNode.Previous = _tail;
                _tail = listNode;
                if (random) listNode.Random = GetRandomNode();
            }
        }

        public void Add(string data) => AddListNode(new ListNode(data), true);

        private ListNode? GetRandomNode()
        {
            if (_head == null || _tail == null) return null;

            int rand = _random.Next(_count);

            ListNode? result;

            if (rand > (_count / 2))
            {
                result = _tail;
                for (var i = 0; i < (_count - rand); i++)
                    result = result?.Previous;
            }
            else
            {
                result = _head;
                for (var i = 0; i < rand; i++)
                    result = result?.Next;
            }

            return result;
        }

        public IEnumerator<ListNode> GetEnumerator()
        {
            ListNode? current = _head;
            while (current != null)
            {
                yield return current;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
