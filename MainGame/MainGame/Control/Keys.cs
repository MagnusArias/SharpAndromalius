﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Control
{
    class Keys
    {
        private const int NUM_KEYS = 16;

        public static Boolean[] keyState = new Boolean[NUM_KEYS];
        private static Boolean[] prevKeyState = new Boolean[NUM_KEYS];

        public static int UP = 0;
        public static int LEFT = 1;
        public static int DOWN = 2;
        public static int RIGHT = 3;
        public static int BUTTON1 = 4;
        public static int BUTTON2 = 5;
        public static int BUTTON3 = 6;
        public static int BUTTON4 = 7;
        public static int ENTER = 8;
        public static int ESCAPE = 9;

        public static void KeySet(int i, Boolean b)
        {
            if (i == KeyEvent.VK_UP) keyState[UP] = b;
            else if (i == KeyEvent.VK_LEFT) keyState[LEFT] = b;
            else if (i == KeyEvent.VK_DOWN) keyState[DOWN] = b;
            else if (i == KeyEvent.VK_RIGHT) keyState[RIGHT] = b;
            else if (i == KeyEvent.VK_Q) keyState[BUTTON1] = b;
            else if (i == KeyEvent.VK_W) keyState[BUTTON2] = b;
            else if (i == KeyEvent.VK_E) keyState[BUTTON3] = b;
            else if (i == KeyEvent.VK_R) keyState[BUTTON4] = b;
            else if (i == KeyEvent.VK_ENTER) keyState[ENTER] = b;
            else if (i == KeyEvent.VK_ESCAPE) keyState[ESCAPE] = b;
        }

        public static void Update()
        {
            for (int i = 0; i < NUM_KEYS; i++)
            {
                prevKeyState[i] = keyState[i];
            }
        }

        public Boolean IsPressed(int i)
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
