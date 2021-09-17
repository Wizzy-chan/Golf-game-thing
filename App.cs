using SDL2;
using System;
using System.Collections.Generic;
using static SDL2.SDL;

namespace Golf_Game_Thing
{
    static class App
    {
        public static bool Running;
        static IntPtr Renderer;
        static IntPtr Window;
        static IntPtr LoadingSurface;
        public static List<GameObject> Objects;
        public static Player PlayerObject;
        public static int SpawnX;
        public static int SpawnY;
        public static int Level;
        public static bool Menu;
        public static Button RestartButton;

        public static void Initialise()
        {
            //SDL Initialisation
            if (SDL_Init(SDL_INIT_EVERYTHING) < 0) { Console.WriteLine(SDL_GetError()); }
            Window = SDL_CreateWindow("Game", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 1600, 900, SDL_WindowFlags.SDL_WINDOW_SHOWN);
            Renderer = SDL_CreateRenderer(Window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);
            LoadingSurface = IntPtr.Zero;

            //Game Initialisation
            Objects = new List<GameObject>();
            PlayerObject = new Player(Renderer, "ball.bmp", LoadingSurface);
            Running = true;
            Menu = false;
            Level = 1;
            LoadLevel();

            RestartButton = new Button(Renderer, LoadingSurface, 50, 50, 100, 100, RestartLevel, "Restart.bmp");

            return;
        }

        public static void OnEvent(SDL_Event e)
        {
            switch (e.type)
            {
                case SDL_EventType.SDL_QUIT:
                    Running = false;
                    break;
                case SDL_EventType.SDL_KEYDOWN:
                    OnKey(e.key.keysym.sym);
                    break;
                case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    PlayerObject.MouseDown(e.button);
                    RestartButton.CheckClick(e.button);
                    break;
                case SDL_EventType.SDL_MOUSEBUTTONUP:
                    PlayerObject.MouseUp(e.button);
                    break;
            }
        }

        public static void OnKey(SDL_Keycode key)
        {
            switch (key)
            {
                case SDL_Keycode.SDLK_r:
                    RestartLevel();
                    break;
            }
        }

        public static void MainLoop()
        {
            while (Running)
            {
                //Handle SDL input events
                while (SDL_PollEvent(out SDL_Event e) != 0)
                {
                    OnEvent(e);
                }

                //Update game state
                Update();

                //Render game state
                Render();

                SDL_Delay(16);
            }

            OnClose();
        }

        public static void Update()
        {
            //Update each GameObject
            foreach (GameObject Object in Objects)
            {
                Object.Update();
            }
            PlayerObject.Update();
        }

        public static void Render()
        {
            //Background
            SDL_SetRenderDrawColor(Renderer, 0xad, 0xd8, 0xe6, 0xff);
            SDL_RenderFillRect(Renderer, IntPtr.Zero);

            if (!Menu)
            {
                //Render each GameObject
                foreach (GameObject Object in Objects)
                {
                    Object.Render();
                }
                PlayerObject.Render();
            }
            RestartButton.Render();

            //Present the buffer to the screen
            SDL_RenderPresent(Renderer);
        }

        public static void OnClose()
        {
            SDL_DestroyWindow(Window);
            SDL_Quit();
        }

        public static void RestartLevel()
        {
            PlayerObject.x = SpawnX;
            PlayerObject.y = SpawnY;
            PlayerObject.xVelocity = 0;
            PlayerObject.yVelocity = 0;
        }

        public static void LoadLevel()
        {
            //This function sucks but I don't care

            //Remove all current objects (thank you garbage collection)
            Objects.Clear();

            // Level Border
            Objects.Add(new Rectangle(Renderer, 0, 0, 50, 900, 0, LoadingSurface));
            Objects.Add(new Rectangle(Renderer, 1550, 0, 50, 900, 0, LoadingSurface));
            Objects.Add(new Rectangle(Renderer, 0, 0, 1600, 50, 0, LoadingSurface));
            Objects.Add(new Rectangle(Renderer, 0, 850, 1600, 50, 0, LoadingSurface));
            SpawnX = 100;
            SpawnY = 850;

            //Load new level based on `Level`
            switch (Level)
            {
                case 1:

                    Objects.Add(new Rectangle(Renderer, 1250, 900, 400, 150, -25, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 700, 750, 400, 300, 25, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 700, 750, 400, 300, 65, LoadingSurface));
                    Objects.Add(new Finish(Renderer, 1100, 848, 40, 22, 0, LoadingSurface));
                    break;

                case 2:

                    Objects.Add(new Rectangle(Renderer, 425, 750, 1225, 150, 0, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 800, 650, 850, 250, 0, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 1225, 550, 475, 350, 0, LoadingSurface));
                    Objects.Add(new Finish(Renderer, 1392, 548, 40, 22, 0, LoadingSurface));
                    break;

                case 3:

                    SpawnY = 500;

                    Objects.Add(new Rectangle(Renderer, 0, 500, 400, 400, 0, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 800, 750, 800, 250, 0, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 1200, 500, 400, 400, 0, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 800, 750, 100, 200, 35, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 1200, 500, 300, 400, 35, LoadingSurface));
                    Objects.Add(new Finish(Renderer, 1355, 498, 40, 22, 0, LoadingSurface));
                    break;

                case 4:

                    Objects.Add(new Rectangle(Renderer, 0, 0, 200, 650, 0, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 350, 650, 300, 250, 0, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 800, 0, 300, 650, 0, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 1250, 650, 350, 250, 0, LoadingSurface));
                    Objects.Add(new Finish(Renderer, 1380, 648, 40, 22, 0, LoadingSurface));
                    break;

                case 5:

                    SpawnY = 550;

                    Objects.Add(new Rectangle(Renderer, 50, 550, 550, 300, 37, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 850, 600, 500, 500, 50, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 850, 600, 550, 300, 20, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 1300, 650, 550, 300, 70, LoadingSurface));
                    Objects.Add(new Rectangle(Renderer, 1550, 400, 500, 500, 50, LoadingSurface));
                    Objects.Add(new Finish(Renderer, 1317, 593, 40, 22, -40, LoadingSurface));
                    break;

                default:

                    Menu = true;

                    break;
            }

            // Reload Level
            RestartLevel();
        }
    }
}
