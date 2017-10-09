using MainGame.Control;
using MainGame.Maps.TileMap;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace MainGame.Objects.Projectiles
{
    class P_Player : ParentObject
    {
        private int count;
        private Boolean remove;

        private Texture2D[] sprites;

        public static int UP = 0;
        public static int LEFT = 1;
        public static int DOWN = 2;
        public static int RIGHT = 3;

        public P_Player(TileMap tm, double x, double y, int dir) : base(tm)
        {
            animation = new Animation();

            facingRight = true;
            this.x = x;
            this.y = y;

            Random r = new Random();
            double d1 = r.Next() * 2.5 - 1.25;
            double d2 = -r.Next() - 0.8;

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

            animation.SetFrames(sprites);
            animation.SetDelay(-1);
        }

        public void Update()
        {
            x += dx;
            y += dy;
            count++;
            if (count == 25) remove = true;
        }

        public Boolean ShouldRemove() { return remove; }

        public void Draw(SpriteBatch g)
        {
            base.Draw(g);
        }
    }
}
