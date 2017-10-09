using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Maps.TileMap;
using MainGame.Control;

namespace MainGame.Objects
{
    abstract class Object
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
        protected Boolean up;
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


        // konstruktor
        public Object(TileMap tm)
        {
            tileMap = tm;
            tileSize = tm.getTileSize();
        }

        public Boolean intersects(Object o)
        {
            Rectangle r1 = getRectangle();
            Rectangle r2 = o.getRectangle();
            return r1.intersects(r2);
        }

        public Boolean intersects(Rectangle r)
        {
            return getRectangle().intersects(r);
        }

        public Boolean contains(Object o)
        {
            Rectangle r1 = getRectangle();
            Rectangle r2 = o.getRectangle();
            return r1.contains(r2);
        }

        public Boolean contains(Rectangle r)
        {
            return getRectangle().contains(r);
        }

        public Rectangle getRectangle()
        {
            return new Rectangle(
                    (int)x - cwidth / 2,
                    (int)y - cheight / 2,
                    cwidth,
                    cheight
            );
        }

        public void calculateCorners(double x, double y)
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

        public void checkTileMapCollision()
        {

            currCol = (int)x / tileSize;
            currRow = (int)y / tileSize;

            xdest = x + dx;
            ydest = y + dy;

            xtemp = x;
            ytemp = y;

            calculateCorners(x, ydest);

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

            calculateCorners(xdest, y);

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
                calculateCorners(x, ydest + 1);
                if (!bottomLeft && !bottomRight)
                {
                    falling = true;
                }
            }

        }

        public int getx() { return (int)x; }
        public int gety() { return (int)y; }
        public int getWidth() { return width; }
        public int getHeight() { return height; }
        public int getCWidth() { return cwidth; }
        public int getCHeight() { return cheight; }

        public void setPosition(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public void setVector(double dx, double dy)
        {
            this.dx = dx;
            this.dy = dy;
        }

        public void setMapPosition()
        {
            xmap = tileMap.getx();
            ymap = tileMap.gety();
        }

        // USTAWIENIE KLAWISZY
        public void setLeft(Boolean b) { left = b; }
        public void setRight(Boolean b) { right = b; }
        public void setUp(Boolean b) { jumping = b; }
        public void setDown(Boolean b) { squat = b; }

        public Boolean notOnScreen()
        {
            return x + xmap + width < 0 ||
                    x + xmap - width > GamePanel.WIDTH ||
                    y + ymap + height < 0 ||
                    y + ymap - height > GamePanel.HEIGHT;
        }

        public void draw(java.awt.Graphics2D g)
        {
            setMapPosition();
            if (facingRight)
            {
                g.drawImage(animation.getImage(), (int)(x + xmap - width / 2), (int)(y + ymap - height / 2), null);
            }
            else
            {
                g.drawImage(animation.getImage(), (int)(x + xmap - width / 2 + width), (int)(y + ymap - height / 2), -width, height, null);
            }

            // draw collision box
            /*Rectangle r = getRectangle();
            r.x += xmap;
            r.y += ymap;
            g.draw(r);*/
        }
    }
}
