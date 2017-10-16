using Microsoft.Xna.Framework.Input;
using System;

namespace MainGame.Control
{
    class MyKeys
    {
        private const int NUM_KEYS = 16;
        private static KeyboardState keyEvent = Keyboard.GetState();

        public static Boolean[] keyState = new Boolean[NUM_KEYS];
        private static Boolean[] prevKeyState = new Boolean[NUM_KEYS];

        public static int _UP = 0;
        public static int _LEFT = 1;
        public static int _DOWN = 2;
        public static int _RIGHT = 3;
        public static int _BUTTON1 = 4;
        public static int _BUTTON2 = 5;
        public static int _BUTTON3 = 6;
        public static int _BUTTON4 = 7;
        public static int _ENTER = 8;
        public static int _ESCAPE = 9;


        public static void Update()
        {
            if (keyEvent.IsKeyDown(Keys.Up)) keyState[_UP] = true; else keyState[_UP] = false;
            if (keyEvent.IsKeyDown(Keys.Down)) keyState[_DOWN] = true; else keyState[_DOWN] = false;
            if (keyEvent.IsKeyDown(Keys.Left)) keyState[_LEFT] = true; else keyState[_LEFT] = false;
            if (keyEvent.IsKeyDown(Keys.Right)) keyState[_RIGHT] = true; else keyState[_RIGHT] = false;
            if (keyEvent.IsKeyDown(Keys.Q)) keyState[_BUTTON1] = true; else keyState[_BUTTON1] = false;
            if (keyEvent.IsKeyDown(Keys.W)) keyState[_BUTTON2] = true; else keyState[_BUTTON2] = false;
            if (keyEvent.IsKeyDown(Keys.E)) keyState[_BUTTON3] = true; else keyState[_BUTTON3] = false;
            if (keyEvent.IsKeyDown(Keys.R)) keyState[_BUTTON4] = true; else keyState[_BUTTON4] = false;
            if (keyEvent.IsKeyDown(Keys.Enter)) keyState[_ENTER] = true; else keyState[_ENTER] = false;
            if (keyEvent.IsKeyDown(Keys.Escape)) keyState[_ESCAPE] = true; else keyState[_ESCAPE] = false;

            for (int i = 0; i < NUM_KEYS; i++)
            {
                prevKeyState[i] = keyState[i];
            }
        }

        public static Boolean IsPressed(int i)
        {
            return keyState[i] && !prevKeyState[i];
        }

        public Boolean AnyKeyPress()
        {
            for (int i = 0; i < NUM_KEYS; i++)
            {
                if (keyState[i]) return true;
            }
            return false;
        }
    }
}
