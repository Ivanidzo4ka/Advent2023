PartOne();
PartTwo();


void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt", 1);

    Console.WriteLine(Sum(data));
}

long Sum(IList<(long x, long y)> nodes)
{
    var ans = 0l;
    for (int i = 0; i < nodes.Count; i++)
        for (int j = i + 1; j < nodes.Count; j++)
        {
            ans += Math.Abs(nodes[i].x - nodes[j].x) + Math.Abs(nodes[i].y - nodes[j].y);
        }

    return ans;

}

void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt", 1000000-1);

    Console.WriteLine(Sum(data));
}


IList<(long x, long y)> Parse(string fileName, int mul)
{
    var lines = File.ReadAllLines(fileName);
    var rows = new bool[lines[0].Length];
    var columns = new bool[lines.Length];
    Array.Fill(rows, true);
    Array.Fill(columns, true);
    var pos = new List<(int x, int y)>();
    for (int i = 0; i < lines.Length; i++)
    {
        for (int j = 0; j < lines[0].Length; j++)
        {
            if (lines[i][j] != '.')
            {
                pos.Add((i, j));
                rows[j] = false;
                columns[i] = false;
            }
        }
    }
    var res = new List<(long x, long y)>();
    foreach (var star in pos)
    {
        var incC = 0;
        var incR = 0;
        for (int i = 0; i < star.x; i++)
            if (columns[i])
                incC++;
        for (int i = 0; i < star.y; i++)
            if (rows[i])
                incR++;
        res.Add((star.x + (mul * incC), star.y + (mul * incR)));
    }

    return res;
}
