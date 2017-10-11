using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Objects.Items
{
    class Item : ParentObject
    {
        protected Texture2D[] sprites;
        protected int tick;
        protected double b;
        protected Player player;

        public Item(TileMap tm, Player pl, Texture2D[] spr) : base(tm)
        {
            player = pl;
            facingRight = true;

            width = 16;
            height = 16;

            collisionWidth = 16;
            collisionHeight = 16;

            sprites = spr;

            animation.SetFrames(sprites);
            animation.SetDelay(-1);

            tick = 0;
            Random r = new Random();
            b = r.Next() * 0.06 + 0.07;
        }
        
        public override void Update()
        {
            tick++;
            V2_xy.Y = (float)Math.Sin(b * tick);
            animation.Update();
        }

        public void DrawInHUD(SpriteBatch g, int x, int y)
        {
            g.Draw(
                animation.GetImage(), 
                new Vector2(x, y), 
                new Rectangle(0, 0, width, height), 
                Color.White, 0.0f, 
                new Vector2(width / 2, height / 2), 
                1.0f, 
                SpriteEffects.None, 
                0.0f);
        }
    }
}
