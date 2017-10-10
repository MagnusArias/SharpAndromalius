using MainGame.Control;
using MainGame.Maps.Tiles;
using MainGame.Objects;
using MainGame.Objects.Enemies;
using MainGame.Objects.Items;
using MainGame.Objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MainGame.Maps.Level2
{
    class Level_2 : GameState
    {
        private const String TILESET = "/Game/Src/Map/Level2/tileset.png";
	    private const String LEVEL = "/Game/Src/Map/Level2/level.map";
	    private const String BACKGROUND = "/Game/Src/Assets/tlo.png";

	    private Background back;
        
        private Player player;
        private TileMap tileMap;
        private List<Rectangle> rec_tb;
        private List<Enemy> enemies;
        private List<Fireball> fireballs;
        private List<P_Player> energyParticles;
        private List<Item> items;

        private HUD hud;
        private Teleport teleport;
        private DebugInfo debug;

        private int eventCount = 0;
        private Boolean eventStart;
        private Boolean eventDead;
        private Boolean eventFinish;

        private Boolean blockInput = false;

        public Level_2(GameStateManager gsm) : base(gsm)
        {
            Init();
        }

        public override void Init()
        {
            back = new Background(BACKGROUND, 0.5);

            // tilemap
            tileMap = new TileMap(30);
            tileMap.LoadTiles(TILESET);
            tileMap.LoadMap(LEVEL);
            tileMap.SetPosition(0, 0);
            tileMap.SetTween(0.025);

            //player
            player = new Player(tileMap);
            player.SetPosition(300, 1115);
            fireballs = new List<Fireball>();
            items = new List<Item>();
            //takie ladne zielone intro
            eventStart = true;
            rec_tb = new List<Rectangle>();
            EventStart();

            //wrogowie
            enemies = new List<Enemy>();

            // energy particle
            energyParticles = new List<P_Player>();

            teleport = new Teleport(tileMap);
            teleport.SetPosition(3760, 1250);

            // init player
            player.Init(enemies, energyParticles, items);
            PopulateEnemies();

            debug = new DebugInfo(tileMap, player);
            hud = new HUD(tileMap);
            hud.Init(player);
        }

        private void PopulateEnemies()
        {
            enemies.Clear();
            E_Skeleton e_s;
            E_Ghost e_g;

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


            tileMap.SetPosition(
                    GlobalVariables.WIDTH / 2 - player.GetX() - (70 * player.SetViewLeftRight()),
                    GlobalVariables.HEIGHT / 2 - player.GetY() - (150 * player.SetViewDown()) + 10
            );


            tileMap.Update();
            tileMap.FixBounds();

            for (int i = 0; i < fireballs.Count; i++)
            {
                Fireball f = (Fireball)fireballs[i];
                f.Update(enemies);
                if (f.IsHit())
                {
                    fireballs.Remove(i);
                    i--;
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy e = (Enemy)enemies[i];
                e.Update();
                if (e.ShouldRemove())
                {
                    enemies.Remove(i);
                    i--;
                }
            }
            teleport.Update();
            debug.Update();
        }

        public override void HandleInput()
        {
            Fireball fb;

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
                if (player.IsDashingReady()) player.SetDashing();
            }

            if (Keys.IsPressed(Keys.BUTTON1))
            {
                if (player.IsFireballReady())
                {
                    fb = new Fireball(tileMap, player.GetFacing());
                    fb.ShootFireball(player.GetX(), player.GetY(), player.GetFacing());
                    fireballs.Add(fb);

                    player.SetAttacking();
                    player.SetFireballCooldown(0);
                }
            }
        }

        public override void Draw(SpriteBatch g)
        {
            g.setColor(Color.Green);
            Rectangle r = new Rectangle(0, 0, GlobalVariables.WIDTH, GlobalVariables.HEIGHT);
            g.fill(r);

            back.Draw(g);

            player.Draw(g);
            teleport.Draw(g);

            for (int i = 0; i < fireballs.Count; i++)
            {
                fireballs[i].Draw(g);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(g);
            }

            tileMap.Draw(g);


            hud.Draw(g);
            debug.Draw(g);

            g.setColor(Color.Black);
            for (int i = 0; i < rec_tb.Count; i++)
            {
                g.fill(rec_tb[i]);
            }
        }

        private void Reset()
        {
            player.Reset();
            player.SetPosition(180, 1115);
            PopulateEnemies();
            blockInput = true;
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
                rec_tb.Add(new Rectangle(0, 0, GlobalVariables.WIDTH, GlobalVariables.HEIGHT / 2));
                rec_tb.Add(new Rectangle(0, 0, GlobalVariables.WIDTH / 2, GlobalVariables.HEIGHT));
                rec_tb.Add(new Rectangle(0, GlobalVariables.HEIGHT / 2, GlobalVariables.WIDTH, GlobalVariables.HEIGHT / 2));
                rec_tb.Add(new Rectangle(GlobalVariables.WIDTH / 2, 0, GlobalVariables.WIDTH / 2, GlobalVariables.HEIGHT));
            }
            if (eventCount > 1 && eventCount < 60)
            { 
                rec_tb[0].Height -= 4;
                rec_tb[1].Width -= 6;
                rec_tb[2].y += 4;
                rec_tb[3].x += 6;
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
                rec_tb.Add(new Rectangle(GlobalVariables.WIDTH / 2, GlobalVariables.HEIGHT / 2, 0, 0));
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
            if (eventCount == 1)
            {
                player.SetTeleporting(true);
                player.Stop();
            }
            else if (eventCount == 5)
            {
                rec_tb.Clear();
                rec_tb.Add(new Rectangle(GlobalVariables.WIDTH / 2, GlobalVariables.HEIGHT / 2, 0, 0));
            }
            else if (eventCount > 10)
            {
                rec_tb[0].x -= 6;
                rec_tb[0].y -= 4;
                rec_tb[0].width += 12;
                rec_tb[0].height += 8;
            }
            if (eventCount == 20)
            {
                gsm.SetState(GameStateManager.LEVEL1);
            }

        }
    }
}
