using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace var1
{
    public partial class Form1 : Form
    {
        List<PointF> polygonPoints = new List<PointF>();
        List<(PointF, PointF)> diagonals = new List<(PointF, PointF)>();
        List<(PointF, PointF, PointF)> triangles = new List<(PointF, PointF, PointF)>();

        public Form1()
        {
            InitializeComponent();

        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Adaugă punct la clic
            if (e.Button == MouseButtons.Left)
            {
                polygonPoints.Add(e.Location);
                this.Invalidate(); // redesenează
            }
        }
        //se sortează punctele poligonului după coordonata Y(în ordine descrescătoare), apoi după coordonata X(în ordine crescătoare).
        //Apoi, pentru fiecare punct din poligon:
        //Se determină vecinii(punctul anterior și următor).
        //Se verifică dacă punctul curent este un vârf reflexiv
        //(un vârf care formează o întoarcere spre stânga, adică o cotitură concavă). Dacă da, se adaugă diagonale către cel mai
        //apropiat vârf aflat deasupra sau dedesubt(în funcție de poziția vârfului curent).
        private void button1_Click(object sender, EventArgs e)
        {
            if (polygonPoints.Count < 3)
            {
                MessageBox.Show("Adaugă cel puțin 3 puncte.");
                return;
            }

            diagonals.Clear();

            var n = polygonPoints.Count;
            var sorted = polygonPoints
                .Select((pt, i) => new { Index = i, Point = pt })
                .OrderByDescending(p => p.Point.Y)
                .ThenBy(p => p.Point.X)
                .ToList();

            for (int i = 0; i < sorted.Count; i++)
            {
                int currIndex = sorted[i].Index;
                int prevIndex = (currIndex - 1 + n) % n;
                int nextIndex = (currIndex + 1) % n;

                PointF prev = polygonPoints[prevIndex];
                PointF curr = polygonPoints[currIndex];
                PointF next = polygonPoints[nextIndex];

                if (IsReflex(prev, curr, next))
                {
                    bool ambeleJos = prev.Y < curr.Y && next.Y < curr.Y;
                    bool ambeleSus = prev.Y > curr.Y && next.Y > curr.Y;

                    if (ambeleJos)
                    {
                        // Split vertex - caută cel mai apropiat vârf deasupra
                        var support = sorted
                            .Where(p => p.Point.Y > curr.Y)
                            .Select(p => p.Point)
                            .OrderBy(p => Distance(p, curr))
                            .FirstOrDefault();

                        diagonals.Add((curr, support));
                    }
                    else if (ambeleSus)
                    {
                        // Merge vertex - caută cel mai apropiat vârf dedesubt
                        var support = sorted
                            .Where(p => p.Point.Y < curr.Y)
                            .Select(p => p.Point)
                            .OrderBy(p => Distance(p, curr))
                            .FirstOrDefault();

                        diagonals.Add((curr, support));
                    }
                }
            }

            this.Invalidate(); // redesenează
        }
        private float Distance(PointF a, PointF b)
        {
            return (float)Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        private bool IsReflex(PointF prev, PointF curr, PointF next)
        {
            // Determină dacă vârful face întoarcere spre stânga
            float cross = (curr.X - prev.X) * (next.Y - curr.Y) - (curr.Y - prev.Y) * (next.X - curr.X);
            return cross < 0; // întoarcere la stânga => reflex
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            if (polygonPoints.Count > 1)
            {
                e.Graphics.DrawPolygon(Pens.Black, polygonPoints.ToArray());
            }

            foreach (var pt in polygonPoints)
            {
                e.Graphics.FillEllipse(Brushes.Black, pt.X - 3, pt.Y - 3, 6, 6);
            }

            foreach (var diag in diagonals)
            {
                e.Graphics.DrawLine(Pens.Blue, diag.Item1, diag.Item2);
            }
            foreach (var t in triangles)
            {
                e.Graphics.DrawPolygon(Pens.Red, new[] { t.Item1, t.Item2, t.Item3 });
            }
        }
       
        //Verifică dacă un triunghi este convex, folosind produsul încrucișat pentru a determina semnul unghiului.
        private bool IsConvex(PointF a, PointF b, PointF c)
        {
            float cross = (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
            return cross < 0;
        }
        //Verifică dacă două puncte se află pe aceeași "lanț" al poligonului(adică, pe aceeași secțiune).
        private bool OnSameChain(int idx, int idxStack)
        {
            int maxY = polygonPoints.IndexOf(polygonPoints.OrderByDescending(p => p.Y).First());
            int minY = polygonPoints.IndexOf(polygonPoints.OrderBy(p => p.Y).First());

            bool isLeft = IsLeftChain(idx, maxY, minY);
            bool isLeftStack = IsLeftChain(idxStack, maxY, minY);
            return isLeft == isLeftStack;
        }
        //Verifică dacă un punct este pe partea stângă a unei secțiuni de 
        //    poligon(lanț) definit de punctele top și bottom.
        private bool IsLeftChain(int idx, int top, int bottom)
        {
            if (top < bottom)
                return idx >= top && idx <= bottom;
            else
                return idx >= top || idx <= bottom;
        }

        private bool IsPointInsidePolygon(PointF pt)
        {
            int n = polygonPoints.Count;
            bool inside = false;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                PointF pi = polygonPoints[i];
                PointF pj = polygonPoints[j];
                if (((pi.Y > pt.Y) != (pj.Y > pt.Y)) &&
                    (pt.X < (pj.X - pi.X) * (pt.Y - pi.Y) / (pj.Y - pi.Y) + pi.X))
                {
                    inside = !inside;
                }
            }
            return inside;
        }
    }
}
