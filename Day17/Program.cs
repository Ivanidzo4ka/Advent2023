

PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = Walk(data, 1, 3);
    Console.WriteLine(ans);
}

int Walk(int[][] data, int min, int max)
{
    var n = data.Length;
    var queue = new PriorityQueue<(int x, int y, int d, int cost), int>();
    var dir = new (int x, int y)[] {
        (1, 0), //down
         (-1, 0), //up
         (0, -1), //left
         (0, 1), //right
          };
    var visited = new int[n, n, 4];
    queue.Enqueue((0, 0, 3, 0), 0);
    queue.Enqueue((0, 0, 0, 0), 0);
    while (queue.Count != 0)
    {
        var cur = queue.Dequeue();
        if (visited[cur.x, cur.y, cur.d] != 0)
            continue;
        visited[cur.x, cur.y, cur.d] = cur.cost;
        if (cur.x == n - 1 && cur.y == n - 1)
            return cur.cost;
        var cost = cur.cost;
        for (int i = 1; i <= max; i++)
        {
            var newx = cur.x + i * dir[cur.d].x;
            var newy = cur.y + i * dir[cur.d].y;
            if (newx < 0 || newy < 0 || newx == n || newy == n)
                break;
            cost += data[newx][newy];
            if (i < min)
                continue;
            foreach (var newDir in Turn(cur.d))
            {
                if (visited[newx, newy, newDir] == 0)
                    queue.Enqueue((newx, newy, newDir, cost), cost);
            }

        }
    }
    return -1;
}
int[] Turn(int dir)
{
    return dir switch
    {
        0 => new int[2] { 2, 3 },
        1 => new int[2] { 2, 3 },
        2 => new int[2] { 0, 1 },
        3 => new int[2] { 0, 1 },
    };
}
void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = Walk(data, 4, 10);
    Console.WriteLine(ans);

}
int[][] Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var ans = new int[lines.Length][];
    for (int i = 0; i < lines.Length; i++)
    {
        ans[i] = lines[i].Select(x => int.Parse(x.ToString())).ToArray();
    }
    var t = ans.Select(x => x.Sum()).Sum();
    return ans;
}
