using MainGame.Control;
using MainGame.Maps.Tiles;
using MainGame.Objects.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MainGame.Objects.Projectiles
{
    class Fireball : ParentObject
    {
        public const String FIREBALLSPRITEMAP = "/Game/Src/Assets/fireball-spriteset.png";

	    private Boolean hit;
        private Boolean remove;
        private Texture2D[] sprites;
        private Texture2D[] hitSprites;
        private Rectangle attackRect;
        private int damage;

        public Fireball(TileMap tm, Boolean right) : base(tm)
        {

            animation = new Animation();

            moveSpeed = 3.8f;
            hit = false;
            remove = false;
            damage = 5;

            width = 30;
            height = 15;
            cwidth = 28;
            cheight = 14;

            attackRect = new Rectangle(0, 0, 28, 14);

            // load sprites
            try
            {

                Texture2D spritesheet = ImageIO.read(
                    getClass().getResourceAsStream(FIREBALLSPRITEMAP)
                );

                sprites = new Texture2D[12];
                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i] = spritesheet.getSubimage(
                        i * width,
                        0,
                        width,
                        height
                    );
                }

                hitSprites = new Texture2D[3];
                for (int i = 0; i < hitSprites.Length; i++)
                {
                    hitSprites[i] = spritesheet.getSubimage(
                        i * width,
                        height,
                        width,
                        height
                    );
                }

                animation.SetFrames(sprites);
                animation.SetDelay(4);

            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
            }

        }

        public void ShootFireball(float startX, float startY, Boolean facing)
        {
            V2_xy.X = startX;
            V2_xy.Y = startY;
            facingRight = facing;

            if (facingRight) V2_dxy.X = moveSpeed;
            else V2_dxy.X = -moveSpeed;
        }

        public void SetHit()
        {
            if (hit) return;
            hit = true;
            animation.SetFrames(hitSprites);
            animation.SetDelay(4);
            V2_dxy.X = 0;
        }

        public Boolean IsHit()
        {
            return hit;
        }

        public Boolean ShouldRemove()
        {
            return remove;
        }

        public void Update(List<Enemy> enemies)
        {

            CheckTileMapCollision();
            SetPosition(xy_temp.X, xy_temp.Y);

            if (V2_dxy.X == 0 && !hit)
            {
                SetHit();
            }

            for (int i = 0; i < enemies.Count; i++)
            {

                Enemy e = enemies[i];

                // sprawdzenie ataku, zadajemy obrazenia wrogowi

                if (e.Intersects(attackRect))
                {
                    e.Hit(damage);
                    hit = true;
                }

            }
            if (!hit)
            {
                attackRect.Y = (int)V2_xy.Y - 7;
                if (facingRight) attackRect.X = (int)V2_xy.X - 7;
                else attackRect.X = (int)V2_xy.X - 20;
            }

            animation.Update();
            if (hit && animation.HasPlayedOnce())
            {
                remove = true;
            }

        }

        public void Draw(SpriteBatch g)
        {
            base.Draw(g);
        }
    }
}
