using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileRobotNavigation
{
    class FindPath
    {
        private int width;
        private int height;
        public FindPath(Render r, Obstacles ob)
        {
            width = r.Width;
            height = r.Height;
            var who = r.DestinationX;
            var me = r.DestinationY;
        }
    }
}
