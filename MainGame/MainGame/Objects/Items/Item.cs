using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Objects.Items
{
    class Item
    {
        protected boolean remove;
        protected BufferedImage[] sprites;

        public ItemParent(TileMap tm)
        {
            super(tm);
            remove = false;
            animation = new Animation();
        }

        public boolean shouldRemove()
        {
            return remove;
        }

        public void canBeRemoved()
        {
            remove = true;
        }

        public void update() { }

        public void draw(java.awt.Graphics2D g)
        {
            super.draw(g);
        }
        public void drawInHUD(java.awt.Graphics2D g, int x, int y)
        {
            g.drawImage(animation.getImage(), x, y, null);
        }
    }
}
