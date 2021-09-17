using System;
using SDL2;
using static SDL2.SDL;

namespace Golf_Game_Thing
{
    class Rectangle : GameObject
    {
        protected int Width;
        protected int Height;
        SDL_Rect SrcRect;

        public Rectangle(IntPtr renderer, int x, int y, int w, int h, int angle, IntPtr loadingSurface, string sprite="Ground.bmp") : base(renderer, true, loadingSurface, sprite)
        {
            this.x = x;
            this.y = y;
            Width = w;
            Height = h;
            Angle = angle;
            SrcRect = new SDL_Rect() { x = 0, y = 0, w = w, h = h };
            RenderRect = new SDL_Rect() { x = x, y = y, w = w, h = h };
        }

        public override Collision CheckCollision(GameObject other) // Returns a Collision object for the GameObject `other`, if a collision has occured
        {
            int relativeX = other.x - this.x;
            int relativeY = other.y - this.y;
            int xVelocity = other.xVelocity;
            int yVelocity = other.yVelocity;
            if (Angle != 0)
            {
                Vector2D rotatedPosition = Vector2D.RotateVector(Angle, relativeX, relativeY);
                relativeX = rotatedPosition.x;
                relativeY = rotatedPosition.y;
                Vector2D rotatedVelocity = Vector2D.RotateVector(Angle, xVelocity, yVelocity);
                xVelocity = rotatedVelocity.x;
                yVelocity = rotatedVelocity.y;
            }
            if (relativeX > 0 && relativeX < Width && relativeY > 0 && relativeY < Height)
            {
                int newX, newY, boundaryX, boundaryY;
                double xDistance = 0;
                double yDistance = 0;
                double collisionAngle;
                // Calculations to work out which side of the rectangle was hit
                if (Math.Sign(xVelocity) == 1) { xDistance = (double)relativeX / xVelocity; }
                else if (Math.Sign(xVelocity) == -1) { xDistance = (double)(Width - relativeX) / -xVelocity; }
                if (Math.Sign(yVelocity) == 1) { yDistance = (double)relativeY / yVelocity; }
                else if (Math.Sign(yVelocity) == -1) { yDistance = (double)(Height - relativeY) / -yVelocity; }

                if ((xDistance <= yDistance || yDistance == 0) && xDistance != 0)
                {
                    if (Math.Sign(xVelocity) == 1) 
                    {
                        newX = -relativeX;
                        newY = relativeY;
                        boundaryX = 0;
                        boundaryY = relativeY;
                        collisionAngle = Angle + 90;
                    }
                    else
                    {
                        newX = 2 * Width - relativeX;
                        newY = relativeY;
                        boundaryX = Width;
                        boundaryY = relativeY;
                        collisionAngle = Angle - 90;
                    }
                }
                else
                {
                    if (Math.Sign(yVelocity) == 1)
                    {
                        newX = relativeX;
                        newY = -relativeY;
                        boundaryX = relativeX;
                        boundaryY = 0;
                        collisionAngle = Angle;
                    }
                    else
                    {
                        newX = relativeX;
                        newY = 2 * Height - relativeY;
                        boundaryX = relativeX;
                        boundaryY = Height;
                        collisionAngle = Angle + 180;
                    }
                }
                if (Angle != 0)
                {
                    Vector2D unrotatedNewPosition = Vector2D.RotateVector(-Angle, newX, newY);
                    newX = unrotatedNewPosition.x;
                    newY = unrotatedNewPosition.y;
                    Vector2D unrotatedBoundaryPosition = Vector2D.RotateVector(-Angle, boundaryX, boundaryY);
                    boundaryX = unrotatedBoundaryPosition.x;
                    boundaryY = unrotatedBoundaryPosition.y;
                }
                newX += this.x;
                newY += this.y;
                boundaryX += this.x;
                boundaryY += this.y;
                return new Collision(new Vector2D(boundaryX, boundaryY), new Vector2D(newX, newY), collisionAngle);
            }
            return null;
        }

        public override void Render()
        {
            SDL_Point topLeft = new SDL_Point() { x = 0, y = 0 };
            SDL_RenderCopyEx(Renderer, Texture, ref SrcRect, ref RenderRect, Angle, ref topLeft, SDL_RendererFlip.SDL_FLIP_NONE);
        }
    }
}
