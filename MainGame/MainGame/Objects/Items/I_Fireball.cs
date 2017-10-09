using MainGame.Maps.TileMap;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Objects.Items
{
    class I_Fireball : Item
    {
        private int tick;
        private double b;

        private Player player;

        public I_Fireball(TileMap tm, Player pl) : base(tm)
        {
            player = pl;
            facingRight = true;

            width = 16;
            height = 16;

            cwidth = 16;
            cheight = 16;

            sprites = Content.Fireball[0];

            animation.SetFrames(sprites);
            animation.SetDelay(-1);

            tick = 0;
            Random r = new Random();
            b = r.Next() * 0.06 + 0.07;
        }

        public void Update()
        {
            tick++;

            y = Math.Sin(b * tick) + y;

            // update animation
            animation.Update();
        }

        public void Draw(SpriteBatch g)
        {
            base.Draw(g);
            if (player.GetSkill(3)) base.DrawInHUD(g, 30, 15);
        }
    }
}
