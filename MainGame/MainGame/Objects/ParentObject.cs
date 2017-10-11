#pragma warning disable CS0618 // Type or member is obsolete
using MainGame.Maps.Tiles;
using MainGame.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MainGame.Objects
{
    abstract class ParentObject
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

        // wymiary
        protected int width;
        protected int height;
        
        // "collision box"
        protected int cwidth;
        protected int cheight;
        protected Boolean debugReady;

        // collision
        protected int currRow;
        protected int currCol;
        protected Vector2 xy_dest;
        protected Vector2 xy_temp;
        protected Boolean topLeft;
        protected Boolean topRight;
        protected Boolean bottomLeft;
        protected Boolean bottomRight;
        protected Boolean playerCatch;

        // poruszanie sie
        protected Boolean left;
        protected Boolean right;
        protected Boolean up;
        protected Boolean squat;
        protected Boolean jumping;
        protected Boolean falling;
        protected Boolean remove;

        // atrybuty poruszania
        protected float moveSpeed;
        protected float maxSpeed;
        protected float stopSpeed;
        protected float fallSpeed;
        protected float maxFallSpeed;
        protected float jumpStart;
        protected float stopJumpSpeed;

        protected int health;
        protected int maxHealth;
        protected float percentHealth;

        protected Boolean dead;

        public ParentObject(TileMap tm)
        {
            tileMap = tm;
            tileSize = tm.GetTileSize();
            remove = false;
            animation = new Animation();
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

        public Rectangle GetRectangle() => new Rectangle(
                    (int)V2_xy.X - cwidth / 2,
                    (int)V2_xy.Y - cheight / 2,
                    cwidth,
                    cheight
            );

        public void CalculateCorners(float x, float y)
        {
            int leftTile = (int)(x - cwidth / 2) / tileSize;
            int rightTile = (int)(x + cwidth / 2 - 1) / tileSize;
            int topTile = (int)(y - cheight / 2) / tileSize;
            int bottomTile = (int)(y + cheight / 2 - 1) / tileSize;

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

            if (V2_dxy.Y < 0)
            {
                if (topLeft || bottomLeft)
                {
                    V2_dxy.Y = 0;
                    xy_temp.X = currCol * tileSize + cwidth / 2;
                }
                else
                {
                    xy_temp.X += V2_dxy.Y;
                }
            }
            if (V2_dxy.Y > 0)
            {
                if (topRight || bottomRight)
                {
                    V2_dxy.Y = 0;
                    xy_temp.X = (currCol + 1) * tileSize - cwidth / 2;
                }
                else
                {
                    xy_temp.X += V2_dxy.Y;
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

        public int GetX() => (int)V2_xy.X;

        public int GetY() => (int)V2_xy.Y;

        public int GetDX() => (int)V2_dxy.X;

        public int GetDY() => (int)V2_dxy.Y;

        public int GetWidth() => width;

        public int GetHeight() => height;

        public int GetCWidth() => cwidth;

        public int GetCHeight() => cheight;

        public void SetPosition(float x, float y)
        {
            V2_xy.X = x;
            V2_xy.Y = y;
        }

        public void SetVector(float dx, float dy)
        {
            V2_dxy.X = dx;
            V2_dxy.Y = dy;
        }

        public void SetMapPosition()
        {
            V2_mapxy.X = tileMap.GetX();
            V2_mapxy.Y = tileMap.GetY();
        }

        public void SetLeft(Boolean b) => left = b;

        public void SetRight(Boolean b) => right = b;

        public void SetUp(Boolean b) => jumping = b;

        public void SetDown(Boolean b) => squat = b;

        public Boolean NotOnScreen()
        {
            return V2_xy.X + V2_mapxy.X + width < 0 ||
                V2_xy.X + V2_mapxy.X - width > GlobalVariables.GAME_WINDOW_WIDTH ||
                V2_xy.Y + V2_mapxy.Y + height < 0 ||
                V2_xy.Y + V2_mapxy.Y - height > GlobalVariables.GAME_WINDOW_HEIGHT;
        }

        public Boolean ShouldRemove() => remove;

        public abstract void Update();

        public void Draw(SpriteBatch g)
        {
            SpriteEffects spriteEffect;
            if (facingRight) spriteEffect = SpriteEffects.None;
            else spriteEffect = SpriteEffects.FlipVertically;

            SetMapPosition();

            g.Draw(
                animation.GetImage(),                                                                           // image
                new Vector2(V2_xy.X + V2_mapxy.X + (width / 2.0f), V2_xy.Y + V2_mapxy.Y + (height / 2.0f)),     // position
                null,                                                                                           // destination rectangle
                new Rectangle(0, 0, animation.GetImage().Width, animation.GetImage().Height),                   // source - if null draws
                new Vector2(width / 2.0f, height / 2.0f),                                                       // origin
                0.0f,                                                                                           // rotation
                new Vector2(1, 1),                                                                              // scale
                Color.White,                                                                                    // color
                spriteEffect,                                                                                   // effects
                0.0f                                                                                            // layerDepth
                );

            // draw collision box
            /*
            Rectangle r = GetRectangle();
            r.X += (int)V2_mapxy.X;
            r.Y += (int)V2_mapxy.Y;
            g.Draw(r);
            */
        }

        public void DrawHP(SpriteBatch g)
        {
            if (playerCatch)
            {
                g.Draw(
                    GlobalVariables.bossHPBar,                                                                  // image
                    new Vector2(72, 122),                                                                       // position
                    new Rectangle(0, 0, (int)((GlobalVariables.GAME_WINDOW_WIDTH - 142) * percentHealth), 14),  // destination rectangle
                    null,                                                                                       // source - if null draws
                    Vector2.Zero,                                                                               // origin
                    0.0f,                                                                                       // rotation
                    new Vector2((float)percentHealth, 0),                                                       // scale
                    Color.White,                                                                                // color
                    SpriteEffects.None,                                                                         // effects
                    0.0f                                                                                        // layerDepth
                    );

                g.Draw(
                    GlobalVariables.bossHPBarOutline,                                                       // image
                    new Vector2(70, 120),                                                                   // position
                    new Rectangle(0, 0, GlobalVariables.GAME_WINDOW_WIDTH - 140, 16),                       // destination rectangle
                    null,                                                                                   // source - if null draws
                    Vector2.Zero,                                                                           // origin
                    0.0f,                                                                                   // rotation
                    new Vector2(1, 1),                                                                      // scale
                    Color.White,                                                                            // color
                    SpriteEffects.None,                                                                     // effects
                    0.0f                                                                                    // layerDepth
                    );

                g.DrawString(GlobalVariables.fontTitle, "Andromalius", new Vector2(GlobalVariables.GAME_WINDOW_WIDTH / 2 - 20, 105), Color.White);
            }
        }
    }
}
