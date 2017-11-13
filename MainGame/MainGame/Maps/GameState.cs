#region Using statements and file description
//-----------------------------------------------------------------------------
// Animation.cs
//
// Originally created: 10.07.2016, 20:43 by Przemysław Dębiec
// 
// Main controller for animations
//-----------------------------------------------------------------------------

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
#endregion
namespace MainGame.Maps
{
    abstract class GameState
    {
        protected GameStateManager gsm;
        protected KeyboardState keyEvent;

        protected Player player;
        public Vector2 newPlayerPos;

        protected TileMap tileMap;
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

        protected Boolean blockInput;

        public GameState(GameStateManager gsm) => this.gsm = gsm;

        public void Init(Texture2D background, Texture2D tileset, String map, Vector2 player_pos, Vector2 teleport_pos)
        {
            blockInput = false;
            back = new Background(background, 0.5f);

            tileMap = new TileMap(30);
            tileMap.LoadTiles(tileset);
            tileMap.LoadMap(map);
            tileMap.SetPosition(0, 0);
            tileMap.SetTween(0.025f);

            items = new List<Item>();
            fireballs = new List<Fireball>();
            enemies = new List<Enemy>();
            energyParticles = new List<P_Player>();

            player = new Player(tileMap);
            newPlayerPos = player_pos;
            player.SetPosition(player_pos.X, player_pos.Y);
            player.Init(enemies, energyParticles, items);
        }


        public virtual void _Update(GameTime gt)
        {
            HandleInput();

            player.Update();

            tileMap.SetPosition(
                   GlobalVariables.GAME_WINDOW_WIDTH / 2 - player.GetX(), //- (70 * player.GetViewLeftRight()),
                   GlobalVariables.GAME_WINDOW_HEIGHT / 2 - player.GetY()// - (150 * player.GetViewDown()) + 10
           );

            tileMap.Update();
            tileMap.FixBounds();
        }

        public virtual void _Draw(SpriteBatch g)
        {
            back.Draw(g);
            player.Draw(g);
            tileMap.Draw(g);
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
               // if (MyKeys.IsPressed(MyKeys._BUTTON4)) debug.SetReady();
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
            //PlaceEnemies();
            blockInput = true;
            //eventCount = 0;
            tileMap.SetShaking(false, 0);
        }
       
        public virtual void Select() { }
    }
}
