
PartOne();
PartTwo();

void PartOne()
{
    var ans = 0L;
    var data = Parse("input.txt");
    foreach (var state in data.states)
    {
        if (Process(state, data.rules) == "A")
            ans += state.Values.Sum();
    }
    Console.WriteLine(ans);
}

void PartTwo()
{
    var ans = 0L;
    var data = Parse("input.txt");
    ans = Walk(data.rules);
    Console.WriteLine(ans);

}
long Walk(Dictionary<string, List<Rules>> rules)
{
    long ans = 0L;
    var loops = new Dictionary<string, Intervals>();
    loops["x"] = new Intervals() { Start = 1, End = 4001 };
    loops["m"] = new Intervals() { Start = 1, End = 4001 };
    loops["a"] = new Intervals() { Start = 1, End = 4001 };
    loops["s"] = new Intervals() { Start = 1, End = 4001 };
    ans = GoOnAWalk("in", rules, loops);
    return ans;
}
long GoOnAWalk(string cur, Dictionary<string, List<Rules>> rules, Dictionary<string, Intervals> loops)
{

    if (cur == "R")
        return 0;
    if (cur == "A")
        return CountState(loops);
    var curRules = rules[cur];
    var newState = new Dictionary<string, Intervals>();
    var ans = 0L;
    newState["x"] = loops["x"];
    newState["m"] = loops["m"];
    newState["a"] = loops["a"];
    newState["s"] = loops["s"];
    foreach (var rule in curRules)
    {
        if (rule.Var != "")
        {
            var t = newState[rule.Var].Break(rule.Compare, rule.Less);
            newState[rule.Var] = t.left;
            ans += GoOnAWalk(rule.Value, rules, newState);
            newState[rule.Var] = t.right;
        }
        else
            ans += GoOnAWalk(rule.Value, rules, newState);
    }
    return ans;
}

long CountState(Dictionary<string, Intervals> loops)
{
    var ans = 1L;
    foreach (var kvp in loops)
        ans *= kvp.Value.End - kvp.Value.Start;
    return ans;
}

string Process(Dictionary<string, int> state, Dictionary<string, List<Rules>> rules)
{
    string cur = "in";
    while (!(cur == "A" || cur == "R"))
    {
        foreach (var rule in rules[cur])
        {
            var res = rule.Check(state);
            if (res != "")
            {
                cur = res;
                break;
            }
        }
    }
    return cur;
}

(Dictionary<string, List<Rules>> rules, List<Dictionary<string, int>> states) Parse(string fileName)
{
    var lines = File.ReadAllLines(fileName);

    bool readRatings = false;
    Dictionary<string, List<Rules>> ruleBook = new();

    var ratings = new List<Dictionary<string, int>>();
    for (int i = 0; i < lines.Length; i++)
    {
        if (lines[i] == "")
        {
            readRatings = true;
        }
        else if (readRatings)
        {
            var line = lines[i].Substring(1, lines[i].Length - 2);
            var curDic = new Dictionary<string, int>();
            ratings.Add(curDic);
            foreach (var rating in line.Split(","))
            {
                var tt = rating.Split("=");
                curDic[tt[0]] = int.Parse(tt[1]);
            }
        }
        else
        {
            var line = lines[i];
            var open = line.IndexOf("{");
            var name = line.Substring(0, open);
            var rules = line.Substring(open + 1, line.Length - open - 2);
            ruleBook[name] = rules.Split(",").Select(x => new Rules(x)).ToList();

        }
    }
    return (ruleBook, ratings);
}

public class Intervals
{
    public int Start;
    public int End;

    public (Intervals left, Intervals right) Break(int val, bool less)
    {
        var left = new Intervals();
        var right = new Intervals();
        {
            if (less)
            {
                //  100 .. 200
                //    < 300
                if (End < val)
                {
                    left.Start = this.Start;
                    left.End = this.End;
                }
                //   <150
                else if (Start < val && val < End)
                {
                    //100 ..150
                    left.Start = this.Start;
                    left.End = val;
                    // 150.. 200
                    right.Start = val;
                    right.End = this.End;
                }
                // <100
                else
                {
                    right.Start = this.Start;
                    right.End = this.End;
                }
            }
            else
            {   //  100 .. 200
                //   >99
                if (val < Start)
                {
                    left.Start = this.Start;
                    left.End = this.End;
                }
                // >150
                else if (Start < val && val < End)
                {
                    // 151..200
                    left.Start = val + 1;
                    left.End = this.End;
                    // 100.. 151
                    right.Start = this.Start;
                    right.End = val + 1;
                    // >200
                }
                else
                {
                    right.Start = this.Start;
                    right.End = this.End;
                }
            }
        }
        return (left, right);
    }
}

public class Rules
{
    public string Var;
    public int Compare;
    public bool Less;
    public string Value;
    public Rules(string rule)
    {
        if (rule.Contains(":"))
        {
            var split = rule.Split(":");
            if (split[0].Contains("<"))
            {
                Less = true;
                var ss = split[0].Split("<");
                Var = ss[0];
                Compare = int.Parse(ss[1]);
                Value = split[1];
            }
            else
            {
                Less = false;
                var ss = split[0].Split(">");
                Var = ss[0];
                Compare = int.Parse(ss[1]);
                Value = split[1];
            }

        }
        else
        {
            Var = "";
            Value = rule;
        }
    }

    public string Check(Dictionary<string, int> values)
    {
        if (Var == "")
            return Value;
        else
        {
            var dicVal = values[Var];
            if (Less)
                return dicVal < Compare ? Value : "";
            else return dicVal > Compare ? Value : "";
        }
    }
}
