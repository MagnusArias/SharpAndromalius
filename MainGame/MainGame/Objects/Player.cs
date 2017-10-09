using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.Control;
using MainGame.Objects.Enemies;
using MainGame.Maps.TileMap;
using MainGame.Objects.Items;
using MainGame.Objects.Projectiles;

namespace MainGame.Objects
{
    class Player : ParentObject
    {
        private ArrayList<Enemy> enemies;
        private ArrayList<EnergyParticle> energyParticles;
        private ArrayList<ItemParent> items;

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
        private double doubleJumpStart;
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
        private ArrayList<BufferedImage[]> sprites;
        private ArrayList<BufferedImage[]> armorSprites;
        private ArrayList<BufferedImage[]> robeSprites;
        private ArrayList<BufferedImage[]> swordSprites;


        private const int[] NUMFRAMES = { 1, 1, 1, 8, 4, 4, 4, 1, 8, 1, 6 };
        private const int[] FRAMEWIDTHS = { 46, 46, 46, 46, 46, 46, 46, 46, 46, 46, 46 };
        private const int[] FRAMEHEIGHTS = { 50, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };
        private const int[] SPRITEDELAYS = { -1, -1, -1, 5, 5, 5, 5, -1, 4, -1, 5 };

        private const int[] swordNUMFRAMES = { 0, 0, 0, 0, 5, 5, 5, 0, 0, 0, 0 };
        private const int[] swordFRAMEWIDTHS = { 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60 };
        private const int[] swordFRAMEHEIGHTS = { 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 };
        private const int[] swordSPRITEDELAYS = { -1, -1, -1, -1, 5, 5, 5, -1, -1, -1, -1 };

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

            attackRect = new Rectangle(0, 0, 0, 0);
            attackRect.width = 20;
            attackRect.height = 10;

            alr = new Rectangle((int)x - 15, (int)y - 45, 45, 45);

            //rozmiary gracza, do wyswietlenia
            width = 45;
            height = 45;

            // rozmiary collision boxa
            cwidth = 20;
            cheight = 45;

            //atrybuty dasha i fireballa
            maxFireballCooldown = 100;
            fireballShooted = false;
            setFireballCooldown(maxFireballCooldown);
            dashCooldown = 190;
            maxDashCooldown = 200;

            //artybuty poruszania sie
            setParameters(boost);

            facingRight = true;
            attack = false;
            hi_attack = false;
            low_attack = false;

            damage = 2;
            health = maxHealth = 50;

            loadGraphics();

            energyParticles = new ArrayList<EnergyParticle>();
            setAnimation(STAND);
        }

        public void init(ArrayList<Enemy> enemies, ArrayList<EnergyParticle> energyParticles, ArrayList<ItemParent> items)
        {
            this.enemies = enemies;
            this.energyParticles = energyParticles;
            this.items = items;
        }

        public void setFireballCooldown(int x)
        {
            fireballCooldown = x;
            fireballShooted = true;
        }

        private void setParameters(int boost)
        {
            moveSpeed = 0.5 * boost;
            maxSpeed = 2.8 * boost;
            stopSpeed = 1.0 * boost;
            fallSpeed = 0.2 * boost;
            maxFallSpeed = 9.0 * boost;
            jumpStart = -5.5 * boost;
            stopJumpSpeed = 0.3 * boost;
            doubleJumpStart = -5 * boost;
        }

        public Boolean isFireballReady()
        {
            return fireballCooldown >= 100 && (falling || jumping || !left || !right) && !knockback && !dashing;
        }

        public Boolean isDashingReady()
        {
            return dashCooldown >= 200 && (falling || jumping || left || right) && !knockback;
        }

        public void setJumping(Boolean b)
        {
            if (knockback) return;


            if (b && !jumping && falling && !alreadyDoubleJump)
            {
                doubleJump = true;
            }
            jumping = b;

        }

        public void setDead()
        {
            health = 0;
            stop();
        }

        public int getHealth()
        {
            return health;
        }
        public int getMaxHealth() { return maxHealth; }
        public int getMana()
        {
            return fireballCooldown;
        }
        public int getMaxMana() { return maxFireballCooldown; }
        public int getSta()
        {
            return dashCooldown;
        }
        public int getMaxSta() { return maxDashCooldown; }
        public Boolean getFacing()
        {
            return facingRight;
        }

        public void setSkill(int number, Boolean state)
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

        public Boolean getSkill(int number)
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

        public void setAttacking()
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

        public void setDashing()
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

        public void reset()
        {
            facingRight = true;
            currentAction = -1;
            health = maxHealth;
            stop();
        }

