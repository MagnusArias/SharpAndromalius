using MainGame.Maps.TileMap;
using MainGame.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Control
{
    class HUD : ParentObject
    {
        private Player player;

        public HUD(TileMap tm) : base(tm)
        {
            try
            {
                hpBar = ImageIO.read(getClass().getResourceAsStream(HPBAR));
                mpBar = ImageIO.read(getClass().getResourceAsStream(FIREBAR));
                staBar = ImageIO.read(getClass().getResourceAsStream(DASHBAR));
                hudBar = ImageIO.read(getClass().getResourceAsStream(HUD));
            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
                Environment.Exit(0);
            }
        }

        public void Init(Player p)
        {
            player = p;
        }

        public void Draw(SpriteBatch g)
        {
            g.drawImage(hpBar, (player.GetHealth() * 2) - 75, 15, null);
            g.drawImage(mpBar, (player.GetMana()) - 75, 15 + 16, null);
            g.drawImage(staBar, (player.GetStamina() / 2) - 75, 15 + 32, null);
            g.drawImage(hudBar, 0, 13, null);
        }
    }
}
