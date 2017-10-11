using MainGame.Maps.Tiles;
using System;

namespace MainGame.Objects.Enemies
{
    class E_Skeleton : Enemy
    {
        private Boolean isDeadSet;

        public E_Skeleton(TileMap tm, Player p) : base(tm)
        {
            player = p;
            lastBreath = 40;

            health = maxHealth = 3;
            isDeadSet = false;
            width = 36;
            height = 48;

            collisionWidth = 25;
            collisionHeight = 45;

            damage = 10;
            moveSpeed = 0.8f;
            fallSpeed = 0.15f;
            maxFallSpeed = 4.0f;
            jumpStart = -5;

            left = false;
            facingRight = true;

            sprites = GlobalVariables.E_SkeletonGreenWalk;

            animation.SetFrames(sprites);
            animation.SetDelay(4);
        }

        public override void Update()
        {
            if (dead)
            {
                if (!isDeadSet)
                {
                    sprites = GlobalVariables.E_SkeletonGreenDead;
                    isDeadSet = true;
                    animation.SetFrames(sprites);
                    animation.SetDelay(4);
                }
                lastBreath--;

                animation.Update();

                if (lastBreath <= 0) remove = true;
            }
            else
            {
                if (!active)
                {
                    if (Math.Abs(player.GetX() - V2_xy.X) < GlobalVariables.GAME_WINDOW_WIDTH) active = true;
                    return;
                }

                GetNextPosition();
                CheckTileMapCollision();
                CalculateCorners(V2_xy.X, xy_dest.Y + 1);

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

                SetPosition(xy_temp.X, xy_temp.Y);

                if (V2_dxy.X == 0 && !dead)
                {
                    left = !left;
                    right = !right;
                    facingRight = !facingRight;
                }

                // update animation
                animation.Update();
            }
        }
    }
}