        public void stop()
        {
            left = right = jumping = flinching = dashing = squat = attack = hi_attack = low_attack = false;
        }

        public void setTeleporting(Boolean b) { teleporting = b; }

        private void getNextPosition()
        {

            if (knockback)
            {
                dy += fallSpeed * 2;
                if (!falling) knockback = false;
                return;
            }


            if (left)
            {
                dx -= moveSpeed;
                if (dx < -maxSpeed)
                {
                    dx = -maxSpeed;
                }
            }
            else if (right)
            {
                dx += moveSpeed;
                if (dx > maxSpeed)
                {
                    dx = maxSpeed;
                }
            }
            else
            {
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

            if ((attack || hi_attack || low_attack || dashing) && !(jumping || falling)) { dx = 0; }

            if (jumping && !falling)
            {
                dy = jumpStart;
                falling = true;

            }

            if (dashing)
            {
                dashCooldown = 0;
                dashTimer++;
                if (facingRight)
                {
                    dx = moveSpeed * (10 - dashTimer * 0.04);
                    for (int i = 0; i < 6; i++)
                    {
                        energyParticles.add(new EnergyParticle(tileMap, x, y + cheight / 4, EnergyParticle.LEFT));
                    }
                }
                else
                {
                    dx = -moveSpeed * (10 - dashTimer * 0.04);
                    for (int i = 0; i < 6; i++)
                    {
                        energyParticles.add(new EnergyParticle(tileMap, x, y + cheight / 4, EnergyParticle.RIGHT));
                    }
                }
            }

            if (doubleJump && skill_doubleJump)
            {
                dy = doubleJumpStart;
                alreadyDoubleJump = true;
                doubleJump = false;
                for (int i = 0; i < 6; i++)
                {
                    energyParticles.add(new EnergyParticle(tileMap, x, y + cheight / 4, EnergyParticle.DOWN));
                }
            }

            if (!falling) alreadyDoubleJump = false;

            if (falling)
            {
                dy += fallSpeed;
                if (dy < 0 && !jumping) dy += stopJumpSpeed;
                if (dy > maxFallSpeed) dy = maxFallSpeed;
            }
        }

        private void setAnimation(int i)
        {
            currentAction = i;

            bodyAnimation.setFrames(sprites.get(currentAction));
            bodyAnimation.setDelay(SPRITEDELAYS[currentAction]);

            armorAnimation.setFrames(armorSprites.get(currentAction));
            armorAnimation.setDelay(SPRITEDELAYS[currentAction]);

            robeAnimation.setFrames(robeSprites.get(currentAction));
            robeAnimation.setDelay(SPRITEDELAYS[currentAction]);

            swordAnimation.setFrames(swordSprites.get(currentAction));
            swordAnimation.setDelay(swordSPRITEDELAYS[currentAction]);

            width = FRAMEWIDTHS[currentAction];
            height = FRAMEHEIGHTS[currentAction];
        }

        public int setViewLeftRight()
        {
            if (facingRight) return 1;
            else return -1;
        }

        public int setViewDown()
        {
            if (squat) return 1;
            else return 0;
        }

        public void hit(int damage)
        {
            if (flinching) return;

            stop();
            health -= damage;
            if (health < 0) health = 0;
            flinching = true;
            flinchCount = 0;

            if (facingRight) dx = -1;
            else dx = 1;
            dy = -3;
            knockback = true;
            falling = true;
            jumping = false;
        }

        public void update()
        {

            getNextPosition();
            checkTileMapCollision();
            setPosition(xtemp, ytemp);

            if (DebugInfo.debugReady)
            {
                setSkill(666, true);
                boost = 2;
            }
            else boost = 1;
            setParameters(boost);

            if (teleporting) energyParticles.add(new EnergyParticle(tileMap, x, y, EnergyParticle.UP));

            if (dx == 0) x = (int)x;
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

            for (int i = 0; i < energyParticles.size(); i++)
            {
                energyParticles.get(i).update();
                if (energyParticles.get(i).shouldRemove())
                {
                    energyParticles.remove(i);
                    i--;
                }
            }

            if (currentAction == ATTACK || currentAction == HIGH_ATTACK || currentAction == LOW_ATTACK)
            {
                if (bodyAnimation.hasPlayedOnce())
                {
                    hi_attack = false;
                    attack = false;
                    low_attack = false;
                }
            }

            if (currentAction == KNOCKBACK)
            {

                if (!bodyAnimation.hasPlayedOnce())
                {
                    knockback = true;
                    if (dy == 0) dx = 0;

                }
            }

            checkEnemyCollision();
            checkItemCollision();

            checkAnimations();

            bodyAnimation.update();
            armorAnimation.update();
            robeAnimation.update();
            swordAnimation.update();

            // ustawienie kierunku
            if (!attack && !hi_attack && !low_attack && !knockback && !dashing)
            {
                if (right) facingRight = true;
                if (left) facingRight = false;
            }
        }


        public void draw(Graphics2D g)
        {

            setMapPosition();


            for (int i = 0; i < energyParticles.size(); i++)
            {
                energyParticles.get(i).draw(g);
            }
            if (flinching && !knockback)
            {
                if (flinchCount % 10 < 5) return;
            }

            if (facingRight)
            {
                g.drawImage(bodyAnimation.getImage(), (int)(x + xmap - width / 2), (int)(y + ymap - height / 2), null);

                if (skill_doubleJump && !skill_dash) g.drawImage(armorAnimation.getImage(), (int)(x + xmap - width / 2), (int)(y + ymap - height / 2), null);
                else if (skill_dash) g.drawImage(robeAnimation.getImage(), (int)(x + xmap - width / 2), (int)(y + ymap - height / 2), null);


                if (!fireballShooted)
                {
                    if (attack || low_attack || hi_attack)
                    {
                        double new_y;

                        if (squat)
                        {
                            new_y = y + ymap - (height / 2) + 10;
                        }
                        else
                        {
                            new_y = y + ymap - height / 2;
                        }

                        if (getSkill(2)) g.drawImage(swordAnimation.getImage(), (int)(x + xmap - width / 2), (int)(new_y), null);
                    }
                }
            }
            else
            {

                g.drawImage(bodyAnimation.getImage(), (int)(x + xmap - width / 2 + width), (int)(y + ymap - height / 2), -width, height, null);
                if (skill_doubleJump && !skill_dash) g.drawImage(armorAnimation.getImage(), (int)(x + xmap - width / 2 + width), (int)(y + ymap - height / 2), -width, height, null);
                else if (skill_dash) g.drawImage(robeAnimation.getImage(), (int)(x + xmap - width / 2 + width), (int)(y + ymap - height / 2), -width, height, null);


                if (!fireballShooted)
                {
                    if (attack || low_attack || hi_attack)
                    {
                        double new_y;

                        if (squat)
                        {
                            new_y = y + ymap - (height / 2) + 10;
                        }
                        else
                        {
                            new_y = y + ymap - height / 2;
                        }

                        if (getSkill(2)) g.drawImage(swordAnimation.getImage(), (int)(x + xmap - width / 2 + width), (int)(new_y), -60, 30, null);
                    }
                }
            }

            if (DebugInfo.debugReady)
            {
                Rectangle r = getRectangle();
                r.x += xmap;
                r.y += ymap;
                g.draw(r);
            }
        }

        private void loadGraphics()
        {
            try
            {

                BufferedImage spritesheet = ImageIO.read(getClass().getResourceAsStream(PLAYERSPRITEMAP));
                BufferedImage spritesheet2 = ImageIO.read(getClass().getResourceAsStream(ARMORSPRITEMAP));
                BufferedImage spritesheet3 = ImageIO.read(getClass().getResourceAsStream(SWORDSPRITEMAP));
                BufferedImage spritesheet4 = ImageIO.read(getClass().getResourceAsStream(ROBESPRITEMAP));

                //tutaj częśc dla człowieczka
                int count = 0;
                sprites = new ArrayList<BufferedImage[]>();
                for (int i = 0; i < NUMFRAMES.length; i++)
                {
                    BufferedImage[] bi = new BufferedImage[NUMFRAMES[i]];
                    for (int j = 0; j < NUMFRAMES[i]; j++) { bi[j] = spritesheet.getSubimage(j * FRAMEWIDTHS[i], count, FRAMEWIDTHS[i], FRAMEHEIGHTS[i]); }
                    sprites.add(bi);
                    count += FRAMEHEIGHTS[i];
                }

                // tutaj część dla zbroi
                count = 0;
                armorSprites = new ArrayList<BufferedImage[]>();
                for (int i = 0; i < NUMFRAMES.length; i++)
                {
                    BufferedImage[] bi = new BufferedImage[NUMFRAMES[i]];
                    for (int j = 0; j < NUMFRAMES[i]; j++) { bi[j] = spritesheet2.getSubimage(j * FRAMEWIDTHS[i], count, FRAMEWIDTHS[i], FRAMEHEIGHTS[i]); }
                    armorSprites.add(bi);
                    count += FRAMEHEIGHTS[i];
                }

                // tutaj czesc dla miecza
                count = 0;
                swordSprites = new ArrayList<BufferedImage[]>();
                for (int i = 0; i < swordNUMFRAMES.length; i++)
                {
                    BufferedImage[] bi = new BufferedImage[swordNUMFRAMES[i]];
                    for (int j = 0; j < swordNUMFRAMES[i]; j++) { bi[j] = spritesheet3.getSubimage(j * swordFRAMEWIDTHS[i], count, swordFRAMEWIDTHS[i], swordFRAMEHEIGHTS[i]); }
                    swordSprites.add(bi);
                    count += swordFRAMEHEIGHTS[i];
                }

                count = 0;
                robeSprites = new ArrayList<BufferedImage[]>();
                for (int i = 0; i < NUMFRAMES.length; i++)
                {
                    BufferedImage[] bi = new BufferedImage[NUMFRAMES[i]];
                    for (int j = 0; j < NUMFRAMES[i]; j++) { bi[j] = spritesheet4.getSubimage(j * FRAMEWIDTHS[i], count, FRAMEWIDTHS[i], FRAMEHEIGHTS[i]); }
                    robeSprites.add(bi);
                    count += FRAMEHEIGHTS[i];
                }
            }
            catch (Exception e)
            {
                e.printStackTrace();
                System.out.println("Error loading graphics for PLAYER.");
                System.exit(0);
            }
        }

        private void checkItemCollision()
        {
            for (int i = 0; i < items.size(); i++)
            {

                ItemParent e = items.get(i);

                if (intersects(e))
                {
                    if (e instanceof ItemDoubleJump) {
                setSkill(0, true);
                e.canBeRemoved();
            }
            if (e instanceof ItemDash) {
                setSkill(1, true);
                e.canBeRemoved();
            }
            if (e instanceof ItemSword) {
                setSkill(2, true);
                e.canBeRemoved();
            }
            if (e instanceof ItemFireball) {
                setSkill(3, true);
                e.canBeRemoved();
            }
        }
    
    private void checkEnemyCollision()
    {
        for (int i = 0; i < enemies.size(); i++)
        {

            Enemy e = enemies.get(i);

            if (currentAction == HIGH_ATTACK)
            {
                if (e.intersects(attackRect))
                {
                    e.hit(damage);
                }
            }
            if (currentAction == ATTACK)
            {
                if (e.intersects(attackRect))
                {
                    e.hit(damage);
                }
            }
            else if (currentAction == LOW_ATTACK)
            {
                if (e.intersects(attackRect))
                {
                    e.hit(damage);
                }
            }

            if (!e.isDead() && intersects(e))
            {
                hit(e.getDamage());
            }

        }
    }
        private void checkAnimations()
        {
            if (teleporting)
            {
                if (currentAction != TELEPORTING)
                {
                    setAnimation(TELEPORTING);
                }
            }
            else if (knockback)
            {
                if (currentAction != KNOCKBACK)
                {
                    setAnimation(KNOCKBACK);
                }
            }
            else if (health == 0)
            {
                if (currentAction != DEAD)
                {
                    setAnimation(DEAD);
                }
            }
            else if (hi_attack)
            {
                if (currentAction != HIGH_ATTACK)
                {
                    setAnimation(HIGH_ATTACK);
                    attackRect.y = (int)y - 16;
                    if (facingRight) attackRect.x = (int)x + 10;
                    else attackRect.x = (int)x - 35;
                }
            }
            else if (attack)
            {
                if (currentAction != ATTACK)
                {
                    setAnimation(ATTACK);
                    attackRect.y = (int)y - 16;
                    if (facingRight) attackRect.x = (int)x + 10;
                    else attackRect.x = (int)x - 35;
                }
            }
            else if (low_attack)
            {
                if (currentAction != LOW_ATTACK)
                {
                    setAnimation(LOW_ATTACK);
                    attackRect.y = (int)y;
                    if (facingRight) attackRect.x = (int)x + 10;
                    else attackRect.x = (int)x - 35;
                }
            }
            else if (dy < 0)
            {
                if (currentAction != JUMPING)
                {
                    setAnimation(JUMPING);
                }
            }
            else if (dy > 0)
            {
                if (currentAction != FALLING)
                {
                    setAnimation(FALLING);
                }
            }
            else if (dashing)
            {
                if (currentAction != WALKING)
                {
                    setAnimation(WALKING);
                }
            }
            else if (left || right)
            {
                if (currentAction != WALKING)
                {
                    setAnimation(WALKING);
                }
            }
            else if (squat)
            {
                if (currentAction != SQUAT)
                {
                    setAnimation(SQUAT);
                }
            }
            else if (currentAction != STAND)
            {
                setAnimation(STAND);
            }
        }
    }
}
