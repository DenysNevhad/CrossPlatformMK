using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Mkr.Tests
{
    public class ProgramTests
    {
        public class Item
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

        public static List<Item> GetItemsFromInput(string input)
        {
            var lines = input.Split('\n');
            var firstLine = lines[0].Split();
            int n = int.Parse(firstLine[0]);
            long r = long.Parse(firstLine[1]);

            var items = new List<Item>();

            for (int i = 0; i < n; i++)
            {
                var line = lines[i + 1].Split();
                long wi = long.Parse(line[0]);
                long di = long.Parse(line[1]);
                items.Add(new Item(i + 1, wi, di));
            }

            return items;
        }

        public static string PlanDrying(string input)
        {
            var items = GetItemsFromInput(input);
            items = items.OrderBy(item => item.Di).ToList();

            var plan = new List<(long time, int id)>();
            long currentTime = 0;

            foreach (var item in items)
            {
                if (item.Wi <= item.Di)
                {
                    currentTime += item.Wi;
                    continue;
                }

                long timeNeededOnBattery = item.Wi - item.Di;

                if (currentTime + timeNeededOnBattery > item.Di)
                {
                    return "Impossible";
                }

                plan.Add((currentTime, item.Id));
                currentTime += timeNeededOnBattery + item.Wi;
            }

            return string.Join("\n", plan.Select(p => $"{p.time} {p.id}"));
        }

        [Fact]
        public void TestValidDryingPlan()
        {
            var input = "5 10\n2 5\n3 6\n5 10\n6 12\n4 9\n";
            var expected = "0 1\n2 2\n7 3\n13 5\nImpossible";

            var result = PlanDrying(input);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestImpossiblePlan()
        {
            var input = "3 10\n3 5\n2 7\n5 10\n";
            var expected = "Impossible";

            var result = PlanDrying(input);

            Assert.Equal(expected, result);
        }
    }
}
