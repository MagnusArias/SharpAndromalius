using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace MainGame.Control
{
    public class Background
    {
        private Texture2D image;

        private Vector2 xy;
        private Vector2 d_xy;
        private Vector2 scale;

        public int width;
        public int height;

        public Background(Texture2D s) => Setup(s, 0.1f, 0.1f);

        public Background(Texture2D s, float d) => Setup(s, d, d);

        public Background(Texture2D s, float d1, float d2) => Setup(s, d1, d2);

        //public Background(Texture2D s, float ms, int x, int y, int w, int h) => SetupExtended(s, ms, x, y, w, h);

        public void Setup(Texture2D s, float s1, float s2)
        {
            image = s;
            width = image.Width;
            height = image.Height;
            scale.X = s1;
            scale.Y = s2;
        }

        /*  public void SetupExtended(Texture2D s, float ms, int x, int y, int w, int h)
          {
              image = GlobalVariables.Background;
              image = image.getSubimage(x, y, w, h);
              width = image.Width;
              height = image.Height;
              scale.X = ms;
              scale.Y = ms;
          }*/

        public void SetPosition(float x, float y)
        {
            this.xy.X = (x * scale.X) % width;
            this.xy.Y = (y * scale.Y) % height;
        }

        public void SetVector(float dx, float dy)
        {
            this.d_xy.X = (float)dx;
            this.d_xy.Y = (float)dy;
        }

        public void SetScale(float xscale, float yscale)
        {
            scale.X = xscale;
            scale.Y = yscale;
        }

        public void SetDimensions(int i1, int i2)
        {
            width = i1;
            height = i2;
        }

        public double GetX() => xy.X;

        public double GetY() => xy.Y;

        public void Update()
        {
            xy.X += d_xy.X;
            while (xy.X <= -width) xy.X += width;
            while (xy.X >= width) xy.X -= width;

            xy.Y += d_xy.Y;
            while (xy.Y <= -height) d_xy.Y += height;
            while (xy.Y >= height) d_xy.Y -= height;
        }

        public void Draw(SpriteBatch g)
        {
            for (int i = 0; i < GlobalVariables.GAME_WINDOW_WIDTH / width * GlobalVariables.GAME_WINDOW_SCALE; i++)
            {
                for (int j = 0; j < GlobalVariables.GAME_WINDOW_HEIGHT / height * GlobalVariables.GAME_WINDOW_SCALE; j++)
                {
                    g.Draw(
                        image, 
                        new Vector2(xy.X + width * i, xy.Y + height * j), 
                        new Rectangle(0, 0, width, height), 
                        Color.White, 0.0f, 
                        new Vector2(width / 2, height / 2), 
                        1.0f, 
                        SpriteEffects.None, 
                        0.0f);
                }
            }
        }
    }
}
