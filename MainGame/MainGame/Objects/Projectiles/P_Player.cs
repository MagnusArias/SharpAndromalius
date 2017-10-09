using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Objects.Projectiles
{
    class P_Player
    {
        private int count;
        private boolean remove;

        private BufferedImage[] sprites;

        public static int UP = 0;
        public static int LEFT = 1;
        public static int DOWN = 2;
        public static int RIGHT = 3;

        public EnergyParticle(TileMap tm, double x, double y, int dir)
        {

            super(tm);
            animation = new Animation();

            facingRight = true;
            this.x = x;
            this.y = y;
            double d1 = Math.random() * 2.5 - 1.25;
            double d2 = -Math.random() - 0.8;
            if (dir == UP)
            {
                dx = d1;
                dy = d2;
            }
            else if (dir == LEFT)
            {
                dx = d2;
                dy = d1;
            }
            else if (dir == DOWN)
            {
                dx = d1;
                dy = -d2;
            }
            else
            {
                dx = -d2;
                dy = d1;
            }

            count = 0;
            sprites = Content.EnergyParticle[0];
            animation.setFrames(sprites);
            animation.setDelay(-1);
        }

        public void update()
        {
            x += dx;
            y += dy;
            count++;
            if (count == 25) remove = true;
        }

        public boolean shouldRemove() { return remove; }

        public void draw(Graphics2D g)
        {
            super.draw(g);
        }
    }
}
