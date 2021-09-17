using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;

namespace Golf_Game_Thing
{
    class Button : GameObject
    {
        Action Command;
        int Width;
        int Height;

        public Button(IntPtr renderer, IntPtr loadingSurface, int x, int y, int w, int h, Action command, string sprite) : base(renderer, true, loadingSurface, sprite)
        {
            Command = command;
            this.x = x;
            this.y = y;
            Width = w;
            Height = h;
        }

        public override void Render()
        {
            RenderRect.x = x;
            RenderRect.y = y;
            SDL_RenderCopy(Renderer, Texture, IntPtr.Zero, ref RenderRect);
        }

        public void CheckClick(SDL_MouseButtonEvent e)
        {
            if (x <= e.x && e.x <= x + Width && y <= e.y && e.y <= y + Height)
            {
                Command();
            }
        }
    }
}
