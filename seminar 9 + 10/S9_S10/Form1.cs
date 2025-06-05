using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace S9_S10
{
    public partial class Form1 : Form
    {
        List<Point> poligon = new List<Point>();
        List<(Point, Point)> diagonale = new List<(Point, Point)>();

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                poligon.Add(e.Location);
                Invalidate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (poligon.Count < 3) return;

            diagonale.Clear();
            int n = poligon.Count;
            int Mod(int x, int m) => (x % m + m) % m;

            var sorted = poligon
                .Select((p, i) => new { Point = p, Index = i })
                .OrderByDescending(p => p.Point.Y)
                .ThenBy(p => p.Point.X)
                .ToList();

            Dictionary<int, int> helper = new Dictionary<int, int>();

            for (int sidx = 0; sidx < n; sidx++)
            {
                int i = sorted[sidx].Index;
                Point pi = poligon[i];
                Point piPrev = poligon[Mod(i - 1, n)];
                Point piNext = poligon[Mod(i + 1, n)];

                var type = GetVertexType(piPrev, pi, piNext);

                if (type == VertexType.Start)
                {
                    helper[i] = i;
                }
                else if (type == VertexType.End)
                {
                    int j = Mod(i - 1, n);
                    if (helper.ContainsKey(j))
                        diagonale.Add((pi, poligon[helper[j]]));
                }
                else if (type == VertexType.Split)
                {
                    // Caut cel mai apropiat vârf sub el din sorted (ideal: segment la stânga)
                    var candidate = sorted.FirstOrDefault(p => p.Point.Y < pi.Y);
                    if (candidate != null)
                    {
                        diagonale.Add((pi, candidate.Point));
                        helper[i] = candidate.Index;
                    }
                }
                else if (type == VertexType.Merge)
                {
                    int j = Mod(i - 1, n);
                    if (helper.ContainsKey(j))
                        diagonale.Add((pi, poligon[helper[j]]));
                }
                else if (type == VertexType.Regular)
                {
                    if (piNext.Y < pi.Y)
                    {
                        int j = Mod(i - 1, n);
                        if (helper.ContainsKey(j))
                            diagonale.Add((pi, poligon[helper[j]]));
                    }
                    else
                    {
                        int j = Mod(i + 1, n);
                        if (helper.ContainsKey(j))
                            diagonale.Add((pi, poligon[helper[j]]));
                    }
                }
            }

            Invalidate();
        }

        private enum VertexType { Start, End, Split, Merge, Regular }

        private VertexType GetVertexType(Point prev, Point current, Point next)
        {
            bool isAbovePrev = prev.Y < current.Y;
            bool isAboveNext = next.Y < current.Y;

            if (isAbovePrev && isAboveNext)
                return IsReflex(prev, current, next) ? VertexType.Split : VertexType.Start;
            else if (!isAbovePrev && !isAboveNext)
                return IsReflex(prev, current, next) ? VertexType.Merge : VertexType.End;
            else
                return VertexType.Regular;
        }

        private bool IsReflex(Point a, Point b, Point c)
        {
            return Cross(a, b, c) < 0;
        }

        private int Cross(Point a, Point b, Point c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (poligon.Count > 1)
                e.Graphics.DrawPolygon(Pens.Black, poligon.ToArray());

            foreach (var diag in diagonale)
                e.Graphics.DrawLine(Pens.Red, diag.Item1, diag.Item2);

            foreach (var pt in poligon)
                e.Graphics.FillEllipse(Brushes.Blue, pt.X - 3, pt.Y - 3, 6, 6);
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}
