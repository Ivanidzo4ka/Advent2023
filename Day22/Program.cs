PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = Fall(data);
    Console.WriteLine(ans);
}
long Fall(List<Brick> data)
{
    var ans = 0L;
    data.Add(new Brick(0, 0, 0, 300, 300, 0));
    data = data.OrderBy(x => x.StartZ).ToList();
    var counts = new int[data.Count, data.Count];
    var standsOn = new int[data.Count];
    for (int i = 1; i < data.Count; i++)
    {
        while (true)
        {
            bool canMove = true;
            for (int j = 0; j < i; j++)
                if (OnBrickOnAnoter(data[i], data[j]))
                {
                    canMove = false;
                    break;
                }

            if (canMove)
            {
                data[i].StartZ--;
                data[i].EndZ--;
                continue;
            }
            else
            {
                for (int j = 0; j < data.Count; j++)
                    if (i != j && OnBrickOnAnoter(data[i], data[j]))
                    {
                        counts[i, j]++;
                        standsOn[i]++;
                    }

            }
            break;
        }
    }
    //Print(data, 3, 10);
    for (int i = 1; i < data.Count; i++)
    {
        List<int> toCheck = new List<int>();
        for (int candidate = 0; candidate < data.Count; candidate++)
            if (counts[candidate, i] == 1)
            {
                toCheck.Add(candidate);
            }
        bool canRemove = true;
        foreach (var check in toCheck)
        {
            if (standsOn[check] == 1)
            {
                canRemove = false;
                break;
            }

        }
        if (canRemove)
            ans++;
    }

    return ans;
}

int FreeFall(List<Brick> data)
{
    data.Add(new Brick(0, 0, 0, 300, 300, 0));
    data = data.OrderBy(x => x.StartZ).ToList();
    //Print(data, 3,10);
    var moved = new bool[data.Count];
    for (int i = 1; i < data.Count; i++)
    {
        while (true)
        {
            bool canMove = true;
            for (int j = 0; j < i; j++)
                if (OnBrickOnAnoter(data[i], data[j]))
                {
                    canMove = false;
                    break;
                }

            if (canMove)
            {
                moved[i] = true;
                data[i].StartZ--;
                data[i].EndZ--;
                continue;
            }
            break;
        }
    }
    //Print(data, 3,10);
    return moved.Sum(x => x ? 1 : 0);
}

void Print(List<Brick> bricks, int maxX, int maxZ)
{
    Console.WriteLine();
    var toPrintX = new int[maxZ][];
    var toPrintY = new int[maxZ][];
    for (int i = 0; i < maxZ; i++)
    {
        toPrintX[i] = new int[maxX];
        toPrintY[i] = new int[maxX];
    }
    for (int i = 0; i < bricks.Count; i++)
    {
        for (int z = bricks[i].StartZ; z <= bricks[i].EndZ; z++)
        {
            for (int x = bricks[i].StartX; x <= bricks[i].EndX; x++)
                if (toPrintX[z][x] == 0)
                {
                    toPrintX[z][x] = i + 1;
                }
                else if (toPrintX[z][x] != i + 1)
                {
                    toPrintX[z][x] = -1;
                }
            for (int y = bricks[i].StartY; y <= bricks[i].EndY; y++)
                if (toPrintY[z][y] == 0)
                {
                    toPrintY[z][y] = i + 1;
                }
                else if (toPrintY[z][y] != i + 1)
                {
                    toPrintY[z][y] = -1;
                }

        }
    }

    for (int z = maxZ - 1; z >= 0; z--)
    {
        Console.WriteLine();
        for (int x = 0; x < maxX; x++)
        {
            if (toPrintX[z][x] == 0)
                Console.Write(".");
            else if (toPrintX[z][x] == -1)
                Console.Write("?");
            else Console.Write((char)(toPrintX[z][x] - 2 + 'A'));
        }
        Console.Write("    ");
        for (int y = 0; y < maxX; y++)
        {
            if (toPrintY[z][y] == 0)
                Console.Write(".");
            else if (toPrintY[z][y] == -1)
                Console.Write("?");
            else Console.Write((char)(toPrintY[z][y] - 2 + 'A'));
        }
    }

}

bool OnBrickOnAnoter(Brick A, Brick B)
{
    if (IntervalInside(A.StartZ - 1, A.EndZ - 1, B.StartZ, B.EndZ))
        if (IntervalInside(A.StartX, A.EndX, B.StartX, B.EndX) &&
        IntervalInside(A.StartY, A.EndY, B.StartY, B.EndY))
            return true;
    return false;
}
bool IntervalInside(int ax, int ay, int bx, int by)
{
    if (bx > ay || ax > by)
        return false;
    var ox = Math.Max(ax, bx);
    var oy = Math.Min(ay, by);
    return ox <= oy;
}
long SolveTwo(List<Brick> data)
{
    var ans = 0L;
    FreeFall(data);
    for (int i = 0; i < data.Count; i++)
    {
        var without = new List<Brick>();
        for (int z = 0; z < data.Count; z++)
            if (z != i && data[z].StartZ != 0 && data[z].EndZ != 0)
            {
                without.Add(new Brick(data[z].StartX, data[z].StartY, data[z].StartZ, data[z].EndX, data[z].EndY, data[z].EndZ));
            }
        var falled = FreeFall(without);
        ans = ans + falled;
    }
    return ans;
}
void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");

    ans = SolveTwo(data);
    Console.WriteLine(ans);
}

List<Brick> Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var res = new List<Brick>();
    foreach (var line in lines)
    {
        var split = line.Split("~");
        var start = split[0].Split(",");
        var end = split[1].Split(",");
        res.Add(new Brick(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]),
            int.Parse(end[0]), int.Parse(end[1]), int.Parse(end[2])));
    }
    return res;

}

public class Brick
{
    public int StartX;
    public int StartY;
    public int StartZ;
    public int EndX;
    public int EndY;
    public int EndZ;

    public Brick(int sx, int sy, int sz, int ex, int ey, int ez)
    {
        StartX = sx;
        StartY = sy;
        StartZ = sz;

        EndX = ex;
        EndY = ey;
        EndZ = ez;
    }
}
