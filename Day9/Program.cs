PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse();
    foreach (var row in data)
    {
        ans += Process(row);
    }
    Console.WriteLine(ans);
}

long Process(List<long> row, bool reverse = false)
{
    var rows = new List<List<long>>();
    rows.Add(row);
    while (true)
    {
        var cur = rows[^1];
        var toAdd = new List<long>(cur.Count - 1);
        for (int i = 1; i < cur.Count; i++)
            toAdd.Add(cur[i] - cur[i - 1]);
        rows.Add(toAdd);
        if (toAdd.Where(x => x == 0).Count() == toAdd.Count())
            break;
    }
    if (reverse)
    {
        for (int i = 0; i < rows.Count; i++)
            rows[i].Reverse();
    }
    for (int i = rows.Count - 1; i > 0; i--)
    {
        if (reverse)
        {
            rows[i - 1].Add(rows[i - 1][^1] - rows[i][^1]);
        }
        else
        {
            rows[i - 1].Add(rows[i][^1] + rows[i - 1][^1]);
        }
    }

    return rows[0][^1];

}

void PartTwo()
{
    var ans = 0L;
    var data = Parse();
    foreach (var row in data)
    {
        ans += Process(row, true);
    }
    Console.WriteLine(ans);
}

List<List<long>> Parse()
{
    var lines = File.ReadAllLines("input.txt");
    var result = new List<List<long>>();
    foreach (var line in lines)
    {
        result.Add(line.Split(' ').Select(x => long.Parse(x)).ToList());
    }
    return result;
}