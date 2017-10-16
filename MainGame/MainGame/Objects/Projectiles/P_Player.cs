using MainGame.Control;
using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Objects.Projectiles
{
    class P_Player : ParentObject
    {
        private int count;

        private Texture2D[] sprites;

        public static int UP = 0;
        public static int LEFT = 1;
        public static int DOWN = 2;
        public static int RIGHT = 3;

        public P_Player(TileMap tm, float x, float y, int dir) : base(tm)
        {
            animation = new Animation();

            facingRight = true;
            V2_xy.X = x;
            V2_xy.Y = y;

            Random r = new Random();
            float d1 = (float)(r.Next() * 2.5 - 1.25);
            float d2 = (float)(-r.Next() - 0.8);

            switch (dir)
            {
                case 0:
                    V2_dxy.X = d1;
                    V2_dxy.Y = d2;
                    break;

                case 1:
                    V2_dxy.X = d2;
                    V2_dxy.Y = d1;
                    break;

                case 2:
                    V2_dxy.X = d1;
                    V2_dxy.Y = -d2;
                    break;

                case 3:
                    V2_dxy.X = -d2;
                    V2_dxy.Y = d1;
                    break;
            }

            count = 0;
            sprites[0] = GlobalVariables.whiteRect;

            animation.SetFrames(sprites);
            animation.SetDelay(-1);
        }

        public override void Update()
        {
            V2_xy.X += V2_dxy.X;
            V2_xy.Y += V2_dxy.Y;
            count++;
            if (count == 25) remove = true;
        }
    }
}
