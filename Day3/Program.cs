PartOne();
PartTwo();

void PartOne()
{
    var ans = 0;
    var l = new List<string>();
    foreach (var line in File.ReadAllLines("input.txt"))
    {
        l.Add(line);
    }
    var n = l.Count;
    var m = l[0].Length;
    var prev = 0;
    var len = 0;
    var dir = new (int x, int y)[8] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) };
    var found = false;
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
        {
            if (char.IsDigit(l[i][j]))
            {
                len++;
                prev *= 10;
                prev += l[i][j] - '0';
                if (found)
                    continue;
                for (int z = 0; z < dir.Length; z++)
                {
                    var newx = i + dir[z].x;
                    var newy = j + dir[z].y;
                    if (newx < 0 || newx >= n || newy < 0 || newy >= m)
                        continue;
                    if (l[newx][newy] != '.' && !char.IsDigit(l[newx][newy]))
                        found = true;
                }

            }
            else
            {
                if (prev != 0)
                {
                    if (found)
                        ans += prev;
                    found = false;
                }
                prev = 0;
                len = 0;
            }
        }
        if (prev != 0)
        {
            if (found)
                ans += prev;
            found = false;
        }
    }

    Console.WriteLine(ans);
}

void PartTwo()
{
    var ans = 0;
    var l = new List<string>();
    foreach (var line in File.ReadAllLines("input.txt"))
    {
        l.Add(line);
    }
    var n = l.Count;
    var m = l[0].Length;
    var prev = 0;
    var len = 0;
    var dir = new (int x, int y)[8] { (1, 1), (1, 0), (1, -1), (0, 1), (0, -1), (-1, 1), (-1, 0), (-1, -1) };
    var numbers = new int[n, m];

    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
        {
            if (char.IsDigit(l[i][j]))
            {
                len++;
                prev *= 10;
                prev += l[i][j] - '0';
            }
            else
            {
                if (prev != 0)
                {
                    for (int z = j - len; z < j; z++)
                        numbers[i, z] = prev;
                    prev = 0;
                    len=0;
                }
            }
        }
        if (prev != 0)
        {
            for (int z = m - len; z < m; z++)
                numbers[i, z] = prev;
            prev = 0;
            len=0;
        }

    }
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < m; j++)
        {
            if (l[i][j]=='*')
                {
                    var t = new HashSet<int>();
                    for(int z=0; z<dir.Length; z++)
                    {
                         var newx = i + dir[z].x;
                        var newy = j + dir[z].y;
                        if (newx < 0 || newx >= n || newy < 0 || newy >= m)
                            continue;
                        if (numbers[newx,newy]!=0)
                            t.Add(numbers[newx, newy]);
                    }
                    if (t.Count==2)
                    {
                        ans+=t.ToArray()[0]*t.ToArray()[1];
                    }
                }
        }
    }
    Console.WriteLine(ans);
}
