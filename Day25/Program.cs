PartOne();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    var minCut = MinCut(data.Length, data);
    ans = (data.Length - minCut) * minCut;
    Console.WriteLine(ans);
}

int MinCut(int n, int[][] matrix)
{
    var v = new List<int>[n];
    int best_cost = 1000000000;
    for (int i = 0; i < n; ++i)
    {
        v[i] = new List<int>();
        v[i].Add(i);
    }
    var w = new int[n];
    var exist = new bool[n];
    var in_a = new bool[n];
    Array.Fill(exist, true);

    for (int ph = 0; ph < n - 1; ++ph)
    {
        Array.Fill(in_a, false);
        Array.Fill(w, 0);
        int prev = 0;
        for (int it = 0; it < n - ph; ++it)
        {
            int sel = -1;
            for (int i = 0; i < n; ++i)
                if (exist[i] && !in_a[i] && (sel == -1 || w[i] > w[sel]))
                    sel = i;
            if (it == n - ph - 1)
            {
                if (w[sel] < best_cost)
                {
                    best_cost = w[sel];
                    if (best_cost == 3)
                        return v[sel].Count;
                }
                v[prev].AddRange(v[sel]);
                for (int i = 0; i < n; ++i)
                {
                    matrix[i][prev] += matrix[sel][i];
                    matrix[prev][i] = matrix[i][prev];
                }
                exist[sel] = false;
            }
            else
            {
                in_a[sel] = true;
                for (int i = 0; i < n; ++i)
                    w[i] += matrix[sel][i];
                prev = sel;
            }
        }
    }
    return -1;
}

int[][] Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var dic = new Dictionary<string, int>();
    var edges = new Dictionary<int, List<int>>();
    foreach (var line in lines)
    {
        var split = line.Split(":");
        var name = split[0];
        if (!dic.TryGetValue(name, out var nameVal))
        {
            nameVal = dic.Keys.Count;
            dic[name] = nameVal;
        }

        if (!edges.TryGetValue(nameVal, out var edgel))
        {
            edgel = new List<int>();
            edges[nameVal] = edgel;
        }
        foreach (var other in split[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()))
        {
            if (!dic.TryGetValue(other, out var otherVal))
            {
                otherVal = dic.Keys.Count;
                dic[other] = otherVal;
            }
            edgel.Add(otherVal);
            if (!edges.TryGetValue(otherVal, out var otherL))
            {
                otherL = new List<int>();
                edges[otherVal] = otherL;
            }
            otherL.Add(nameVal);
        }
    }

    var matrix = new int[dic.Keys.Count][];
    for (int i = 0; i < dic.Keys.Count; i++)
        matrix[i] = new int[dic.Keys.Count];

    foreach (var kvp in edges)
    {
        foreach (var val in kvp.Value)
        {
            var x = kvp.Key;
            var y = val;
            matrix[x][y] = 1;
            matrix[y][x] = 1;
        }
    }

    return matrix;
}
