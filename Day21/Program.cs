PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = Walk(data.map, data.start, 64);
    Console.WriteLine(ans);
}

void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans= WalkInfinite(data.map, data.start);
    //ans = TryToZoomOutWalk(data.map, data.start);
    Console.WriteLine(ans);
}
bool ToOriginalMap(bool[][] map, int x, int y)
{
    //FIX ME.
    var originx = ((x % map.Length) + map.Length) % map.Length;
    var originy = ((y % map.Length) + map.Length) % map.Length; ;
    return map[originx][originy];
}

long WalkInfinite(bool[][] map, (int x, int y) start)
{
    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    var oddVisited = new HashSet<(int x, int y)>();
    var evenVisited = new HashSet<(int x, int y)>();
    var queue = new Queue<(int x, int y)>();
    var newQueue = new Queue<(int x, int y)>();
    var total = new List<int>();
    var delta = new List<int>();
    var deltaDelta = new List<int>();
    total.Add(0);
    queue.Enqueue(start);
    var ans = 0L;
    var step = 0;
    while (step <= 327)
    {
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();

            for (int i = 0; i < dir.Length; i++)
            {
                var newx = cur.x + dir[i].x;
                var newy = cur.y + dir[i].y;
                if (ToOriginalMap(map, newx, newy) && !oddVisited.Contains((newx, newy)) && !evenVisited.Contains((newx, newy)))
                {
                    newQueue.Enqueue((newx, newy));
                    if (step % 2 == 0)
                        evenVisited.Add((newx, newy));
                    else
                        oddVisited.Add((newx, newy));
                }
            }
        }
        if (newQueue.Count == 0)
            break;
        (queue, newQueue) = (newQueue, queue);

        step++;
        if (step % 131 == 65)
        {
            total.Add(step % 2 == 0 ? oddVisited.Count : evenVisited.Count);

            delta.Add(total[^1] - total[^2]);
            if (delta.Count > 1)
                deltaDelta.Add(delta[^1] - delta[^2]);
            if (deltaDelta.Count != 0)
                Console.WriteLine($"{step} {total[^1]} {delta[^1]} {deltaDelta[^1]}");
        }
        //Console.WriteLine($"{step} {oddVisited.Count} {evenVisited.Count}");
    }
    step = 327;
    ans = total[3];
    long inc = delta[2];
    var limit = 26501365;
    while (step != limit)
    {
        inc += deltaDelta[1];
        ans += inc;
        step += 131;
    }
    return ans;
}
long TryToZoomOutWalk(bool[][] map, (int x, int y) start)
{
    var queue = new Queue<(int step, int x, int y, (int x, int y) start)>();
    queue.Enqueue((0, 0, 0, start));
    var visited = new HashSet<(int x, int y)>();
    visited.Add((0, 0));
    var step = 0;
    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    var limit = 11;
    var oddAns = 0L;
    var evenAns = 0L;
    Dictionary<(int x, int y), (int stepsToDone, int evenCount, int oddCount, (int step, int x, int y)[] nextOne)> cache = new();
    while (queue.Count != 0)
    {
        var cur = queue.Dequeue();
        if (!cache.TryGetValue(cur.start, out var spread))
        {
            spread = WalkTheMaze(map, cur.start, int.MaxValue);
            cache[cur.start] = spread;
        }
        if (cur.step + spread.stepsToDone >= limit)
        {
            var limitState = WalkTheMaze(map, cur.start, limit - cur.step);
            if (cur.step % 2 == 0)
            {
                oddAns += limitState.oddCount;
                evenAns += limitState.evenCount;
            }
            else
            {
                oddAns += limitState.evenCount;
                evenAns += limitState.oddCount;
            }
        }
        else
        {
            if (cur.step % 2 == 0)
            {
                oddAns += spread.oddCount;
                evenAns += spread.evenCount;
            }
            else
            {
                oddAns += spread.evenCount;
                evenAns += spread.oddCount;
            }
        }
        for (int i = 0; i < spread.nextOne.Length; i++)
        {
            if (cur.step + spread.nextOne[i].step <= limit)
            {
                var newx = cur.x + dir[i].x;
                var newy = cur.y + dir[i].y;

                if (!visited.Contains((newx, newy)))
                {

                    queue.Enqueue((cur.step + spread.nextOne[i].step - 1, newx, newy, (spread.nextOne[i].x, spread.nextOne[i].y)));
                    visited.Add((newx, newy));
                }
            }
        }

    }
    return 0;
}
(int stepsToDone, int evenCount, int oddCount, (int step, int x, int y)[] nextOne) WalkTheMaze(bool[][] map, (int x, int y) start, int stepLimit)
{
    var next = new bool[4];
    (int step, int x, int y)[] nextOne = new (int step, int x, int y)[4];
    var n = map.Length;
    var oddVisited = new HashSet<(int x, int y)>();
    var evenVisited = new HashSet<(int x, int y)>();
    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    long ans = 0;
    var queue = new Queue<(int x, int y)>();
    var newQueue = new Queue<(int x, int y)>();
    queue.Enqueue(start);
    var step = 1;
    if (stepLimit == 0)
        return (-1, 1, 0, null);

    while (step <= stepLimit)
    {
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();

            for (int i = 0; i < dir.Length; i++)
            {
                var newx = cur.x + dir[i].x;
                var newy = cur.y + dir[i].y;
                if (newx < 0 || newy < 0 || newx == n || newy == n)
                {
                    if (!next[i])
                    {
                        nextOne[i] = (step, ((newx % map.Length) + map.Length) % map.Length, ((newy % map.Length) + map.Length) % map.Length);
                        next[i] = true;
                    }
                    continue;
                }
                if (map[newx][newy] && !oddVisited.Contains((newx, newy)) && !evenVisited.Contains((newx, newy)))
                {
                    {
                        newQueue.Enqueue((newx, newy));
                        if (step % 2 == 0)
                            evenVisited.Add((newx, newy));
                        else
                            oddVisited.Add((newx, newy));
                    }
                }
            }
        }
        if (newQueue.Count == 0)
            break;
        (queue, newQueue) = (newQueue, queue);
        step++;
        Print(map, evenVisited, oddVisited, 1, 1);
    }
    return (step, evenVisited.Count, oddVisited.Count, nextOne);
}
void Print(bool[][] map, HashSet<(int x, int y)> oddVisited, HashSet<(int x, int y)> evenVisited, int zoomx = 3, int zoomy = 5)
{
    var n = map.Length;

    for (int i = -zoomx / 2; i <= zoomx / 2; i++)
    {

        for (int x = 0; x < n; x++)
        {
            Console.WriteLine();
            for (int j = -zoomy / 2; j <= zoomy / 2; j++)

            {

                for (int y = 0; y < n; y++)
                {

                    if (!map[x][y])
                        Console.Write('#');
                    else
                    {
                        var nx = x + (n * i);
                        var ny = y + (n * j);
                        if (oddVisited.Contains((nx, ny)))
                            Console.Write('█');
                        else if (evenVisited.Contains((nx, ny)))
                            Console.Write('☺');
                        else
                            Console.Write('.');
                    }
                }
            }
        }
    }
    Console.WriteLine();
}
long Walk(bool[][] map, (int x, int y) start, int numSteps)
{
    var n = map.Length;
    var m = map[0].Length;
    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    long ans = 0;
    var queue = new Queue<(int x, int y)>();
    var newQueue = new Queue<(int x, int y)>();
    var steps = new int[n, m];
    queue.Enqueue(start);
    var step = 1;
    while (step <= numSteps + 1)
    {
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();
            steps[cur.x, cur.y] = step;
            for (int i = 0; i < dir.Length; i++)
            {
                var newx = cur.x + dir[i].x;
                var newy = cur.y + dir[i].y;
                if (newx < 0 || newy < 0 || newx == n || newy == m)
                    continue;
                if (map[newx][newy])
                {
                    newQueue.Enqueue((newx, newy));
                    map[newx][newy] = false;
                }
            }
        }
        if (newQueue.Count == 0)
            break;
        (queue, newQueue) = (newQueue, queue);
        step++;
    }
    for (int i = 0; i < n; i++)
        for (int j = 0; j < m; j++)
        {
            if (steps[i, j] % 2 == 1)
                ans++;
        }
    return ans;

}

(bool[][] map, (int x, int y) start) Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var n = lines.Length;
    var m = lines[0].Length;
    var map = new bool[n][];
    var x = 0;
    var y = 0;
    for (int i = 0; i < n; i++)
    {
        map[i] = new bool[m];
        for (int j = 0; j < m; j++)
            if (lines[i][j] == '.')
            {
                map[i][j] = true;
            }
            else if (lines[i][j] == 'S')
            {
                x = i;
                y = j;
                map[i][j] = true;
            }

    }
    return (map, (x, y));
}
