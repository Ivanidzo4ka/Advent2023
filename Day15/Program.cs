PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    foreach (var s in data)
    {
        ans += Hash(s);
    }
    Console.WriteLine(ans);
}

void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    var box = new List<List<Box>>();
    for (int i = 0; i < 256; i++)
        box.Add(new List<Box>());
    foreach (var line in data)
    {
        if (line.Contains('-'))
        {
            var sp = line.Split('-');
            var label = sp[0];
            var current = Hash(label);
            Dash(box, current, label);
        }
        else
        {
            var sp = line.Split('=');
            var label = sp[0];
            var lens = int.Parse(sp[1]);
            var current = Hash(label);
            Equal(box, current, label, lens);
        }
    }
    for (int z =0; z<256; z++)
    {
        for (int i=0; i< box[z].Count; i++)
            ans+= (long)(z+1)*(i+1)*box[z][i].Lens;
    }
    Console.WriteLine(ans);
}

IList<string> Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var split = lines[0].Split(',');
    return split;

}

void Dash(List<List<Box>> boxes, int current, string label)
{
    for (int i = boxes[current].Count-1; i>=0; i--)
        if (boxes[current][i].Label==label)
        {
            boxes[current].RemoveAt(i);
            return;
        }
    
}

void Equal(List<List<Box>> boxes, int current, string label, int lens)
{
    for (int i = 0; i < boxes[current].Count; i++)
    {
        if (boxes[current][i].Label == label)
        {
            boxes[current][i].Lens = lens;
            return;
        }
    }
    boxes[current].Add(new Box() { Label = label, Lens = lens });
}

int Hash(string s)
{
    var ans = 0;
    foreach (var l in s)
    {
        ans += l;
        ans *= 17;
        ans %= 256;
    }
    return ans;
}

public class Box
{
    public string Label;
    public int Lens;
}