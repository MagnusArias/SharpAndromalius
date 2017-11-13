using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace MainGame.Control
{
    public class Background
    {
        private Texture2D image;

        private Vector2 XY;
        private Vector2 D_XY;
        private Vector2 scale;

        private int width;
        private int height;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public Background(Texture2D s) => Setup(s, 0.1f, 0.1f);

        public Background(Texture2D s, float d) => Setup(s, d, d);

        public Background(Texture2D s, float d1, float d2) => Setup(s, d1, d2);

        //public Background(Texture2D s, float ms, int x, int y, int w, int h) => SetupExtended(s, ms, x, y, w, h);

        public void Setup(Texture2D s, float s1, float s2)
        {
            image = s;
            Width = image.Width;
            Height = image.Height;
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

        public void SetPosition(float x, float y) => XY = new Vector2((x * scale.X) % Width, (y * scale.Y) % Height);

        public void SetVector(float dx, float dy) => D_XY = new Vector2(dx, dy);

        public void SetScale(float xscale, float yscale) => new Vector2(xscale, yscale);

        public void SetDimensions(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public void Update()
        {
            
            XY.X += D_XY.X;
            while (XY.X <= -Width) XY.X += Width;
            while (XY.X >= Width) XY.X -= Width;

            XY.Y += D_XY.Y;
            while (XY.Y <= -Height) D_XY.Y += Height;
            while (XY.Y >= Height) D_XY.Y -= Height;
        }

        public void Draw(SpriteBatch g)
        {
            for (int i = 0; i < GlobalVariables.GAME_WINDOW_WIDTH / Width * GlobalVariables.GAME_WINDOW_SCALE; i++)
            {
                for (int j = 0; j < GlobalVariables.GAME_WINDOW_HEIGHT / Height * GlobalVariables.GAME_WINDOW_SCALE; j++)
                {
                    g.Draw(
                        image, 
                        new Vector2(XY.X + Width * i, XY.Y + Height * j), 
                        new Rectangle(0, 0, Width, Height), 
                        Color.White, 0.0f, 
                        new Vector2(Width / 2, Height / 2), 
                        1.0f, 
                        SpriteEffects.None, 
                        0.0f);
                }
            }
        }
    }
}
