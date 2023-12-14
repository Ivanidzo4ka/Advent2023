using System.Text;
PartOne();
PartTwo();
void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    North(data);
    ans = Calc(data);
    Console.WriteLine(ans);
}
int Calc(char[][] data)
{
    var n = data.Length;
    var m = data[0].Length;
    var ans = 0;
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
            if (data[i][j] == 'O')
                ans += (n - i);
    }
    return ans;
}
void North(char[][] data)
{
    var n = data.Length;
    for (int i = 1; i < n; i++)
    {
        for (int j = 0; j < n; j++)
        {
            if (data[i][j] == 'O')
            {
                var t = i;
                while (t > 0 && data[t - 1][j] == '.')
                {
                    (data[t][j], data[t - 1][j]) = (data[t - 1][j], data[t][j]);
                    t--;
                }
            }
        }
    }
}

void West(char[][] data)
{
    var n = data.Length;

    for (int j = 1; j < n; j++)
    {
        for (int i = 0; i < n; i++)
        {
            if (data[i][j] == 'O')
            {
                var t = j;
                while (t > 0 && data[i][t - 1] == '.')
                {
                    (data[i][t], data[i][t - 1]) = (data[i][t - 1], data[i][t]);
                    t--;
                }
            }
        }
    }
}

void East(char[][] data)
{
    var n = data.Length;

    for (int j = n - 1; j >= 0; j--)
    {
        for (int i = 0; i < n; i++)
        {
            if (data[i][j] == 'O')
            {
                var t = j;
                while (t < n - 1 && data[i][t + 1] == '.')
                {
                    (data[i][t], data[i][t + 1]) = (data[i][t + 1], data[i][t]);
                    t++;
                }
            }
        }
    }
}
void South(char[][] data)
{
    var n = data.Length;
    for (int i = n - 1; i >= 0; i--)
    {
        for (int j = 0; j < n; j++)
        {
            if (data[i][j] == 'O')
            {
                var t = i;
                while (t < n - 1 && data[t + 1][j] == '.')
                {
                    (data[t][j], data[t + 1][j]) = (data[t + 1][j], data[t][j]);
                    t++;
                }
            }
        }
    }
}

void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    var count = 0;
    var limit = 1_000_000_000;
    Dictionary<string, int> set = new Dictionary<string, int>();

    var found = false;
    while (true)
    {
        North(data);
        West(data);
        South(data);
        East(data);
        count++;
        var key = ToString(data);
        if (!set.ContainsKey(key))
            set.Add(key, count);
        else
        {
            if (!found)
            {
                var pos = set[key];
                var loopsize = count - pos;
                limit = count + (limit - count) % loopsize;
                found = true;
            }
        }
        if (count == limit)
            break;
    }
    ans = Calc(data);
    Console.WriteLine(ans);
}
string ToString(char[][] data)
{
    var sb = new StringBuilder();
    var n = data.Length;
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            sb.Append(data[i][j]);
    return sb.ToString();
}
void Print(char[][] data)
{
    Console.WriteLine();
    for (int i = 0; i < data.Length; i++)
        Console.WriteLine(string.Join("", data[i].Select(x => x)));
}
char[][] Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var res = new char[lines.Length][];
    for (int i = 0; i < lines.Length; i++)
        res[i] = lines[i].ToArray();
    return res;

}