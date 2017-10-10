using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Control;
using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects
{
    class Teleport : ParentObject
    {
        private Texture2D[] sprites;

        public Teleport(TileMap tm) : base(tm)
        {
            animation = new Animation();

            facingRight = true;
            width = height = 32;
            cwidth = 64;
            cheight = 64;

            try
            {
                Texture2D spritesheet = ImageIO.read(getClass().getResourceAsStream(PORTALSPRITEMAP));

                sprites = new Texture2D[4];
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i] = spritesheet.getSubimage(i * width, 0, width, height);
                }

                animation.SetFrames(sprites);
                animation.SetDelay(8);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
                Environment.Exit(0);
            }

        }

        public void Update()
        {
            animation.Update();
        }

        public void Draw(SpriteBatch g)
        {
            base.Draw(g);
        }
    }
}
