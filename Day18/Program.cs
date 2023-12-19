
PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt", false);
    ans = Walk(data);
    Console.WriteLine(ans);
}
int Walk(List<(int dir, int step)> data)
{
    var limit = 450;
    var field = new bool[limit, limit];
    var offsetx = 200;
    var offsety = 150;
    var posx = offsetx;
    var posy = offsety;

    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    field[posx, posy] = true;

    for (int z = 0; z < data.Count; z++)

    {
        for (int i = 1; i <= data[z].step; i++)
        {
            posx += dir[data[z].dir].x;
            posy += dir[data[z].dir].y;
            field[posx, posy] = true;
        }
    }
    Print(field, limit, offsetx, offsety);
    var queue = new Queue<(int x, int y)>();
    queue.Enqueue((offsetx + 1, offsety + 1));
    field[offsetx + 1, offsety + 1] = true;
    while (queue.Count != 0)
    {
        var cur = queue.Dequeue();
        for (int i = 0; i < dir.Length; i++)
        {
            var newx = cur.x + dir[i].x;
            var newy = cur.y + dir[i].y;
            if (!field[newx, newy])
            {
                field[newx, newy] = true;
                queue.Enqueue((newx, newy));
            }
        }
    }

    var ans = 0;
    for (int i = 0; i < limit; i++)
        for (int j = 0; j < limit; j++)
            if (field[i, j])
                ans++;
    return ans;

}

void Print(bool[,] field, int limit, int x, int y)
{
    Console.WriteLine();
    for (int i = 0; i < limit; i++)
    {
        for (int j = 0; j < limit; j++)
        {
            Console.Write(field[i, j] ? "#" : '.');
        }
        Console.WriteLine();
    }
}
void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt", true);
    ans = Solve(data);
    Console.WriteLine(ans);

}

long SolveShoelace(List<(int dir, int step)> data)
{
    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    var x = 0L;
    var y = 0L;
    var perimeter = 0L;
    var points = new List<(long x, long y)>();
    points.Add((0, 0));
    foreach (var inst in data)
    {
        x = x + dir[inst.dir].x * inst.step;
        y = y + dir[inst.dir].y * inst.step;
        points.Add((x, y));
        perimeter += inst.step;
    }
    var area = 0L;
    points.Reverse();
    for (int i = 0; i < points.Count - 1; i++)
    {
        area += ((points[i].x * points[i + 1].y) - (points[i + 1].x * points[i].y));
    }
    area /= 2;
    return 1 + area + (perimeter / 2);

}
long Solve(List<(int dir, int step)> data)
{
    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    var x = 0L;
    var y = 0L;
    var perimeter = 0L;
    var area = 0L;
    foreach (var inst in data)
    {
        x = x + dir[inst.dir].x * inst.step;
        y = y + dir[inst.dir].y * inst.step;
        perimeter += inst.step;
        // if we going down, count rectangle of width y and height inst.step by adding to area.
        if (inst.dir == 0)
            area += y * inst.step;
        // if we going down, count rectangle of width y and height inst.step by removing it from area.
        else if (inst.dir == 1)
            area -= y * inst.step;
    }
    // Pick's theorem 
    // https://en.wikipedia.org/wiki/Pick's_theorem
    // A = I +B/2-1
    // we looking for I+B, so I = A-B/2 +1
    // I+B = A+B/2 +1
    return 1 + area + perimeter / 2;
}
List<(int dir, int step)> Parse(string fileName, bool partTwo)
{
    var lines = File.ReadAllLines(fileName);
    List<(int dir, int step)> res = new();
    foreach (var line in lines)
    {
        var split = line.Split(' ');
        if (partTwo)
        {
            var hex = split[2].Substring(2, 5);
            var it = Convert.ToInt32(hex, 16);
            res.Add((split[2][7] switch { '0' => 3, '1' => 0, '2' => 2, '3' => 1 }, Convert.ToInt32(hex, 16)));

        }
        else
            res.Add((split[0] switch { "R" => 3, "L" => 2, "D" => 0, "U" => 1 }, int.Parse(split[1])));
    }
    return res;
}
