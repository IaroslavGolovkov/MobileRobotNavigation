using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

//Создание препятствий
namespace MobileRobotNavigation
{
    class Obstacles
    {
        public int numOfObstacles;  //Количество препятствий

        public List<float> ObstacleX;   //Координаты X центров препятствий
        public List<float> ObstacleY;   //Координаты Y центров препятствий
        public List<float> ObstacleSize;    //Диаметры препятствий

        private const int minSize = 30; //Минимальный размер препятствия
        private const int maxSize = 80; //Максимальный размер препятствия

        private int FieldWidth;     //Ширина поля
        private int FieldHeight;    //Высота поля
        public Obstacles(int num, PictureBox pb)  //Remove //Создание препятствий
        {
            FieldWidth = pb.Width;
            FieldHeight = pb.Height;
            numOfObstacles = num;
            Random RNG = new Random();
            ObstacleSize = new List<float>();
            ObstacleX = new List<float>();
            ObstacleY = new List<float>();
            for (int i = 0; i < num; i++)
            {
                ObstacleSize.Add(RNG.Next(minSize, maxSize));
                ObstacleX.Add(RNG.Next((int)ObstacleSize[i] / 2, 
                    FieldWidth - (int)ObstacleSize[i] / 2));
                ObstacleY.Add(RNG.Next((int)ObstacleSize[i] / 2, 
                    FieldHeight - (int)ObstacleSize[i] / 2));
            }
        }   
        public void CreateObstacles(int num)    //Done
        {
            numOfObstacles = num;
            Random RNG = new Random();
            ObstacleSize = new List<float>();
            ObstacleX = new List<float>();
            ObstacleY = new List<float>();
            for (int i = 0; i < num; i++)
            {
                ObstacleSize.Add(RNG.Next(minSize, maxSize));
                ObstacleX.Add(RNG.Next((int)ObstacleSize[i] / 2,
                    FieldWidth - (int)ObstacleSize[i] / 2));
                ObstacleY.Add(RNG.Next((int)ObstacleSize[i] / 2,
                    FieldHeight - (int)ObstacleSize[i] / 2));
            }
        }
        public Obstacles(PictureBox pb) //Done
        {
            FieldWidth = pb.Width;
            FieldHeight = pb.Height;
        }
    }
}
