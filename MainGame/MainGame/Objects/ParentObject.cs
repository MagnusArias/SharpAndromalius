using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Maps.TileMap;
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
        protected double xmap;
        protected double ymap;

        // pozycja i wektor
        protected Vector2 V2_xy;
        protected double x;
        protected double y;
        protected double dx;
        protected double dy;
        protected Boolean facingRight;
        protected Animation animation;

        // wymiary
        protected int width;
        protected int height;

        // "collision box"
        protected int cwidth;
        protected int cheight;

        // collision
        protected int currRow;
        protected int currCol;
        protected double xdest;
        protected double ydest;
        protected double xtemp;
        protected double ytemp;
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
            tileSize = tm.getTileSize();
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
            int leftTile = (int)(V2_xy.X - cwidth / 2) / tileSize;
            int rightTile = (int)(V2_xy.X + cwidth / 2 - 1) / tileSize;
            int topTile = (int)(V2_xy.Y - cheight / 2) / tileSize;
            int bottomTile = (int)(V2_xy.Y + cheight / 2 - 1) / tileSize;
            if (topTile < 0 || bottomTile >= tileMap.getNumRows() || leftTile < 0 || rightTile >= tileMap.getNumCols())
            {
                topLeft = topRight = bottomLeft = bottomRight = false;
                return;
            }
            int tl = tileMap.getType(topTile, leftTile);
            int tr = tileMap.getType(topTile, rightTile);
            int bl = tileMap.getType(bottomTile, leftTile);
            int br = tileMap.getType(bottomTile, rightTile);
            topLeft = tl == Tile.SOLID;
            topRight = tr == Tile.SOLID;
            bottomLeft = bl == Tile.SOLID;
            bottomRight = br == Tile.SOLID;
        }

        public void CheckTileMapCollision()
        {

            currCol = (int)V2_xy.X / tileSize;
            currRow = (int)V2_xy.Y / tileSize;

            xdest = V2_xy.X + dx;
            ydest = V2_xy.Y + dy;

            xtemp = V2_xy.X;
            ytemp = V2_xy.Y;

            CalculateCorners(V2_xy.X, ydest);

            if (dy < 0)
            {
                if (topLeft || topRight)
                {
                    dy = 0;
                    ytemp = currRow * tileSize + cheight / 2;
                }
                else
                {
                    ytemp += dy;
                }
            }
            if (dy > 0)
            {
                if (bottomLeft || bottomRight)
                {
                    dy = 0;
                    falling = false;
                    ytemp = (currRow + 1) * tileSize - cheight / 2;
                }
                else
                {
                    ytemp += dy;
                }
            }

            CalculateCorners(xdest, V2_xy.Y);

            if (dx < 0)
            {
                if (topLeft || bottomLeft)
                {
                    dx = 0;
                    xtemp = currCol * tileSize + cwidth / 2;
                }
                else
                {
                    xtemp += dx;
                }
            }
            if (dx > 0)
            {
                if (topRight || bottomRight)
                {
                    dx = 0;
                    xtemp = (currCol + 1) * tileSize - cwidth / 2;
                }
                else
                {
                    xtemp += dx;
                }
            }

            if (!falling)
            {
                CalculateCorners(V2_xy.X, ydest + 1);
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
            return dx;
        }

        public double GetDY()
        {
            return dy;
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
            this.dx = dx;
            this.dy = dy;
        }

        public void SetMapPosition()
        {
            xmap = tileMap.getx();
            ymap = tileMap.gety();
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
            return V2_xy.X + xmap + width < 0 ||
                    V2_xy.X + xmap - width > GlobalVariables.WIDTH ||
                    V2_xy.Y + ymap + height < 0 ||
                    V2_xy.Y + ymap - height > GlobalVariables.HEIGHT;
        }

        public Vector2 Origin
        {
            get { return new Vector2(width / 2.0f, height); }
        }

        public void Draw(SpriteBatch g)
        {
            SetMapPosition();

            if (facingRight)
            {
                g.Draw(animation.GetImage(), V2_xy, new Rectangle(0,0, width, height), Color.White, 0.0f, Origin, 1.0f, null, 0.0f );
            }
            else
            {
                //g.Draw(Texture2D texture, Vector2 position, Rectangle sourceRecangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
                g.Draw(animation.GetImage(), V2_xy, new Rectangle(0, 0, width, height), Color.White, 0.0f, Origin, 1.0f, null, 0.0f);
            }

            // draw collision box
            if (GlobalVariables.DEBUG_READY)
            {
                Rectangle r = GetRectangle();
                r.X += (int)xmap;
                r.Y += (int)ymap;
                g.Draw(r);
            }
        }
    }
}
