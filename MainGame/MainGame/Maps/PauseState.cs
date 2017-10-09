using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Maps
{
    class PauseState
    {
        private Font font;
        private int currentChoice = 0;

        private String[] options = {
        "Play",
        "Quit"
    };

        public PauseState(GameStateManager gsm)
        {

            super(gsm);
            // fonts
            font = new Font("Century Gothic", Font.PLAIN, 14);
        }

        public void init() { }

        public void update()
        {
            handleInput();
        }

        public void draw(Graphics2D g)
        {
            Color myColour = new Color(0, 0, 0, 32);
            g.setColor(myColour);
            g.fillRect(0, 0, GamePanel.WIDTH, GamePanel.HEIGHT);

            g.setFont(font);
            g.setColor(Color.WHITE);
            g.drawString("Wroc do gry", GamePanel.WIDTH / 2 - 100, GamePanel.HEIGHT / 2);
            g.drawString("Wyjdz z gry ", GamePanel.WIDTH / 2 - 100, GamePanel.HEIGHT / 2 + 30);

            if (currentChoice == 0) g.fillRect(GamePanel.WIDTH / 2 - 120, GamePanel.HEIGHT / 2 - 5, 5, 5);
            else if (currentChoice == 1) g.fillRect(GamePanel.WIDTH / 2 - 120, GamePanel.HEIGHT / 2 - 5 + 30, 5, 5);

        }

        public void handleInput()
        {
            if (Keys.isPressed(Keys.ESCAPE)) gsm.setPaused(false);

            if (Keys.isPressed(Keys.ENTER)) select();
            if (Keys.isPressed(Keys.UP))
            {
                if (currentChoice > 0)
                {
                    currentChoice--;
                }
            }
            if (Keys.isPressed(Keys.DOWN))
            {
                if (currentChoice < options.length - 1) { currentChoice++; }
            }
        }

        private void select()
        {
            if (currentChoice == 1) { System.exit(0); }
            else if (currentChoice == 0) { gsm.setPaused(false); }
        }
    }
}
