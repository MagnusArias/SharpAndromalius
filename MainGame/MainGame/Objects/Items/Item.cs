using MainGame.Control;
using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Objects.Items
{
    class Item : ParentObject
    {
        protected Texture2D[] sprites;

        public Item(TileMap tm) : base(tm) { }

        public void CanBeRemoved() => remove = true;

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
