using MainGame.Control;
using MainGame.Maps.TileMap;
using Microsoft.Xna.Framework.Graphics;

using System;


namespace MainGame.Objects.Items
{
    class Item : ParentObject
    {
        protected Boolean remove;
        protected Texture2D[] sprites;

        public Item(TileMap tm) : base(tm)
        {
            remove = false;
            animation = new Animation();
        }

        public Boolean ShouldRemove()
        {
            return remove;
        }

        public void CanBeRemoved()
        {
            remove = true;
        }

        public void Update() { }

        public void Draw(java.awt.Graphics2D g)
        {
            base.Draw(g);
        }

        public void DrawInHUD(java.awt.Graphics2D g, int x, int y)
        {
            g.drawImage(animation.getImage(), x, y, null);
        }
    }
}
