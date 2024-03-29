﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace MainGame.Maps
{
    class GameStateManager
    {
        private GameState[] gameStates;
        private int currentState;
        private PauseState pauseState;
        private Boolean paused;

        private const int NUMGAMESTATES = 16;

        public const int MENUSTATE = 0;
        public const int LEVEL1 = 1;
        public const int LEVEL2 = 2;

        public GameStateManager()
        {
            gameStates = new GameState[NUMGAMESTATES];

            pauseState = new PauseState(this);
            paused = false;

            currentState = MENUSTATE;
            LoadState(currentState);
        }

        private void LoadState(int state)
        {
            if (state == MENUSTATE) gameStates[state] = new MenuState(this);
            else if (state == LEVEL1) gameStates[state] = new Level_1(this);
            else if (state == LEVEL2) gameStates[state] = new Level_2(this);
        }

        private void UnloadState(int state) => gameStates[state] = null;

        public void SetState(int state)
        {
            UnloadState(currentState);
            currentState = state;
            LoadState(currentState);
        }

        public void SetPaused(Boolean b) => paused = b;

        public void _Update(GameTime gm)
        {
            if (paused)
            {
                pauseState._Update(gm);
                return;
            }
            if (gameStates[currentState] != null) gameStates[currentState]._Update(gm);
        }

        public void _Draw(SpriteBatch g)
        {
            if (paused)
            {
                pauseState._Draw(g);
                return;
            }
            if (gameStates[currentState] != null) gameStates[currentState]._Draw(g);
            else
            {
                Color myColour = new Color(255, 255, 255, 127);
                g.Draw(GlobalVariables.blackRect, new Rectangle(0, 0, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT), Color.Black);
            }
        }
    }
}
