PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = SolveOne(data.flops, data.conj, data.broadCast);
    Console.WriteLine(ans);
}

void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = SolveTwo(data.flops, data.conj, data.broadCast);
    Console.WriteLine(ans);
}

long SolveOne(Dictionary<string, FlipFlop> flops, Dictionary<string, Conjuction> conj, string[] broadCast)
{
    var ans = 0L;
    var low = 0L;
    var high = 0L;
    for (int i = 0; i < 1000; i++)
    {
        var t = PressButton(flops, conj, broadCast, null, i);
        low += t.Low;
        high += t.High;
    }
    return low * high;
}

long SolveTwo(Dictionary<string, FlipFlop> flops, Dictionary<string, Conjuction> conj, string[] broadCast)
{
    var ans = 0L;
    var cache = new Dictionary<string, long>();
    var count = conj.Select(x=>x.Value.SendTo).SelectMany(x=>x).ToHashSet().Count-1;
    while (cache.Count!=count)
    {
        ans++;
        var t = PressButton(flops, conj, broadCast, cache, ans);

    }
    ans =1;
    foreach(var kvp in cache)
    {
        ans = Lcm(ans, kvp.Value);
    }
    return ans;
}

long Lcm(long a, long b)
{
    long temp = Gcd(a, b);

    return temp!=0 ? (a / temp * b) : 0;
}

long Gcd(long a, long b)
{
    while (true)
    {
        if (a == 0) return b;
        b %= a;
        if (b == 0) return a;
        a %= b;
    }
}

(long Low, long High) PressButton(Dictionary<string, FlipFlop> flops, Dictionary<string, Conjuction> conj, string[] broadCast, Dictionary<string, long> cache, long iter)
{
    long lowCount = 1;
    long highCount = 0;
    var queue = new Queue<(Pulse pulse, string name, string prev)>();
    var newQueue = new Queue<(Pulse pulse, string name, string prev)>();

    foreach (var b in broadCast)
        queue.Enqueue((Pulse.Low, b, "broadcast"));
    while (true)
    {
        while (queue.Count != 0)
        {
            var cur = queue.Dequeue();
            if (cur.pulse == Pulse.Low)
                lowCount++;
            else
                highCount++;

            if (flops.TryGetValue(cur.name, out var val))
            {
                var pulse = val.Process(cur.pulse);
                if (pulse != Pulse.Nothing)
                {
                    foreach (var pass in val.SendTo)
                        newQueue.Enqueue((pulse, pass, cur.name));
                }
            }
            else if (conj.TryGetValue(cur.name, out var con))
            {
                var pulse = con.Process(cur.prev, cur.pulse);
                if (pulse == Pulse.Low && cache != null)
                {
                    foreach (var pass in con.SendTo)
                    {
                        if (!cache.TryGetValue(pass, out var rr))
                        {
                            cache[pass] = iter;
                        }
                    }
                }
                foreach (var pass in con.SendTo)
                    newQueue.Enqueue((pulse, pass, cur.name));
            }
        }
        if (newQueue.Count == 0)
            break;
        (queue, newQueue) = (newQueue, queue);
    }
    return (lowCount, highCount);
}
(Dictionary<string, FlipFlop> flops, Dictionary<string, Conjuction> conj, string[] broadCast) Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    string[] broadCast = null;
    var flops = new Dictionary<string, FlipFlop>();
    var conj = new Dictionary<string, Conjuction>();
    for (int i = 0; i < lines.Length; i++)
    {
        if (lines[i][0] == '%')
        {
            var t = lines[i].IndexOf("->");
            var name = lines[i].Substring(1, t - 2);
            var sendTo = lines[i].Substring(t + 2).Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            flops.Add(name, new FlipFlop(sendTo));
        }
        else if (lines[i][0]=='&')
        {
            var t = lines[i].IndexOf("->");
            var name = lines[i].Substring(1, t - 2);
            var sendTo = lines[i].Substring(t + 2).Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            conj.Add(name, new Conjuction(sendTo));
        }
        else 
        {
            broadCast = lines[i].Substring("broadcaster -> ".Length).Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
        }
    }

    foreach (var flop in flops)
    {
        foreach (var send in flop.Value.SendTo)
        {
            if (conj.TryGetValue(send, out var val))
            {
                val.Point(flop.Key);
            }
        }
    }
    return (flops, conj, broadCast);
}


public class FlipFlop
{
    public bool State;
    public string[] SendTo;
    public FlipFlop(string[] sendTo)
    {
        SendTo = sendTo;
    }
    public Pulse Process(Pulse pulse)
    {
        if (pulse is Pulse.High)
            return Pulse.Nothing;
        State = !State;
        if (State)
            return Pulse.High;
        return Pulse.Low;
    }


}

public class Conjuction
{
    public string[] SendTo;
    public Conjuction(string[] sendTo)
    {
        state = new Dictionary<string, Pulse>();
        SendTo = sendTo;
    }
    public void Point(string p)
    {
        state[p] = Pulse.Low;
    }
    public Dictionary<string, Pulse> state;
    public Pulse Process(string prev, Pulse pulse)
    {
        state[prev] = pulse;
        foreach (var kvp in state)
        {
            if (kvp.Value == Pulse.Low)
                return Pulse.High;
        }
        return Pulse.Low;
    }
}

public enum Pulse
{
    High,
    Low,
    Nothing
}