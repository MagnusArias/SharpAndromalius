using MainGame.Objects;
using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace MainGame.Control
{
    class DebugInfo : ParentObject
    {
        private String playerX;
        private String playerY;
        private String playerDX;
        private String playerDY;

        private String playerHealth;
        private String playerMana;
        private String playerDash;

        private Player p;
        private SpriteFont font;

        public DebugInfo(TileMap tm, Player pl) : base(tm)
        {
            p = pl;
            Convert();
            debugReady = false;
        }

        private void Convert()
        {
            playerX = "X:" + p.GetX().ToString();
            playerY = "Y:" + p.GetY().ToString();
            playerDX = "DX:" + p.GetDX().ToString();
            playerDY = "DY:" + p.GetDY().ToString();

            playerHealth = p.GetHealth().ToString() + "/" + p.GetMaxHealth().ToString();
            playerMana = p.GetMana().ToString() + "/" + p.GetMaxMana().ToString();
            playerDash = p.GetStamina().ToString() + "/" + p.GetMaxStamina().ToString();
        }

        public void Update() => Convert();

        public Boolean GetStatus() => debugReady;

        public void SetReady() => debugReady = !debugReady;

        public new void Draw(SpriteBatch g)
        {
            if (debugReady)
            {
                SetMapPosition();
                //g.DrawString(spriteFont, text, position, color);

                g.DrawString(font, playerX, new Vector2(GlobalVariables.GAME_WINDOW_WIDTH - 150, 20), Color.Green);
                g.DrawString(font, playerY, new Vector2(GlobalVariables.GAME_WINDOW_WIDTH - 150, 30), Color.Green);
                g.DrawString(font, playerDX, new Vector2(GlobalVariables.GAME_WINDOW_WIDTH - 100, 20), Color.Green);
                g.DrawString(font, playerDY, new Vector2(GlobalVariables.GAME_WINDOW_WIDTH - 100, 30), Color.Green);

                g.DrawString(font, playerHealth, new Vector2(20, 25), Color.Green);
                g.DrawString(font, playerMana, new Vector2(20, 43), Color.Green);
                g.DrawString(font, playerDash, new Vector2(20, 60), Color.Green);
            }
        }
    }
}
