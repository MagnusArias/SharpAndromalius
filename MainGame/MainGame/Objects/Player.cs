#pragma warning disable CS0618 // Type or member is obsolete
using System;

using System.Collections.Generic;

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
        private int boost;

        // rozne timery, do immoratala i do dasha
        private long flinchCount;
        private int dashTimer;
        private int dashCooldown;
        private int maxDashCooldown;

        // ANIMACJE
        private List<Texture2D[]> sprites;
        private List<Texture2D[]> armorSprites;
        private List<Texture2D[]> robeSprites;
        private List<Texture2D[]> swordSprites;


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
            collisionWidth = 20;
            collisionHeight = 45;

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

        public Boolean IsFireballReady() => fireballCooldown >= 100 && (falling || jumping || !left || !right) && !knockback && !dashing;

        public Boolean IsDashingReady() => dashCooldown >= 200 && (falling || jumping || left || right) && !knockback;

        public void SetJumping(Boolean b)
        {
            if (knockback) return;
            if (b && !jumping && falling && !alreadyDoubleJump) doubleJump = true;
            jumping = b;
        }

        public void SetDead()
        {
            health = 0;
            Stop();
        }

        public int GetHealth() => health;

        public int GetMaxHealth() => maxHealth;

        public int GetMana() => fireballCooldown;

        public int GetMaxMana() => maxFireballCooldown;

        public int GetStamina() => dashCooldown;

        public int GetMaxStamina() => maxDashCooldown;

        public Boolean GetFacing() => facingRight;

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

        public void Stop() => left = right = jumping = flinching = dashing = squat = attack = hi_attack = low_attack = false;

        public void SetTeleporting(Boolean b) => teleporting = b;

        public override void GetNextPosition()
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
                        energyParticles.Add(new P_Player(tileMap, V2_xy.X, V2_xy.Y + collisionHeight / 4, P_Player.LEFT));
                    }
                }
                else
                {
                    V2_dxy.X = -moveSpeed * (10 - dashTimer * 0.04f);
                    for (int i = 0; i < 6; i++)
                    {
                        energyParticles.Add(new P_Player(tileMap, V2_xy.X, V2_xy.Y + collisionHeight / 4, P_Player.RIGHT));
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
                    energyParticles.Add(new P_Player(tileMap, V2_xy.X, V2_xy.Y + collisionHeight / 4, P_Player.DOWN));
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

            armorAnimation.SetFrames(armorSprites[currentAction]);
            armorAnimation.SetDelay(SPRITEDELAYS[currentAction]);

            robeAnimation.SetFrames(robeSprites[currentAction]);
            robeAnimation.SetDelay(SPRITEDELAYS[currentAction]);

            swordAnimation.SetFrames(swordSprites[currentAction]);
            swordAnimation.SetDelay(swordSPRITEDELAYS[currentAction]);

            width = FRAMEWIDTHS[currentAction];
            height = FRAMEHEIGHTS[currentAction];
        }

        public int GetViewLeftRight()
        {
            if (facingRight) return 1;
            else return -1;
        }

        public int GetViewDown()
        {
            if (squat) return 1;
            else return 0;
        }

        public override void Hit(int damage)
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

        public override void Update()
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

        public override void Draw(SpriteBatch g)
        {
            SpriteEffects spriteEffect;
            if (facingRight) spriteEffect = SpriteEffects.None;
            else spriteEffect = SpriteEffects.FlipVertically;

            SetMapPosition();

            for (int i = 0; i < energyParticles.Count; i++) energyParticles[i].Draw(g);

            if (flinching && !knockback && flinchCount % 10 < 5) return;

            Animation arm = new Animation();
            if (skill_doubleJump && !skill_dash) arm = armorAnimation;
            else if (skill_dash) arm = robeAnimation;

            // drawing body 
            g.Draw(
                 bodyAnimation.GetImage(),                                                                       // image
                 new Vector2(V2_xy.X + V2_mapxy.X - (width / 2.0f), V2_xy.Y + V2_mapxy.Y - (height / 2.0f)),     // position
                 null,                                                                                           // destination rectangle
                 new Rectangle(0, 0, bodyAnimation.GetImage().Width, bodyAnimation.GetImage().Height),           // source - if null draws
                 new Vector2(width / 2.0f, height / 2.0f),                                                       // origin
                 0.0f,                                                                                           // rotation
                 new Vector2(1, 1),                                                                              // scale
                 Color.White,                                                                                    // color
                 spriteEffect,                                                                                   // effects
                 0.0f                                                                                            // layerDepth
                 );

            // drawing armor
            g.Draw(
                 arm.GetImage(),                                                                                 // image
                 new Vector2(V2_xy.X + V2_mapxy.X - (width / 2.0f), V2_xy.Y + V2_mapxy.Y - (height / 2.0f)),     // position
                 null,                                                                                           // destination rectangle
                 new Rectangle(0, 0, arm.GetImage().Width, arm.GetImage().Height),                               // source - if null draws
                 new Vector2(width / 2.0f, height / 2.0f),                                                       // origin
                 0.0f,                                                                                           // rotation
                 new Vector2(1, 1),                                                                              // scale
                 Color.White,                                                                                    // color
                 spriteEffect,                                                                                   // effects
                 0.0f                                                                                            // layerDepth
                 );

            // draw sword
            if (!fireballShooted)
            {
                if (attack || low_attack || hi_attack)
                {
                    float new_y = V2_xy.Y + V2_mapxy.Y - (height / 2.0f);
                    if (squat) new_y += 10;

                    if (GetSkill(2)) g.Draw(
                        swordAnimation.GetImage(),                                                                      // image
                        new Vector2(V2_xy.X + V2_mapxy.X - (width / 2.0f), new_y),                                      // position
                        null,                                                                                           // destination rectangle
                        new Rectangle(0, 0, swordAnimation.GetImage().Width, swordAnimation.GetImage().Height),         // source - if null draws
                        new Vector2(width / 2.0f, height / 2.0f),                                                       // origin
                        0.0f,                                                                                           // rotation
                        new Vector2(1, 1),                                                                              // scale
                        Color.White,                                                                                    // color
                        spriteEffect,                                                                                   // effects
                        0.0f                                                                                            // layerDepth
                        );
                }
            }

            /*
            if (GlobalVariables.DEBUG_READY)
            {
                Rectangle r = GetRectangle();
                r.X += (int)V2_mapxy.X;
                r.Y += (int)V2_mapxy.Y;
                g.Draw(r);
            }
            */
        }

        private void LoadGraphics()
        {
            LoadSubImages(GlobalVariables.Player_Main, sprites);
            LoadSubImages(GlobalVariables.Armor_Red, armorSprites);
            LoadSubImages(GlobalVariables.Skill_Sword, swordSprites);
            LoadSubImages(GlobalVariables.Robe_Black, robeSprites);
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

        public Color[] GetSubImage(Color[] colorData, int width, Rectangle rec)
        {
            Color[] color = new Color[rec.Width * rec.Height];
            for (int x = 0; x < rec.Width; x++)
            {
                for (int y = 0; y < rec.Height; y++)
                {
                    color[x + y * rec.Width] = colorData[x + rec.X + (y + rec.Y) * width];
                }
            }
            return color;
        }

        public void LoadSubImages(Texture2D sourceSpritesheet, List<Texture2D[]> destinationSprites)
        {
            int count = 0;
            Color[] imageData = new Color[sourceSpritesheet.Width * sourceSpritesheet.Height];
            Texture2D subImage;
            Rectangle sourceRec;

            destinationSprites = new List<Texture2D[]>();

            for (int i = 0; i < this.NUMFRAMES.Length; i++)
            {
                Texture2D[] bi = new Texture2D[this.NUMFRAMES[i]];

                for (int j = 0; j < this.NUMFRAMES[i]; j++)
                {
                    sourceRec = new Rectangle(j * this.FRAMEWIDTHS[i], count, this.FRAMEWIDTHS[i], this.FRAMEHEIGHTS[i]);
                    Color[] imagePiece = this.GetSubImage(imageData, sourceSpritesheet.Width, sourceRec);
                    subImage = new Texture2D(Game1.Instance.GraphicsDevice, sourceRec.Width, sourceRec.Height);
                    subImage.SetData<Color>(imagePiece);
                    bi[j] = subImage;
                }

                destinationSprites.Add(bi);
                count += this.FRAMEHEIGHTS[i];
            }
        }
    }
}
