PartOne();
PartTwo();

void PartOne()
{
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
    var n = maze.Count;
    var visited = new int[maze.Count][];
    var come = new (int x, int y)[maze.Count][];
    for (int i = 0; i < n; i++)
    {
        visited[i] = new int[m];
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

    var zoom = new int[maze.Count * 2 + 1][];
    for (int i = 0; i < 2 * n + 1; i++)
        zoom[i] = new int[m * 2 + 1];
    zoom[2 * last.x + 1][2 * last.y + 1] = -1;
    foreach (var move in Pipe(maze[last.x][last.y]))
        zoom[move.x + (last.x * 2) + 1][move.y + (last.y * 2) + 1] = -1;
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
                zoom[t.x * 2 + 1][t.y * 2 + 1] = -1;
                foreach (var move in Pipe(maze[t.x][t.y]))
                {
                    zoom[move.x + (t.x * 2) + 1][move.y + (t.y * 2) + 1] = -1;
                }
                t = prev;
                val--;
            }
        }
    }

    zoom[2 * start.x + 1][2 * start.y + 1] = -1;
    //    PrintSimple(zoom);
    return (zoom, ans);
}
void PrintSimple(int[][] visited)
{
    Console.WriteLine();
    for (int i = 0; i < visited.Length; i++)
        Console.WriteLine(string.Join(" ", visited[i].Select(x => x < 0 ? x == -1 ? "█" : "☺" : x == 1000 ? "+" : "=")));
}
void PartTwo()
{
    var ans = 0L;
    var data = Parse();
    var res = Walk(data.maze, data.start);
    var n = res.visited.Length;
    var m = res.visited[0].Length;

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
    for (int i = 0; i < data.maze.Count; i++)
    {
        for (int j = 0; j < data.maze[0].Length; j++)
        {

            if (res.visited[i * 2 + 1][j * 2 + 1] == 0)
            {
                res.visited[i * 2 + 1][2 * j + 1] = 1000;
                ans++;
            }
        }
    }
    // PrintSimple(res.visited);
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

(int x, int y) East() => (0, 1);
(int x, int y) West() => (0, -1);
(int x, int y) North() => (-1, 0);
(int x, int y) South() => (1, 0);
IList<(int x, int y)> Pipe(char pipe)
{
    return pipe switch
    {
        '-' => new List<(int x, int y)>() { East(), West() },
        '|' => new List<(int x, int y)>() { North(), South() },
        'L' => new List<(int x, int y)>() { East(), North() },
        'J' => new List<(int x, int y)>() { West(), North() },
        '7' => new List<(int x, int y)>() { West(), South() },
        'F' => new List<(int x, int y)>() { East(), South() },
        _ => new List<(int x, int y)>()
    };
}