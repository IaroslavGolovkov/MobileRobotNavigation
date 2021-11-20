using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRobotNavigation
{
    class FindPath
    {
        //Ширина и высота поля
        public double width;  
        public double height;
        //Координаты начала построения пути
        //public int startX;
        //public int startY;
        //Координаты точки назначения
        public double destX;
        public double destY;
        //Распределение потенциалов по полю
        public double[,] Potential;
        public double maxPotential = 0;
        //Параметры трехмерных Гауссовых функций
        private double sigma;
        private double sigmaK = 0.5;
        private double A = 0.5;

        public FindPath(Render rd, Robot r, Obstacles ob)
        {
            width = rd.Width;
            height = rd.Height;
            destX = r.DestinationX;
            destY = r.DestinationY;
            Potential = new double[rd.Width, rd.Height];
            double[] distToCorner = new double[4];
            distToCorner[0] = Math.Sqrt(Math.Pow(destX-0.0,2) + Math.Pow(destY -0.0, 2));
            distToCorner[1] = Math.Sqrt(Math.Pow(destX - width,2) + Math.Pow(destY -0.0, 2));
            distToCorner[2] = Math.Sqrt(Math.Pow(destX -0.0,2) + Math.Pow(destY-height, 2));
            distToCorner[3] = Math.Sqrt(Math.Pow(destX - width,2) + Math.Pow(destY - height, 2));
            double gradR = distToCorner.Max();
            //Построение градиента
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    Potential[x, y] = Math.Sqrt(Math.Pow(x - destX,2) + Math.Pow(y - destY, 2)) / gradR;
                    for(int i = 0; i < ob.numOfObstacles; i++)
                    {
                        sigma = sigmaK * ob.ObstacleSize[i];
                        Potential[x, y] = Potential[x, y] + A * Math.Exp(-(Math.Pow(x - ob.ObstacleX[i], 2) / (2*Math.Pow(sigma, 2)) + Math.Pow(y - ob.ObstacleY[i], 2) / (2 * Math.Pow(sigma, 2))));
                    }
                    if (Potential[x, y] > maxPotential)
                        maxPotential = Potential[x, y];
                }
            }
        }        
        public void CalculateAngle(Robot r)
        {
            int x = (int)r.PositionX;
            int y = (int)r.PositionY;
            double dx = Potential[x, y] - Potential[x - 1, y];
            double dy = Potential[x, y] - Potential[x, y - 1];
            r.Angle = -Math.Atan2(-dy, -dx);            
        }
    }
}
