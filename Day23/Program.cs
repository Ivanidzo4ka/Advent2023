var dir = new (int x, int y, char p)[] { (1, 0, 'v'), (-1, 0, '^'), (0, 1, '>'), (0, -1, '<') };
PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = Walk(data);
    Console.WriteLine(ans);
}
long Walk(IList<string> map)
{
    var n = map.Count;
    var visited = new bool[n][];
    for (int i = 0; i < n; i++)
    {
        visited[i] = new bool[n];
    }
    visited[0][1] = true;
    return DFS(map, visited, 0, 1) - 1;
}
long DFS(IList<string> map, bool[][] visited, int x, int y)
{
    if (x == visited.Length - 1)
        return 1;
    var ans = 0L;
    //Print(map, x, y);
    for (int i = 0; i < 4; i++)
    {
        if (map[x][y] != '.' && dir[i].p != map[x][y])
            continue;
        var newx = x + dir[i].x;
        var newy = y + dir[i].y;
        if (newx == visited.Length || newy == visited.Length || newx < 0 || newy < 0)
            continue;
        if (map[newx][newy] == '#')
            continue;
        if (!visited[newx][newy])
        {
            visited[newx][newy] = true;
            ans = Math.Max(ans, DFS(map, visited, newx, newy));
            visited[newx][newy] = false;
        }
    }
    return ans == 0 ? 0 : ans + 1;
}
void Print(IList<string> map, Dictionary<(int x, int y), int> dic)
{

    for (int i = 0; i < map.Count; i++)
    {

        for (int j = 0; j < map.Count; j++)
            if (dic.TryGetValue((i, j), out var val))
                Console.Write((char)(val + 'A'));
            else
                Console.Write(map[i][j]);
        Console.WriteLine();
    }
}
void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    var n = data.Count;
    var points = new List<(int x, int y)>();
    points.Add((0, 1));
    for (int i = 1; i < n - 1; i++)
        for (int j = 1; j < n - 1; j++)
            if (data[i][j] != '#')
            {
                var count = 0;
                for (int d = 0; d < 4; d++)
                {
                    var newx = i + dir[d].x;
                    var newy = j + dir[d].y;
                    if (data[newx][newy] != '#')
                        count++;
                }
                if (count > 2)
                {
                    points.Add((i, j));
                }
            }

    points.Add((n - 1, n - 2));
    var pointsDic = new Dictionary<(int x, int y), int>();
    for (int i = 0; i < points.Count; i++)
        pointsDic.Add(points[i], i);

    Print(data, pointsDic);
    var edges = new List<List<(int to, int dist)>>();
    for (int i = 0; i < points.Count; i++)
    {
        edges.Add(GetEdges(data, points[i], pointsDic));
    }
    var been = new bool[points.Count];
    been[0] = true;
    ans = FindLongest(edges, 0, been, 0);
    Console.WriteLine(ans);
}

long FindLongest(List<List<(int to, int dist)>> edges, int cur, bool[] been, int count)
{
    var ans = -1L;
    if (cur == edges.Count - 1)
        return 0;
    foreach (var edge in edges[cur])
    {
        if (!been[edge.to])
        {
            been[edge.to] = true;
            var t = FindLongest(edges, edge.to, been, count + edge.dist);
            if (t != -1)
                ans = Math.Max(ans, t + edge.dist);
            been[edge.to]=false;
        }
    }
    return ans;
}

List<(int to, int dist)> GetEdges(IList<string> map, (int x, int y) start, Dictionary<(int x, int y), int> dic)
{
    var visited = new bool[map.Count, map.Count];
    var result = new List<(int to, int dist)>();
    var queue = new Queue<(int x, int y, int step)>();
    visited[start.x, start.y] = true;
    queue.Enqueue((start.x, start.y, 0));
    while (queue.Count != 0)
    {
        var cur = queue.Dequeue();
        //Print(map, new Dictionary<(int x, int y), int>() { [(cur.x, cur.y)] = 1 });
        if (cur.step != 0 && dic.TryGetValue((cur.x, cur.y), out var val))
        {
            result.Add((val, cur.step));
            continue;
        }
        for (int d = 0; d < 4; d++)
        {
            var newx = cur.x + dir[d].x;
            var newy = cur.y + dir[d].y;
            if (newx >= map.Count || newy >= map.Count || newx < 0 || newy < 0)
                continue;
            if (map[newx][newy] != '#' && !visited[newx, newy])
            {
                visited[newx, newy] = true;
                queue.Enqueue((newx, newy, cur.step + 1));
            }
        }
    }
    return result;
}

IList<string> Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);

    return lines;
}