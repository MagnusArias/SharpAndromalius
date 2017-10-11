using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Maps.Tiles
{
    class Tile
    {
        private Texture2D image;
        private int type;

        public const int AIR = 0;
        public const int SOLID = 1;

        public Tile(Texture2D image, int type)
        {
            this.image = image;
            this.type = type;
        }

        public Texture2D GetImage() => image;

        public int GetTileType() => type;
    }
}
