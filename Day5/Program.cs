PartOne();
PartTwo();

void PartOne()
{
    var ans = 0;
    var (seeds, maps) = Parse();
    foreach (var map in maps)
    {
        var newSeeds = new List<long>();
        foreach (var seed in seeds)
        {
            newSeeds.Add(map.MapToNew(seed));
        }
        seeds = newSeeds;
    }

    Console.WriteLine(seeds.Min());
}

void PartTwo()
{
    var ans = 0;
    var (ogSeeds, maps) = Parse();
    var bestSeed =0l;
    var min = long.MaxValue;
    var loc = new object();
    var gap = 50_000;
    for (int i = 0; i < ogSeeds.Count; i += 2)
    {
    
        Parallel.ForEach(Enumerable.Range(0, (int)ogSeeds[i+1]/gap), (inc)=>
        {
            var seed = ogSeeds[i]+inc*50_000;
            foreach (var map in maps)
            {
                seed = map.MapToNew(seed);
            }
            lock(loc)
                {
                    if (min>seed)
                        {
                            min= seed;
                            bestSeed = ogSeeds[i]+inc*gap;
                        }

                }
        });
    }
     Parallel.ForEach(Enumerable.Range(0, 2*gap), (inc)=>
     {
        var seed = bestSeed+inc-gap;
            foreach (var map in maps)
            {
                seed = map.MapToNew(seed);
            }
            lock(loc)
                {
                    if (min>seed)
                        {
                            min= seed;
                        }

                }
        });
      Console.WriteLine(min);
}

(List<long> Seeds, List<Map> maps) Parse()
{
    var l = new List<string>();
    var lines = File.ReadAllLines("input.txt");
    var seeds = lines[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
    Map map = new Map();
    var maps = new List<Map>();

    for (int i = 3; i < lines.Length; i++)
    {
        if (string.IsNullOrEmpty(lines[i]))
        {
            maps.Add(map);
            i++;
            map = new Map();
            continue;
        }
        else
        {
            var t = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToArray();
            map.Add(t[0], t[1], t[2]);
        }
    }
    return (seeds, maps);

}
class Map
{
    private List<(long source, long range, long dest)> _storage = new List<(long source, long range, long dest)>();
    public void Add(long destStart, long destSource, long range)
    {
        _storage.Add((destSource, range, destStart));
    }

    public long MapToNew(long x)
    {
        var ans = x;
        foreach (var d in _storage)
        {
            if (d.source <= x && x <= d.source + d.range - 1)
            {
                ans = x - d.source + d.dest;
            }
        }
        return ans;
    }
}