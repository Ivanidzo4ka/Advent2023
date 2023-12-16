
PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = Walk(data, 0, 0, Direction.Right);
    Console.WriteLine(ans);
}

void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    var n = data.Length;

    for (int i = 0; i < n; i++)
    {
        ans = Math.Max(ans, Walk(data, 0, i, Direction.Down));
        ans = Math.Max(ans, Walk(data, n - 1, i, Direction.Up));
        ans = Math.Max(ans, Walk(data, i, 0, Direction.Right));
        ans = Math.Max(ans, Walk(data, i, n - 1, Direction.Left));
    }
    Console.WriteLine(ans);
}
int Walk(char[][] data, int x, int y, Direction d)
{
    var n = data.Length;
    var been = new bool[n, n, 4];
    var queue = new Queue<(int x, int y, Direction d)>();
    queue.Enqueue((x, y, d));
    been[x, y, (int)d] = true;

    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    while (queue.Count != 0)
    {
        var cur = queue.Dequeue();
        foreach (var newDir in Modify(data[cur.x][cur.y], cur.d))
        {
            var newx = dir[(int)newDir].x + cur.x;
            var newy = dir[(int)newDir].y + cur.y;
            if (!(newx < 0 || newx == n || newy < 0 || newy == n))
            {
                if (!been[newx, newy, (int)newDir])
                {
                    queue.Enqueue((newx, newy, newDir));
                    been[newx, newy, (int)newDir] = true;
                }
            }
        }
    }
    var ans = 0;
    for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            if (been[i, j, 0] || been[i, j, 1] || been[i, j, 2] || been[i, j, 3])
                ans++;
    return ans;
}
IEnumerable<Direction> Modify(char modifier, Direction direction)
{
    return modifier switch
    {
        '.' => new Direction[1] { direction },

        '|' => direction switch
        {
            Direction.Up => new Direction[1] { Direction.Up },
            Direction.Down => new Direction[1] { Direction.Down },
            Direction.Left or Direction.Right => new Direction[2] { Direction.Up, Direction.Down }
        },
        '-' => direction switch
        {
            Direction.Left => new Direction[1] { Direction.Left },
            Direction.Right => new Direction[1] { Direction.Right },
            Direction.Up or Direction.Down => new Direction[2] { Direction.Left, Direction.Right }
        },
        '/' => direction switch
        {
            Direction.Left => new Direction[1] { Direction.Down },
            Direction.Right => new Direction[1] { Direction.Up },
            Direction.Up => new Direction[1] { Direction.Right },
            Direction.Down => new Direction[1] { Direction.Left }
        },
        '\\' => direction switch
        {
            Direction.Left => new Direction[1] { Direction.Up },
            Direction.Right => new Direction[1] { Direction.Down },
            Direction.Up => new Direction[1] { Direction.Left },
            Direction.Down => new Direction[1] { Direction.Right }
        },

    };
}
void Print(bool[,,] been, int n)
{
    Console.WriteLine();
    for (int i = 0; i < n; i++)
    {
        for (int j = 0; j < n; j++)
            if (been[i, j, 0] || been[i, j, 1] || been[i, j, 2] || been[i, j, 3])
            {
                Console.Write('X');
            }
            else
                Console.Write('.');
        Console.WriteLine();
    }

}
char[][] Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var ans = new char[lines.Length][];
    for (int i = 0; i < lines.Length; i++)
    {
        ans[i] = lines[i].ToArray();
    }
    return ans;
}

enum Direction
{
    Down = 0,
    Up = 1,
    Left = 2,
    Right = 3
}
