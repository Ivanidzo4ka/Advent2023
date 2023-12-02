PartOne();
PartTwo();

void PartOne()
{
    var ans=0;
    foreach (string line in File.ReadAllLines("input.txt"))
    {
        var colon = line.IndexOf(':');
        var id = int.Parse(line.Substring(5, colon-5));
        var rest= line.Substring(colon+2);
        var split = rest.Split(";");
        var good=true;
        foreach(var sp in split)
        {
            var (green, red, blue) = Break(sp);
            if (green>13|| red> 12|| blue>14)
            {
                good=false;
                break;
            }
        }
        if (good) ans+=id;
    }

    Console.WriteLine(ans);
}
(int green, int red, int blue) Break(string line)
{
    var t = line.Split(",");
    var green = 0;
    var red =0;
    var blue = 0;
    foreach(var p in t)
    {
        var d = p.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var v = int.Parse(d[0]);
        if (d[1]=="red") red = v;
        if (d[1]=="blue") blue = v;
        if (d[1]=="green") green = v;
    }
    return (green, red, blue);
}

void PartTwo()
{
    var ans=0;
    foreach (string line in File.ReadAllLines("input.txt"))
    {
        var colon = line.IndexOf(':');
        var rest= line.Substring(colon+2);
        var split = rest.Split(";");
        var ming = 0;
        var minr = 0;
        int minb = 0;
        foreach(var sp in split)
        {
            var (green, red, blue) = Break(sp);
            ming=Math.Max(ming, green);
            minr=Math.Max(minr, red);
            minb=Math.Max(minb, blue);
        }
        ans+= ming*minr*minb;
    }


    Console.WriteLine(ans);
}
