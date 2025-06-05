using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace S10
{
    public partial class Form1 : Form
    {
        private List<PointF> points = new List<PointF>();
        private List<Tuple<int, int>> diagonals = new List<Tuple<int, int>>();
        private List<Tuple<int, int, int>> triangles = new List<Tuple<int, int, int>>();
        private bool isPolygonClosed = false;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            MouseClick += Form1_MouseClick;
            Paint += Form1_Paint;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isPolygonClosed)
            {
                points.Add(new PointF(e.X, e.Y));
                Invalidate();
            }
        }

        private void buttonInchide_Click(object sender, EventArgs e)
        {
            if (points.Count >= 3)
            {
                isPolygonClosed = true;
                Invalidate();
            }
        }

        private void buttonPartizioneaza_Click(object sender, EventArgs e)
        {
            if (!isPolygonClosed || points.Count < 4) return;

            diagonals.Clear();
            int n = points.Count;

            for (int i = 0; i < n; i++)
            {
                if (IsReflex(i))
                {
                    int bestCandidate = -1;
                    float minDistance = float.MaxValue;

                    for (int j = 0; j < n; j++)
                    {
                        if (j == i || j == (i - 1 + n) % n || j == (i + 1) % n) continue;

                        if (!IntersectsExistingEdges(i, j))
                        {
                            float distance = Distance(points[i], points[j]);
                            if (distance < minDistance)
                            {
                                bestCandidate = j;
                                minDistance = distance;
                            }
                        }
                    }

                    if (bestCandidate != -1)
                    {
                        diagonals.Add(Tuple.Create(i, bestCandidate));
                    }
                }
            }

            Invalidate();
        }

        private void buttonTrianguleaza_Click(object sender, EventArgs e)
        {
            triangles.Clear();
            List<Tuple<int, int>> addedDiagonals = new List<Tuple<int, int>>(diagonals);

            foreach (var diag in addedDiagonals)
            {
                int a = diag.Item1;
                int b = diag.Item2;
                List<int> subPolygon = new List<int>();
                int current = a;
                subPolygon.Add(current);

                while (current != b)
                {
                    current = (current + 1) % points.Count;
                    subPolygon.Add(current);
                }

                Triangulate(subPolygon);
            }

            Invalidate();
        }

        private void Triangulate(List<int> subPolygon)
        {
            if (subPolygon.Count < 3) return;

            List<int> indices = new List<int>(subPolygon);

            while (indices.Count > 3)
            {
                bool earFound = false;
                for (int i = 0; i < indices.Count; i++)
                {
                    int prev = indices[(i - 1 + indices.Count) % indices.Count];
                    int curr = indices[i];
                    int next = indices[(i + 1) % indices.Count];

                    if (IsConvex(prev, curr, next))
                    {
                        bool pointInside = false;
                        for (int j = 0; j < indices.Count; j++)
                        {
                            int p = indices[j];
                            if (p == prev || p == curr || p == next) continue;
                            if (PointInsideTriangle(points[p], points[prev], points[curr], points[next]))
                            {
                                pointInside = true;
                                break;
                            }
                        }

                        if (!pointInside)
                        {
                            triangles.Add(Tuple.Create(prev, curr, next));
                            diagonals.Add(Tuple.Create(Math.Min(prev, next), Math.Max(prev, next)));
                            indices.RemoveAt(i);
                            earFound = true;
                            break;
                        }
                    }
                }

                if (!earFound) break;
            }

            if (indices.Count == 3)
            {
                triangles.Add(Tuple.Create(indices[0], indices[1], indices[2]));
            }
        }

        private bool IntersectsExistingEdges(int i, int j)
        {
            PointF a = points[i];
            PointF b = points[j];

            for (int k = 0; k < points.Count; k++)
            {
                int k1 = k;
                int k2 = (k + 1) % points.Count;
                if (k1 == i || k1 == j || k2 == i || k2 == j) continue;
                if (SegmentsIntersect(a, b, points[k1], points[k2])) return true;
            }

            foreach (var diag in diagonals)
            {
                if ((diag.Item1 == i && diag.Item2 == j) || (diag.Item1 == j && diag.Item2 == i)) continue;
                if (SegmentsIntersect(a, b, points[diag.Item1], points[diag.Item2])) return true;
            }

            return false;
        }

        private float Distance(PointF a, PointF b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private bool IsReflex(int index)
        {
            int n = points.Count;
            int prev = (index - 1 + n) % n;
            int next = (index + 1) % n;
            return Cross(points[prev], points[index], points[next]) < 0;
        }

        private bool IsConvex(int i, int j, int k)
        {
            return Cross(points[i], points[j], points[k]) > 0;
        }

        private float Cross(PointF a, PointF b, PointF c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        private bool PointInsideTriangle(PointF p, PointF a, PointF b, PointF c)
        {
            float area = Math.Abs(Cross(a, b, c)) / 2f;
            float area1 = Math.Abs(Cross(p, b, c)) / 2f;
            float area2 = Math.Abs(Cross(a, p, c)) / 2f;
            float area3 = Math.Abs(Cross(a, b, p)) / 2f;
            return Math.Abs(area - (area1 + area2 + area3)) < 1e-1;
        }

        private bool SegmentsIntersect(PointF a, PointF b, PointF c, PointF d)
        {
            float d1 = Cross(c, d, a);
            float d2 = Cross(c, d, b);
            float d3 = Cross(a, b, c);
            float d4 = Cross(a, b, d);
            return d1 * d2 < 0 && d3 * d4 < 0;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen edgePen = new Pen(Color.Blue, 2);
            Brush vertexBrush = Brushes.DarkBlue;
            Font font = new Font("Arial", 10);
            Pen diagPen = new Pen(Color.Red, 1) { DashPattern = new float[] { 3, 3 } };
            Pen triPen = new Pen(Color.Green, 1);

            for (int i = 0; i < points.Count; i++)
            {
                g.FillEllipse(vertexBrush, points[i].X - 3, points[i].Y - 3, 6, 6);
                g.DrawString(i.ToString(), font, vertexBrush, points[i].X + 5, points[i].Y + 5);
                if (i > 0)
                    g.DrawLine(edgePen, points[i - 1], points[i]);
            }

            if (isPolygonClosed && points.Count > 2)
            {
                g.DrawLine(edgePen, points[points.Count - 1], points[0]);
            }

            foreach (var d in diagonals)
            {
                g.DrawLine(diagPen, points[d.Item1], points[d.Item2]);
            }

            foreach (var t in triangles)
            {
                g.DrawPolygon(triPen, new PointF[] { points[t.Item1], points[t.Item2], points[t.Item3] });
            }
        }
    }
}
