PartOne();
PartTwo();
void PartOne()
{
    var ans =0;
    foreach (var line in File.ReadAllLines("input.txt"))
    {
        var gt = line.Split(':');
        var cards = gt[1].Split('|');
        var winning =cards[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x=>int.Parse(x)).ToHashSet();
        var got = cards[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x=>int.Parse(x)).Where(x=>winning.Contains(x)).Count();
        if (got!=0)
            ans += 1<<(got-1);

    }
    Console.WriteLine(ans);
}


void PartTwo()
{
    var ans =0;
    var data =  File.ReadAllLines("input.txt");
    var mul = new int[data.Length];
    Array.Fill(mul,1);
    for(int i=0; i<data.Length; i++)
    {
        var line = data[i];
        var gt = line.Split(':');
        var cards = gt[1].Split('|');
        var winning =cards[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x=>int.Parse(x)).ToHashSet();
        var got = cards[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x=>int.Parse(x)).Where(x=>winning.Contains(x)).Count();
        if (got!=0)
        {
            for(int z = 0; z<got&& 1+i+z<data.Length; z++)
            {
                
                mul[i+z+1]+=mul[i];
            }
        }

    }
    Console.WriteLine(mul.Sum());
}

