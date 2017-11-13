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
        private List<Texture2D[]> sprites;

        public Teleport(TileMap tm) : base(tm)
        {
            animation = new Animation();

            facingRight = true;
            width = height = 32;
            collisionWidth = 64;
            collisionHeight = 64;

            sprites = LoadSubImages(GlobalVariables.Teleport);
            //sprites[i] = spritesheet.GetData<Rectangle>(new Rectangle(0,0,30,30)[]);
            //sprites[i] = spritesheet.getSubimage(i * width, 0, width, height);

            animation.SetFrames(sprites[0]);
            animation.SetDelay(8);
        }

        public override void Update() => animation.Update();

        public Color[] GetSubImage(Color[] colorData, int width, Rectangle rec)
        {
            Color[] color = new Color[rec.Width * rec.Height];

            for (int y = 0; y < rec.Height; y++)
            {
                for (int x = 0; x < rec.Width; x++)
                {
                    color[x + y * rec.Width] = colorData[x + rec.X + ((y + rec.Y) * rec.Width)];
                }
            }
            return color;
        }

        public List<Texture2D[]> LoadSubImages(Texture2D sourceSpritesheet)
        {
            Color[] imageData = new Color[sourceSpritesheet.Width * sourceSpritesheet.Height];
            Texture2D subImage;
            Rectangle sourceRec;

            List<Texture2D[]> destinationSprites = new List<Texture2D[]>();

            for (int i = 0; i < 4; i++)
            {
                Texture2D[] bi = new Texture2D[4];

                sourceRec = new Rectangle(i * 32, 0, 32,32);
                Color[] imagePiece = this.GetSubImage(imageData, sourceSpritesheet.Width, sourceRec);
                subImage = new Texture2D(Game1.Instance.GraphicsDevice, sourceRec.Width, sourceRec.Height);
                subImage.SetData<Color>(imagePiece);
                bi[i] = subImage;

                destinationSprites.Add(bi);
            }

            return destinationSprites;
        }
    }
}
