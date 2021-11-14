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
        List<List<double>> Potential;
        //Точки маршрута
        List<List<int>> Path;

        public FindPath(Render rd, Robot r, Obstacles ob)
        {
            width = rd.Width;
            height = rd.Height;
            startX = (int)r.PositionX;
            startY = (int)r.PositionY;
            destX = (int)rd.DestinationX;
            destY = (int)rd.DestinationY;
            Potential = new List<List<double>>();
        }
        public void moveToDestination(Robot r)
        {
            r.MoveToPoint();
        }
    }
}
