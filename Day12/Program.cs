PartOne();
PartTwo();
void PartOne()
{
    var ans = 0l;
    var data = Parse("input.txt");
    for (int i = 0; i < data.input.Count; i++)
    {
        var count = data.input[i].Where(x => x == '?').Count();
        var pos = data.input[i].IndexOf("?");
        ans += Solve(data.input[i].ToArray(), data.pattern[i], pos, count);
    }
    Console.WriteLine(ans);
}

int Solve(char[] input, int[] pattern, int pos, int count)
{
    if (count == 0)
    {
        var pat = GetPattern(input);
        var same = true;
        if (pattern.Length != pat.Length)
            return 0;
        for (int i = 0; i < pattern.Length; i++)
            if (pattern[i] != pat[i])
                return 0;
        return 1;
    }
    var ans = 0;
    var newPos = pos + 1;
    while (count > 1 && input[newPos] != '?') newPos++;
    input[pos] = '#';
    ans += Solve(input, pattern, newPos, count - 1);
    input[pos] = '.';
    ans += Solve(input, pattern, newPos, count - 1);
    input[pos] = '?';
    return ans;
}


int[] GetPattern(char[] input)
{
    var pos = 0;
    var ans = new List<int>();
    var count = 0;
    for (int i = 0; i < input.Length; i++)
    {
        if (input[i] == '#')
        {
            count++;
        }
        else if (count != 0)
        {
            ans.Add(count);
            count = 0;
        }
    }
    if (count != 0)
        ans.Add(count);
    return ans.ToArray();
}

void PartTwo()
{
    var ans = 0l;
    var data = Parse("input.txt");
    var obj = new object();
    Parallel.ForEach(Enumerable.Range(0, data.input.Count), (i) =>
    {
        var input = data.input[i];
        var pattern = data.pattern[i];
        input = $".{input}?{input}?{input}?{input}?{input}";
        var newPattern = new List<int>();
        newPattern.AddRange(pattern);
        newPattern.AddRange(pattern);
        newPattern.AddRange(pattern);
        newPattern.AddRange(pattern);
        newPattern.AddRange(pattern);
        pattern = newPattern.ToArray();
        var t = SolveDP(input, pattern);
        lock (obj)
        {
            ans += t;
        }

    });
    Console.WriteLine(ans);
}

(List<string> input, List<int[]> pattern) Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var input = new List<string>();
    var pattern = new List<int[]>();
    foreach (var line in lines)
    {
        var split = line.Split(' ');
        input.Add(split[0]);
        pattern.Add(split[1].Split(',').Select(x => int.Parse(x)).ToArray());
    }
    return (input, pattern);
}

long SolveDP(string s, int[] pattern)
{
    var dp = new long[pattern.Length + 1, s.Length, 2];


    var pos = 0;
    while (pos < s.Length && s[pos] != '#')
    {
        dp[0, pos, 0] = 1;
        pos++;
    }

    for (int p = 1; p <= pattern.Length; p++)
    {
        for (int end = 1; end < s.Length; end++)
        {
            if (s[end] == '#')
            {
                var sp = end;
                var count = pattern[p - 1];
                while (sp > 0 && s[sp] != '.' && count > 0) { sp--; count--; }
                if (0 == count && s[sp] != '#')
                {
                    dp[p, end, 1] = dp[p - 1, sp, 0];
                }
            }
            else if (s[end] == '.')
            {
                dp[p, end, 0] = dp[p, end - 1, 0];
                dp[p, end, 0] += dp[p, end - 1, 1];
            }
            else
            {
                dp[p, end, 0] += dp[p, end - 1, 0];
                dp[p, end, 0] += dp[p, end - 1, 1];
                var sp = end;
                var count = pattern[p - 1];
                while (sp > 0 && s[sp] != '.' && count > 0) { sp--; count--; }
                if (0 == count && s[sp] != '#')
                {
                    dp[p, end, 1] = dp[p - 1, sp, 0];
                }
            }


        }
    }
    return dp[pattern.Length, s.Length - 1, 0] + dp[pattern.Length, s.Length - 1, 1];
}
/*
?###??????????###??????????###??????????###??????????###?????????
.??..??...?##.
.??..??...?##.?.??..??...?##.?.??..??...?##.?.??..??...?##.?.??..??...?##.
 1,1,3,1,1,3,1,1,3,1,1,3,1,1,3

 dp[i] [z]
 ??#??#??##.#???? 4,2,2,1,2
012345678 
 dp[0][0]=1
 dp[1][0] = 1
 dp[2][0] = 1;
 dp[1234][1] = 1   ####
 dp[12345][1] = 1  .####
 dp[123456][1] =1  ..####
 dp[1234567][1] =-1 
 
 dp[12345678][1] -1

 dp[12345678][2] = 
 

*/