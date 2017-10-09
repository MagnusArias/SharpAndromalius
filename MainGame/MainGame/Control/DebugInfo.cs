using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Objects;
using MainGame.Maps.TileMap;
using Microsoft.Xna.Framework.Graphics;

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

        public void Update()
        {
            Convert();
        }
        public Boolean GetStatus()
        {
            return debugReady;
        }

        public void SetReady()
        {
            debugReady = !debugReady;
        }
        public void Draw(SpriteBatch g)
        {
            if (debugReady)
            {
                SetMapPosition();
                g.setColor(java.awt.Color.GREEN);
                g.DrawString(playerX, GlobalVariables.WIDTH - 150, 20);
                g.DrawString(playerY, GlobalVariables.WIDTH - 150, 30);
                g.DrawString(playerDX, GlobalVariables.WIDTH - 100, 20);
                g.DrawString(playerDY, GlobalVariables.WIDTH - 100, 30);

                g.DrawString(playerHealth, 20, 25);
                g.DrawString(playerMana, 20, 43);
                g.DrawString(playerDash, 20, 60);
            }
        }
    }
}
