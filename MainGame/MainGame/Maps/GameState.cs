using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Maps
{
    class GameState
    {
        protected GameStateManager gsm;

        public GameState(GameStateManager gsm)
        {
            this.gsm = gsm;
        }

        public abstract void init();
        public abstract void update();
        public abstract void draw(Graphics2D g);
        public abstract void handleInput();
    }
}
