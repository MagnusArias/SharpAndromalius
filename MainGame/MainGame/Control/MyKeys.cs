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
        
        public static void KeySet(Boolean b)
        {
            if (keyEvent.IsKeyDown(Keys.Up)) keyState[_UP] = b;
            else if (keyEvent.IsKeyDown(Keys.Left)) keyState[_LEFT] = b;
            else if (keyEvent.IsKeyDown(Keys.Down)) keyState[_DOWN] = b;
            else if (keyEvent.IsKeyDown(Keys.Right)) keyState[_RIGHT] = b;
            else if (keyEvent.IsKeyDown(Keys.Q)) keyState[_BUTTON1] = b;
            else if (keyEvent.IsKeyDown(Keys.W)) keyState[_BUTTON2] = b;
            else if (keyEvent.IsKeyDown(Keys.E)) keyState[_BUTTON3] = b;
            else if (keyEvent.IsKeyDown(Keys.R)) keyState[_BUTTON4] = b;
            else if (keyEvent.IsKeyDown(Keys.Enter)) keyState[_ENTER] = b;
            else if (keyEvent.IsKeyDown(Keys.Escape)) keyState[_ESCAPE] = b;
        }

        public static void Update()
        {
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
