using MainGame.Control;
using Microsoft.Xna.Framework;
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
        }

        public override void Init() { }

        public override void Update()
        {
            HandleInput();
        }

        public override void Draw(SpriteBatch g)
        {
            Color myColour = new Color(0, 0, 0, 32);

            g.fillRect(0, 0, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT);

            g.DrawString(GlobalVariables.fontTitle, "Wroc do gry", new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 100, GlobalVariables.GAME_WINDOW_HEIGHT / 2), myColour);
            g.DrawString(GlobalVariables.fontTitle, "Wyjdz z gry ", new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 100, GlobalVariables.GAME_WINDOW_HEIGHT / 2 + 30), myColour);

            if (currentChoice == 0) g.fillRect(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 120, GlobalVariables.GAME_WINDOW_HEIGHT / 2 - 5, 5, 5);
            else if (currentChoice == 1) g.fillRect(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 120, GlobalVariables.GAME_WINDOW_HEIGHT / 2 - 5 + 30, 5, 5);

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
