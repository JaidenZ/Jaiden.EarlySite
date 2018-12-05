using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Jaide.SystemConsole
{
    class Program
    {

        public static int[] LittleNumArray = new int[1000];
        public static int[] MoreNumArray = new int[10000];

        static void Main(string[] args)
        {
            //初始化数据
            Random randon = new Random();
            for (int i = 0; i < LittleNumArray.Length; i++)
            {
                LittleNumArray[i] = randon.Next();
            }
            for (int i = 0; i < MoreNumArray.Length; i++)
            {
                MoreNumArray[i] = randon.Next();
            }

            //排序数组
            ConsoleNum(LittleNumArray);
            ConsoleNum(MoreNumArray);

            Console.ReadKey(false);
        }





        public static void ConsoleNum(int[] numarray)
        {
            List<int> littlelistfor = new List<int>();
            List<int> littlelistforeach = new List<int>();
            //开始排序
            //计时for循环
            int a = System.Environment.TickCount;
            for (int i = 0; i < numarray.Length; i++)
            {
                for (int t = 0; t < numarray.Length; t++)
                {
                    if(numarray[i] > numarray[t])
                    {
                        littlelistfor.Add(numarray[i]);
                    }
                    else
                    {
                        littlelistfor.Add(numarray[t]);
                    }
                }
            }
            Console.WriteLine(string.Format("for 循环冒泡排序长度为{0}的数组用时{1}", numarray.Length,System.Environment.TickCount - a));

            //计时foreach
            int b = System.Environment.TickCount;
            foreach (int min in numarray)
            {
                foreach (int max in numarray)
                {
                    if (min > max)
                    {
                        littlelistforeach.Add(min);
                    }
                    else
                    {
                        littlelistforeach.Add(max);
                    }
                }
            }
            Console.WriteLine(string.Format("foreach 循环冒泡排序长度为{0}的数组用时{1}", numarray.Length, System.Environment.TickCount - b));
        }
    }
}
