using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Maps
{
    class MenuState
    {
        private int currentChoice = 0;
        private String[] options = {
        "Cycuszki",
        "Kanapka"
    };

        private Color titleColor;
        private Font titleFont;

        private Font font;
        private Font font2;
        private double b;
        private int tick;
        private double yPos;

        public MenuState(GameStateManager gsm)
        {

            super(gsm);
            // titles and fonts
            titleColor = Color.WHITE;
            titleFont = new Font("Viner Hand ITC", Font.PLAIN, 28);
            font = new Font("Arial", Font.PLAIN, 14);
            font2 = new Font("Arial", Font.PLAIN, 10);

            tick = 0;
            b = Math.random() * 0.06 + 0.07;
            yPos = (GamePanel.HEIGHT / 2 - 100);
        }

        public void init() { }

        public void update()
        {
            // check keys
            handleInput();
            tick++;
            yPos += Math.sin(b * tick);
        }

        public void draw(Graphics2D g)
        {

            Color myColour = new Color(0, 0, 0, 32);
            g.setColor(myColour);
            g.fillRect(0, 0, GamePanel.WIDTH, GamePanel.HEIGHT);

            // draw title
            g.setColor(titleColor);
            g.setFont(titleFont);
            g.drawString("ANDROMALIUS", GamePanel.WIDTH / 2 - 150, (int)yPos);

            // draw menu options
            g.setFont(font);
            g.setColor(Color.WHITE);
            g.drawString("Graj", GamePanel.WIDTH / 2 - 100, GamePanel.HEIGHT / 2);
            g.drawString("Zakoncz", GamePanel.WIDTH / 2 - 100, GamePanel.HEIGHT / 2 + 30);

            // draw point
            if (currentChoice == 0) g.fillRect(GamePanel.WIDTH / 2 - 120, GamePanel.HEIGHT / 2 - 5, 5, 5);
            else if (currentChoice == 1) g.fillRect(GamePanel.WIDTH / 2 - 120, GamePanel.HEIGHT / 2 - 5 + 30, 5, 5);

            // other
            g.setFont(font2);
            g.drawString("Dwa tysiunce szesnascie, kopirajt baj Pszemeg i Anyszka", 10, GamePanel.HEIGHT - 50);

        }

        private void select()
        {
            if (currentChoice == 0) { gsm.setState(GameStateManager.LEVEL1); }
            else if (currentChoice == 1) { System.exit(0); }
        }

        public void handleInput()
        {
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
    }
}
