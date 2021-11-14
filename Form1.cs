using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileRobotNavigation
{
    public partial class Form1 : Form
    {        
        Render field;
        Robot robot;
        Obstacles fieldObstacles;
        Lidar lidar;
        Plot plot;
        FindPath fp;

        bool ShowLidar = true;
        bool ShowPlot = true;
        bool DestinationSet = false;
        bool PathfindingActive = false;

        public Form1()
        {
            InitializeComponent();
            fieldObstacles = new Obstacles(pictureBox1);
            field = new Render(pictureBox1);
            robot = new Robot(pictureBox1);
            lidar = new Lidar(robot);
            plot = new Plot(pictureBox2);
            pathfind_button.Enabled = false;
            PlotButton.Enabled = false;
            LidarButton.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            LidarButton.Enabled = true;
            PlotButton.Enabled = true;
            this.KeyPreview = true;
            int num = int.Parse(textBox1.Text);
            fieldObstacles.CreateObstacles(num);

            field.RenderObstacles(fieldObstacles);
            field.ShowObstacles(pictureBox1);
            robot.Set_Default_Position();

            lidar.UpdatePosition(robot);
            lidar.FindDistance(field.obstaclesBmp);
            
            field.Clear();            
            if (ShowLidar)
                field.Refresh(pictureBox1, robot, lidar);
            else
                field.Refresh(pictureBox1, robot);
            if (DestinationSet)
                field.RenderDestinationPoint(robot);
            if (ShowPlot)
                plot.SetData(lidar.Data);
            else
                plot.Clear();
            plot.Show(pictureBox2);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    robot.moveForward = !robot.moveForward;
                    if (robot.moveBackward)
                        robot.moveBackward = false;
                    //robot.Move(true);
                    break;
                case Keys.A:
                    robot.moveLeft = !robot.moveLeft;
                    if (robot.moveRight)
                        robot.moveRight = false;
                    //robot.Rotate(true);
                    break;
                case Keys.S:
                    robot.moveBackward = !robot.moveBackward;
                    if (robot.moveForward)
                        robot.moveForward = false;
                    //robot.Move(false);
                    break;
                case Keys.D:
                    robot.moveRight = !robot.moveRight;
                    if (robot.moveLeft)
                        robot.moveLeft = false;
                    //robot.Rotate(false);
                    break;
                //case Keys.Escape:
                //    Application.Exit();
                //    break;
                default:
                    break;
            }            
        }

        private void PlotButton_Click(object sender, EventArgs e)
        {
            ShowPlot = !ShowPlot;
            if (ShowPlot)
                PlotButton.Text = "Скрыть график";
            else
                PlotButton.Text = "Показать график";
        }

        private void LidarButton_Click(object sender, EventArgs e)
        {
            ShowLidar = !ShowLidar;
            if (ShowLidar)
                LidarButton.Text = "Скрыть лидар";
            else
                LidarButton.Text = "Показать лидар";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (!PathfindingActive)
            {
                DestinationSet = true;  //Точка будет отрисовываться
                var mouseEventArgs = e as MouseEventArgs;
                //field.SetDestinationPoint((float)mouseEventArgs.X, (float)mouseEventArgs.Y);
                robot.SetDestinationPoint((float)mouseEventArgs.X, (float)mouseEventArgs.Y);
                pathfind_button.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(robot.moveForward)
                robot.Move(true);           
            if(robot.moveLeft)
                robot.Rotate(true);
            if(robot.moveBackward)
                robot.Move(false);
            if(robot.moveRight)
                robot.Rotate(false);

            if (robot.CollisionFound(fieldObstacles))
            {
                robot.Set_Default_Position();
            }

            if (robot.TargetReached())
            {
                robot.StopRobot();
                PathfindingActive = false;
                pathfind_button.Text = "Двигаться к точке";
            }

            lidar.UpdatePosition(robot);
            lidar.FindDistance(field.obstaclesBmp);
            field.Clear();
            if (ShowLidar)
                field.Refresh(pictureBox1, robot, lidar);
            else
                field.Refresh(pictureBox1, robot);
            if (DestinationSet)
                field.RenderDestinationPoint(robot);
            if (ShowPlot)
                plot.SetData(lidar.Data);
            else
                plot.Clear();
            plot.Show(pictureBox2);
        }

        private void pathfind_button_Click(object sender, EventArgs e)
        {
            PathfindingActive = !PathfindingActive;
            if (PathfindingActive)
            {
                pathfind_button.Text = "Остановить";
                robot.StopRobot();
                this.KeyPreview = false;
                button1.Enabled = false;
                //fp = new FindPath(field, robot, fieldObstacles);
                //Двигаться к точке
                //fp.moveToDestination(robot);
                robot.MoveToPoint();
            }
            else
            {
                pathfind_button.Text = "Двигаться к точке";
                robot.StopRobot();
                this.KeyPreview = true;
                button1.Enabled = true;
            }
        }
    }
}
