using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

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

        public void LoadTiles(Texture2D s)
        {
            tileset = s;

            numTilesAcross = tileset.Width / tileSize;
            tiles = new Tile[6, numTilesAcross];

            Color[] imageData = new Color[tileset.Width * tileset.Height];
            tileset.GetData<Color>(imageData);

            for (int col = 0; col < numTilesAcross; col++)
            {
                for (int i = 0; i < 2; i++)
                {
                    Rectangle sourceRec = new Rectangle(col * tileSize, tileSize * i, tileSize, tileSize);
                    Color[] imagePiece = this.GetSubImage(imageData, tileset.Width, sourceRec);
                    Texture2D subimage = new Texture2D(Game1.Instance.GraphicsDevice, sourceRec.Width, sourceRec.Height);
                    subimage.SetData<Color>(imagePiece);
                    tiles[0, col] = new Tile(subimage, Tile.AIR);
                }

                for (int i = 2; i < 6; i++)
                {
                    Rectangle sourceRec = new Rectangle(col * tileSize, tileSize * i, tileSize, tileSize);
                    Color[] imagePiece = this.GetSubImage(imageData, tileset.Width, sourceRec);
                    Texture2D subimage = new Texture2D(Game1.Instance.GraphicsDevice, width: sourceRec.Width, height: sourceRec.Height);
                    subimage.SetData<Color>(imagePiece);
                    tiles[0, col] = new Tile(subimage, Tile.SOLID);
                }
            }
        }

        public void LoadMap(String s)
        {
            StreamReader ins = new StreamReader(File.OpenRead(s));

            numCols = Int32.Parse(ins.ReadLine());
            numRows = Int32.Parse(ins.ReadLine());

            map = new int[numRows, numCols];
            width = numCols * tileSize;
            height = numRows * tileSize;


            xy_min.X = GlobalVariables.GAME_WINDOW_WIDTH - width;
            xy_max.X = 0;
            xy_min.Y = GlobalVariables.GAME_WINDOW_HEIGHT - height;
            xy_max.Y = 0;

            String[] delims = new String[]{"  ", "\n"};
            for (int row = 0; row < numRows; row++)
            {
                String line = ins.ReadLine();
                String[] tokens = line.Split(delims, StringSplitOptions.None);
                for (int col = 0; col < numCols; col++)
                {
                    map[row, col] = Int32.Parse(tokens[col]);
                }
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

        public Color[] GetSubImage(Color[] colorData, int width, Rectangle rec)
        {
            Color[] color = new Color[rec.Width * rec.Height];
            for (int x = 0; x < rec.Width; x++)
            {
                for (int y = 0; y < rec.Height; y++)
                {
                    color[x + y * rec.Width] = colorData[x + rec.X + (y + rec.Y) * width];
                }
            }
            return color;
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
