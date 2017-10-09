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
            playerX = "X:" + p.getx().ToString();
            playerY = "Y:" + p.gety().ToString();
            playerDX = "DX:" + p.getdx().ToString();
            playerDY = "DY:" + p.getdy().ToString();

            playerHealth = p.getHealth().ToString() + "/" + p.getMaxHealth().ToString();
            playerMana = p.getMana().ToString() + "/" + p.getMaxMana().ToString();
            playerDash = p.getSta().ToString() + "/" + p.getMaxSta().ToString();
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
                setMapPosition();
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
