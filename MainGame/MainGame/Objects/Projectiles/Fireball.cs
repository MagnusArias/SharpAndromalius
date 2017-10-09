using MainGame.Control;
using MainGame.Maps.TileMap;
using MainGame.Objects.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

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

            moveSpeed = 3.8;
            hit = false;
            remove = false;
            damage = 5;

            width = 30;
            height = 15;
            cwidth = 28;
            cheight = 14;

            attackRect = new Rectangle(0, 0, 0, 0);
            attackRect.Width = 28;
            attackRect.Height = 14;

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

        public void ShootFireball(double startX, double startY, Boolean facing)
        {
            x = startX;
            y = startY;
            facingRight = facing;

            if (facingRight) dx = moveSpeed;
            else dx = -moveSpeed;
        }

        public void SetHit()
        {
            if (hit) return;
            hit = true;
            animation.SetFrames(hitSprites);
            animation.SetDelay(4);
            dx = 0;
        }

        public Boolean IsHit()
        {
            return hit;
        }

        public Boolean ShouldRemove()
        {
            return remove;
        }

        public void Update(ArrayList enemies)
        {

            CheckTileMapCollision();
            SetPosition(xtemp, ytemp);

            if (dx == 0 && !hit)
            {
                SetHit();
            }

            for (int i = 0; i < enemies.Count; i++)
            {

                Enemy e = (Enemy)enemies[i];

                // sprawdzenie ataku, zadajemy obrazenia wrogowi

                if (e.Intersects(attackRect))
                {
                    e.Hit(damage);
                    hit = true;
                }

            }
            if (!hit)
            {
                attackRect.Y = (int)y - 7;
                if (facingRight) attackRect.X = (int)x - 7;
                else attackRect.X = (int)x - 20;
            }

            animation.Update();
            if (hit && animation.HasPlayedOnce())
            {
                remove = true;
            }

        }

        public void Draw(Graphics2D g)
        {
            base.Draw(g);
        }
    }
}
