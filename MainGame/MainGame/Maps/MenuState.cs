#region Using statements and file description
//-----------------------------------------------------------------------------
// MenuState.cs
//
// Originally created: 10.07.2016, 14:45 by Przemysław Dębiec
// 
// Main menu of the game, the first visible screen after starting
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
#endregion

namespace MainGame.Maps
{
    class MenuState : GameState
    {
        private int currentChoice = 0;
        private String[] options = {
            "Graj",
            "Zakoncz"
        };

        private String TITLE = "Sharp Andromalius";
        private Vector2 titleSize;

        private double b;
        private int tick;
        private double yPos;

        public MenuState(GameStateManager gsm) : base(gsm)
        {
            tick = 0;
            Random r = new Random();
            b = r.Next(0,1) * 0.06 + 0.07;
            titleSize = GlobalVariables.fontTitle.MeasureString(TITLE);
            yPos = (GlobalVariables.GAME_WINDOW_HEIGHT / 3);
        }

        public override void _Update(GameTime gt)
        {
            keyEvent = Keyboard.GetState();
            HandleInput();
            tick++;
            yPos += Math.Sin(b * tick) ;
        }

        public override void _Draw(SpriteBatch g)
        {
            g.Draw(
                GlobalVariables.blackRect, 
                new Rectangle(0, 0, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT), 
                Color.Black);

            // draw title
            g.DrawString(
                GlobalVariables.fontTitle, 
                TITLE, 
                new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2, (float)yPos), 
                Color.White,
                0.0f,
                titleSize/2,
                GlobalVariables.GAME_WINDOW_SCALE,
                SpriteEffects.None,
                0.0f);
            
            // draw menu options
            for (int i = 0; i < options.Length; i++)
            {
                g.DrawString(
                    GlobalVariables.fontSimple,
                    options[i],
                    new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2, (GlobalVariables.GAME_WINDOW_HEIGHT / 2) + (i*30*GlobalVariables.GAME_WINDOW_SCALE)),
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
                    (GlobalVariables.GAME_WINDOW_HEIGHT / 2) + (currentChoice*30*GlobalVariables.GAME_WINDOW_SCALE), 
                    15, 
                    15), 
                Color.White);

            // other
            g.DrawString(
                GlobalVariables.fontSimple, 
                "2017, Copyright (r) Przemyslaw Debiec", 
                new Vector2(10, GlobalVariables.GAME_WINDOW_HEIGHT - 50), 
                Color.White);
        }

        public override void Select()
        {
            if (currentChoice == 0) { gsm.SetState(GameStateManager.LEVEL1); }
            else if (currentChoice == 1) { Environment.Exit(0); }
        }

        public override void HandleInput()
        {
            if (keyEvent.IsKeyDown(Keys.Up) && currentChoice > 0) currentChoice--;
            if (keyEvent.IsKeyDown(Keys.Enter)) Select();
            if (keyEvent.IsKeyDown(Keys.Down) && (currentChoice < options.Length - 1)) currentChoice++;
        }
    }
}
