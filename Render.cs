using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace MobileRobotNavigation
{
    class Render
    {
        public Bitmap bmp;  //Буфер для рисования
        public Bitmap obstaclesBmp;
        private Graphics obstaclesG;    //Изображение с препятствиями
        private Graphics g; //Экземпляр Graphics
        public int Width;  //Ширина поля
        public int Height; //Высота поля

        public float DestinationX;
        public float DestinationY;
        private const int DestinationPointSize = 21;
        //private PictureBox pb;

        public Render(PictureBox picturebox)    //Создание поверхности на основе PictureBox
        {            
            Width = picturebox.Width;
            Height = picturebox.Height;
            bmp = new Bitmap(Width, Height);
            g = Graphics.FromImage(bmp);

            obstaclesBmp = new Bitmap(Width, Height);
            obstaclesG = Graphics.FromImage(obstaclesBmp);

            DestinationX = 0;
            DestinationY = 0;

            Defaults();
        }
        public void RenderObstacles(Obstacles obs)  
        {
            obstaclesG.Clear(Color.White);
            Pen pen = new Pen(Color.Black, 3.0F);
            for (int i = 0; i < obs.numOfObstacles; i++)
            {
                obstaclesG.DrawEllipse(pen,
                    obs.ObstacleX[i] - obs.ObstacleSize[i] / 2,
                    obs.ObstacleY[i] - obs.ObstacleSize[i] / 2,
                    obs.ObstacleSize[i],
                    obs.ObstacleSize[i]);
            }
        }//Отрисовка препятствий на отдельном graphics
        public void RenderDestinationPoint(Robot r)
        {
            g.FillEllipse(Brushes.Yellow,
                r.DestinationX - DestinationPointSize / 2,
                r.DestinationY - DestinationPointSize / 2,
                DestinationPointSize,
                DestinationPointSize);
        }
        public void ShowObstacles(PictureBox pb)
        {
            pb.BackgroundImage = obstaclesBmp;
        }        
        public void RenderRobot(Robot r)
        {
            g.FillEllipse(Brushes.White,
                r.PositionX - r.Size / 2.0F,
                r.PositionY - r.Size / 2.0F,
                r.Size,
                r.Size);
            Pen pen = new Pen(Color.Black);
            g.DrawEllipse(pen,
                r.PositionX - r.Size / 2.0F,
                r.PositionY - r.Size / 2.0F,
                r.Size,
                r.Size);
            g.DrawLine(pen,
                r.PositionX,
                r.PositionY,
                r.PositionX + r.Size / 2.0F * (float)Math.Cos(r.Angle),
                r.PositionY + r.Size / 2.0F * (float)-Math.Sin(r.Angle));
        }
        public void RenderLidar(Lidar l)   //Отрисовка лучей лидара в буфере
        {
            for(int i = 0; i < l.Resolution; i++)
            {
                g.DrawLine(Pens.Red, l.PositionX, l.PositionY, l.DataX[i], l.DataY[i]);
            }
        }
        public void Clear() //Очистка поверхности
        {
            g.Clear(Color.Transparent);
        }
        private void Show(PictureBox pb) //Отображение буфера на PictureBox
        {            
            pb.Image = bmp;
        }
        public void Refresh(PictureBox pb, Robot r, Lidar l)
        {
            //Clear();            
            RenderRobot(r);
            RenderLidar(l);
            Show(pb);
        }
        public void Refresh(PictureBox pb, Robot r)
        {
            //Clear();
            RenderRobot(r);
            Show(pb);
        }
        private void Defaults() //Настройки по умолчанию
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            obstaclesG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
        public void RenderGradient(PictureBox pb, FindPath fp)
        {
            pb.Width = (int)fp.width;
            pb.Height = (int)fp.height;
            Bitmap grad = new Bitmap(pb.Width, pb.Height);
            for(int x = 0; x < fp.width; x++)
            {
                for(int y = 0; y < fp.height; y++)
                {
                    int C = (int)(fp.Potential[x, y] / fp.maxPotential * 255);
                    grad.SetPixel(x, y, Color.FromArgb(1, 1, C));
                }
            }            
            pb.Image = grad;
        }
    }
}
