using MainGame.Control;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MainGame.Maps
{
    class MenuState : GameState
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

        public MenuState(GameStateManager gsm) : base(gsm)
        {
            titleColor = Color.WHITE;
            titleFont = new Font("Viner Hand ITC", Font.PLAIN, 28);
            font = new Font("Arial", Font.PLAIN, 14);
            font2 = new Font("Arial", Font.PLAIN, 10);

            tick = 0;
            b = Math.random() * 0.06 + 0.07;
            yPos = (GlobalVariables.HEIGHT / 2 - 100);
        }

        public override void Init() { }

        public override void Update()
        {
            // check keys
            HandleInput();
            tick++;
            yPos += Math.Sin(b * tick);
        }

        public override void Draw(SpriteBatch g)
        {

            Color myColour = new Color(0, 0, 0, 32);
            g.setColor(myColour);
            g.fillRect(0, 0, GlobalVariables.WIDTH, GlobalVariables.HEIGHT);

            // draw title
            g.setColor(titleColor);
            g.setFont(titleFont);
            g.DrawString("ANDROMALIUS", GlobalVariables.WIDTH / 2 - 150, (int)yPos);

            // draw menu options
            g.setFont(font);
            g.setColor(Color.WHITE);
            g.DrawString("Graj", GlobalVariables.WIDTH / 2 - 100, GlobalVariables.HEIGHT / 2);
            g.DrawString("Zakoncz", GlobalVariables.WIDTH / 2 - 100, GlobalVariables.HEIGHT / 2 + 30);

            // draw point
            if (currentChoice == 0) g.fillRect(GlobalVariables.WIDTH / 2 - 120, GlobalVariables.HEIGHT / 2 - 5, 5, 5);
            else if (currentChoice == 1) g.fillRect(GlobalVariables.WIDTH / 2 - 120, GlobalVariables.HEIGHT / 2 - 5 + 30, 5, 5);

            // other
            g.setFont(font2);
            g.DrawString("2017, Copyright Przemyslaw Debiec, MIT license", 10, GlobalVariables.HEIGHT - 50);

        }

        private void select()
        {
            if (currentChoice == 0) { gsm.SetState(GameStateManager.LEVEL1); }
            else if (currentChoice == 1) { Environment.Exit(0); }
        }

        public override void HandleInput()
        {
            if (Keys.IsPressed(Keys.ENTER)) select();
            if (Keys.IsPressed(Keys.UP))
            {
                if (currentChoice > 0)
                {
                    currentChoice--;
                }
            }
            if (Keys.IsPressed(Keys.DOWN))
            {
                if (currentChoice < options.Length - 1) { currentChoice++; }
            }
        }
    }
}
