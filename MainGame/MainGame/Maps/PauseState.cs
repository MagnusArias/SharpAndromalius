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
            "Return to game",
            "Exit game"
        };

        public PauseState(GameStateManager gsm) : base(gsm) { }

        public new void _Update() => HandleInput();

        public new void _Draw(SpriteBatch g)
        {
            Color myColour = new Color(0, 0, 0, 32);

            //g.fillRect(0, 0, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT);

            g.DrawString(GlobalVariables.fontTitle, options[0], new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 100, GlobalVariables.GAME_WINDOW_HEIGHT / 2), myColour);
            g.DrawString(GlobalVariables.fontTitle, options[1], new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 100, GlobalVariables.GAME_WINDOW_HEIGHT / 2 + 30), myColour);

           // if (currentChoice == 0) g.fillRect(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 120, GlobalVariables.GAME_WINDOW_HEIGHT / 2 - 5, 5, 5);
           // else if (currentChoice == 1) g.fillRect(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 120, GlobalVariables.GAME_WINDOW_HEIGHT / 2 - 5 + 30, 5, 5);

        }

        public new void HandleInput()
        {
            if (MyKeys.IsPressed(MyKeys._ESCAPE)) gsm.SetPaused(false);
            
            if (MyKeys.IsPressed(MyKeys._UP) && currentChoice > 0) currentChoice--;
            if (MyKeys.IsPressed(MyKeys._ENTER)) Select();
            if (MyKeys.IsPressed(MyKeys._DOWN) && (currentChoice < options.Length - 1)) currentChoice++;
        }

        public override void Select()
        {
            if (currentChoice == 1) { Environment.Exit(0); }
            else if (currentChoice == 0) { gsm.SetPaused(false); }
        }
    }
}
