PartOne();
PartTwo();

void PartOne()
{
    var ans = 0;
    var data = Parse();
    var res = Walk(data.maze, data.start);
    Console.WriteLine(res.steps - 1);
}


(int[][] visited, int steps) Walk(IList<string> maze, (int x, int y) start)
{
    var ans = 0;
    var queue = new Queue<(int x, int y)>();
    var dir = new (int x, int y)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
    var m = maze[0].Length;
    var visited = new int[maze.Count][];
    var come = new (int x, int y)[maze.Count][];
    var noJunk = new int[maze.Count][];
    for (int i = 0; i < maze.Count; i++)
    {
        visited[i] = new int[m];
        noJunk[i] = new int[m];
        come[i] = new (int x, int y)[m];
    }
    visited[start.x][start.y] = 1;
    queue.Enqueue((start.x, start.y));
    var newQueue = new Queue<(int x, int y)>();
    (int x, int y) last = (0, 0);
    while (true)
    {
        ans++;

        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();
            last = cur;
            for (int i = 0; i < dir.Length; i++)
            {
                var newx = cur.x + dir[i].x;
                var newy = cur.y + dir[i].y;
                if (CanGo(cur.x, cur.y, newx, newy, maze) && visited[newx][newy] == 0)
                {
                    newQueue.Enqueue((newx, newy));
                    visited[newx][newy] = ans + 1;
                    come[newx][newy] = (cur.x, cur.y);
                }
            }
        }
        if (newQueue.Count == 0)
            break;
        (queue, newQueue) = (newQueue, queue);

    }
    //Print(visited);

    for (int i = 0; i < dir.Length; i++)
    {
        var tx = last.x + dir[i].x;
        var ty = last.y + dir[i].y;

        var val = ans;
        if (tx < 0 || ty < 0 || tx >= maze.Count || ty >= maze[0].Length)
            continue;
        if (visited[tx][ty] == val - 1)
        {
            var t = (x: tx, y: ty);

            while (val != 2)
            {
                var prev = come[t.x][t.y];
                visited[t.x][t.y] = -1;
                noJunk[t.x][t.y] = -1;
                t = prev;
                val--;
                // Print(visited);
            }
        }
    }
    noJunk[last.x][last.y] = -1;
    noJunk[start.x][start.y] = -1;
    //Print(visited);
    return (noJunk, ans);
}
void Print(int[][] visited, IList<string> maze)
{
    Console.WriteLine(" ");
    for (int i = 0; i < visited.Length; i++)
    {
        for (int j = 0; j < visited[i].Length; j++)
        {
            if (visited[i][j] == -1)
                Console.Write(maze[i][j]);
            else if (visited[i][j] == 1000)
                Console.Write('☺');
            else
                Console.Write('█');
        }
        Console.WriteLine();
    }
}
void PrintSimple(int[][] visited)
{
    for (int i = 0; i < visited.Length; i++)
    {
        Console.WriteLine(string.Join(" ", visited[i].Select(x => x < 0 ? x == -1 ? "█" : "☺" : x == 1000 ? "+" : "=")));
        //visited.Length;
    }
}
void PartTwo()
{
    var ans = 0L;
    var data = Parse();
    var res = Walk(data.maze, data.start);
    var n = data.maze.Count;
    var m = data.maze[0].Length;
    PrintSimple(res.visited);
    for (int i = 0; i < n; i++)
        for (int j = 0; j < m; j++)
            if (res.visited[i][j] != -1)
                res.visited[i][j] = 0;

    for (int i = 0; i < n; i++)
    {
        if (res.visited[i][0] == 0)
            Spread(res.visited, i, 0);
        if (res.visited[i][m - 1] == 0)
            Spread(res.visited, i, m - 1);
    }
    for (int i = 0; i < m; i++)
    {
        if (res.visited[0][i] == 0)
            Spread(res.visited, 0, i);
        if (res.visited[n - 1][i] == 0)
            Spread(res.visited, n - 1, i);
    }
    //Print(res.visited);
    for (int i = 0; i < n; i++)
    {
        var count = 0;
        for (int j = 0; j < m; j++)
        {

            if (res.visited[i][j] == 0)
            {
                if (count % 2 == 1)
                {
                    ans++;
                    res.visited[i][j] = 1000;
                }

            }
            else
            {
                if (res.visited[i][j] == -1)
                    if (data.maze[i][j] == 'L' || data.maze[i][j] == 'J' || data.maze[i][j] == '|')
                        count++;
            }
        }
    }
    Print(res.visited, data.maze);
    Console.WriteLine(ans);

}

void Spread(int[][] map, int x, int y)
{
    var dir = new (int x, int y)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
    var queue = new Queue<(int x, int y)>();
    queue.Enqueue((x, y));
    map[x][y] = -2;
    var newQueue = new Queue<(int x, int y)>();
    while (true)
    {
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();
            for (int i = 0; i < dir.Length; i++)
            {
                var newx = cur.x + dir[i].x;
                var newy = cur.y + dir[i].y;
                if (newx < 0 || newy < 0 || newx >= map.Length || newy >= map[0].Length)
                    continue;
                if (map[newx][newy] == 0)
                {
                    newQueue.Enqueue((newx, newy));
                    map[newx][newy] = -2;
                }
            }
        }
        if (newQueue.Count == 0)
            break;
        (queue, newQueue) = (newQueue, queue);
    }
}
(IList<string> maze, (int x, int y) start) Parse()
{
    var lines = File.ReadAllLines("input.txt");
    for (int i = 0; i < lines.Length; i++)
    {
        for (int j = 0; j < lines[0].Length; j++)
            if (lines[i][j] == 'S')
                return (lines, (i, j));
    }
    return (lines, (0, 0));
}

bool CanGo(int x, int y, int newx, int newy, IList<string> maze)
{
    if (newx < 0 | newy < 0 || newx >= maze.Count || newy >= maze[0].Length)
        return false;
    var difx = x - newx;
    var dify = y - newy;

    foreach (var diffs in Pipe(maze[newx][newy]))
        if (diffs.x == difx && diffs.y == dify)
            return true;
    return false;
}

List<(int x, int y)> Pipe(char pipe)
{
    var res = new List<(int x, int y)>();
    if (pipe == '.')
        return res;
    if (pipe == '-')
    {
        // east
        res.Add((0, 1));
        // west
        res.Add((0, -1));
        return res;
    }
    if (pipe == '|')
    {
        //south
        res.Add((1, 0));
        //north
        res.Add((-1, 0));
        return res;
    }
    if (pipe == 'L')
    {
        // east
        res.Add((0, 1));
        //north
        res.Add((-1, 0));
        return res;
    }
    if (pipe == 'J')
    {
        //north
        res.Add((-1, 0));
        // west
        res.Add((0, -1));
        return res;
    }
    if (pipe == '7')
    {
        // west
        res.Add((0, -1));
        //south
        res.Add((1, 0));
        return res;
    }
    if (pipe == 'F')
    {
        //south
        res.Add((1, 0));
        // east
        res.Add((0, 1));
        return res;
    }
    return res;
}