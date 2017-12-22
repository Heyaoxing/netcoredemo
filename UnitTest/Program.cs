using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, int> person = new Dictionary<int, int>();
            #region 制作数据
            for (int i = 1; i <= 100; i++)
            {
                person.Add(i, 100);
            }
            #endregion

            #region 随机将一个人的1块钱给另一个随机的人
            for (int i = 1; i <= 10000; i++)
            {
                for (int j = 1; j <= 100; j++)
                {
                    person[j] = person[j] - 1;
                    var reward = new Random(j + i+DateTime.Now.Millisecond).Next(1, 101);
                    person[reward] = person[reward] + 1;
                }
            }
            var list = from a in person orderby a.Value descending select a;
            foreach (var item in list)
            {
                Console.WriteLine($"{item.Key}:{item.Value}");
            }

            #endregion

            Console.Read();
        }
    }
}
