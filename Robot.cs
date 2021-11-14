using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileRobotNavigation
{
    
    class Robot
    {
        public int fieldWidth;             //Ширина поверхности
        public int fieldHeight;            //Высота поверхности

        private double AngleDelta;          //Приращение угла поворота
        private float PositionDelta;        //Приращение положения

        public float PositionX;             //Положение на оси X
        public float PositionY;             //Положение на оси Y
        public double Angle;                //Угол поворота в радианах

        public float Size = 50.0F;    //Диаметр робота

        private const float robotVectorLength = 100.0F; //Вектор для обеспечения точного поворота

        public bool moveForward = false;
        public bool moveBackward = false;
        public bool moveLeft = false;
        public bool moveRight = false;

        public Robot(PictureBox pb)
        {
            fieldWidth = pb.Width;
            fieldHeight = pb.Height;
            Set_Defaults();
        }
        public void Set_Default_Position()  //Установка положения по умолчанию
        {
            PositionX = (float)fieldWidth / 2.0F;
            PositionY = (float)fieldHeight - 40.0F;
            Angle = Math.PI / 2.0;
            moveForward = false;
            moveBackward = false;
            moveLeft = false;
            moveRight = false;
        }
        private void Set_Defaults() //Установка значений по умолчанию
        {
            AngleDelta = Math.PI / 16.0;
            PositionDelta = 5.0F;
            Set_Default_Position();
        }
        public void Set_Delta(double alpha, float dx)   //Установка приращений
        {
            AngleDelta = alpha;
            PositionDelta = dx;
        }
        public void Set_Position(int x, int y, double a)    //Установка положения
        {
            PositionX = x;
            PositionY = y;
            Angle = a;
        }
        public void Rotate(bool direction)    //Поворот робота
        {
            double dAngle = AngleDelta;
            if (!direction)
                dAngle = -dAngle;
            AngleFit(dAngle, -Math.PI, Math.PI);
        }
        public void Move(bool direction)  //Движение робота
        {
            float dx = (robotVectorLength * (float)Math.Cos(Angle)) / robotVectorLength * PositionDelta;
            float dy = (robotVectorLength * -(float)Math.Sin(Angle)) / robotVectorLength * PositionDelta;
            if(!direction)
            {
                dx = -dx;
                dy = -dy;
            }
            if (CheckBorders(dx, dy))
            {
                PositionX += dx;
                PositionY += dy;
            }
            else
                Set_Default_Position();
            
        }
        private bool CheckBorders(float dx, float dy)   //Проверка выхода за границы
        {
            if (PositionX + dx < fieldWidth - Size / 2 &&
                PositionX + dx > 0 + Size / 2 &&
                PositionY + dy < fieldHeight - Size / 2 &&
                PositionY + dy > 0 + Size / 2)
                return true;
            return false;
        }
        private void AngleFit(double delta, double lLim, double rLim)   //Ограничение угла поворота в заданном диапазоне
        {
            if (Angle + delta <= rLim &&
                Angle + delta > lLim)
                Angle += delta;
            else 
            { 
                if (Angle + delta > rLim)
                    Angle += delta + 2 * lLim;
                if (Angle + delta <= lLim)
                    Angle += delta + 2 * rLim;
            }
            
        }
        public bool CollisionFound(Obstacles ob)
        {
            for(int i = 0; i < ob.numOfObstacles; i++)
            {
                if (Math.Sqrt(Math.Pow(PositionX - ob.ObstacleX[i], 2) + Math.Pow(PositionY - ob.ObstacleY[i], 2)) <= (ob.ObstacleSize[i] + Size) / 2.0F)
                    return true;
            }
            return false;
        }        
    }
}
