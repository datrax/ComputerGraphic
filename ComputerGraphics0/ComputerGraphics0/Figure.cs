using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerGraphics0
{
    public class Figure
    {

        public Figure()
        {
        }
        private class Primitive
        {
            public List<Point> vertexSet = new List<Point>();
            public Pen pen = new Pen(Color.AliceBlue, 4);

            public Primitive(Pen pen, List<Point> vertexSet)
            {
                this.vertexSet = vertexSet;
                this.pen = pen;
            }

        }

        List<Primitive> primitiveSet = new List<Primitive>();
        public void AddLine(Pen pen, Point point1, Point point2)
        {
            List<Point> points = new List<Point> { point1, point2 };
            primitiveSet.Add(new Primitive(pen, points));
        }
        public void AddLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            List<Point> points = new List<Point>
            {
                new Point(Convert.ToInt32(x1), Convert.ToInt32(y1)),
                new Point(Convert.ToInt32(x2), Convert.ToInt32(y2))
            };
            primitiveSet.Add(new Primitive(pen, points));
        }

        public void AddCircle(Point center, Point from, float angle2, Pen pen)
        {
            const float step = 1;
            float x = from.X - center.X;
            float y = center.Y - from.Y;
            List<Point> points = new List<Point>();
            for (float angle = 0; angle <= angle2; angle += step)
            {
                Point p = ClockWiseRotation(new Point(Convert.ToInt32(x), Convert.ToInt32(y)), angle);
                p = new Point(p.X + center.X, center.Y - p.Y);
                points.Add(p);
            }
            primitiveSet.Add(new Primitive(pen, points));
        }
        public void AddCircle(Point center, float radius, float angle1, float angle2, Pen pen)
        {
            const float step = 1;
            List<Point> points = new List<Point>();
            for (float angle = angle1 + step; angle <= angle2; angle += step)
            {
                Point p = ClockWiseRotation(new Point(0, Convert.ToInt32(radius)), angle);
                p = new Point(p.X + center.X, center.Y - p.Y);
                points.Add(p);
            }
            primitiveSet.Add(new Primitive(pen, points));
        }
        private Point ClockWiseRotation(Point point, float angle)
        {
            angle *= (float)Math.PI / 180.0f;
            return new Point(Convert.ToInt32(point.X * Math.Cos(angle) + point.Y * Math.Sin(angle)),
               Convert.ToInt32(-point.X * Math.Sin(angle) + point.Y * Math.Cos(angle)));
        }

        public void Draw(Graphics e)
        {
            foreach (var primitive in primitiveSet)
            {
                for (var i = 0; i < primitive.vertexSet.Count - 1; i++)
                {
                    e.DrawLine(primitive.pen, primitive.vertexSet[i], primitive.vertexSet[i + 1]);
                }

            }
        }

        public void Move(float x, float y)
        {
            foreach (var t in primitiveSet)
                t.vertexSet =
                    MatrixManager.GetAsPoints(MatrixManager.MultipleMatrix(MatrixManager.GetMovementMatrix(x, y),
                        MatrixManager.ConvertToMatrix(t.vertexSet)));
        }
        public void Rotate(float x, float y,float angle)
        {
           var matrx= MatrixManager.MultipleMatrix(
                MatrixManager.MultipleMatrix(MatrixManager.GetMovementMatrix(x, y),
                    MatrixManager.GetRotationMatrix(angle)), MatrixManager.GetMovementMatrix(-x, -y));
            foreach (var t in primitiveSet)
                t.vertexSet =
                    MatrixManager.GetAsPoints(MatrixManager.MultipleMatrix(matrx,
                        MatrixManager.ConvertToMatrix(t.vertexSet)));
        }
    }
}

    


