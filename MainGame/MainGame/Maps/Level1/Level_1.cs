using MainGame.Control;
using MainGame.Maps.Tiles;
using MainGame.Objects;
using MainGame.Objects.Enemies;
using MainGame.Objects.Items;
using MainGame.Objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MainGame.Maps.Level1
{
    class Level_1 : GameState
    {

        private const String TILESet = "/Game/Src/Map/Level1/tileSet.png";
	    private const String LEVEL = "/Game/Src/Map/Level1/level1.map";
	    private const String BACKGROUND = "/Game/Src/AsSets/tlo.png";

	    private Background back;
        private List<Rectangle> rec_tb;
        private Player player;
        private TileMap tileMap;
        private List<Enemy> enemies;
        private List<Fireball> fireballs;
        private List<P_Player> energyParticles;
        private List<Item> items;
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
            tileMap.SetTween(0.025f);

            player = new Player(tileMap);
            player.SetPosition(180, 1115);

            items = new List<Item>();
            PutHereItems();

            enemies = new List<Enemy>();
            PopulateEnemies();

            fireballs = new List<Fireball>();
            energyParticles = new List<P_Player>();

            teleport = new Teleport(tileMap);
            teleport.SetPosition(3100, 2300);

            player.Init(enemies, energyParticles, items);

            debug = new DebugInfo(tileMap, player);
            hud = new HUD(tileMap);
            hud.Init(player);

            eventStart = true;
            rec_tb = new List<Rectangle>();
            EventStart();
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
            enemies.Add(es);

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(1035, 1118);
            enemies.Add(es);

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(808, 1118);
            enemies.Add(es);

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(340, 200);
            enemies.Add(es);

            es = new E_Skeleton(tileMap, player);
            es.SetPosition(1764, 1088);
            enemies.Add(es);


            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1464, 1088);
            enemies.Add(eg);

            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1956, 1088);
            enemies.Add(eg);

            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1720, 2250);
            enemies.Add(eg);



            eb = new E_Boss(tileMap, player);
            eb.SetPosition(2550, 1750);
            enemies.Add(eb);
        }

        public override void Update()
        {
            HandleInput();

            if (teleport.Contains(player))
            {
                eventFinish = blockInput = true;
            }

            if (player.GetHealth() == 0 || player.GetY() > tileMap.GetHeight()) { eventDead = blockInput = true; }

            if (eventStart) EventStart();
            if (eventDead) EventDead();
            if (eventFinish) EventFinish();

            back.SetPosition(tileMap.GetX(), tileMap.GetY());

            player.Update();

            if (player.GetX() > 2240 && player.GetY() > 1530 && player.GetY() < 1870)
            {
                tileMap.SetPosition(
                    GlobalVariables.GAME_WINDOW_WIDTH / 2 - player.GetX() - 200,
                    GlobalVariables.GAME_WINDOW_HEIGHT / 2 - player.GetY() + 100
                );
            }
            else
            {
                tileMap.SetPosition(
                        GlobalVariables.GAME_WINDOW_WIDTH / 2 - player.GetX() - (70 * player.SetViewLeftRight()),
                        GlobalVariables.GAME_WINDOW_HEIGHT / 2 - player.GetY() - (150 * player.SetViewDown()) + 10
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
                    fireballs.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                Item e = items[i];
                e.Update();
                if (e.ShouldRemove())
                {
                    items.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy e = enemies[i];
                e.Update();
                if (e.ShouldRemove())
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }
            teleport.Update();
            debug.Update();
        }

        public override void HandleInput()
        {
            Fireball fb;

            if (!blockInput)
            {
                player.SetJumping(Keys.keyState[Keys.UP]);
                player.SetLeft(Keys.keyState[Keys.LEFT]);
                player.SetRight(Keys.keyState[Keys.RIGHT]);
                player.SetDown(Keys.keyState[Keys.DOWN]);

                if (Keys.IsPressed(Keys.BUTTON3)) player.SetAttacking();
                if (Keys.IsPressed(Keys.BUTTON4)) debug.SetReady();
                if (Keys.IsPressed(Keys.ENTER)) Reset();

                if (Keys.IsPressed(Keys.ESCAPE)) gsm.SetPaused(true);

                if (Keys.IsPressed(Keys.BUTTON2))
                {
                    if (player.IsDashingReady() && player.GetSkill(1)) player.SetDashing();
                }

                if (Keys.IsPressed(Keys.BUTTON1))
                {
                    if (player.IsFireballReady() && player.GetSkill(3))
                    {
                        fb = new Fireball(tileMap, player.GetFacing());
                        fb.ShootFireball(player.GetX(), player.GetY(), player.GetFacing());
                        fireballs.Add(fb);

                        player.SetAttacking();
                        player.SetFireballCooldown(0);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch g)
        {
            g.SetColor(java.awt.Color.GREEN);
            Rectangle r = new Rectangle(0, 0, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT);
            g.fill(r);

            back.Draw(g);

            player.Draw(g);
            teleport.Draw(g);

            for (int i = 0; i < fireballs.Count; i++) fireballs[i].Draw(g);

            for (int i = 0; i < enemies.Count; i++) enemies[i].Draw(g);

            tileMap.Draw(g);

            hud.Draw(g);
            for (int i = 0; i < items.Count; i++) items[i].Draw(g);

            debug.Draw(g);
            eb.DrawHPBar(g);

            g.SetColor(java.awt.Color.BLACK);
            for (int i = 0; i < rec_tb.Count; i++)
            {
                g.fill(rec_tb[i]);
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
            EventStart();
        }

        private void EventStart()
        {
            eventCount++;
            if (eventCount == 1)
            {
                rec_tb.Clear();
                rec_tb.Add(new Rectangle(0, 0, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT / 2));
                rec_tb.Add(new Rectangle(0, 0, GlobalVariables.GAME_WINDOW_WIDTH / 2, GlobalVariables.GAME_WINDOW_HEIGHT));
                rec_tb.Add(new Rectangle(0, GlobalVariables.GAME_WINDOW_HEIGHT / 2, GlobalVariables.GAME_WINDOW_WIDTH, GlobalVariables.GAME_WINDOW_HEIGHT / 2));
                rec_tb.Add(new Rectangle(GlobalVariables.GAME_WINDOW_WIDTH / 2, 0, GlobalVariables.GAME_WINDOW_WIDTH / 2, GlobalVariables.GAME_WINDOW_HEIGHT));
            }
            if (eventCount > 1 && eventCount < 60)
            {
                rec_tb[0].Height -= 4;
                rec_tb[1].Width -= 6;
                rec_tb[2].Y += 4;
                rec_tb[3].X += 6;
            }
            if (eventCount == 60)
            {
                eventStart = false;
                eventCount = 0;
                rec_tb.Clear();
            }
        }

        private void EventDead()
        {
            eventCount++;
            if (eventCount == 1)
            {
                player.SetDead();
                player.Stop();
            }
            if (eventCount == 60)
            {
                rec_tb.Clear();
                rec_tb.Add(new Rectangle(GlobalVariables.GAME_WINDOW_WIDTH / 2, GlobalVariables.GAME_WINDOW_HEIGHT / 2, 0, 0));
            }
            else if (eventCount > 60)
            {
                rec_tb[0].X -= 6;
                rec_tb[0].Y -= 4;
                rec_tb[0].Width += 12;
                rec_tb[0].Height += 8;
            }
            if (eventCount >= 120)
            {
                if (player.GetHealth() == 0)
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

        private void EventFinish()
        {
            eventCount++;
            if (eventCount == 30)
            {
                player.SetTeleporting(true);
                player.Stop();
            }
            else if (eventCount == 45)
            {
                rec_tb.Clear();
                rec_tb.Add(new Rectangle(GlobalVariables.GAME_WINDOW_WIDTH / 2, GlobalVariables.GAME_WINDOW_HEIGHT / 2, 0, 0));
            }
            else if (eventCount > 60)
            {
                rec_tb[0].X -= 6;
                rec_tb[0].Y -= 4;
                rec_tb[0].Width += 12;
                rec_tb[0].Height += 8;
            }
            if (eventCount == 120)
            {
                gsm.SetState(GameStateManager.LEVEL2);
            }
        }
    }
}
