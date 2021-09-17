using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golf_Game_Thing
{
    class Collision
    {
        public Vector2D Location { get; }
        public Vector2D BoundaryLocation { get; }
        public double Angle { get; }

        public Collision(Vector2D boundaryLoc, Vector2D loc, double angle)
        {
            BoundaryLocation = boundaryLoc;
            Location = loc;
            Angle = angle;
        }
    }
}
