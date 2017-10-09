using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Objects.Enemies
{
    class E_Skeleton
    {
        private boolean active;

        private boolean isDeadSet;

        public EnemySkeleton(TileMap tm, Player p)
        {

            super(tm);
            player = p;
            lastBreath = 40;

            health = maxHealth = 3;
            isDeadSet = false;
            width = 36;
            height = 48;

            cwidth = 25;
            cheight = 45;

            damage = 10;
            moveSpeed = 0.8;
            fallSpeed = 0.15;
            maxFallSpeed = 4.0;
            jumpStart = -5;

            left = false;
            facingRight = true;

            sprites = Content.EnemySkeleton[0];

            animation.setFrames(sprites);
            animation.setDelay(4);
        }

        private void getNextPosition()
        {
            if (!dead)
            {
                if (left) dx = -moveSpeed;
                else if (right) dx = moveSpeed;
                else dx = 0;
                if (falling)
                {
                    dy += fallSpeed;
                    if (dy > maxFallSpeed) dy = maxFallSpeed;
                }
                if (jumping && !falling)
                {
                    dy = jumpStart;
                }
            }
            else
            {
                dx = 0;
                dy = 0;
            }
        }

        public void update()
        {

            if (dead)
            {
                if (!isDeadSet)
                {
                    sprites = Content.EnemySkeletonDead[0];
                    isDeadSet = true;
                    animation.setFrames(sprites);
                    animation.setDelay(4);
                }
                lastBreath--;

                animation.update();

                if (lastBreath <= 0) remove = true;
            }
            else
            {
                if (!active)
                {
                    if (Math.abs(player.getx() - x) < GamePanel.WIDTH) active = true;
                    return;
                }

                getNextPosition();
                checkTileMapCollision();
                calculateCorners(x, ydest + 1);

                if (!bottomLeft)
                {
                    left = false;
                    right = facingRight = true;
                }
                if (!bottomRight)
                {
                    left = true;
                    right = facingRight = false;
                }
                setPosition(xtemp, ytemp);

                if (dx == 0 && !dead)
                {
                    left = !left;
                    right = !right;
                    facingRight = !facingRight;
                }

                // update animation
                animation.update();
            }
        }

        public void draw(Graphics2D g)
        {
            super.draw(g);
        }
    }
}
