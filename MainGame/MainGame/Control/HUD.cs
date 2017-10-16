using MainGame.Maps.Tiles;
using MainGame.Objects;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Control
{
    class HUD : ParentObject
    {
        private Player player;

        public HUD(TileMap tm) : base(tm)
        {
           /* try
            {
                hpBar = ImageIO.read(getClass().getResourceAsStream(HPBAR));
                mpBar = ImageIO.read(getClass().getResourceAsStream(FIREBAR));
                staBar = ImageIO.read(getClass().getResourceAsStream(DASHBAR));
                hudBar = ImageIO.read(getClass().getResourceAsStream(HUDS));
            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
                Environment.Exit(0);
            }*/
        }

        public void Init(Player p) => player = p;

        /*public override void Draw(SpriteBatch g)
        {
            g.Draw(hpBar, (player.GetHealth() * 2) - 75, 15, null);
            g.Draw(mpBar, (player.GetMana()) - 75, 15 + 16, null);
            g.Draw(staBar, (player.GetStamina() / 2) - 75, 15 + 32, null);
            g.Draw(hudBar, 0, 13, null);
        }*/
    }
}
