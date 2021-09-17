using System;
using SDL2;
using static SDL2.SDL;

namespace Golf_Game_Thing
{
    class GameObject
    {
        public IntPtr Texture;
        public IntPtr Renderer;
        public SDL_Rect RenderRect;
        public int x;
        public int y;
        public int xVelocity;
        public int yVelocity;
        public double Angle;
        public bool Static;

        public GameObject(IntPtr renderer, bool @static, IntPtr loadingSurface, string sprite = null)
        {
            Renderer = renderer;
            Static = @static;
            Angle = 0;
            if (sprite is not null)
            {
                InitialiseTexture(sprite, loadingSurface);
            }
        }

        public void InitialiseTexture(string sprite, IntPtr loadingSurface)
        {
            loadingSurface = SDL_LoadBMP(sprite);
            if (loadingSurface == IntPtr.Zero) { Console.WriteLine(SDL_GetError()); }
            Texture = SDL_CreateTextureFromSurface(Renderer, loadingSurface);
            SDL_FreeSurface(loadingSurface);
            int width, height;
            SDL_QueryTexture(Texture, out _, out _, out width, out height);
            RenderRect = new SDL_Rect() { w = width, h = height };
        }

        public virtual void Render()
        {
            RenderRect.x = x-RenderRect.w/2;
            RenderRect.y = y-RenderRect.h/2;
            SDL_RenderCopy(Renderer, Texture, IntPtr.Zero, ref RenderRect);
        }

        public virtual void Update()
        {
            if (!Static) 
            {
                x += xVelocity;
                y += yVelocity;
            }
        }

        public virtual Collision CheckCollision(GameObject other)
        {
            return null; // Unimplemented if uncollidable
        }
    }
}