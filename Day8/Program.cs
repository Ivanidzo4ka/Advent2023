PartOne();
PartTwo();
void PartOne()
{
    var ans = 0;
    var data = Parse("input.txt");
    var cur = "AAA";
    var pos = 0;
    while (cur != "ZZZ")
    {
        ans++;
        if (data.Path[pos] == 'L')
            cur = data.Nodes[cur].left;
        else
            cur = data.Nodes[cur].right;
        pos++;
        pos %= data.Path.Length;
    }

    Console.WriteLine(ans);
}
void PartTwo()
{

    var data = Parse("input.txt");
    var start = new List<string>();
    var end = new List<string>();
    foreach (var kvp in data.Nodes)
    {
        if (kvp.Key[2] == 'Z')
        {
            end.Add(kvp.Key);
        }
        else if (kvp.Key[2] == 'A')
        {
            start.Add(kvp.Key);
        }
    }
    var res = 1l;
    var d = new List<long>();
    for (int i = 0; i < start.Count; i++)
        for (int j = 0; j < end.Count; j++)
        {
            var set = new HashSet<(int, string)>();
            var cur = start[i];
            var pos = 0;
            var ans = 0;
            var good = true;
            while (cur != end[j])
            {
                if (set.Contains((pos, cur)))
                {
                    Console.WriteLine($"{start[i]} to {end[j]} unreach");
                    good = false;
                    break;
                }
                set.Add((pos, cur));

                ans++;
                if (data.Path[pos] == 'L')
                    cur = data.Nodes[cur].left;
                else
                    cur = data.Nodes[cur].right;

                pos++;
                pos %= data.Path.Length;
            }
            if (good)
            {
                Console.WriteLine($"{start[i]} to {end[j]} {ans}");
                d.Add(ans);
            }
        }
    for (int i = 0; i < d.Count; i++)
        res  = Lcm(res, d[i]);
    Console.WriteLine(res);
}

long Lcm(long a, long b)
{
    long temp = Gcd(a, b);

    return temp!=0 ? (a / temp * b) : 0;
}

long Gcd(long a, long b)
{
    while (true)
    {
        if (a == 0) return b;
        b %= a;
        if (b == 0) return a;
        a %= b;
    }
}


(string Path, Dictionary<string, (string left, string right, int num)> Nodes) Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var dic = new Dictionary<string, (string, string, int)>();
    for (int i = 2; i < lines.Length; i++)
    {
        var split = lines[i].Split('=');
        var start = split[0].Trim();
        var nodes = split[1].Split(',');
        dic.Add(start, (nodes[0][2..], nodes[1].Substring(1, 3), i - 2));

    }
    return (lines[0], dic);
}
