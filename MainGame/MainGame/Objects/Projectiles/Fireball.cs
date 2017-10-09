using MainGame.Control;
using MainGame.Maps.TileMap;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


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

        public void shootFireball(double startX, double startY, boolean facing)
        {
            x = startX;
            y = startY;
            facingRight = facing;

            if (facingRight) dx = moveSpeed;
            else dx = -moveSpeed;
        }

        public void setHit()
        {
            if (hit) return;
            hit = true;
            animation.setFrames(hitSprites);
            animation.setDelay(4);
            dx = 0;
        }

        public boolean isHit()
        {
            return hit;
        }

        public boolean shouldRemove()
        {
            return remove;
        }

        public void update(ArrayList<Enemy> enemies)
        {

            checkTileMapCollision();
            setPosition(xtemp, ytemp);

            if (dx == 0 && !hit)
            {
                setHit();
            }

            for (int i = 0; i < enemies.size(); i++)
            {

                Enemy e = enemies.get(i);

                // sprawdzenie ataku, zadajemy obrazenia wrogowi

                if (e.intersects(attackRect))
                {
                    e.hit(damage);
                    hit = true;
                }

            }
            if (!hit)
            {
                attackRect.y = (int)y - 7;
                if (facingRight) attackRect.x = (int)x - 7;
                else attackRect.x = (int)x - 20;
            }

            animation.update();
            if (hit && animation.hasPlayedOnce())
            {
                remove = true;
            }

        }

        public void draw(Graphics2D g)
        {
            super.draw(g);
        }
    }
}
