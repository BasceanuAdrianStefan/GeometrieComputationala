namespace _1_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p1 = new Pen(Color.Green, 3), p2 = new Pen(Color.Blue, 3);
            Random r = new Random();
            int n = r.Next(10, 20);
            int minx = int.MaxValue, maxx = int.MinValue;
            int miny = int.MaxValue, maxy = int.MinValue;
            Point[] points = new Point[n];
            for (int i = 0; i < n; i++)
            {
                int x = r.Next(0, this.ClientSize.Width);
                int y = r.Next(0, this.ClientSize.Height);
                points[i] = new Point(x, y);
                g.FillEllipse(Brushes.Red, x - 2, y - 2, 4, 4);
                if(x < minx) minx = x;
                if(x > maxx) maxx = x;
                if(y < miny) miny = y;
                if(y > maxy) maxy = y;
            }
            g.DrawRectangle(p1, minx, miny, maxx - minx, maxy - miny);
        }
    }
}
