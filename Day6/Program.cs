PartOne();
PartTwo();
void PartOne()
{
    var data = Parse();
    var ans = 1;
    foreach(var dat in data)
    {
        ans*=SolveOne(dat.time, dat.distance);
    }
    Console.WriteLine(ans);
}

void PartTwo()
{
 var data = ParseTwo();

        var ans = SolveTwo(data.time, data.distance);
    
    Console.WriteLine(ans);
}

(int time, int distance)[] Parse()
{
    var lines = File.ReadAllLines("input.txt");
    var t = lines[0].Split(':',StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x=>int.Parse(x)).ToArray();
    var d = lines[1].Split(':',StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x=>int.Parse(x)).ToArray();
    var ans = new  (int time, int distance)[t.Length]; 
    for(int i=0; i< t.Length;i++)
        {
            ans[i].time = t[i];
            ans[i].distance=d[i];
        }
        return ans;
}

(long time, long distance) ParseTwo()
{
    var lines = File.ReadAllLines("input.txt");
   return ( long.Parse(lines[0].Split(':',StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ","")),
    long.Parse(lines[1].Split(':',StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ","")));

}

int SolveOne(int time, int distance)
{
    var ans=0;
    for(int i=1; i< time; i++)
    {
        if (i*(time-i)>distance)
        ans++;
    }
    return ans;
}

long SolveTwo(long time, long distance)
{
    var ans=0l;
    for(long i=1; i< time; i++)
    {
        if (i*(time-i)>distance)
        ans++;
    }
    return ans;
}