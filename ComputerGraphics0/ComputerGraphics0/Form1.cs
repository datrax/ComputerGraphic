using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ComputerGraphics0
{
    public partial class Form1 : Form
    {
        float KeyX1 = -5;
        float KeyY1 = -5;
        float radius = 3;

        float KeyY2 = 7;
        float KeyY3 = 10;
        float centerRadius = 3f;

        PaintEventArgs e;
        List<Point> vertexes = new List<Point>();
        Pen dashPen = new Pen(Color.AliceBlue, 3);
        Pen pen = new Pen(Color.AliceBlue, 4);
        private int size;
        public int Size
        {
            set
            {
                if (value > 0)
                    size = value;
            }
            get
            {
                return size;
            }
        }
        public Point ToGlobalCoordinates(float x, float y)
        {
            return new Point(Convert.ToInt32(x * Size + pictureBox1.Width / 2.0), Convert.ToInt32(pictureBox1.Height / 2.0 - y * Size));
        }
        public Point ToLocalCoordinates(Point point)
        {
            return new Point(Convert.ToInt32((point.X - pictureBox1.Width / 2.0) / Size), Convert.ToInt32(( pictureBox1.Height / 2.0-point.Y) / Size));
        }
        public Form1()
        {
            Size = 20;
            InitializeComponent();

            dashPen.DashStyle = DashStyle.DashDot;
            pictureBox1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseWheel);
            pictureBox1.MouseMove += PictureBox1_MouseMove;
        }

        private async void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
          //  await Task.Run(() =>
         //   {
                foreach (var vertex in vertexes)
                {
                    if (Math.Abs(e.X - vertex.X) < 3 && Math.Abs(e.Y - vertex.Y) < 3)
                    {
                        Point localpoint = ToLocalCoordinates(vertex);
                        string text = "N "+vertexes.IndexOf(vertex)+"\nGlobal:" + vertex.X + " " + vertex.Y + "\nLocal:" + localpoint.X + " " + localpoint.Y;
                        this.Invoke(new Action(() =>
                         toolTip1.Show(text, pictureBox1, 1000)));
                        return;
                    }
                }
         //   });
        }

        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Delta > 0)
                Size += 2;
            else
                Size -= 2;
            pictureBox1.Invalidate();

        }
        private void DrawFigure(object sender, PaintEventArgs e)
        {
            vertexes.Clear();
            this.e = e;
            DrawGrid(e);
            DrawAxes(e);


            DrawCircle(ToGlobalCoordinates(KeyX1, KeyY1), radius * Size, 0, 360, e);
            DrawCircle(ToGlobalCoordinates(-KeyX1, KeyY1), radius * Size, 0, 360, e);
            DrawCircle(ToGlobalCoordinates(0, 0), centerRadius * Size, 0, 360, e);
            DrawCircle(ToGlobalCoordinates(0, KeyY2), centerRadius * Size, 90, 270, e);
            Point point = ClockWiseRotation(new Point(0, Convert.ToInt32(radius * size)), 135);
            Point keyCenter = ToGlobalCoordinates(KeyX1, KeyY1);
            Point point1 = new Point(point.X + keyCenter.X, keyCenter.Y - point.Y);
            Point point3 = new Point(keyCenter.X - point.X, keyCenter.Y + point.Y);
            Point keyCenter2 = ToGlobalCoordinates(-KeyX1, KeyY1);
            point = ClockWiseRotation(new Point(0, Convert.ToInt32(radius * size)), 225);
            Point point2 = new Point(point.X + keyCenter2.X, keyCenter2.Y - point.Y);
            Point point4 = new Point(keyCenter2.X - point.X, keyCenter2.Y + point.Y);
            //Point point2=new Point()
            DrawLine(pen, point1.X, point1.Y, point2.X, point2.Y);

            DrawLine(dashPen, point1.X, point1.Y, point3.X, point3.Y);
            DrawLine(dashPen, point2.X, point2.Y, point4.X, point4.Y);

            DrawLine(dashPen, point3.X, point1.Y, point1.X, point3.Y);
            DrawLine(dashPen, point2.X, point4.Y, point4.X, point2.Y);

            DrawLine(pen, point3.X, point3.Y, ToGlobalCoordinates(0 - centerRadius, KeyY2).X, ToGlobalCoordinates(0 - centerRadius, KeyY2).Y);
            DrawLine(pen, point4.X, point4.Y, ToGlobalCoordinates(0 + centerRadius, KeyY2).X, ToGlobalCoordinates(0 + centerRadius, KeyY2).Y);

            int centerX1 = Convert.ToInt32((point3.X + ToGlobalCoordinates(0 - centerRadius, KeyY2).X) / 2.0);
            int centerY1 = Convert.ToInt32((point3.Y + ToGlobalCoordinates(0 - centerRadius, KeyY2).Y) / 2.0);

            int centerX2 = Convert.ToInt32((point4.X + ToGlobalCoordinates(0 + centerRadius, KeyY2).X) / 2.0);
            int centerY2 = Convert.ToInt32((point4.Y + ToGlobalCoordinates(0 + centerRadius, KeyY2).Y) / 2.0);

            DrawCircle(new Point(centerX1, centerY1), new Point(point3.X, point3.Y), 180, e);
            DrawCircle(new Point(centerX2, centerY2), ToGlobalCoordinates(Convert.ToInt32(centerRadius), KeyY2), 180, e);

            point = ClockWiseRotation(new Point(point3.X - centerX1, point3.Y - centerY1), -90);
            point.X += centerX1;
            point.Y += centerY1;
            DrawLine(dashPen, new Point(centerX1, centerY1), new Point(point.X, point.Y));

            point = ClockWiseRotation(new Point(point4.X - centerX2, point4.Y - centerY2), 90);
            point.X += centerX2;
            point.Y += centerY2;
            DrawLine(dashPen, new Point(centerX2, centerY2), new Point(point.X, point.Y));
            DrawLine(dashPen, ToGlobalCoordinates(-centerRadius, KeyY2), ToGlobalCoordinates(centerRadius, KeyY2));
            //  DrawCircle(ToGlobalCoordinates(0, 0), ToGlobalCoordinates(1, 1), 180, e);

            DrawLine(pen, ToGlobalCoordinates(0, KeyY3).X, ToGlobalCoordinates(0, KeyY3).Y, ToGlobalCoordinates(0 - centerRadius, KeyY2).X, ToGlobalCoordinates(0 - centerRadius, KeyY2).Y);
            DrawLine(pen, ToGlobalCoordinates(0, KeyY3).X, ToGlobalCoordinates(0, KeyY3).Y, ToGlobalCoordinates(0 + centerRadius, KeyY2).X, ToGlobalCoordinates(0 + centerRadius, KeyY2).Y);

        }

        private void Redraw(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
        private void DrawAxes(PaintEventArgs e)
        {
            
            DrawLine(dashPen, pictureBox1.Width / 2.0f, 0, pictureBox1.Width / 2.0f, Height);
            DrawLine(dashPen, 0, pictureBox1.Height / 2.0f, Width, pictureBox1.Height / 2.0f);
        }

        private void DrawLine(Pen pen, Point point1, Point point2)
        {
            e.Graphics.DrawLine(pen, point1, point2);
            if (!vertexes.Contains(point1))
                vertexes.Add(point1);
            if (!vertexes.Contains(point2))
                vertexes.Add(point2);
        }
        private void DrawLine(Pen pen, float x1, float y1, float x2, float y2)
        {
            e.Graphics.DrawLine(pen, x1, y1, x2, y2);
            if (!vertexes.Contains(new Point(Convert.ToInt32(x1), Convert.ToInt32(y1))))
                vertexes.Add(new Point(Convert.ToInt32(x1), Convert.ToInt32(y1)));
            if (!vertexes.Contains(new Point(Convert.ToInt32(x2), Convert.ToInt32(y2))))
                vertexes.Add(new Point(Convert.ToInt32(x2), Convert.ToInt32(y2)));
        }
        private void DrawCircle(Point center, float radius, float angle1, float angle2, PaintEventArgs e)
        {
            if (!vertexes.Contains(center))
                vertexes.Add(center);
            float step = 1;
            for (float angle = angle1 + step; angle <= angle2; angle += step)
            {
                float x1, y1, x2, y2;
                Point p = ClockWiseRotation(new Point(0, Convert.ToInt32(radius)), angle);
                x1 = p.X;
                y1 = p.Y;
                p = ClockWiseRotation(new Point(0, Convert.ToInt32(radius)), angle - step);
                x2 = p.X;
                y2 = p.Y;
                DrawLine(new Pen(Color.AliceBlue, 3), x1 + center.X, center.Y - y1, x2 + center.X, center.Y - y2);
            }

        }
        private void DrawCircle(Point center, Point from, float angle2, PaintEventArgs e)
        {
            if (!vertexes.Contains(center))
                vertexes.Add(center);
            float step = 1;
            float x = from.X - center.X;
            float y = center.Y - from.Y;
            for (float angle = step; angle <= angle2; angle += step)
            {
                float x1, y1, x2, y2;
                Point p = ClockWiseRotation(new Point(Convert.ToInt32(x), Convert.ToInt32(y)), angle);
                x1 = p.X;
                y1 = p.Y;
                p = ClockWiseRotation(new Point(Convert.ToInt32(x), Convert.ToInt32(y)), angle - step);
                x2 = p.X;
                y2 = p.Y;
                DrawLine(new Pen(Color.AliceBlue, 3), x1 + center.X, center.Y - y1, x2 + center.X, center.Y - y2);
            }
        }
        private Point ClockWiseRotation(Point point, float angle)
        {
            angle *= (float)Math.PI / 180.0f;
            return new Point(Convert.ToInt32(point.X * Math.Cos(angle) + point.Y * Math.Sin(angle)),
               Convert.ToInt32(-point.X * Math.Sin(angle) + point.Y * Math.Cos(angle)));
        }
        private void DrawGrid(PaintEventArgs e)
        {
            Font font = new Font("Microsoft Sans Serif", 8 * Size / 20+1);
            Brush brush = new SolidBrush(Color.Black);
            for (float i = 1; pictureBox1.Width / 2.0 + i * Size < pictureBox1.Width; i++)
            {
                
                DrawLine(new Pen(Color.Azure), pictureBox1.Width / 2.0f + i * Size, 0, pictureBox1.Width / 2.0f + i * Size, Height);
                e.Graphics.DrawString(i.ToString(),font,brush, pictureBox1.Width / 2.0f + i * Size-5,pictureBox1.Height/2.0f+5f);
                DrawLine(new Pen(Color.Azure), pictureBox1.Width / 2.0f - i * Size, 0, pictureBox1.Width / 2.0f - i * Size, Height);
                e.Graphics.DrawString((-i).ToString(), font, brush, pictureBox1.Width / 2.0f - i * Size - 5, pictureBox1.Height / 2.0f + 5f);
            }
            for (float i = 1; pictureBox1.Height / 2.0 + i * Size < pictureBox1.Height; i++)
            {
                DrawLine(new Pen(Color.Azure), 0, pictureBox1.Height / 2.0f + i * Size, Width, pictureBox1.Height / 2.0f + i * Size);
                e.Graphics.DrawString((-i).ToString(), font, brush, pictureBox1.Width / 2.0f + 5f, pictureBox1.Height / 2.0f + i * Size - 5);
                DrawLine(new Pen(Color.Azure), 0, pictureBox1.Height / 2.0f - i * Size, Width, pictureBox1.Height / 2.0f - i * Size);
                e.Graphics.DrawString((i).ToString(), font, brush, pictureBox1.Width / 2.0f + 5f, pictureBox1.Height / 2.0f - i * Size);
            }
        }

        private void SetSize(object sender, EventArgs e)
        {
            int k = 0;
            Int32.TryParse(textBox1.Text, out k);
            if (k != 0)
                Size = k;
            pictureBox1.Invalidate();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            KeyX1 = (float)(numericUpDown1.Value);
            pictureBox1.Invalidate();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            KeyY1 = (float)(numericUpDown2.Value);
            pictureBox1.Invalidate();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            radius = (float)(numericUpDown3.Value);
            pictureBox1.Invalidate();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            KeyY2 = (float)(numericUpDown4.Value);
            pictureBox1.Invalidate();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            centerRadius = (float)(numericUpDown5.Value);
            pictureBox1.Invalidate();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            KeyY3 = (float)(numericUpDown6.Value);
            pictureBox1.Invalidate();
        }
    }
}
