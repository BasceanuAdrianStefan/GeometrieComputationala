using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace seminar10
{
    //Partiționarea unui poligon simplu în poligoane monotone, prin identificarea vârfurilor de întoarcere și unirea
    //acestora prin diagonale cu vârfurile corespunzătoare.

    public partial class Form1 : Form
    {
        Graphics g;
        Pen p1;
        const int raza = 2;
        int n = 0;
        List<Point> p = new List<Point>();
        bool poligon_inchis = false;

        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();
            p1 = new Pen(Color.Green, 3);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            p.Add(PointToClient(new Point(MousePosition.X, MousePosition.Y)));
            if (!poligon_inchis)
            {
                g.DrawEllipse(p1, p[n].X, p[n].Y, raza, raza);
                if (n > 0)
                    g.DrawLine(p1, p[n - 1], p[n]);
                n++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (n < 3)
                return;
            if (!poligon_inchis)
            {
                if (n < 3)
                    return;
                g.DrawLine(p1, p[n - 1], p[0]);
                poligon_inchis = true;
            }

            p1 = new Pen(Color.Red);

            if (n >= 3)
                for (int i = 0; i < n; i++)
                    if (reflex(i))
                    {
                        if (i == 0)
                        {
                            if (p[n - 1].Y > p[i].Y && p[i + 1].Y > p[i].Y)
                                if (Primul_deasupra(i) != -1)
                                {
                                    int j = Primul_deasupra(i);
                                    g.DrawLine(p1, p[i], p[j]);
                                }
                            if (p[n - 1].Y < p[i].Y && p[i + 1].Y < p[i].Y)
                                if (Primul_dedesubt(i) != -1)
                                {
                                    int j = Primul_dedesubt(i);
                                    g.DrawLine(p1, p[i], p[j]);
                                }
                        }
                        else if (i == n - 1)
                        {
                            if (p[i - 1].Y > p[i].Y && p[0].Y > p[i].Y)
                                if (Primul_deasupra(i) != -1)
                                {
                                    int j = Primul_deasupra(i);
                                    g.DrawLine(p1, p[i], p[j]);
                                }
                            if (p[i - 1].Y < p[i].Y && p[0].Y < p[i].Y)
                                if (Primul_dedesubt(i) != -1)
                                {
                                    int j = Primul_dedesubt(i);
                                    g.DrawLine(p1, p[i], p[j]);
                                }
                        }
                        else
                        {
                            if (p[i - 1].Y > p[i].Y && p[i + 1].Y > p[i].Y)
                                if (Primul_deasupra(i) != -1)
                                {
                                    int j = Primul_deasupra(i);
                                    g.DrawLine(p1, p[i], p[j]);
                                }
                            if (p[i - 1].Y < p[i].Y && p[i + 1].Y < p[i].Y)
                                if (Primul_dedesubt(i) != -1)
                                {
                                    int j = Primul_dedesubt(i);
                                    g.DrawLine(p1, p[i], p[j]);
                                }
                        }
                    }
        }

        private int Primul_dedesubt(int i)
        {
            for (int k = p[i].Y + 1; k < Height; k++)
                for (int h = 0; h < n; h++)
                    if (p[h].Y == k && isdiagonala(i, h))
                        return h;
            return -1;
        }

        private int Primul_deasupra(int i)
        {
            for (int k = p[i].Y - 1; k >= 0; k--)
                for (int h = 0; h < n; h++)
                    if (p[h].Y == k && isdiagonala(i, h))
                        return h;
            return -1;
        }

        private bool isdiagonala(int i, int j)
        {
            int nr_diagonale = 0;
            Tuple<int, int>[] diagonale = new Tuple<int, int>[n - 3];
            bool intersectie = false;
            for (int k = 0; k < n - 1; k++)
                if (i != k && i != (k + 1) && j != k && j != (k + 1) && intersecteaza(p[i], p[j], p[k], p[k + 1]))
                {
                    intersectie = true;
                    break;
                }
            if (i != n - 1 && i != 0 && j != n - 1 && j != 0 && intersecteaza(p[i], p[j], p[n - 1], p[0]))
                intersectie = true;
            if (!intersectie)
            {
                if (in_interiorul_poligonului(i, j))
                {
                    diagonale[nr_diagonale] = new Tuple<int, int>(i, j);
                    nr_diagonale++;
                    return true;
                }
            }
            return false;
        }

        private bool intersecteaza(Point s1, Point s2, Point p1, Point p2)
        {
            if (Sarrus(p2, p1, s1) * Sarrus(p2, p1, s2) <= 0 && Sarrus(s2, s1, p1) * Sarrus(s2, s1, p2) <= 0)
                return true;
            return false;
        }

        private double Sarrus(Point p1, Point p2, Point p3)
        {
            return p1.X * p2.Y + p2.X * p3.Y + p3.X * p1.Y - p3.X * p2.Y - p2.X * p1.Y - p1.X * p3.Y;
        }

        private bool in_interiorul_poligonului(int pi, int pj)
        {
            int pi_ant = (pi > 0) ? pi - 1 : n - 1;
            int pi_urm = (pi < n - 1) ? pi + 1 : 0;
            if ((convex(pi) && intoarcere_spre_stanga(pi, pj, pi_urm) && intoarcere_spre_stanga(pi, pi_ant, pj)) ||
            (reflex(pi) && !(intoarcere_spre_dreapta(pi, pj, pi_urm) && intoarcere_spre_dreapta(pi, pi_ant, pj))))
                return true;
            return false;
        }

        private bool convex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_dreapta(p_ant, p, p_urm);
        }

        private bool reflex(int p)
        {
            int p_ant = (p > 0) ? p - 1 : n - 1;
            int p_urm = (p < n - 1) ? p + 1 : 0;
            return intoarcere_spre_stanga(p_ant, p, p_urm);
        }

        private bool intoarcere_spre_stanga(int p1, int p2, int p3)
        {
            if (Sarrus(p[p1], p[p2], p[p3]) < 0)
                return true;
            return false;
        }

        private bool intoarcere_spre_dreapta(int p1, int p2, int p3)
        {
            if (Sarrus(p[p1], p[p2], p[p3]) > 0)
                return true;
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
