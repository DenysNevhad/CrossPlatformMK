using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    struct Item
    {
        public int Id;
        public long Wi;
        public long Di;

        public Item(int id, long wi, long di)
        {
            Id = id;
            Wi = wi;
            Di = di;
        }
    }

    static void Main()
    {
        var input = File.ReadAllLines("INPUT.txt");
        var firstLine = input[0].Split();
        int n = int.Parse(firstLine[0]);
        long r = long.Parse(firstLine[1]);

        var items = new List<Item>();

        for (int i = 0; i < n; i++)
        {
            var line = input[i + 1].Split();
            long wi = long.Parse(line[0]);
            long di = long.Parse(line[1]);
            items.Add(new Item(i + 1, wi, di));
        }

        items = items.OrderBy(item => item.Di).ToList();

        var plan = new List<(long time, int id)>();
        long currentTime = 0;

        foreach (var item in items)
        {
            long timeOnLine = item.Wi;

            if (currentTime + timeOnLine <= item.Di)
            {
                currentTime += timeOnLine;
            }
            else
            {
                long timeNeededOnBattery = item.Wi - r;

                if (currentTime + timeNeededOnBattery > item.Di)
                {
                    File.WriteAllText("OUTPUT.txt", "Impossible");
                    return;
                }

                plan.Add((currentTime, item.Id));
                currentTime += timeNeededOnBattery;
            }
        }

        using (var writer = new StreamWriter("OUTPUT.txt"))
        {
            foreach (var (time, id) in plan)
            {
                writer.WriteLine($"{time} {id}");
            }
        }
    }
}
