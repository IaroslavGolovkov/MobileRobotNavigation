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

        public FindPath(Render rd, Robot r, Obstacles ob)
        {
            width = rd.Width;
            height = rd.Height;
            startX = r.PositionX;
            startY = r.PositionY;
            destX = rd.DestinationX;
            destY = rd.DestinationY;



        }
    }
}
