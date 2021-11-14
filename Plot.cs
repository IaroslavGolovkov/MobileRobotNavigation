using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MobileRobotNavigation
{
    class Plot
    {
        public Bitmap bmp;
        private Graphics g;
        private int Width;
        private int Height;
        public Plot(PictureBox pb)  //Создание графика на основе PictureBox
        {
            Width = pb.Width;
            Height = pb.Height;
            bmp = new Bitmap(Width, Height);
            g = Graphics.FromImage(bmp);
        }
        public void Clear() //Очистка графика
        {
            g.Clear(Color.White);
        }
        public void SetData(List<float> data)  //Отображение графика
        {
            Clear();
            Pen pen = new Pen(Color.Blue, 2.0F);
            float max = data.Max();
            float dx = (float)(Width - 20) / (float)(data.Count + 1);
            for (int i = 0; i < data.Count; i++)
            {
                g.DrawLine(pen, dx * (i + 1) + 10.0F, Height - 10, dx * (i + 1) + 10.0F, Height - ((Height - 10) / max * data[i]));
                g.DrawString(data[i].ToString(), new Font("Arial", 6), Brushes.Black, new Point((int)(dx * (i + 1) + 10), Height - 10));
            }
        }
        public void Show(PictureBox pb)
        {
            pb.Image = bmp;
        }
    }
}
