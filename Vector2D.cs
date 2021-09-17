using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golf_Game_Thing
{
    class Vector2D
    {
        public int x;
        public int y;

        public Vector2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2D RotateVector(double angle, Vector2D vector)
        {
            angle = Math.PI * angle / 180;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            double rotatedX = vector.x * cos + vector.y * sin;
            double rotatedY = -vector.x * sin + vector.y * cos;
            return new Vector2D((int)rotatedX, (int)rotatedY);
        }

        public static Vector2D RotateVector(double angle, int x, int y)
        {
            angle = Math.PI * angle / 180;
            double sin = Math.Sin(angle);
            double cos = Math.Cos(angle);
            double rotatedX = x * cos + y * sin;
            double rotatedY = -x * sin + y * cos;
            return new Vector2D((int)rotatedX, (int)rotatedY);
        }
    }
}
