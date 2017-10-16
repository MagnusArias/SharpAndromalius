using MainGame.Maps.Tiles;
using System;

namespace MainGame.Objects.Enemies
{
    class E_Ghost : Enemy
    {
        private Boolean isJumping;

        private const int IDLE = 0;
        private const int JUMPING = 1;
        private const int ATTACKING = 2;

        private int currentAction;

        private int attackTick;
        private int attackDelay = 30;
        private int step;

        public E_Ghost(TileMap tm, Player p) : base(tm)
        {
            player = p;

            health = maxHealth = 1;
            lastBreath = 5;
            width = 25;
            height = 40;

            collisionWidth = 25;
            collisionHeight = 40;

            damage = 15;

            moveSpeed = 1.5f;
            fallSpeed = 0.15f;
            maxFallSpeed = 4.0f;
            jumpStart = -5;

            attackTick = 0;
            facingRight = true;

            sprites = GlobalVariables.E_GhostBlueWalk;

           // animation.SetFrames(sprites);
            animation.SetDelay(4);
        }

        private double CalculateDistance(Player p)
        {
            double dist = Math.Abs(
                Math.Sqrt(
                    Math.Pow((V2_xy.X - p.GetX()), 2.00) +
                    Math.Pow((V2_xy.Y - p.GetY()), 2.00)
                    )
                );

            return dist;
        }

        public override void Update()
        {
            if (dead)
            {
                lastBreath--;
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
                SetPosition(xy_temp.X, xy_temp.Y);

                facingRight = player.GetX() >= V2_xy.X;

                // idle
                if (step == 0)
                {
                    if (currentAction != IDLE)
                    {
                        currentAction = IDLE;
                        animation.SetDelay(-1);
                    }
                    attackTick++;
                    if (attackTick >= attackDelay && CalculateDistance(player) < 60)
                    {
                        step++;
                        attackTick = 0;
                    }
                }
                // jump away
                if (step == 1)
                {
                    if (currentAction != JUMPING)
                    {
                        currentAction = JUMPING;
                        animation.SetDelay(-1);
                    }
                    isJumping = true;
                    if (facingRight) left = true;
                    else right = true;
                    if (falling)
                    {
                        step++;
                    }
                }
                // attack
                if (step == 2)
                {
                    if (V2_dxy.Y > 0 && currentAction != ATTACKING)
                    {
                        currentAction = ATTACKING;

                    }
                    if (currentAction == ATTACKING && animation.HasPlayedOnce())
                    {
                        step++;
                        currentAction = JUMPING;

                    }
                }
                // done attacking
                if (step == 3)
                {
                    if (V2_dxy.Y == 0) step++;
                }
                // land
                if (step == 4)
                {
                    step = 0;
                    left = right = isJumping = false;
                }

                // update animation
                animation.Update();
            }
        }
    }
}
