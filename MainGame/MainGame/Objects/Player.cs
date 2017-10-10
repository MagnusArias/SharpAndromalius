using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Control;
using MainGame.Objects.Enemies;
using MainGame.Maps.Tiles;
using MainGame.Objects.Items;
using MainGame.Objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MainGame.Objects
{
    class Player : ParentObject
    {
        private List<Enemy> enemies;
        private List<P_Player> energyParticles;
        private List<Item> items;

        public const String PLAYERSPRITEMAP = "/Game/Src/Assets/player-spritemap.png";
	    public const String ARMORSPRITEMAP = "/Game/Src/Assets/armor05-spritemap.png";
	    public const String ROBESPRITEMAP = "/Game/Src/Assets/robe02-spritemap.png";
	    public const String SWORDSPRITEMAP = "/Game/Src/Assets/sword-slash.png";
	
	    // dostepne ruchy
	    protected Boolean hi_attack;
        protected Boolean attack;
        protected Boolean low_attack;
        private Boolean doubleJump;
        private Boolean alreadyDoubleJump;
        private float doubleJumpStart;
        private Boolean teleporting;
        private Boolean dashing;
        public Boolean knockback;
        private Boolean flinching;

        private Boolean skill_doubleJump;
        private Boolean skill_sword;
        private Boolean skill_dash;
        private Boolean skill_fireball;

        //stuff do fireballa
        public Boolean fireballShooted;
        public int fireballCooldown;
        public int maxFireballCooldown;

        // dynks do animacji
        protected int currentAction;

        //takie drobne rzeczy gracza
        private int health;
        private int maxHealth;
        private int damage;

        private int boost;

        // rozne timery, do immoratala i do dasha
        private long flinchCount;
        private int dashTimer;
        private int dashCooldown;
        private int maxDashCooldown;

        // ANIMACJE
        private List<Texture2D>[] sprites;
        private List<Texture2D>[] armorSprites;
        private List<Texture2D>[] robeSprites;
        private List<Texture2D>[] swordSprites;


        private readonly int[] NUMFRAMES = { 1, 1, 1, 8, 4, 4, 4, 1, 8, 1, 6 };
        private readonly int[] FRAMEWIDTHS = { 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46 };
        private readonly int[] FRAMEHEIGHTS = { 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };
        private readonly int[] SPRITEDELAYS = { -1, -1, -1, 5, 5, 5, 5, -1, 4, -1, 5 };

        private readonly int[] swordNUMFRAMES = { 0, 0, 0, 0, 5, 5, 5, 0, 0, 0, 0 };
        private readonly int[] swordFRAMEWIDTHS = { 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60 };
        private readonly int[] swordFRAMEHEIGHTS = { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 };
        private readonly int[] swordSPRITEDELAYS = { -1, -1, -1, -1, 5, 5, 5, -1, -1, -1, -1 };

        //klasy animacji
        protected Animation bodyAnimation = new Animation();
        protected Animation armorAnimation = new Animation();
        protected Animation robeAnimation = new Animation();
        protected Animation swordAnimation = new Animation();

        private Rectangle attackRect;
        private Rectangle aur;
        private Rectangle alr;

        // akcje animacji, spojrz na obrazek
        private const int STAND = 0;
        private const int JUMPING = 1;
        private const int FALLING = 2;
        private const int WALKING = 3;
        private const int ATTACK = 4;
        private const int HIGH_ATTACK = 5;
        private const int LOW_ATTACK = 6;
        private const int SQUAT = 7;
        private const int KNOCKBACK = 8;
        private const int DEAD = 9;
        private const int TELEPORTING = 10;

        public Player(TileMap tm) : base(tm)
        {
            boost = 1;
            skill_doubleJump = skill_sword = skill_dash = skill_fireball = false;

            attackRect = new Rectangle(0, 0, 20, 10);

            alr = new Rectangle((int)V2_xy.X - 15, (int)V2_xy.Y - 45, 45, 45);

            //rozmiary gracza, do wyswietlenia
            width = 45;
            height = 45;

            // rozmiary collision boxa
            cwidth = 20;
            cheight = 45;

            //atrybuty dasha i fireballa
            maxFireballCooldown = 100;
            fireballShooted = false;
            SetFireballCooldown(maxFireballCooldown);
            dashCooldown = 190;
            maxDashCooldown = 200;

            //artybuty poruszania sie
            SetParameters(boost);

            facingRight = true;
            attack = false;
            hi_attack = false;
            low_attack = false;

            damage = 2;
            health = maxHealth = 50;

            LoadGraphics();

            energyParticles = new List<P_Player>();
            SetAnimation(STAND);
        }

        public void Init(List<Enemy> enemies, List<P_Player> energyParticles, List<Item> items)
        {
            this.enemies = enemies;
            this.energyParticles = energyParticles;
            this.items = items;
        }

        public void SetFireballCooldown(int x)
        {
            fireballCooldown = x;
            fireballShooted = true;
        }

        private void SetParameters(float boost)
        {
            moveSpeed = 0.5f * boost;
            maxSpeed = 2.8f * boost;
            stopSpeed = 1.0f * boost;
            fallSpeed = 0.2f * boost;
            maxFallSpeed = 9.0f * boost;
            jumpStart = -5.5f * boost;
            stopJumpSpeed = 0.3f * boost;
            doubleJumpStart = -5f * boost;
        }

        public Boolean IsFireballReady()
        {
            return fireballCooldown >= 100 && (falling || jumping || !left || !right) && !knockback && !dashing;
        }

        public Boolean IsDashingReady()
        {
            return dashCooldown >= 200 && (falling || jumping || left || right) && !knockback;
        }

        public void SetJumping(Boolean b)
        {
            if (knockback) return;


            if (b && !jumping && falling && !alreadyDoubleJump)
            {
                doubleJump = true;
            }
            jumping = b;

        }

        public void SetDead()
        {
            health = 0;
            Stop();
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetMana()
        {
            return fireballCooldown;
        }

        public int GetMaxMana()
        {
            return maxFireballCooldown;
        }

        public int GetStamina()
        {
            return dashCooldown;
        }

        public int GetMaxStamina()
        {
            return maxDashCooldown;
        }

        public Boolean GetFacing()
        {
            return facingRight;
        }

        public void SetSkill(int number, Boolean state)
        {
            switch (number)
            {
                case 0:
                    skill_doubleJump = state;
                    break;

                case 1:
                    skill_dash = state;
                    break;

                case 2:
                    skill_sword = state;
                    break;

                case 3:
                    skill_fireball = state;
                    break;

                case 666:
                    skill_doubleJump = state;
                    skill_dash = state;
                    skill_sword = state;
                    skill_fireball = state;
                    break;

                default:
                    break;
            }
        }

        public Boolean GetSkill(int number)
        {
            switch (number)
            {
                case 0:
                    return skill_doubleJump;

                case 1:
                    return skill_dash;

                case 2:
                    return skill_sword;

                case 3:
                    return skill_fireball;

                default: return false;
            }
        }

        public void SetAttacking()
        {
            if (knockback) return;
            if (dashing) return;

            if (skill_sword)
            {
                if ((jumping || falling) && (!attack || !hi_attack) && !squat)
                {
                    hi_attack = true;
                    attack = false;
                    low_attack = false;
                }
                else if (squat && (!attack || !low_attack) && !jumping && !falling)
                {
                    hi_attack = false;
                    attack = false;
                    low_attack = true;
                }
                else if (!squat && !jumping && !falling && !attack && !low_attack && !hi_attack)
                {
                    hi_attack = false;
                    attack = true;
                    low_attack = false;
                }
            }
        }

        public void SetDashing()
        {
            if (knockback) return;
            if (skill_dash)
            {
                if (!attack && !hi_attack && !low_attack && !dashing && (left || right || jumping || falling) && !squat)
                {
                    dashing = true;
                    dashTimer = 0;
                }
            }
        }

        public void Reset()
        {
            facingRight = true;
            currentAction = -1;
            health = maxHealth;
            Stop();
        }

        public void Stop()
        {
            left = right = jumping = flinching = dashing = squat = attack = hi_attack = low_attack = false;
        }

        public void SetTeleporting(Boolean b)
        {
            teleporting = b;
        }

        private void GetNextPosition()
        {

            if (knockback)
            {
                V2_dxy.Y += fallSpeed * 2.0f;
                if (!falling) knockback = false;
                return;
            }


            if (left)
            {
                V2_dxy.X -= moveSpeed;
                if (V2_dxy.X < -maxSpeed)
                {
                    V2_dxy.X = -maxSpeed;
                }
            }
            else if (right)
            {
                V2_dxy.X += moveSpeed;
                if (V2_dxy.X > maxSpeed)
                {
                    V2_dxy.X = maxSpeed;
                }
            }
            else
            {
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

            if ((attack || hi_attack || low_attack || dashing) && !(jumping || falling))
            {
                V2_dxy.X = 0;
            }

            if (jumping && !falling)
            {
                V2_dxy.Y = jumpStart;
                falling = true;

            }

            if (dashing)
            {
                dashCooldown = 0;
                dashTimer++;
                if (facingRight)
                {
                    V2_dxy.X = moveSpeed * (10 - dashTimer * 0.04f);
                    for (int i = 0; i < 6; i++)
                    {
                        energyParticles.Add(new P_Player(tileMap, V2_xy.X, V2_xy.Y + cheight / 4, P_Player.LEFT));
                    }
                }
                else
                {
                    V2_dxy.X = -moveSpeed * (10 - dashTimer * 0.04f);
                    for (int i = 0; i < 6; i++)
                    {
                        energyParticles.Add(new P_Player(tileMap, V2_xy.X, V2_xy.Y + cheight / 4, P_Player.RIGHT));
                    }
                }
            }

            if (doubleJump && skill_doubleJump)
            {
                V2_dxy.Y = doubleJumpStart;
                alreadyDoubleJump = true;
                doubleJump = false;
                for (int i = 0; i < 6; i++)
                {
                    energyParticles.Add(new P_Player(tileMap, V2_xy.X, V2_xy.Y + cheight / 4, P_Player.DOWN));
                }
            }

            if (!falling) alreadyDoubleJump = false;

            if (falling)
            {
                V2_dxy.Y += fallSpeed;
                if (V2_dxy.Y < 0 && !jumping) V2_dxy.Y += stopJumpSpeed;
                if (V2_dxy.Y > maxFallSpeed) V2_dxy.Y = maxFallSpeed;
            }
        }

        private void SetAnimation(int i)
        {
            currentAction = i;

            bodyAnimation.SetFrames(sprites[currentAction]);
            bodyAnimation.SetDelay(SPRITEDELAYS[currentAction]);

            armorAnimation.SetFrames(armorSprites.get(currentAction));
            armorAnimation.SetDelay(SPRITEDELAYS[currentAction]);

            robeAnimation.SetFrames(robeSprites.get(currentAction));
            robeAnimation.SetDelay(SPRITEDELAYS[currentAction]);

            swordAnimation.SetFrames(swordSprites.get(currentAction));
            swordAnimation.SetDelay(swordSPRITEDELAYS[currentAction]);

            width = FRAMEWIDTHS[currentAction];
            height = FRAMEHEIGHTS[currentAction];
        }

        public int SetViewLeftRight()
        {
            if (facingRight) return 1;
            else return -1;
        }

        public int SetViewDown()
        {
            if (squat) return 1;
            else return 0;
        }

        public void Hit(int damage)
        {
            if (flinching) return;

            Stop();
            health -= damage;
            if (health < 0) health = 0;
            flinching = true;
            flinchCount = 0;

            if (facingRight) V2_dxy.X = -1;
            else V2_dxy.X = 1;
            V2_dxy.Y = -3;
            knockback = true;
            falling = true;
            jumping = false;
        }

        public void Update()
        {

            GetNextPosition();
            CheckTileMapCollision();
            SetPosition(xy_temp.X, xy_temp.Y);

            if (GlobalVariables.DEBUG_READY)
            {
                SetSkill(666, true);
                boost = 2;
            }
            else boost = 1;
            SetParameters(boost);

            if (teleporting) energyParticles.Add(new P_Player(tileMap, V2_xy.X, V2_xy.Y, P_Player.UP));

            if (V2_dxy.X == 0) V2_xy.X = (int)V2_xy.X;
            if (fireballCooldown > 15) fireballShooted = false;

            if (fireballCooldown >= 100) fireballCooldown = 100;
            else fireballCooldown++;

            if (dashCooldown >= 200) dashCooldown = 200;
            else dashCooldown++;

            if (dashing && dashTimer > 40) dashing = false;

            if (flinching)
            {
                flinchCount++;
                if (flinchCount > 120)
                {
                    flinching = false;
                }
            }

            for (int i = 0; i < energyParticles.Count; i++)
            {
                energyParticles[i].Update();
                if (energyParticles[i].ShouldRemove())
                {
                    energyParticles.RemoveAt(i);
                    i--;
                }
            }

            if (currentAction == ATTACK || currentAction == HIGH_ATTACK || currentAction == LOW_ATTACK)
            {
                if (bodyAnimation.HasPlayedOnce())
                {
                    hi_attack = false;
                    attack = false;
                    low_attack = false;
                }
            }

            if (currentAction == KNOCKBACK)
            {

                if (!bodyAnimation.HasPlayedOnce())
                {
                    knockback = true;
                    if (V2_dxy.Y == 0) V2_dxy.X = 0;

                }
            }

            CheckEnemyCollision();
            CheckItemCollision();

            CheckAnimations();

            bodyAnimation.Update();
            armorAnimation.Update();
            robeAnimation.Update();
            swordAnimation.Update();

            // ustawienie kierunku
            if (!attack && !hi_attack && !low_attack && !knockback && !dashing)
            {
                if (right) facingRight = true;
                if (left) facingRight = false;
            }
        }

        public void Draw(SpriteBatch g)
        {

            SetMapPosition();


            for (int i = 0; i < energyParticles.Count; i++)
            {
                energyParticles[i].Draw(g);
            }
            if (flinching && !knockback)
            {
                if (flinchCount % 10 < 5) return;
            }

            if (facingRight)
            {
                g.Draw(bodyAnimation.GetImage(), V2_xy, source, Color.White, 0.0f, Origin, 1.0f, SpriteEffects.None, 0.0f);
                g.Draw(bodyAnimation.GetImage(), (int)(x + xmap - width / 2), (int)(y + ymap - height / 2), null);

                if (skill_doubleJump && !skill_dash) g.Draw(armorAnimation.GetImage(), (int)(x + xmap - width / 2), (int)(y + ymap - height / 2), null);
                else if (skill_dash) g.Draw(robeAnimation.GetImage(), (int)(x + xmap - width / 2), (int)(y + ymap - height / 2), null);


                if (!fireballShooted)
                {
                    if (attack || low_attack || hi_attack)
                    {
                        double new_y;

                        if (squat)
                        {
                            new_y = V2_xy.Y + V2_mapxy.Y - (height / 2) + 10;
                        }
                        else
                        {
                            new_y = V2_xy.Y + V2_mapxy.Y - height / 2;
                        }

                        if (GetSkill(2)) g.Draw(swordAnimation.GetImage(), (int)(x + xmap - width / 2), (int)(new_y), null);
                    }
                }
            }
            else
            {

                g.Draw(bodyAnimation.GetImage(), (int)(x + xmap - width / 2 + width), (int)(y + ymap - height / 2), -width, height, null);
                if (skill_doubleJump && !skill_dash) g.Draw(armorAnimation.GetImage(), (int)(x + xmap - width / 2 + width), (int)(y + ymap - height / 2), -width, height, null);
                else if (skill_dash) g.Draw(robeAnimation.GetImage(), (int)(x + xmap - width / 2 + width), (int)(y + ymap - height / 2), -width, height, null);


                if (!fireballShooted)
                {
                    if (attack || low_attack || hi_attack)
                    {
                        double new_y;

                        if (squat)
                        {
                            new_y = V2_xy.Y + V2_mapxy.Y - (height / 2) + 10;
                        }
                        else
                        {
                            new_y = V2_xy.Y + V2_mapxy.Y - height / 2;
                        }

                        if (GetSkill(2))
                        {
                            g.Draw(swordAnimation.GetImage(), (int)(x + xmap - width / 2 + width), (int)(new_y), -60, 30, null);
                        }
                    }
                }
            }

            if (GlobalVariables.DEBUG_READY)
            {
                Rectangle r = GetRectangle();
                r.X += (int)V2_mapxy.X;
                r.Y += (int)V2_mapxy.Y;
                g.Draw(r);
            }
        }

        private void LoadGraphics()
        {
            try
            {

                Texture2D spritesheet = ImageIO.read(getClass().getResourceAsStream(PLAYERSPRITEMAP));
                Texture2D spritesheet2 = ImageIO.read(getClass().getResourceAsStream(ARMORSPRITEMAP));
                Texture2D spritesheet3 = ImageIO.read(getClass().getResourceAsStream(SWORDSPRITEMAP));
                Texture2D spritesheet4 = ImageIO.read(getClass().getResourceAsStream(ROBESPRITEMAP));

                //tutaj częśc dla człowieczka
                int count = 0;
                sprites = new List<Texture2D>[8];
                for (int i = 0; i < NUMFRAMES.Length; i++)
                {
                    Texture2D[] bi = new Texture2D[NUMFRAMES[i]];
                    for (int j = 0; j < NUMFRAMES[i]; j++) { bi[j] = spritesheet.getSubimage(j * FRAMEWIDTHS[i], count, FRAMEWIDTHS[i], FRAMEHEIGHTS[i]); }
                    sprites.Add(bi);
                    
                    count += FRAMEHEIGHTS[i];
                }

                // tutaj część dla zbroi
                count = 0;
                armorSprites = new List<Texture2D>[8];
                for (int i = 0; i < NUMFRAMES.Length; i++)
                {
                    Texture2D[] bi = new Texture2D[NUMFRAMES[i]];
                    for (int j = 0; j < NUMFRAMES[i]; j++) { bi[j] = spritesheet2.getSubimage(j * FRAMEWIDTHS[i], count, FRAMEWIDTHS[i], FRAMEHEIGHTS[i]); }
                    armorSprites.Add(bi);
                    count += FRAMEHEIGHTS[i];
                }

                // tutaj czesc dla miecza
                count = 0;
                swordSprites = new List<Texture2D>[8];
                for (int i = 0; i < swordNUMFRAMES.Length; i++)
                {
                    Texture2D[] bi = new Texture2D[swordNUMFRAMES[i]];
                    for (int j = 0; j < swordNUMFRAMES[i]; j++) { bi[j] = spritesheet3.getSubimage(j * swordFRAMEWIDTHS[i], count, swordFRAMEWIDTHS[i], swordFRAMEHEIGHTS[i]); }
                    swordSprites.Add(bi);
                    count += swordFRAMEHEIGHTS[i];
                }

                count = 0;
                robeSprites = new List<Texture2D>[8];
                for (int i = 0; i < NUMFRAMES.Length; i++)
                {
                    Texture2D[] bi = new Texture2D[NUMFRAMES[i]];
                    for (int j = 0; j < NUMFRAMES[i]; j++) { bi[j] = spritesheet4.getSubimage(j * FRAMEWIDTHS[i], count, FRAMEWIDTHS[i], FRAMEHEIGHTS[i]); }
                    robeSprites.Add(bi);
                    count += FRAMEHEIGHTS[i];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nStackTrace ---\n{0}", e.StackTrace);
                Environment.Exit(0);
            }
        }
        
        private void CheckItemCollision()
        {
            for (int i = 0; i < items.Count; i++)
            {

                Item e = items[i];

                if (Intersects(e))
                {
                    if (e is I_DJump)
                    {
                        SetSkill(0, true);
                        e.CanBeRemoved();
                    }

                    if (e is I_Dash)
                    {
                        SetSkill(1, true);
                        e.CanBeRemoved();
                    }

                    if (e is I_Sword)
                    {
                        SetSkill(2, true);
                        e.CanBeRemoved();
                    }

                    if (e is I_Fireball)
                    {
                        SetSkill(3, true);
                        e.CanBeRemoved();
                    }
                }
            }
        }
    
        private void CheckEnemyCollision()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy e = enemies[i];

                if (currentAction == HIGH_ATTACK)
                {
                    if (e.Intersects(attackRect))
                    {
                        e.Hit(damage);
                    }
                }
                if (currentAction == ATTACK)
                {
                    if (e.Intersects(attackRect))
                    {
                        e.Hit(damage);
                    }
                }
                else if (currentAction == LOW_ATTACK)
                {
                    if (e.Intersects(attackRect))
                    {
                        e.Hit(damage);
                    }
                }

                if (!e.IsDead() && Intersects(e))
                {
                    Hit(e.GetDamage());
                }

            }
        }

        private void CheckAnimations()
        {
            if (teleporting)
            {
                if (currentAction != TELEPORTING)
                {
                    SetAnimation(TELEPORTING);
                }
            }
            else if (knockback)
            {
                if (currentAction != KNOCKBACK)
                {
                    SetAnimation(KNOCKBACK);
                }
            }
            else if (health == 0)
            {
                if (currentAction != DEAD)
                {
                    SetAnimation(DEAD);
                }
            }
            else if (hi_attack)
            {
                if (currentAction != HIGH_ATTACK)
                {
                    SetAnimation(HIGH_ATTACK);
                    attackRect.Y = (int)V2_xy.Y - 16;
                    if (facingRight) attackRect.X = (int)V2_xy.X + 10;
                    else attackRect.X = (int)V2_xy.X - 35;
                }
            }
            else if (attack)
            {
                if (currentAction != ATTACK)
                {
                    SetAnimation(ATTACK);
                    attackRect.Y = (int)V2_xy.Y - 16;
                    if (facingRight) attackRect.X = (int)V2_xy.X + 10;
                    else attackRect.X = (int)V2_xy.X - 35;
                }
            }
            else if (low_attack)
            {
                if (currentAction != LOW_ATTACK)
                {
                    SetAnimation(LOW_ATTACK);
                    attackRect.Y = (int)V2_xy.Y;
                    if (facingRight) attackRect.X = (int)V2_xy.X + 10;
                    else attackRect.X = (int)V2_xy.X - 35;
                }
            }
            else if (V2_dxy.Y < 0)
            {
                if (currentAction != JUMPING)
                {
                    SetAnimation(JUMPING);
                }
            }
            else if (V2_dxy.Y > 0)
            {
                if (currentAction != FALLING)
                {
                    SetAnimation(FALLING);
                }
            }
            else if (dashing)
            {
                if (currentAction != WALKING)
                {
                    SetAnimation(WALKING);
                }
            }
            else if (left || right)
            {
                if (currentAction != WALKING)
                {
                    SetAnimation(WALKING);
                }
            }
            else if (squat)
            {
                if (currentAction != SQUAT)
                {
                    SetAnimation(SQUAT);
                }
            }
            else if (currentAction != STAND)
            {
                SetAnimation(STAND);
            }
        }
    }
}
