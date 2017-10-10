using MainGame.Control;
using MainGame.Maps.Tiles;
using MainGame.Objects;
using MainGame.Objects.Enemies;
using MainGame.Objects.Items;
using MainGame.Objects.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;


namespace MainGame.Maps.Level1
{
    class Level_1 : GameState
    {

        private const String TILESet = "/Game/Src/Map/Level1/tileSet.png";
	    private const String LEVEL = "/Game/Src/Map/Level1/level1.map";
	    private const String BACKGROUND = "/Game/Src/AsSets/tlo.png";

	    private Background back;
        private ArrayList rec_tb;
        private Player player;
        private TileMap tileMap;
        private ArrayList enemies;
        private ArrayList fireballs;
        private ArrayList energyParticles;
        private ArrayList items;
        private HUD hud;
        private Teleport teleport;

        private E_Boss eb;
        private DebugInfo debug;

        private int eventCount = 0;
        private Boolean eventStart;
        private Boolean eventDead;
        private Boolean eventFinish;
        private Boolean blockInput = false;

        public Level_1(GameStateManager gsm) : base(gsm)
        {
            Init();
            blockInput = false;
        }

        public override void Init()
        {
            back = new Background(BACKGROUND, 0.5);

            tileMap = new TileMap(30);
            tileMap.LoadTiles(TILESet);
            tileMap.LoadMap(LEVEL);
            tileMap.SetPosition(0, 0);
            tileMap.SetTween(0.025);

            player = new Player(tileMap);
            player.SetPosition(180, 1115);

            items = new ArrayList();
            PutHereItems();

            enemies = new ArrayList();
            PopulateEnemies();

            fireballs = new ArrayList();
            energyParticles = new ArrayList();

            teleport = new Teleport(tileMap);
            teleport.SetPosition(3100, 2300);

            player.Init(enemies, energyParticles, items);

            debug = new DebugInfo(tileMap, player);
            hud = new HUD(tileMap);
            hud.Init(player);

            eventStart = true;
            rec_tb = new ArrayList();
            eventStart();
        }

        private void PutHereItems()
        {
            items.Clear();
            I_DJump i_dj;
            I_Sword i_s;
            I_Dash i_d;
            I_Fireball i_fb;

            i_dj = new I_DJump(tileMap, player); // pierwsza instancja do umieszczenia na mapie
            i_dj.SetPosition(272, 1530);
            items.Add(i_dj);

            i_d = new I_Dash(tileMap, player);
            i_d.SetPosition(3080, 1000);
            items.Add(i_d);

            i_s = new I_Sword(tileMap, player);
            i_s.SetPosition(1000, 1500);
            items.Add(i_s);

            i_fb = new I_Fireball(tileMap, player);
            i_fb.SetPosition(210, 2140);
            items.Add(i_fb);


            i_dj = new I_DJump(tileMap, player); // druga instancja do wyswietlania w hud
            i_dj.SetPosition(0, 0);
            items.Add(i_dj);

            i_d = new I_Dash(tileMap, player);
            i_d.SetPosition(0, 0);
            items.Add(i_d);

            i_s = new I_Sword(tileMap, player);
            i_s.SetPosition(0, 0);
            items.Add(i_s);

            i_fb = new I_Fireball(tileMap, player);
            i_fb.SetPosition(0, 0);
            items.Add(i_fb);
        }

