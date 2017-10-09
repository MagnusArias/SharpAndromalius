using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGamme.Maps
{
    class GameStateManager
    {
        private GameState[] gameStates;
        private int currentState;
        private PauseState pauseState;
        private boolean paused;

        private static final int NUMGAMESTATES = 16;

        public static final int MENUSTATE = 0;
        public static final int LEVEL1 = 1;
        public static final int LEVEL2 = 2;

        public GameStateManager()
        {

            gameStates = new GameState[NUMGAMESTATES];

            pauseState = new PauseState(this);
            paused = false;

            currentState = MENUSTATE;
            loadState(currentState);
        }

        private void loadState(int state)
        {
            if (state == MENUSTATE) gameStates[state] = new MenuState(this);
            else if (state == LEVEL1) gameStates[state] = new Level1(this);
            else if (state == LEVEL2) gameStates[state] = new Level2(this);
        }

        private void unloadState(int state)
        {
            gameStates[state] = null;
        }

        public void setState(int state)
        {
            unloadState(currentState);
            currentState = state;
            loadState(currentState);
        }

        public void setPaused(boolean b)
        {
            paused = b;
        }

        public void update()
        {
            if (paused)
            {
                pauseState.update();
                return;
            }
            if (gameStates[currentState] != null) gameStates[currentState].update();
        }

        public void draw(java.awt.Graphics2D g)
        {
            if (paused)
            {
                pauseState.draw(g);
                return;
            }
            if (gameStates[currentState] != null) gameStates[currentState].draw(g);
            else
            {
                Color myColour = new Color(255, 255, 255, 127);

                g.setColor(myColour);
                g.fillRect(0, 0, GamePanel.WIDTH, GamePanel.HEIGHT);
            }
        }
    }
}
