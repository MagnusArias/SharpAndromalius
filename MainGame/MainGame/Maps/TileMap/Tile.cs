using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Maps.Tiles
{
    class Tile
    {
        private Texture2D image;
        private int type;

        public const int AIR = 0;
        public const int SOLID = 1;

        public Tile(Texture2D i, int t)
        {
            image = i;
            type = t;
        }

        public Texture2D GetImage() => image;

        public int GetTileType() => type;
    }
}
