using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace seminar12
{
    public partial class Form1 : Form
    {
        private List<PointF> points = new List<PointF>();
        public Form1()
        {
            InitializeComponent();
        }
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (points.Count < 3)
            {
                points.Add(e.Location);
                this.Invalidate(); 
            }
            else
            {
                points.Clear();
                points.Add(e.Location); 
                this.Invalidate();
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int i = 0; i < points.Count; i++)
            {
                PointF pt = points[i];
                g.FillEllipse(Brushes.Black, pt.X - 3, pt.Y - 3, 6, 6);

            }

            if (points.Count == 3)
            {
                PointF A = points[0], B = points[1], C = points[2];

                Pen Pen1 = new Pen(Color.Yellow, 5);
                g.DrawLine(Pen1, A, B);
                g.DrawLine(Pen1, B, C);
                g.DrawLine(Pen1, C, A);

               DrawVor(g, A, B, C);

                PointF center = Center(A, B, C);
                g.FillEllipse(Brushes.Magenta, center.X - 5, center.Y - 5, 10, 10);

                float radius = Distanta(center, A);
                g.DrawEllipse(Pens.Purple, center.X - radius, center.Y - radius, radius * 2, radius * 2);
            }
        }
        private void DrawVor(Graphics g, PointF A, PointF B, PointF C)
        {
            PointF center = Center(A, B, C);

            PointF[] midAB = GetMidAndPerpDir(A, B);
            PointF[] midBC = GetMidAndPerpDir(B, C);
            PointF[] midCA = GetMidAndPerpDir(C, A);

            PointF extAB = Expand(midAB[0], midAB[1], 1000);
            PointF extBC = Expand(midBC[0], midBC[1], 1000);
            PointF extCA = Expand(midCA[0], midCA[1], 1000);

            Brush[] colors = { Brushes.Red, Brushes.Green, Brushes.Blue };
            PointF[][] regiuni = new PointF[][]
            {
                 new PointF[] { center, extAB, extCA }, 
                 new PointF[] { center, extAB, extBC }, 
                 new PointF[] { center, extBC, extCA }  
            };

            PointF[] pointss = { A, B, C };
            for (int i = 0; i < 3; i++)
            {
                g.FillPolygon(colors[i], regiuni[i]);

                PointF regiuneCenter = new PointF(
                    (regiuni[i][0].X + regiuni[i][1].X + regiuni[i][2].X) / 3,
                    (regiuni[i][0].Y + regiuni[i][1].Y + regiuni[i][2].Y) / 3
                );

            }

            Med(g, A, B, Color.White);
            Med(g, B, C, Color.White);
            Med(g, C, A, Color.White);
        }

        private void Med(Graphics g, PointF p1, PointF p2, Color color)
        {
            PointF mid = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            if (dx == 0) 
            {
                g.DrawLine(new Pen(color), mid.X - 1000, mid.Y, mid.X + 1000, mid.Y);
            }
            else if (dy == 0) 
            {
                g.DrawLine(new Pen(color), mid.X, mid.Y - 1000, mid.X, mid.Y + 1000);
            }
            else
            {
                float slope = dy / dx;
                float perpSlope = -1 / slope;
                float b = mid.Y - perpSlope * mid.X;

                PointF pA = new PointF(mid.X - 200, perpSlope * (mid.X - 200) + b);
                PointF pB = new PointF(mid.X + 200, perpSlope * (mid.X + 200) + b);
                g.DrawLine(new Pen(color), pA, pB);
            }
        }
        private PointF[] GetMidAndPerpDir(PointF p1, PointF p2) 
        {
            PointF mid = new PointF((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            PointF perp = new PointF(-dy, dx);
            float len = (float)Math.Sqrt(perp.X * perp.X + perp.Y * perp.Y);
            return new PointF[] { mid, new PointF(perp.X / len, perp.Y / len) };
        }

        private PointF Expand(PointF start, PointF dir, float dist)
        {
            return new PointF(start.X + dir.X * dist, start.Y + dir.Y * dist);
        }
        private PointF Center(PointF A, PointF B, PointF C)
        {
            float D = 2 * (A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y));
            float Ux = ((A.X * A.X + A.Y * A.Y) * (B.Y - C.Y) +
                        (B.X * B.X + B.Y * B.Y) * (C.Y - A.Y) +
                        (C.X * C.X + C.Y * C.Y) * (A.Y - B.Y)) / D;
            float Uy = ((A.X * A.X + A.Y * A.Y) * (C.X - B.X) +
                        (B.X * B.X + B.Y * B.Y) * (A.X - C.X) +
                        (C.X * C.X + C.Y * C.Y) * (B.X - A.X)) / D;
            return new PointF(Ux, Uy);
        }
        private float Distanta(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) +
                                    (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
