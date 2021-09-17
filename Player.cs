using SDL2;
using System;
using static SDL2.SDL;

namespace Golf_Game_Thing
{
    class Player : GameObject
    {
        public double Drag;
        public int ClickX;
        public int ClickY;
        public bool Aiming;
        public int MaxVelocity;

        public Player(IntPtr renderer, string sprite, IntPtr loadingSurface) : base(renderer, false, loadingSurface, sprite)
        {
            x = 800;
            y = 450;
            Drag = 0.01;
            Aiming = false;
            MaxVelocity = 30;
        }

        public override void Update()
        {
            bool levelEnd = false;
            x += xVelocity;
            y += yVelocity;
            if (yVelocity < 25) { yVelocity += 1; }
            xVelocity -= (int)(xVelocity * xVelocity * Drag) * Math.Sign(xVelocity);
            foreach (GameObject Object in App.Objects)
            {
                if (Object == this) { continue; }
                Collision collisionResult = Object.CheckCollision(this);
                if (collisionResult is not null)
                {
                    if (Object is Finish)
                    {
                        levelEnd = true;
                        break;
                    }
                    Vector2D velocityVector = Vector2D.RotateVector(collisionResult.Angle, xVelocity, yVelocity);
                    velocityVector.y = (int)(velocityVector.y * -0.5);
                    velocityVector.x = (int)(velocityVector.x * 0.8);
                    if (Math.Abs(velocityVector.y) <= 3 / (1+Angle%180/180))
                    {
                        velocityVector.y = 0;
                        this.x = collisionResult.BoundaryLocation.x;
                        this.y = collisionResult.BoundaryLocation.y;
                    }
                    else
                    {
                        this.x = collisionResult.Location.x;
                        this.y = collisionResult.Location.y;
                    }
                    velocityVector = Vector2D.RotateVector(-collisionResult.Angle, velocityVector);
                    xVelocity = velocityVector.x;
                    yVelocity = velocityVector.y;
                    break; // Only handle one collision per frame
                }
            }
            if (levelEnd)
            {
                App.Level += 1;
                App.LoadLevel();
                App.RestartLevel();
            }
        }
        
        public void MouseDown(SDL_MouseButtonEvent e)
        {
            if (xVelocity == 0 && Math.Abs(yVelocity) <= 1) // Stationary, allowance for yVelocity due to gravity
            {
                ClickX = e.x;
                ClickY = e.y;
                Aiming = true;
            }
        }

        public void MouseUp(SDL_MouseButtonEvent e)
        {
            if (Aiming)
            {
                int newXVelocity = (e.x - ClickX) / 10;
                int newYVelocity = (e.y - ClickY) / 10;

                int totalVelocity = (int)(Math.Sqrt(newXVelocity * newXVelocity + newYVelocity * newYVelocity));
                if (totalVelocity > MaxVelocity)
                {
                    newXVelocity = newXVelocity * MaxVelocity / totalVelocity;
                    newYVelocity = newYVelocity * MaxVelocity / totalVelocity;
                }

                xVelocity = newXVelocity;
                yVelocity = newYVelocity;
                
                Aiming = false;
            }
        }

        public override void Render()
        {
            if (Aiming)
            {
                int mouseX, mouseY, dx, dy;
                if (SDL_GetMouseState(out mouseX, out mouseY) < 0) { Console.WriteLine(SDL_GetError()); }
                dx = (mouseX - ClickX) / 2;
                dy = (mouseY - ClickY) / 2;
                int lineLength = (int)Math.Sqrt(dx * dx + dy * dy);
                if (lineLength > 150)
                {
                    dx = dx * 150 / lineLength;
                    dy = dy * 150 / lineLength;
                }
                if (SDL_SetRenderDrawColor(Renderer, 0x00, 0x00, 0x00, 0x80) < 0) { Console.WriteLine(SDL_GetError()); }
                if (SDL_RenderDrawLine(Renderer, x, y, dx + x, dy + y) < 0) { Console.WriteLine(SDL_GetError()); }
            }
            base.Render();
        }
    }
}