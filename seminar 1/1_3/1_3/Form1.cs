namespace _1_3
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
            Pen p = new Pen(Color.Green, 2);
            Pen p2 = new Pen(Color.Red, 2);
            Random r = new Random();
            int n = r.Next(10, 100);
            Point[] points = new Point[n];
           
            Point q = new Point(r.Next(0, this.ClientSize.Width),r.Next(0, this.ClientSize.Height));
            g.DrawEllipse(p2, q.X - 5, q.Y - 5, 10, 10);
            float mindist = float.MaxValue;
            for (int i = 0; i < n; i++)
            {
                int x = r.Next(0, this.ClientSize.Width);
                int y = r.Next(0, this.ClientSize.Height);
                g.DrawEllipse(p, x - 2, y - 2, 4, 4);
                points[i] = new Point(x, y);
            }
            for(int i = 0; i < n-1; i++)
            {
                float dist = (float)Math.Sqrt(Math.Pow(points[i].X - q.X, 2) + Math.Pow(points[i].Y - q.Y, 2));
                if (dist < mindist)
                {
                    mindist = dist;
                }
            }
            g.DrawArc(p, q.X - mindist, q.Y - mindist, 2 * mindist, 2 * mindist, 0, 360);

        }
    }
}
