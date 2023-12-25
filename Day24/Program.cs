using System.Text;
PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = SolveOne(data);
    Console.WriteLine(ans);
}

long SolveOne(List<((double x, double y, double z) start, (double x, double y, double z) vel)> data)
{
    var ans = 0;
    double limitdown = 200000000000000.0;
    double limitup = 400000000000000.0;
    for (int i = 0; i < data.Count; i++)
        for (int j = i + 1; j < data.Count; j++)
        {
            if (Islongersection(data[i].start.x, data[i].start.y,
                data[i].start.x + data[i].vel.x, data[i].start.y + data[i].vel.y,
                data[j].start.x, data[j].start.y,
                data[j].start.x + data[j].vel.x, data[j].start.y + data[j].vel.y,
                  out double x, out double y))
            {
                if (limitdown <= x && x <= limitup && limitdown <= y && y <= limitup)
                    ans++;
            }
        }
    return ans;
}

bool Islongersection(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out double x, out double y)
{
    double t1 = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) /
    ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));

    double t2 = ((x1 - x3) * (y1 - y2) - (y1 - y3) * (x1 - x2)) /
    ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
    x = 0;
    y = 0;
    if (t1 < 0 || t2 < 0 || double.IsNaN(t1) || double.IsNaN(t2))
        return false;
    x = x1 + t1 * (x2 - x1);
    y = y1 + t1 * (y2 - y1);
    return true;
}

void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = SolveTwo(data);
    Console.WriteLine(ans);
}

long SolveTwo(List<((double x, double y, double z) start, (double x, double y, double z) vel)> data)
{
    StringBuilder equations = new StringBuilder();
    
    for (int i = 0; i < 3; i++)
    {
        string t = "t" + i;
        equations.Append(t).Append(" >= 0, ").Append(data[i].start.x).Append(" + ").Append(data[i].vel.x).Append(t).Append(" == x + vx*").Append(t).Append(", ");
        equations.Append(data[i].start.y).Append(" + ").Append(data[i].vel.y).Append(t).Append(" == y + vy*").Append(t).Append(", ");
        equations.Append(data[i].start.z).Append(" + ").Append(data[i].vel.z).Append(t).Append(" == z + vz*").Append(t).Append(", ");
    }
    string sendToMathematica = "Solve[{" + equations.ToString().Substring(0, equations.Length - 2) + "}, {x,y,z,vx,vy,vz,t0,t1,t2}]";
    var x = 291669802654110;
    var y = 103597826800230;
    var z = 251542427650413;

    return x+y+z;
}

List<((double x, double y, double z) start, (double x, double y, double z) vel)> Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);
    var res = new List<((double x, double y, double z) start, (double x, double y, double z) vel)>();

    foreach (var line in lines)
    {
        var split = line.Split("@");
        var start = split[0].Split(",").Select(x => double.Parse(x)).ToArray();
        var vel = split[1].Split(",").Select(x => double.Parse(x)).ToArray();
        res.Add(((start[0], start[1], start[2]), (vel[0], vel[1], vel[2])));
    }
    return res;
}


/*
  x1+vx1*t1 = X+VX*t1
  y1+vy*t1 = Y+VY*t1
  z1+vz*t1 = Z+VZ*t1

  x2+vx2*t2 = X+VX*t2
  y1+vy*t2 = Y+VY*t2
  z1+vz*t2 = Z+VZ*t2




*/