using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MainGame.Control
{
    public class Background
    {
        private Texture2D image;

        private Vector2 xy;
        private Vector2 d_xy;

        public int width;
        public int height;

        private double xscale;
        private double yscale;

        public Background(String s)
        {
            Setup(s, 0.1, 0.1);
        }

        public Background(String s, double d)
        {
            Setup(s, d, d);
        }

        public Background(String s, double d1, double d2)
        {
            Setup(s, d1, d2);
        }

        public Background(String s, double ms, int x, int y, int w, int h)
        {
            SetupExtended(s, ms, x, y, w, h);
        }

 
        public void SetupExtended(String s, double ms, int x, int y, int w, int h)
        {
            try
            {
                image = GlobalVariables.Background;
                image = image.getSubimage(x, y, w, h);
                width = image.Width;
                height = image.Height;
                xscale = ms;
                yscale = ms;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
            }
        }

        public void Setup(String s, double s1, double s2)
        {
            try
            {
                width = image.Width;
                height = image.Height;
                xscale = s1;
                yscale = s2;

            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
            }


        }
        public void SetPosition(double x, double y)
        {
            this.xy.X = (float)(x * xscale) % width;
            this.xy.Y = (float)(y * yscale) % height;
        }

        public void SetVector(double dx, double dy)
        {
            this.d_xy.X = (float)dx;
            this.d_xy.Y = (float)dy;
        }

        public void SetScale(double xscale, double yscale)
        {
            this.xscale = xscale;
            this.yscale = yscale;
        }

        public void SetDimensions(int i1, int i2)
        {
            width = i1;
            height = i2;
        }

        public double GetX()
        {
            return xy.X;
        }

        public double GetY()
        {
            return xy.Y;
        }

        public void update()
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
            for (int i = 0; i < GlobalVariables.WIDTH / width * GlobalVariables.SCALE; i++)
            {
                for (int j = 0; j < GlobalVariables.HEIGHT / height * GlobalVariables.SCALE; j++)
                {
                    g.Draw(image, (int)(x + width * i), (int)(y + height * j), null);
                }
            }

            if (xy.X < 0)
            {
                g.Draw(image, (int)x + GlobalVariables.WIDTH, (int)y, null);
            }
            if (xy.X > 0)
            {
                g.Draw(image, (int)x - GlobalVariables.WIDTH, (int)y, null);
            }
            if (xy.Y < 0)
            {
                g.Draw(image, (int)x, (int)y + GlobalVariables.HEIGHT, null);
            }
            if (xy.Y > 0)
            {
                g.Draw(image, (int)x, (int)y - GlobalVariables.HEIGHT, null);
            }
        }
    }
}
