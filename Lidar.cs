using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MobileRobotNavigation
{
    struct Walls
    {
        public float w1x;
        public float w1y;
        public float w2x;
        public float w2y;
        public float w3x;
        public float w3y;
        public float w4x;
        public float w4y;
        public float padding;
    }
    class Lidar
    {
        public List<float> Data;
        public List<float> DataX;
        public List<float> DataY;

        private double OperatingAngle;
        public int Resolution;
        private int DefaultRange = 500;
        private int MinRange = 0;

        public float PositionX;
        public float PositionY;
        private double Angle;

        private int fieldWidth;
        private int fieldHeight;
        private Walls walls;
        public Lidar(Robot r)
        {
            PositionX = r.PositionX;
            PositionY = r.PositionY;
            Angle = r.Angle;
            Data = new List<float>();
            DataX = new List<float>();
            DataY = new List<float>();
            fieldWidth = r.fieldWidth;
            fieldHeight = r.fieldHeight;
            //CalculateWallsCoordinates(walls);
            DefaultRange = (int)(Math.Sqrt(Math.Pow(fieldWidth - r.Size / 2, 2) + (Math.Pow(fieldHeight - r.Size / 2, 2))));
            MinRange = (int)r.Size / 2 + 1;
            Defaults();
        }
        
        private void Defaults()
        {
            OperatingAngle = 2.0 * Math.PI;
            Resolution = 70;
        }
        public void SetParam(double operatingAngle, int resolution)
        {
            OperatingAngle = operatingAngle;
            Resolution = resolution;
        }
        public void UpdatePosition(Robot r)
        {
            /*for(int i = 0; i < Resolution; i++)
            {
                DataX[i] += r.PositionX - PositionX;
                DataY[i] += r.PositionY - PositionY;
            }*/
            PositionX = r.PositionX;
            PositionY = r.PositionY;
            Angle = r.Angle;
        }

        public void FindDistance(Bitmap bmp)
        {
            Data.Clear();
            DataX.Clear();
            DataY.Clear();

            float dx = (float)-(OperatingAngle - OperatingAngle / Resolution) / 2.0F + (float)Angle;
            for (int i = 0; i < Resolution; i++)
            {
                Data.Add(DefaultRange);
                DataX.Add(PositionX + Data[i] * (float)Math.Cos(dx));
                DataY.Add(PositionY + Data[i] * (float)-Math.Sin(dx));
                dx += (float)OperatingAngle / (Resolution);
            }
            for (int i = 0; i < Resolution; i++)
            {
                for(int j = MinRange; j < DefaultRange; j++)
                {
                    float x = CalculateBezier((float)PositionX, DataX[i], ((float)j / (float)DefaultRange));
                    float y = CalculateBezier((float)PositionY, DataY[i], ((float)j / (float)DefaultRange));
                    if(x < 2 || y < 2 || x > fieldWidth-2 || y > fieldHeight-2)
                    {
                        Data[i] = (float)j;
                        DataX[i] = x;
                        DataY[i] = y;
                        break;
                    }
                    if(bmp.GetPixel((int)x,(int)y).R == 0)
                    {
                        Data[i] = (float)j;
                        DataX[i] = x;
                        DataY[i] = y;
                        break;
                    }
                }
            }


        }
        private float CalculateBezier(float P0, float P1, float t)
        {
            return ((1 - t) * P0 + t * P1);
        }
        /*public void FindDistance(Obstacles ob)
        {
            List<float> RelativeObsX = new List<float>(ob.ObstacleX);   //Координаты препятствий в системе робота
            List<float> RelativeObsY = new List<float>(ob.ObstacleY);   //без учета поворота

            for (int i = 0; i < ob.numOfObstacles; i++)
            {
                RelativeObsX[i] -= PositionX;
                RelativeObsY[i] -= PositionY;
            }

            Data.Clear();
            DataX.Clear();
            DataY.Clear();
            float dx = (float)-(OperatingAngle - OperatingAngle / Resolution) / 2.0F + (float)Angle;
            for (int i = 0; i < Resolution; i++)
            {
                Data.Add(DefaultRange);
                DataX.Add(PositionX + Data[i] * (float)Math.Cos(dx));
                DataY.Add(PositionY + Data[i] * (float)-Math.Sin(dx));
                dx += (float)OperatingAngle / (Resolution);
            }
            //Поиск стен
            
            for(int i = 0; i < Resolution; i++)
            {
                IntersectionPoint(walls.w1x, walls.w1y, 
                                  walls.w2x, walls.w2y,
                                  PositionX, PositionY, 
                                  DataX[i], DataY[i]);
                IntersectionPoint(walls.w2x, walls.w2y, 
                                  walls.w3x, walls.w3y,
                                  PositionX, PositionY, 
                                  DataX[i], DataY[i]);
                IntersectionPoint(walls.w3x, walls.w3y, 
                                  walls.w4x, walls.w4y,
                                  PositionX, PositionY, 
                                  DataX[i], DataY[i]);
                IntersectionPoint(walls.w4x, walls.w4y, 
                                  walls.w1x, walls.w1y,
                                  PositionX, PositionY, 
                                  DataX[i], DataY[i]);
            }
            
            /*dx = (float)-(OperatingAngle - OperatingAngle / Resolution) / 2.0F + (float)Angle;
            for (int i = 0; i < Resolution; i++)
            {
                int padding = 10;
                var IntersectionPoints = new List<Point> {
                (IntersectionPoint(new Point(0 + padding, 0 + padding),
                                   new Point(0 + padding, fieldHeight - padding),
                                   new Point((int)PositionX, (int)PositionY),
                                   new Point((int)DataX[i], (int)DataY[i]))),
                (IntersectionPoint(new Point(0 + padding, fieldHeight - padding),
                                   new Point(fieldWidth - padding, fieldHeight - padding),
                                   new Point((int)PositionX, (int)PositionY),
                                   new Point((int)DataX[i], (int)DataY[i]))),
                (IntersectionPoint(new Point(fieldWidth - padding, fieldHeight - padding),
                                   new Point(fieldWidth - padding, 0 + padding),
                                   new Point((int)PositionX, (int)PositionY),
                                   new Point((int)DataX[i], (int)DataY[i]))),
                (IntersectionPoint(new Point(fieldWidth - padding, 0 + padding),
                                   new Point(0 + padding, 0 + padding),
                                   new Point((int)PositionX, (int)PositionY),
                                   new Point((int)DataX[i], (int)DataY[i])))
                };
                var DistanceToIntersection = new List<float>();
                float min = float.MaxValue;
                int minID = 0;
                for(int j = 0; j < IntersectionPoints.Count(); j++)
                {
                    DistanceToIntersection.Add((float)Math.Sqrt(Math.Pow(PositionX - IntersectionPoints[j].X, 2) + Math.Pow(PositionY - IntersectionPoints[j].Y, 2)));
                    if (DistanceToIntersection[j] < min)
                    {
                        min = DistanceToIntersection[j];
                        minID = j;
                    }
                }
                DataX[i] = IntersectionPoints[minID].X;
                DataY[i] = IntersectionPoints[minID].Y;
                Data[i] = (float)Math.Sqrt(Math.Pow(DataX[i], 2) + Math.Pow(DataY[i], 2));
                dx += (float)OperatingAngle / (Resolution);
                
            }
        }*/        
        private Point IntersectionPoint(Point A, Point B, Point C, Point D) //AB - стена, CD - луч
        {
            float denominator = (D.X - C.X) * (B.Y - A.Y) - (B.X - A.X) * (D.Y - C.Y);
            if (denominator == 0)
                return (D);
            float r = ((B.X - A.X) * (C.Y - A.Y) - (C.X - A.X) * (B.Y - A.Y)) / denominator;            
            if (r < 0)
                return (D);
            float s = ((A.X - C.X) * (D.Y - C.Y) - (D.Y - C.Y) * (A.Y - C.Y)) / denominator;
            if (s < 0 || s > 1)
                return(D);

            return new Point((int)s * (B.X - A.X) + A.X, (int)s * (B.Y - A.Y) + A.Y);
        }
        private Tuple<float, float> IntersectionPoint(float x1, float y1,   //Стена
                                                    float x2, float y2,     //Стена
                                                    float x3, float y3,     //Луч
                                                    float x4, float y4)     //Луч
        {            
            float denum = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (denum == 0) //Если линии параллельны или совпадают
                return null;
            float numX = (x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4);
            float numY = (x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4);
            float Px = numX / denum;
            float Py = numY / denum;
            float tx = (Px - x1) / (x2 - x1);
            float ty = (Py - y1) / (y2 - y1);
            if (tx > 1 || tx < 0 || ty > 1 || ty < 0)   //Если точка пересечения находится вне отрезков
                return null;
            return new Tuple<float, float>(Px, Py);
        }
        private void CalculateWallsCoordinates(Walls w)
        {
            w.padding = 10;

            w.w1x = 0 + w.padding;
            w.w1y = 0 + w.padding;

            w.w2x = 0 + w.padding;
            w.w2y = fieldHeight - w.padding;

            w.w3x = fieldWidth - w.padding;
            w.w3y = fieldHeight - w.padding;

            w.w4x = fieldWidth - w.padding;
            w.w4y = 0 + w.padding;
        }
    }
}
