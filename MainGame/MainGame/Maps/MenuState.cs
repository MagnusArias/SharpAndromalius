using MainGame.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MainGame.Maps
{
    class MenuState : GameState
    {
        private int currentChoice = 0;
        private String[] options = {
            "Graj",
            "Zakoncz"
        };

        private double b;
        private int tick;
        private double yPos;

        public MenuState(GameStateManager gsm) : base(gsm)
        {
            tick = 0;
            Random r = new Random();
            b = r.Next() * 0.06 + 0.07;
            yPos = (GlobalVariables.GAME_WINDOW_HEIGHT / 2 - 100);
        }

        public new void _Update()
        {
            // check keys
            HandleInput();
            tick++;
            yPos += Math.Sin(b * tick);
        }

        public override void _Draw(SpriteBatch g)
        {
            g.Draw(GlobalVariables.blackRect, new Rectangle(0, 0, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT), Color.Black);

            // draw title
            g.DrawString(GlobalVariables.fontTitle, "ANDROMALIUS", new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 150, (float)yPos), Color.White);

            // draw menu options
            g.DrawString(GlobalVariables.fontSimple, options[0], new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 100, GlobalVariables.GAME_WINDOW_HEIGHT / 2), Color.White);
            g.DrawString(GlobalVariables.fontSimple, options[1], new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 100, GlobalVariables.GAME_WINDOW_HEIGHT / 2 + 30), Color.White);

            // draw point
            if (currentChoice == 0) g.Draw(GlobalVariables.blackRect, new Rectangle(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 120, GlobalVariables.GAME_WINDOW_HEIGHT / 2 - 5, 5, 5), Color.White);
            else if (currentChoice == 1) g.Draw(GlobalVariables.blackRect, new Rectangle(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 120, GlobalVariables.GAME_WINDOW_HEIGHT / 2 - 5 + 30, 5, 5), Color.White);

            // other
            g.DrawString(GlobalVariables.fontTitle, "2017, Copyright Przemyslaw Debiec, MIT license", new Vector2(10, GlobalVariables.GAME_WINDOW_HEIGHT - 50), Color.White);
        }

        public override void Select()
        {
            if (currentChoice == 0) { gsm.SetState(GameStateManager.LEVEL1); }
            else if (currentChoice == 1) { Environment.Exit(0); }
        }

        public new void HandleInput()
        {
            if (MyKeys.IsPressed(MyKeys._UP) && currentChoice > 0) currentChoice--;
            if (MyKeys.IsPressed(MyKeys._ENTER)) Select();
            if (MyKeys.IsPressed(MyKeys._DOWN) && (currentChoice < options.Length - 1)) currentChoice++;
        }
    }
}
