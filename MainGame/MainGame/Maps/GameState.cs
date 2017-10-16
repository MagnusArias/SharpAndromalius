using MainGame.Control;
using MainGame.Maps.Tiles;
using MainGame.Objects;
using MainGame.Objects.Enemies;
using MainGame.Objects.Items;
using MainGame.Objects.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MainGame.Maps
{
    abstract class GameState
    {
        protected GameStateManager gsm;
        protected KeyboardState keyEvent;

        protected Player player;
        public Vector2 newPlayerPos;

        protected TileMap tileMap;
        protected HUD hud;
        protected Teleport teleport;
        protected DebugInfo debug;
        protected Background back;

        protected List<Rectangle> rec_tb;

        protected List<Enemy> enemies;
        protected E_Skeleton es;
        protected E_Ghost eg;
        protected E_Boss eb;

        protected List<Fireball> fireballs;
        protected List<P_Player> energyParticles;

        protected List<Item> items;
        protected I_DJump i_dj;
        protected I_Sword i_s;
        protected I_Dash i_d;
        protected I_Fireball i_fb;

        protected int eventCount = 0;
        protected Boolean eventStart;
        protected Boolean eventDead;
        protected Boolean eventFinish;
        protected Boolean blockInput;

        public GameState(GameStateManager gsm) => this.gsm = gsm;

        public void Init(String background, String tileset, String map, Vector2 player_pos, Vector2 teleport_pos)
        {
            blockInput = false;
            back = new Background(background, 0.5f);

            tileMap = new TileMap(30);
            tileMap.LoadTiles(tileset);
            tileMap.LoadMap(map);
            tileMap.SetPosition(0, 0);
            tileMap.SetTween(0.025f);

            teleport = new Teleport(tileMap);
            teleport.SetPosition(teleport_pos.X, teleport_pos.Y);

            eventStart = true;

            rec_tb = new List<Rectangle>();
            items = new List<Item>();
            fireballs = new List<Fireball>();
            enemies = new List<Enemy>();
            energyParticles = new List<P_Player>();

            player = new Player(tileMap);
            newPlayerPos = player_pos;
            player.SetPosition(player_pos.X, player_pos.Y);
            player.Init(enemies, energyParticles, items);

            debug = new DebugInfo(tileMap, player);
            hud = new HUD(tileMap);
            hud.Init(player);

            PlaceEnemies();
        }

        public void PlaceEnemies() => enemies.Clear();

        public void PlaceItems() => items.Clear();

        public virtual void _Update(GameTime gt)
        {
            HandleInput();

            if (teleport.Contains(player)) eventFinish = blockInput = true;

            if (player.GetHealth() == 0 || player.GetY() > tileMap.GetHeight()) eventDead = blockInput = true;

            if (eventStart) EventStart();
            if (eventDead) EventDead();
            if (eventFinish) EventFinish(GameStateManager.LEVEL2);

            back.SetPosition(tileMap.GetX(), tileMap.GetY());

            player.Update();

            tileMap.SetPosition(
                   GlobalVariables.GAME_WINDOW_WIDTH / 2 - player.GetX() - (70 * player.GetViewLeftRight()),
                   GlobalVariables.GAME_WINDOW_HEIGHT / 2 - player.GetY() - (150 * player.GetViewDown()) + 10
           );


            tileMap.Update();
            tileMap.FixBounds();

            for (int i = 0; i < fireballs.Count; i++)
            {
                Fireball f = fireballs[i];
                f.Update(enemies);
                if (f.IsHit()) fireballs.RemoveAt(i--);

            }

            for (int i = 0; i < items.Count; i++)
            {
                Item e = items[i];
                e.Update();
                if (e.ShouldRemove()) items.RemoveAt(i--);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy e = enemies[i];
                e.Update();
                if (e.ShouldRemove()) enemies.RemoveAt(i--);
            }
            teleport.Update();
            debug.Update();
        }

        public virtual void _Draw(SpriteBatch g)
        {
            back.Draw(g);
            player.Draw(g);

            for (int i = 0; i < fireballs.Count; i++) fireballs[i].Draw(g);
            for (int i = 0; i < items.Count; i++) items[i].Draw(g);
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].Draw(g);
                if (enemies[i] is E_Boss) enemies[i].DrawHP(g);
            }

            teleport.Draw(g);
            tileMap.Draw(g);

            debug.Draw(g);
            hud.Draw(g);
        }

        public virtual void HandleInput()
        {
            Fireball fb;

            if (!blockInput)
            {
                player.SetJumping(MyKeys.keyState[MyKeys._UP]);
                player.SetLeft(MyKeys.keyState[MyKeys._LEFT]);
                player.SetRight(MyKeys.keyState[MyKeys._RIGHT]);
                player.SetDown(MyKeys.keyState[MyKeys._DOWN]);

                if (MyKeys.IsPressed(MyKeys._BUTTON3)) player.SetAttacking();
                if (MyKeys.IsPressed(MyKeys._BUTTON4)) debug.SetReady();
                //if (Keys.IsPressed(Keys.ENTER)) Reset();

                if (MyKeys.IsPressed(MyKeys._ESCAPE)) gsm.SetPaused(true);

                if (MyKeys.IsPressed(MyKeys._BUTTON2) && player.IsDashingReady()) player.SetDashing();


                if (MyKeys.IsPressed(MyKeys._BUTTON1))
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
        }

        public void Reset(Vector2 player_pos)
        {
            player.Reset();
            player.SetPosition(player_pos.X, player_pos.Y);
            PlaceEnemies();
            blockInput = true;
            eventCount = 0;
            tileMap.SetShaking(false, 0);
            eventStart = true;
            EventStart();
        }

        public void EventStart()
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
                /*rec_tb[0].Height -= 4;
                rec_tb[1].Width -= 6;
                rec_tb[2].Y += 4;
                rec_tb[3].X += 6;*/
            }
            if (eventCount == 60)
            {
                eventStart = false;
                eventCount = 0;
                rec_tb.Clear();
            }
        }

        public void EventDead()
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
               /* rec_tb[0].X -= 6;
                rec_tb[0].Y -= 4;
                rec_tb[0].Width += 12;
                rec_tb[0].Height += 8;*/
            }
            if (eventCount >= 120)
            {
                if (player.GetHealth() == 0) gsm.SetState(GameStateManager.MENUSTATE);
                else
                {
                    eventDead = blockInput = false;
                    eventCount = 0;
                    Reset(newPlayerPos);
                }
            }
        }

        public void EventFinish(int newState)
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
               /* rec_tb[0].X -= 6;
                rec_tb[0].Y -= 4;
                rec_tb[0].Width += 12;
                rec_tb[0].Height += 8;*/
            }
            if (eventCount == 120) gsm.SetState(newState);
        }

        public virtual void Select() { }
    }
}
