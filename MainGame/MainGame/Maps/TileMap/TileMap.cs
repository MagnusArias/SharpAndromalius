using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Maps.Tiles
{
    class TileMap
    {
        // pozycja
        private Vector2 V2_xy;

        // krawedzie
        private Vector2 xy_min;
        private Vector2 xy_max;
        private float tween;

        // mapa
        private int[,] map;
        private int tileSize;
        private int numRows;
        private int numCols;
        private int width;
        private int height;

        // tileset jako obrazek
        private Texture2D tileset;
        private int numTilesAcross;
        private Tile[,] tiles;

        // do rysowania
        private int rowOffset;
        private int colOffset;
        private int numRowsToDraw;
        private int numColsToDraw;

        // efekty
        private Boolean shaking;
        private int intensity;

        public TileMap(int tileSize)
        {
            this.tileSize = tileSize;
            numRowsToDraw = GlobalVariables.GAME_WINDOW_HEIGHT / tileSize + 2;
            numColsToDraw = GlobalVariables.GAME_WINDOW_WIDTH / tileSize + 2;
            tween = 0.07F;
        }

        public void LoadTiles(String s)
        {
            try
            {
              /*  tileset = ImageIO.read(getClass().getResourceAsStream(s));

                numTilesAcross = tileset.Width / tileSize;
                tiles = new Tile[6, numTilesAcross];

                Texture2D subimage;
                for (int col = 0; col < numTilesAcross; col++)
                {
                    subimage = tileset.GetData<Texture2D>(new Rectangle(col * tileSize, tileSize * 0, tileSize, tileSize));
                    tiles[0, col] = new Tile(subimage, Tile.AIR);

                    subimage = tileset.getSubimage(col * tileSize, tileSize * 1, tileSize, tileSize);
                    tiles[1, col] = new Tile(subimage, Tile.AIR);

                    subimage = tileset.getSubimage(col * tileSize, tileSize * 2, tileSize, tileSize);
                    tiles[2, col] = new Tile(subimage, Tile.SOLID);

                    subimage = tileset.getSubimage(col * tileSize, tileSize * 3, tileSize, tileSize);
                    tiles[3, col] = new Tile(subimage, Tile.SOLID);

                    subimage = tileset.getSubimage(col * tileSize, tileSize * 4, tileSize, tileSize);
                    tiles[4, col] = new Tile(subimage, Tile.SOLID);

                    //subimage = tileset.getSubimage(col * tileSize, tileSize * 5, tileSize, tileSize);
                    tiles[5, col] = new Tile(subimage, Tile.SOLID);
                }*/

            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
            }
        }

        public void LoadMap(String s)
        {

            try
            {
                //InputStream ins = getClass().getResourceAsStream(s);
               // BufferedReader br = new BufferedReader(new InputStreamReader(ins));

               // numCols = int.Parse(br.readLine());
                //numRows = int.Parse(br.readLine());

                map = new int[numRows, numCols];
                width = numCols * tileSize;
                height = numRows * tileSize;


                xy_min.X = GlobalVariables.GAME_WINDOW_WIDTH - width;
                xy_max.X = 0;
                xy_min.Y = GlobalVariables.GAME_WINDOW_HEIGHT - height;
                xy_max.Y = 0;

                char delims = ' ';
               /* for (int row = 0; row < numRows; row++)
                {
                   /String line = br.readLine();
                    String[] tokens = line.Split(delims);
                    for (int col = 0; col < numCols; col++)
                    {
                        map[row, col] = int.Parse(tokens[col]);
                    }
                }*/

            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
            }
        }

        public int GetTileSize() => tileSize;

        public float GetX() => V2_xy.X;

        public float GetY() => V2_xy.Y;

        public int GetWidth() => width;

        public int GetHeight() => height;

        public int GetNumRows() => numRows;

        public int GetNumCols() => numCols;

        public Boolean IsShaking() => shaking;

        public void SetShaking(Boolean b, int i)
        {
            shaking = b;
            intensity = i;
        }

        public int GetType(int row, int col)
        {
            int rc = map[row, col];
            int r = rc / numTilesAcross;
            int c = rc % numTilesAcross;
            return tiles[r, c].GetTileType();
        }

        public void SetTween(float d) => tween = d;

        public void SetBounds(int i1, int i2, int i3, int i4)
        {
            xy_min.X = GlobalVariables.GAME_WINDOW_WIDTH - i1;
            xy_min.Y = GlobalVariables.GAME_WINDOW_WIDTH - i2;
            xy_max.X = i3;
            xy_max.Y = i4;
        }

        public void SetPosition(float x, float y)
        {
            this.V2_xy.X += (x - this.V2_xy.X) * tween;
            this.V2_xy.Y += (y - this.V2_xy.Y) * tween;

            FixBounds();

            colOffset = (int)-this.V2_xy.X / tileSize;
            rowOffset = (int)-this.V2_xy.Y / tileSize;
        }

        public void FixBounds()
        {
            if (V2_xy.X < xy_min.X) V2_xy.X = xy_min.X;
            if (V2_xy.Y < xy_min.Y) V2_xy.Y = xy_min.Y;
            if (V2_xy.X > xy_max.X) V2_xy.X = xy_max.X;
            if (V2_xy.Y > xy_max.Y) V2_xy.Y = xy_max.X;
        }

        public void Update()
        {
            Random r = new Random();

            if (shaking)
            {
                this.V2_xy.X += r.Next() * intensity - intensity / 2;
                this.V2_xy.Y += r.Next() * intensity - intensity / 2;
            }
        }

        public void Draw(SpriteBatch g)
        {

            for (int row = rowOffset; row < rowOffset + numRowsToDraw; row++)
            {

                if (row >= numRows) break;

                for (int col = colOffset; col < colOffset + numColsToDraw; col++)
                {

                    if (col >= numCols) break;
                    if (map[row, col] == 0) continue;

                    int rc = map[row, col];
                    int r = rc / numTilesAcross;
                    int c = rc % numTilesAcross;

                    // tile, position as V2_Xy, scale as tileSize
                    g.Draw(
                        tiles[r, c].GetImage(), 
                        new Vector2(V2_xy.X + col * tileSize, V2_xy.Y + row * tileSize), 
                        new Rectangle(0, 0, width, height), 
                        Color.White, 
                        0.0f, 
                        new Vector2(width / 2, height / 2), 
                        1.0f, 
                        SpriteEffects.None, 
                        0.0f
                        );

                    Rectangle rec = new Rectangle((int)V2_xy.X + col * tileSize, (int)V2_xy.Y + row * tileSize, 30, 30);
                    if (GlobalVariables.DEBUG_READY && rc > 59) g.Draw(GlobalVariables.blackRect, rec, Color.Black);

                }
            }
        }
    }
}
