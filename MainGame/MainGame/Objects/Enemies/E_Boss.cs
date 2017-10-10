using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Control;
using MainGame.Objects;
using MainGame.Maps.Tiles;
using Microsoft.Xna.Framework.Graphics;

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
        private Boolean playerCatch;
        private int szerokosc;

        private double hp_max;
        private double hp;
        private double maxHp;

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

            moveSpeed = 0.1;
            maxSpeed = 1.8;
            stopSpeed = 0.1;

            facingRight = false;

            sprites = GlobalVariables.Enemy_Boss1[0];

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

        public void Update()
        {
            tick++;
            eventCount++;

            hp = (double)health;
            maxHp = (double)maxHealth;

            hp_max = hp / maxHp;

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
                        dx += moveSpeed;
                        if (dx > maxSpeed)
                        {
                            dx = maxSpeed;
                        }
                    }
                    else
                    {
                        dx -= moveSpeed;
                        if (dx < -maxSpeed)
                        {
                            dx = -maxSpeed;
                        }
                    }
                }
                else if (Math.Abs(player.GetX() - V2_xy.X) > 250 && Math.Abs(player.GetX() - V2_xy.X) < 400)
                { // oddalimy sie, ale nas goni
                    if (facingRight)
                    {
                        dx += moveSpeed;
                        if (dx > maxSpeed)
                        {
                            dx = maxSpeed;
                        }
                    }
                    else
                    {
                        dx -= moveSpeed;
                        if (dx < -maxSpeed)
                        {
                            dx = -maxSpeed;
                        }
                    }
                }
            }
            else if (Math.Abs(player.GetX() - V2_xy.X) > 400 && !playerCatch)
            { // ale jezeli jestesmy daleko, to opuszcza poscig
                if (dx > 0)
                {
                    dx -= stopSpeed;
                    if (dx < 0)
                    {
                        dx = 0;

                    }
                }
                else if (dx < 0)
                {
                    dx += stopSpeed;
                    if (dx > 0)
                    {
                        dx = 0;
                    }
                }
            }
            else playerCatch = false;

            V2_xy.X += (float)dx;
            V2_xy.Y = (float)Math.Sin(b * tick) + V2_xy.Y;

            // update animation
            animation.Update();

        }

        public void Draw(SpriteBatch g)
        {
            base.Draw(g);
        }

        public void DrawHPBar(SpriteBatch g)
        {
            if (dead) return;

            if (playerCatch)
            {

                g.Draw(hpBar, 72, 122, (int)((GlobalVariables.WIDTH - 142) * hp_max), 14, null);
                g.Draw(hpBarOutline, 0 + 70, 0 + 120, GlobalVariables.WIDTH - 140, 16, null);

                g.DrawString("Andromalius", GlobalVariables.WIDTH / 2 - 20, 105);
            }
        }
    }
}
