using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Maps.TileMap;
using MainGame.Control;
using Microsoft.Xna.Framework;

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
                    (int)x - cwidth / 2,
                    (int)y - cheight / 2,
                    cwidth,
                    cheight
            );
        }

        public void CalculateCorners(double x, double y)
        {
            int leftTile = (int)(x - cwidth / 2) / tileSize;
            int rightTile = (int)(x + cwidth / 2 - 1) / tileSize;
            int topTile = (int)(y - cheight / 2) / tileSize;
            int bottomTile = (int)(y + cheight / 2 - 1) / tileSize;
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

            currCol = (int)x / tileSize;
            currRow = (int)y / tileSize;

            xdest = x + dx;
            ydest = y + dy;

            xtemp = x;
            ytemp = y;

            CalculateCorners(x, ydest);

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

            CalculateCorners(xdest, y);

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
                CalculateCorners(x, ydest + 1);
                if (!bottomLeft && !bottomRight)
                {
                    falling = true;
                }
            }

        }

        public int GetX()
        {
            return (int)x;
        }

        public int GetY()
        {
            return (int)y;
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
            this.x = x;
            this.y = y;
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
            return x + xmap + width < 0 ||
                    x + xmap - width > GlobalVariables.WIDTH ||
                    y + ymap + height < 0 ||
                    y + ymap - height > GlobalVariables.HEIGHT;
        }

        public void Draw(java.awt.Graphics2D g)
        {
            SetMapPosition();

            if (facingRight)
            {
                g.drawImage(animation.GetImage(), (int)(x + xmap - width / 2), (int)(y + ymap - height / 2), null);
            }
            else
            {
                g.drawImage(animation.GetImage(), (int)(x + xmap - width / 2 + width), (int)(y + ymap - height / 2), -width, height, null);
            }

            // draw collision box
            if (GlobalVariables.DEBUG_READY)
            {
                Rectangle r = GetRectangle();
                r.X += (int)xmap;
                r.Y += (int)ymap;
                g.draw(r);
            }
        }
    }
}
