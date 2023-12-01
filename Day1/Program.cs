PartOne();
PartTwo();

void PartOne()
{
    var lines = File.ReadAllLines("input.txt");
    var sum = 0;
    foreach (var line in lines)
    {
        int first = -1;
        int last = -1;
        for (int i = 0; i < line.Length; i++)
        {
            if (char.IsDigit(line[i]))
            {
                if (first == -1)
                    first = line[i] - '0';
                last = line[i] - '0';
            }
        }
        int num = 10 * first + last;
        sum += num;
    }
    Console.WriteLine(sum);
}

void PartTwo()
{
    var lines = File.ReadAllLines("input.txt");
    var sum = 0;
    foreach (var line in lines)
    {
        int first = -1;
        int last = -1;
        for (int i = 0; i < line.Length; i++)
        {
            var digit = -1;
            if (char.IsDigit(line[i]))
            {
                digit = line[i] - '0';
            }else
            {
                if (line.Length-i>=5)
                {
                    var next = line.Substring(i, 5);
                    if (next=="seven") digit=7;
                    if (next=="eight") digit=8;
                    if (next=="three") digit=3;
                }
                if (line.Length-i>=4)
                {
                    var next = line.Substring(i, 4);
                    if (next=="four") digit=4;
                    if (next=="five") digit=5;
                    if (next=="nine") digit=9;
                }
                if (line.Length-i>=3)
                {
                    var next = line.Substring(i, 3);
                    if (next=="one") digit=1;
                    if (next=="two") digit=2;
                    if (next=="six") digit=6;
                }

            }

            if (digit != -1)
            {
                if (first == -1)
                    first = digit;
                last = digit;
            }

        }
        int num = 10 * first + last;
        sum += num;
    }
    Console.WriteLine(sum);
}