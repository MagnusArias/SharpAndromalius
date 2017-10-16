using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Control;
using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
            collisionWidth = 64;
            collisionHeight = 64;

            try
            {
                Texture2D spritesheet = GlobalVariables.Teleport;
                sprites = new Texture2D[4];
                for (int i = 0; i < sprites.Length; i++)
                {
                    Rectangle rec = new Rectangle
                    {
                        X = i * width,
                        Y = 0,
                        Width = width,
                        Height = height
                    };
                   // sprites[i] = spritesheet.GetData<Rectangle>(new Rectangle(0,0,30,30)[]);
                    //sprites[i] = spritesheet.getSubimage(i * width, 0, width, height);
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

        public override void Update() => animation.Update();
    }
}
