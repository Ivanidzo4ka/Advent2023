PartOne();
PartTwo();

void PartOne()
{
    var ans = 0l;
    var data = ParseOne();
    var d = data.OrderBy(x => x.rank).ThenBy(x => x.cost).ToArray();
    for (int i = 0; i < d.Length; i++)
        ans += d[i].bid * (i + 1);
    Console.WriteLine(ans);
}

void PartTwo()
{
    var ans = 0l;
    var data = ParseTwo();
    var d = data.OrderBy(x => x.rank).ThenBy(x => x.cost).ToArray();
    for (int i = 0; i < d.Length; i++)
        ans += d[i].bid * (i + 1);
    Console.WriteLine(ans);
}

List<(int rank, int cost, int bid)> ParseOne()
{
    var lines = File.ReadAllLines("input.txt");
    var ans = new List<(int rank, int cost, int bid)>();
    var dic = new Dictionary<int, string>();
    foreach (var line in lines)
    {
        var t = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var hand = t[0];
        var bid = int.Parse(t[1]);
        var cost = 0;
        foreach (var l in hand)
        {
            cost *= 13;
            cost += Letter(l);
        }
        dic.Add(cost, hand);
        ans.Add((Rank(hand), cost, bid));
    }
    return ans;
}

List<(int rank, int cost, int bid)> ParseTwo()
{
    var lines = File.ReadAllLines("input.txt");
    var ans = new List<(int rank, int cost, int bid)>();
    var dic = new Dictionary<int, string>();
    foreach (var line in lines)
    {
        var t = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var hand = t[0];
        var bid = int.Parse(t[1]);
        var cost = 0;
        foreach (var l in hand)
        {
            cost *= 13;
            cost += LetterJoker(l);
        }
        dic.Add(cost, hand);
        ans.Add((RankJoker(hand), cost, bid));
    }
    return ans;
}

/*
AAK2A
00B10

AAQAA

00A00
*/
int Rank(string s)
{
    var dic = new Dictionary<char, int>();
    foreach (var c in s)
    {
        dic.TryGetValue(c, out var val);
        dic[c] = val + 1;
    }
    if (dic.Keys.Count == 1)
        return 7;
    if (dic.Keys.Count == 2)
    {
        if (dic.Values.First() == 3 || dic.Values.First() == 2)
            return 5;
        return 6;
    }
    if (dic.Values.Contains(3))
        return 4;
    if (dic.Keys.Count == 3)
        return 3;
    if (dic.Values.Contains(2))
        return 2;

    return 1;
}
int RankJoker(string s)
{
    var dic = new Dictionary<char, int>();
    foreach (var c in s)
    {
        dic.TryGetValue(c, out var val);
        dic[c] = val + 1;
    }
    if (dic.ContainsKey('J'))
    {
        if (dic.Keys.Count != 1)
        {
            var t = dic['J'];
            dic.Remove('J');
            var max = dic.Values.Max();
            var toChange = dic.Where(x => x.Value == max).First();
            dic[toChange.Key] += t;
        }

    }
    if (dic.Keys.Count == 1)
        return 7;
    if (dic.Keys.Count == 2)
    {
        if (dic.Values.First() == 3 || dic.Values.First() == 2)
            return 5;
        return 6;
    }
    if (dic.Values.Contains(3))
        return 4;
    if (dic.Keys.Count == 3)
        return 3;
    if (dic.Values.Contains(2))
        return 2;

    return 1;
}


int Letter(char c)
{
    return c switch
    {
        '2' => 0,
        '3' => 1,
        '4' => 2,
        '5' => 3,
        '6' => 4,
        '7' => 5,
        '8' => 6,
        '9' => 7,
        'T' => 8,
        'J' => 9,
        'Q' => 10,
        'K' => 11,
        'A' => 12,
    };
}

int LetterJoker(char c)
{
    return c switch
    {
        'J' => 0,
        '2' => 1,
        '3' => 2,
        '4' => 3,
        '5' => 4,
        '6' => 5,
        '7' => 6,
        '8' => 7,
        '9' => 8,
        'T' => 9,
        'Q' => 10,
        'K' => 11,
        'A' => 12,
    };
}



