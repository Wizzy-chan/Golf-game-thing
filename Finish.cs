using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golf_Game_Thing
{
    class Finish : Rectangle
    {

        public Finish(IntPtr renderer, int x, int y, int width, int height, int angle, IntPtr loadingSurface) : base(renderer, x, y, width, height, angle, loadingSurface, "finish.bmp")
        {

        }
    }
}
