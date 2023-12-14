PartOne();
PartTwo();
void PartOne()
{
    var ans = 0l;
    var data = Parse("input.txt");
    foreach (var mirror in data)
    {
        ans += Solve(mirror);
    }
    Console.WriteLine(ans);
}

int Solve(List<string> mirror, int tolerance=0)
{

    for (int i = 0; i < mirror.Count - 1; i++)
        if (Horisontal(mirror, i, tolerance))
            return 100 * (i + 1);
    for (int i = 0; i < mirror[0].Length - 1; i++)
        if (Vertical(mirror, i,tolerance))
            return i + 1;
    Console.WriteLine("something wrong");
    return 0;
}

bool Horisontal(List<string> mirror, int line, int tolerance = 0)
{
    var l = line;
    var r = line + 1;
    var diff = 0;
    while (l >= 0 && r < mirror.Count)
    {
        for (int i = 0; i < mirror[0].Length; i++)
            if (mirror[l][i] != mirror[r][i])
                diff++;
        l--;
        r++;
    }
    return diff == tolerance;
}

bool Vertical(List<string> mirror, int line, int tolerance = 0)
{
    var l = line;
    var r = line + 1;
    var diff = 0;
    while (l >= 0 && r < mirror[0].Length)
    {
        for (int i = 0; i < mirror.Count; i++)
            if (mirror[i][l] != mirror[i][r])
                diff++;
        l--;
        r++;
    }
    return diff == tolerance;
}

void PartTwo()
{
   var ans = 0l;
    var data = Parse("input.txt");
    foreach (var mirror in data)
    {
        ans += Solve(mirror,1);
    }
    Console.WriteLine(ans);
}

List<List<string>> Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var mirror = new List<string>();
    var mirrors = new List<List<string>>();
    foreach (var line in lines)
    {
        if (string.IsNullOrEmpty(line))
        {
            mirrors.Add(mirror);
            mirror = new List<string>();
        }
        else
        {
            mirror.Add(line);
        }
    }
    mirrors.Add(mirror);
    return mirrors;
}