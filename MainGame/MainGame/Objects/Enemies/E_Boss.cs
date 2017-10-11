using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace MainGame.Objects.Enemies
{
    class E_Boss : Enemy
    {
	    
        private Boolean active;
        private int eventCount;

        private int tick;
        private double a;
        private double b;
        private int currentAction;

        public E_Boss(TileMap tm, Player p) : base(tm)
        {
            player = p;

            health = 50;
            maxHealth = 50;

            lastBreath = 5;
            width = 57 * 2;
            height = 88 * 2;

            eventCount = 0;
            cwidth = 57 * 2;
            cheight = 88 * 2;

            damage = 50;

            playerCatch = false;

            moveSpeed = 0.1f;
            maxSpeed = 1.8f;
            stopSpeed = 0.1f;

            facingRight = false;

            sprites = GlobalVariables.E_Boss;

            animation.SetFrames(sprites);
            animation.SetDelay(4);

            Random r = new Random();
            b = r.Next() * 0.06 + 0.07;

            tick = 0;

            try
            {
                hpBar = ImageIO.read(getClass().getResourceAsStream(BOSSHPBAR));
                hpBarOutline = ImageIO.read(getClass().getResourceAsStream(BOSSBAROUTLINE));
            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
                Environment.Exit(0);
            }
        }

        public new void Update()
        {
            tick++;
            eventCount++;

            percentHealth = health / maxHealth;

            if (eventCount % 50 == 0)
            {

            }
            if (dead)
            {
                lastBreath--;
                if (lastBreath <= 0) remove = true;
            }

            facingRight = player.GetX() >= V2_xy.X;


            if (player.GetX() > 2240 && player.GetY() > 1530 && player.GetY() < 1870)
            {
                playerCatch = true;

                if (Math.Abs(player.GetX() - V2_xy.X) < 250)
                { // jestesmy blisko bossa, to sie oddala od nas

                    if (!facingRight)
                    {
                        V2_dxy.X += moveSpeed;
                        if (V2_dxy.X > maxSpeed)
                        {
                            V2_dxy.X = maxSpeed;
                        }
                    }
                    else
                    {
                        V2_dxy.X -= moveSpeed;
                        if (V2_dxy.X < -maxSpeed)
                        {
                            V2_dxy.X = -maxSpeed;
                        }
                    }
                }
                else if (Math.Abs(player.GetX() - V2_xy.X) > 250 && Math.Abs(player.GetX() - V2_xy.X) < 400)
                { // oddalimy sie, ale nas goni
                    if (facingRight)
                    {
                        V2_dxy.X += moveSpeed;
                        if (V2_dxy.X > maxSpeed)
                        {
                            V2_dxy.X = maxSpeed;
                        }
                    }
                    else
                    {
                        V2_dxy.X -= moveSpeed;
                        if (V2_dxy.X < -maxSpeed)
                        {
                            V2_dxy.X = -maxSpeed;
                        }
                    }
                }
            }
            else if (Math.Abs(player.GetX() - V2_xy.X) > 400 && !playerCatch)
            { // ale jezeli jestesmy daleko, to opuszcza poscig
                if (V2_dxy.X > 0)
                {
                    V2_dxy.X -= stopSpeed;
                    if (V2_dxy.X < 0)
                    {
                        V2_dxy.X = 0;
                    }
                }
                else if (V2_dxy.X < 0)
                {
                    V2_dxy.X += stopSpeed;
                    if (V2_dxy.X > 0)
                    {
                        V2_dxy.X = 0;
                    }
                }
            }
            else playerCatch = false;

            V2_xy.X += V2_dxy.X;
            V2_xy.Y = (float)Math.Sin(b * tick) + V2_xy.Y;

            // update animation
            animation.Update();
        }
    }
}
