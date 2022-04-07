using SaberInteractive;

ListRandom ListRandom = new();
Random Random = new();

int lenghtListRandom = 100;
int randomNumber = Random.Next(lenghtListRandom + 1);

for (int i = 0; i < lenghtListRandom; i++) ListRandom.Add(i.ToString());

ListRandom.Serialize(new FileStream("DoublyLinked", FileMode.Create));
Console.WriteLine(ListRandom.ElementAt(randomNumber).Random.Data);

ListRandom.Deserialize(new FileStream("DoublyLinked", FileMode.Open));
Console.WriteLine(ListRandom.ElementAt(randomNumber).Random.Data);