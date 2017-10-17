using MainGame.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Maps
{
    class PauseState : GameState
    {
        private int currentChoice = 0;

        private String[] options = {
            "Return to game",
            "Exit game"
        };

        public PauseState(GameStateManager gsm) : base(gsm) { }

        public override void _Update(GameTime gt) => HandleInput();

        public override void _Draw(SpriteBatch g)
        {
            Color myColour = new Color(0, 0, 0, 32);

            g.Draw(
                GlobalVariables.blackRect, 
                new Rectangle(0, 0, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT), 
                myColour);

            // draw menu options
            for (int i = 0; i < options.Length; i++)
            {
                g.DrawString(
                    GlobalVariables.fontSimple,
                    options[i],
                    new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2, (GlobalVariables.GAME_WINDOW_HEIGHT / 2) + (i * 30 * GlobalVariables.GAME_WINDOW_SCALE)),
                    Color.White,
                    0.0f,
                    GlobalVariables.fontSimple.MeasureString(options[i]) / 2,
                    GlobalVariables.GAME_WINDOW_SCALE,
                    SpriteEffects.None,
                    0.0f);
            }

            // draw point
            g.Draw(
                GlobalVariables.whiteRect, 
                new Rectangle(
                    GlobalVariables.GAME_WINDOW_WIDTH / 2 - 130, 
                    (GlobalVariables.GAME_WINDOW_HEIGHT / 2) + (currentChoice * 30 * GlobalVariables.GAME_WINDOW_SCALE), 
                    15,
                    15), 
                Color.White);

        }

        public override void HandleInput()
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
