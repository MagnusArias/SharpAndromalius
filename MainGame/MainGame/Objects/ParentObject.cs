using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Maps.Tiles;
using MainGame.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects
{
    class ParentObject
    {
        // bajery do tilemapy
        protected TileMap tileMap;
        protected int tileSize;
        protected Vector2 V2_mapxy;

        // pozycja i wektor
        protected Vector2 V2_xy;
        protected Vector2 V2_dxy;
        protected Boolean facingRight;
        protected Animation animation;
        protected Vector2 V2_draw;

        // wymiary
        protected int width;
        protected int height;

        // "collision box"
        protected int cwidth;
        protected int cheight;

        // collision
        protected int currRow;
        protected int currCol;
        protected Vector2 xy_dest;
        protected Vector2 xy_temp;
        protected Boolean topLeft;
        protected Boolean topRight;
        protected Boolean bottomLeft;
        protected Boolean bottomRight;

        // poruszanie sie
        protected Boolean left;
        protected Boolean right;
        protected Boolean squat;
        protected Boolean jumping;
        protected Boolean falling;

        // atrybuty poruszania
        protected double moveSpeed;
        protected double maxSpeed;
        protected double stopSpeed;
        protected double fallSpeed;
        protected double maxFallSpeed;
        protected double jumpStart;
        protected double stopJumpSpeed;

        //kontrola
        protected Boolean debugReady;

        public ParentObject(TileMap tm)
        {
            tileMap = tm;
            tileSize = tm.GetTileSize();
        }

        public Boolean Intersects(ParentObject o)
        {
            Rectangle r1 = GetRectangle();
            Rectangle r2 = o.GetRectangle();
            return r1.Intersects(r2);
        }

        public Boolean Intersects(Rectangle r)
        {
            return GetRectangle().Intersects(r);
        }

        public Boolean Contains(ParentObject o)
        {
            Rectangle r1 = GetRectangle();
            Rectangle r2 = o.GetRectangle();
            return r1.Contains(r2);
        }

        public Boolean Contains(Rectangle r)
        {
            return GetRectangle().Contains(r);
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(
                    (int)V2_xy.X - cwidth / 2,
                    (int)V2_xy.Y - cheight / 2,
                    cwidth,
                    cheight
            );
        }

        public void CalculateCorners(double x, double y)
        {
            int leftTile =      (int)(V2_xy.X - cwidth / 2)     / tileSize;
            int rightTile =     (int)(V2_xy.X + cwidth / 2 - 1) / tileSize;
            int topTile =       (int)(V2_xy.Y - cheight / 2)    / tileSize;
            int bottomTile =    (int)(V2_xy.Y + cheight / 2 - 1)/ tileSize;

            if (topTile < 0 || bottomTile >= tileMap.GetNumRows() || leftTile < 0 || rightTile >= tileMap.GetNumCols())
            {
                topLeft = topRight = bottomLeft = bottomRight = false;
                return;
            }

            int tl = tileMap.GetType(topTile, leftTile);
            int tr = tileMap.GetType(topTile, rightTile);
            int bl = tileMap.GetType(bottomTile, leftTile);
            int br = tileMap.GetType(bottomTile, rightTile);

            topLeft = tl == Tile.SOLID;
            topRight = tr == Tile.SOLID;
            bottomLeft = bl == Tile.SOLID;
            bottomRight = br == Tile.SOLID;
        }

        public void CheckTileMapCollision()
        {

            currCol = (int)V2_xy.X / tileSize;
            currRow = (int)V2_xy.Y / tileSize;

            xy_dest.X = V2_xy.X + V2_dxy.X;
            xy_dest.Y = V2_xy.Y + V2_dxy.Y;

            xy_temp.X = V2_xy.X;
            xy_temp.Y = V2_xy.Y;

            CalculateCorners(V2_xy.X, xy_dest.Y);

            if (V2_dxy.Y < 0)
            {
                if (topLeft || topRight)
                {
                    V2_dxy.Y = 0;
                    xy_temp.Y = currRow * tileSize + cheight / 2;
                }
                else
                {
                    xy_temp.Y += V2_dxy.Y;
                }
            }
            if (V2_dxy.Y > 0)
            {
                if (bottomLeft || bottomRight)
                {
                    V2_dxy.Y = 0;
                    falling = false;
                    xy_temp.Y = (currRow + 1) * tileSize - cheight / 2;
                }
                else
                {
                    xy_temp.Y += V2_dxy.Y;
                }
            }

            CalculateCorners(xy_dest.X, V2_xy.Y);

            if (V2_dxy.X < 0)
            {
                if (topLeft || bottomLeft)
                {
                    V2_dxy.X = 0;
                    xy_temp.X = currCol * tileSize + cwidth / 2;
                }
                else
                {
                    xy_temp.X += V2_dxy.X;
                }
            }
            if (V2_dxy.X > 0)
            {
                if (topRight || bottomRight)
                {
                    V2_dxy.X = 0;
                    xy_temp.X = (currCol + 1) * tileSize - cwidth / 2;
                }
                else
                {
                    xy_temp.X += V2_dxy.X;
                }
            }

            if (!falling)
            {
                CalculateCorners(V2_xy.X, xy_dest.Y + 1);
                if (!bottomLeft && !bottomRight)
                {
                    falling = true;
                }
            }

        }
        
        public int GetX()
        {
            return (int)V2_xy.X;
            
        }

        public int GetY()
        {
            return (int)V2_xy.Y;
        }

        public double GetDX()
        {
            return V2_dxy.X;
        }

        public double GetDY()
        {
            return V2_dxy.Y;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public int GetCWidth()
        {
            return cwidth;
        }

        public int GetCHeight()
        {
            return cheight;
        }

        public void SetPosition(double x, double y)
        {
            this.V2_xy.X = (float)x;
            this.V2_xy.Y = (float)y;
        }

        public void SetVector(double dx, double dy)
        {
            this.V2_dxy.X = (float)dx;
            this.V2_dxy.Y = (float)dy;
        }

        public void SetMapPosition()
        {
            V2_mapxy.X = (float)tileMap.GetX();
            V2_mapxy.Y = (float)tileMap.GetY();
        }

        public void SetLeft(Boolean b)
        {
            left = b;
        }

        public void SetRight(Boolean b)
        {
            right = b;
        }

        public void SetUp(Boolean b)
        {
            jumping = b;
        }

        public void SetDown(Boolean b)
        {
            squat = b;
        }

        public Boolean NotOnScreen()
        {
            return V2_xy.X + V2_mapxy.X + width < 0 ||
                    V2_xy.X + V2_mapxy.X - width > GlobalVariables.WIDTH ||
                    V2_xy.Y + V2_mapxy.Y + height < 0 ||
                    V2_xy.Y + V2_mapxy.Y - height > GlobalVariables.HEIGHT;
        }

        public Vector2 Origin
        {
            get { return new Vector2(width / 2.0f, height / 2.0f); }
        }

        public void Draw(SpriteBatch g)
        {
            SetMapPosition();
            Rectangle source = new Rectangle(0, 0, animation.GetImage().Width, animation.GetImage().Height);

            V2_draw.X = V2_xy.X + V2_mapxy.X - (width / 2.0f);
            V2_draw.Y = V2_xy.Y + V2_mapxy.Y - (height / 2.0f);

            if (facingRight)
            {
                g.Draw(animation.GetImage(), V2_draw, source, Color.White, 0.0f, Origin, 1.0f, SpriteEffects.None, 0.0f);
            }
            else
            {
                g.Draw(animation.GetImage(), V2_draw, source, Color.White, 0.0f, Origin, 1.0f, SpriteEffects.FlipVertically, 0.0f);
            }

            // draw collision box
            /*
            if (GlobalVariables.DEBUG_READY)
            {
                Rectangle r = GetRectangle();
                r.X += (int)xmap;
                r.Y += (int)ymap;
                g.Draw(r);
            }
            */
        }
    }
}
