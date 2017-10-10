using MainGame.Control;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MainGame.Maps
{
    class PauseState : GameState
    {
        private SpriteFont font;
        private int currentChoice = 0;

        private String[] options = {
            "Play",
            "Quit"
        };

        public PauseState(GameStateManager gsm) : base(gsm)
        {
            // fonts
            font = new SpriteFont("Century Gothic", PLAIN, 14);
            
        }

        public override void Init() { }

        public override void Update()
        {
            HandleInput();
        }

        public override void Draw(SpriteBatch g)
        {
            Color myColour = new Color(0, 0, 0, 32);
            g.setColor(myColour);
            g.fillRect(0, 0, GlobalVariables.WIDTH, GlobalVariables.HEIGHT);

            g.setFont(font);
            g.setColor(Color.WHITE);
            g.DrawString("Wroc do gry", GlobalVariables.WIDTH / 2 - 100, GlobalVariables.HEIGHT / 2);
            g.DrawString("Wyjdz z gry ", GlobalVariables.WIDTH / 2 - 100, GlobalVariables.HEIGHT / 2 + 30);

            if (currentChoice == 0) g.fillRect(GlobalVariables.WIDTH / 2 - 120, GlobalVariables.HEIGHT / 2 - 5, 5, 5);
            else if (currentChoice == 1) g.fillRect(GlobalVariables.WIDTH / 2 - 120, GlobalVariables.HEIGHT / 2 - 5 + 30, 5, 5);

        }

        public override void HandleInput()
        {
            if (Keys.IsPressed(Keys.ESCAPE)) gsm.SetPaused(false);

            if (Keys.IsPressed(Keys.ENTER)) Select();
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

        private void Select()
        {
            if (currentChoice == 1) { Environment.Exit(0); }
            else if (currentChoice == 0) { gsm.SetPaused(false); }
        }
    }
}
