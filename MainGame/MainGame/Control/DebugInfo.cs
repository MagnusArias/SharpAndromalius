using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Objects;
using MainGame.Maps.TileMap;

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

        private void convert()
        {
            playerX = "X:" + p.GetX().ToString();
            playerY = "Y:" + p.GetY().ToString();
            playerDX = "DX:" + p.GetDX().ToString();
            playerDY = "DY:" + p.GetDY().ToString();

            playerHealth = p.GetHealth().ToString() + "/" + p.GetMaxHealth().ToString();
            playerMana = p.GetMana().ToString() + "/" + p.GetMaxMana().ToString();
            playerDash = p.GetStamina().ToString() + "/" + p.GetMaxStamina().ToString();
        }

        public DebugInfo(TileMap tm, Player pl) : base(tm)
        {
            p = pl;
            convert();
            debugReady = false;
        }

        public void update()
        {
            convert();
        }
        public Boolean getStatus()
        {
            return debugReady;
        }

        public void setReady()
        {
            debugReady = !debugReady;
        }
        public void draw(Graphics2D g)
        {
            if (debugReady)
            {
                SetMapPosition();
                g.setColor(java.awt.Color.GREEN);
                g.drawString(playerX, Game1.WIDTH - 150, 20);
                g.drawString(playerY, Game1.WIDTH - 150, 30);
                g.drawString(playerDX, Game1.WIDTH - 100, 20);
                g.drawString(playerDY, Game1.WIDTH - 100, 30);

                g.drawString(playerHealth, 20, 25);
                g.drawString(playerMana, 20, 43);
                g.drawString(playerDash, 20, 60);
            }
        }
    }
}
