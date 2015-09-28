using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics0
{
    internal static class MatrixManager
    {
        public static float[,] GetMovementMatrix(float x, float y)
        {
            return new float[,]
            {
                {1, 0, x}, {0, 1, y}, {0, 0, 1}
            };
        }

        public static float[,] GetRotationMatrix(float angle)
        {
            angle *= (float)Math.PI / 180.0f;
            return new float[,]
            {
                {(float)Math.Cos(angle),(float) Math.Sin(angle), 0}, {-(float) Math.Sin(angle), (float)Math.Cos(angle), 0}, {0, 0, 1}
            };
        }
        public static float[,] MultipleMatrix(float[,] a, float[,] b)
        {
      
            float[,] r = new float[a.GetLength(0), b.GetLength(1)];
            Parallel.For(0, a.GetLength(0), (i) =>
            {
                for (int j = 0; j < b.GetLength(1); j++)
                {
                    for (int k = 0; k < b.GetLength(0); k++)
                    {
                        r[i, j] += a[i, k] * b[k, j];
                    }
                }
            });
            return r;

        }

        public static float[,] ConvertToMatrix(List<Point> points)
        {
            float[,] matr = new float[ 3,points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                matr[0, i] = points[i].X;
                matr[1, i] = points[i].Y;
                matr[2, i] = 1;
            }
            return matr;
        }

        public static List<Point> GetAsPoints(float[,] matrix)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                points.Add(new Point(Convert.ToInt32(matrix[0, i]), Convert.ToInt32(matrix[1, i])));
            }
            return points;
        }
    }
}
