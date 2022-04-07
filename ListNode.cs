namespace SaberInteractive
{
    public class ListNode
    {
        public ListNode? Previous;
        public ListNode? Next;
        public ListNode? Random;
        public string Data;
        public ListNode(string data) => Data = data;
    }
}