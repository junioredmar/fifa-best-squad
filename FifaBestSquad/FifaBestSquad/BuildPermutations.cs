using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifaBestSquad
{
    public class BuildPermutations
    {
        private int sum;

        public List<string> results;

        public List<string> Build(string tobedone)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine("Building Permutations...");

            results = new List<string>();
            sum = 0;
            char[] arr = tobedone.ToCharArray();
            GetPer(arr);

            stopWatch.Stop();
            Console.WriteLine("First Permutation" + results.FirstOrDefault());
            Console.WriteLine("Last Permutation: " + results.LastOrDefault());
            Console.WriteLine("Done building permutations. Iterations: " + sum);
            Console.WriteLine("TEMPO TOTAL LEVADO:" + Math.Round(stopWatch.Elapsed.TotalSeconds) + " segundos");
            Console.WriteLine("---------------------------");

            return results;
        }

        private void Swap(ref char a, ref char b)
        {
            if (a == b) return;

            a ^= b;
            b ^= a;
            a ^= b;
        }

        public void GetPer(char[] list)
        {
            int x = list.Length - 1;
            GetPer(list, 0, x);
        }

        private void GetPer(char[] list, int k, int m)
        {
            if (k == m)
            {
                sum++;
                results.Add(new string(list));
                //Console.WriteLine(list);
            }
            else
                for (int i = k; i <= m; i++)
                {
                    Swap(ref list[k], ref list[i]);
                    GetPer(list, k + 1, m);
                    Swap(ref list[k], ref list[i]);
                }
        }

    }
}
