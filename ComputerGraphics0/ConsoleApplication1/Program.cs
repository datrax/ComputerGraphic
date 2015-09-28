using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] a = new int[,] { {1,2,3}, {1,1,1}, {1,1,1} };
            int[,] b = new int[,] { {1,0,5}, {0,1,0},{0,0,1} };
            int[,] c = new int[a.GetLength(0), b.GetLength(1)];
            Random rnd = new Random();

            //присваиваем значения матрицам(массивам)
       
            //ну и собственно сам алгоритм перемножения 
            //и вывода получившейся матрицы
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int k = 0; k < c.GetLength(1); k++)
                {
                    for (int j = 0; j < a.GetLength(1); j++)
                    {
                        c[i, k] += a[j, k] * b[i, j];
                    }
                    Console.Write("{0} ", c[i, k]);
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
