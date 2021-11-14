using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRobotNavigation
{
    class FindPath
    {
        //Ширина и высота поля
        private int width;  
        private int height;
        //Координаты начала построения пути
        public int startX;
        public int startY;
        //Координаты точки назначения
        public int destX;
        public int destY;
        //Препятствия
        private Obstacles obs;
        //Распределение потенциалов по полю
        double[,] Potential;
        //Точки маршрута
        
        //Значения градиента для начальной и конечной точки, а также для препятствий
        const double gradStart = 1;
        const double gradDestination = -1;
        const double gradObstacle = 1;

        public FindPath(Render rd, Robot r, Obstacles ob)
        {
            width = rd.Width;
            height = rd.Height;
            startX = (int)r.PositionX;
            startY = (int)r.PositionY;
            destX = (int)rd.DestinationX;
            destY = (int)rd.DestinationY;
            Potential = new double[width, height];
            double A = destX - startX;
            double B = destY - startY;
            double C1 = A * startX + B * startY;
            double C2 = A * destX + B * destY;
            //Построение градиента
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    double C = A * x + B * y;
                    
                    Potential[x, y] = (gradStart * (C2 - C) + gradDestination * (C - C1))/(C2 - C1);
                    for(int i = 0; i < ob.numOfObstacles; i++)
                    {
                        if (Math.Pow(x - ob.ObstacleX[x], 2) + Math.Pow(y - ob.ObstacleY[y], 2) < ob.ObstacleSize[i] + r.Size / 2)
                            Potential[x, y] = gradObstacle;
                    }
                }
            }
        }
        public void CalculatePath()
        {

        }
    }
}
