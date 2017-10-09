using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Maps.TileMap
{
    class Tile
    {
        // TODO - poszukac zamiennika buffered_image
        private String image;
        private int type;

        //tile types
        public const int AIR = 0;
        public const int SOLID = 1;

        public Tile(String image, int type)
        {
            this.image = image;
            this.type = type;
        }

        public String getImage()
        {
            return image;
        }

        public int getType()
        {
            return type;
        }
    }
}