        private void PopulateEnemies()
        {
            enemies.Clear();
            E_Skeleton es;
            E_Ghost eg;

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(660, 1175);
            enemies.add(es);

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(1035, 1118);
            enemies.add(es);

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(808, 1118);
            enemies.add(es);

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(340, 200);
            enemies.add(es);

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(1764, 1088);
            enemies.add(es);


            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1464, 1088);
            enemies.add(eg);

            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1956, 1088);
            enemies.add(eg);

            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1720, 2250);
            enemies.add(eg);



            eb = new E_Boss(tileMap, player);
            eb.SetPosition(2550, 1750);
            enemies.add(eb);
        }

        public override void Update()
        {
            HandleInput();

            if (teleport.Contains(player))
            {
                eventFinish = blockInput = true;
            }

            if (player.GetHealth() == 0 || player.GetY() > tileMap.GetHeight()) { eventDead = blockInput = true; }

            if (eventStart) eventStart();
            if (eventDead) eventDead();
            if (eventFinish) eventFinish();

            back.SetPosition(tileMap.GetX(), tileMap.GetY());

            player.Update();

            if (player.GetX() > 2240 && player.GetY() > 1530 && player.GetY() < 1870)
            {
                tileMap.SetPosition(
                    GlobalVariables.WIDTH / 2 - player.GetX() - 200,
                    GlobalVariables.HEIGHT / 2 - player.GetY() + 100
                );
            }
            else
            {
                tileMap.SetPosition(
                        GlobalVariables.WIDTH / 2 - player.GetX() - (70 * player.SetViewLeftRight()),
                        GlobalVariables.HEIGHT / 2 - player.GetY() - (150 * player.SetViewDown()) + 10
                );

            }

            tileMap.Update();
            tileMap.FixBounds();

            for (int i = 0; i < fireballs.Count; i++)
            {
                Fireball f = fireballs[i];
                f.Update(enemies);
                if (f.IsHit())
                {
                    fireballs.Remove(i);
                    i--;
                }
            }

            for (int i = 0; i < items.size(); i++)
            {
                ItemParent e = items.get(i);
                e.update();
                if (e.shouldRemove())
                {
                    items.remove(i);
                    i--;
                }
            }

            for (int i = 0; i < enemies.size(); i++)
            {
                Enemy e = enemies.get(i);
                e.update();
                if (e.shouldRemove())
                {
                    enemies.remove(i);
                    i--;
                }
            }
            teleport.update();
            debug.update();
        }

        public override void HandleInput()
        {
            FireBall fb;

            if (!blockInput)
            {
                player.SetJumping(Keys.keyState[Keys.UP]);
                player.SetLeft(Keys.keyState[Keys.LEFT]);
                player.SetRight(Keys.keyState[Keys.RIGHT]);
                player.SetDown(Keys.keyState[Keys.DOWN]);

                if (Keys.isPressed(Keys.BUTTON3)) player.SetAttacking();
                if (Keys.isPressed(Keys.BUTTON4)) debug.SetReady();
                if (Keys.isPressed(Keys.ENTER)) Reset();

                if (Keys.isPressed(Keys.ESCAPE)) gsm.SetPaused(true);

                if (Keys.isPressed(Keys.BUTTON2))
                {
                    if (player.isDashingReady() && player.getSkill(1)) player.SetDashing();
                }

                if (Keys.isPressed(Keys.BUTTON1))
                {
                    if (player.isFireballReady() && player.getSkill(3))
                    {
                        fb = new FireBall(tileMap, player.getFacing());
                        fb.shootFireball(player.GetX(), player.GetY(), player.getFacing());
                        fireballs.add(fb);

                        player.SetAttacking();
                        player.SetFireballCooldown(0);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch g)
        {
            g.SetColor(java.awt.Color.GREEN);
            Rectangle r = new Rectangle(0, 0, GlobalVariables.WIDTH, GlobalVariables.HEIGHT);
            g.fill(r);

            back.draw(g);

            player.draw(g);
            teleport.draw(g);

            for (int i = 0; i < fireballs.size(); i++) fireballs.get(i).draw(g);

            for (int i = 0; i < enemies.size(); i++) enemies.get(i).draw(g);

            tileMap.draw(g);

            hud.draw(g);
            for (int i = 0; i < items.size(); i++) items.get(i).draw(g);




            debug.draw(g);
            eb.drawHPBar(g);

            g.SetColor(java.awt.Color.BLACK);
            for (int i = 0; i < rec_tb.size(); i++)
            {
                g.fill(rec_tb.get(i));
            }
        }

        private void Reset()
        {
            player.Reset();
            player.SetPosition(180, 1115);
            player.SetSkill(666, false);
            PopulateEnemies();
            PutHereItems();
            blockInput = false;
            eventCount = 0;
            tileMap.SetShaking(false, 0);
            eventStart = true;
            eventStart();
        }

        private void eventStart()
        {
            eventCount++;
            if (eventCount == 1)
            {
                rec_tb.clear();
                rec_tb.add(new Rectangle(0, 0, GlobalVariables.WIDTH, GlobalVariables.HEIGHT / 2));
                rec_tb.add(new Rectangle(0, 0, GlobalVariables.WIDTH / 2, GlobalVariables.HEIGHT));
                rec_tb.add(new Rectangle(0, GlobalVariables.HEIGHT / 2, GlobalVariables.WIDTH, GlobalVariables.HEIGHT / 2));
                rec_tb.add(new Rectangle(GlobalVariables.WIDTH / 2, 0, GlobalVariables.WIDTH / 2, GlobalVariables.HEIGHT));
            }
            if (eventCount > 1 && eventCount < 60)
            {
                rec_tb.get(0).height -= 4;
                rec_tb.get(1).width -= 6;
                rec_tb.get(2).y += 4;
                rec_tb.get(3).x += 6;
            }
            if (eventCount == 60)
            {
                eventStart = false;
                eventCount = 0;
                rec_tb.clear();
            }
        }

        private void eventDead()
        {
            eventCount++;
            if (eventCount == 1)
            {
                player.SetDead();
                player.stop();
            }
            if (eventCount == 60)
            {
                rec_tb.clear();
                rec_tb.add(new Rectangle(
                    GlobalVariables.WIDTH / 2, GlobalVariables.HEIGHT / 2, 0, 0));
            }
            else if (eventCount > 60)
            {
                rec_tb.get(0).x -= 6;
                rec_tb.get(0).y -= 4;
                rec_tb.get(0).width += 12;
                rec_tb.get(0).height += 8;
            }
            if (eventCount >= 120)
            {
                if (player.getHealth() == 0)
                {
                    gsm.SetState(GameStateManager.MENUSTATE);
                }
                else
                {
                    eventDead = blockInput = false;
                    eventCount = 0;
                    Reset();
                }
            }
        }

        private void eventFinish()
        {
            eventCount++;
            if (eventCount == 30)
            {
                player.SetTeleporting(true);
                player.stop();
            }
            else if (eventCount == 45)
            {
                rec_tb.clear();
                rec_tb.add(new Rectangle(
                        GlobalVariables.WIDTH / 2, GlobalVariables.HEIGHT / 2, 0, 0));
            }
            else if (eventCount > 60)
            {
                rec_tb.get(0).x -= 6;
                rec_tb.get(0).y -= 4;
                rec_tb.get(0).width += 12;
                rec_tb.get(0).height += 8;
            }
            if (eventCount == 120)
            {

                gsm.SetState(GameStateManager.LEVEL2);
            }

        }
    }
}
