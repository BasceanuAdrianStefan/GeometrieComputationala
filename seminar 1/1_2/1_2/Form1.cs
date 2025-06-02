namespace _1_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p1 = new Pen(Color.Green, 3), p2 = new Pen(Color.Blue, 3);
            Random r = new Random();
            int n1 = r.Next(100), n2 = r.Next(100);
            float raza1 = 1, raza2 = 2;
            PointF[] m1 = new PointF[n1];
            PointF[] m2 = new PointF[n2];
            for (int i = 0; i < n1; i++)
            {
                m1[i].X = r.Next(10, this.ClientSize.Width - 10);
                m1[i].Y = r.Next(10, this.ClientSize.Height - 10);
                g.DrawEllipse(p1, m1[i].X - raza1, m1[i].Y - raza1, raza1 * 2, raza1 * 2);
            }
            for (int i = 0; i < n2; i++)
            {
                m2[i].X = r.Next(10, this.ClientSize.Width - 10);
                m2[i].Y = r.Next(10, this.ClientSize.Height - 10);
                g.DrawEllipse(p2, m2[i].X - raza2, m2[i].Y - raza2, raza2 * 2, raza2 * 2);
            }
            float dist, x = 0, y = 0;
            for (int i = 0; i < n1; i++)
            {
                float dist_min = float.MaxValue;
                for (int j = 0; j < n2; j++)
                {
                    dist = (float)Math.Sqrt(Math.Pow(m1[i].X - m2[j].X, 2) + Math.Pow(m1[i].Y -
                   m2[j].Y, 2));
                    if (dist_min > dist)
                    {
                        dist_min = dist;
                        x = m2[j].X;
                        y = m2[j].Y;
                    }
                }
                Pen p = new Pen(Color.Black, 1);
                g.DrawLine(p, m1[i].X, m1[i].Y, x, y);
            }
        }
    }
}
