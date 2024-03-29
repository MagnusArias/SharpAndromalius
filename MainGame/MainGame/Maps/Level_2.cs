﻿using MainGame.Objects.Enemies;
using MainGame.Objects.Items;
using Microsoft.Xna.Framework;

namespace MainGame.Maps
{
    class Level_2 : GameState
    {
        public Level_2(GameStateManager gsm) : base(gsm) => Init(
                GlobalVariables.Background,
                GlobalVariables.Tileset_2,
                GlobalVariables.MAP_2,
                new Vector2(200, 300),
                new Vector2(1500,200));       
    
        private new void PlaceItems()
        {
            items.Clear();

            //
            //  Instance for MAP
            //
            i_dj = new I_DJump(tileMap, player, null);
            i_dj.SetPosition(272, 1530);
            items.Add(i_dj);

            i_d = new I_Dash(tileMap, player, null);
            i_d.SetPosition(3080, 1000);
            items.Add(i_d);

            i_s = new I_Sword(tileMap, player, null);
            i_s.SetPosition(1000, 1500);
            items.Add(i_s);

            i_fb = new I_Fireball(tileMap, player, null);
            i_fb.SetPosition(210, 2140);
            items.Add(i_fb);

            //
            //  Instance for HUD
            //
            i_dj = new I_DJump(tileMap, player, null);
            i_dj.SetPosition(0, 0);
            items.Add(i_dj);

            i_d = new I_Dash(tileMap, player, null);
            i_d.SetPosition(0, 0);
            items.Add(i_d);

            i_s = new I_Sword(tileMap, player, null);
            i_s.SetPosition(0, 0);
            items.Add(i_s);

            i_fb = new I_Fireball(tileMap, player, null);
            i_fb.SetPosition(0, 0);
            items.Add(i_fb);
        }

        private new void PlaceEnemies()
        {
            enemies.Clear();

            //
            //  SKELETONS
            //
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

            //
            //  GHOSTS
            //
            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1464, 1088);
            enemies.Add(eg);

            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1956, 1088);
            enemies.Add(eg);

            eg = new E_Ghost(tileMap, player);
            eg.SetPosition(1720, 2250);
            enemies.Add(eg);

            //
            //  BOSS
            //
            eb = new E_Boss(tileMap, player);
            eb.SetPosition(2550, 1750);
            enemies.Add(eb);
        }
    }
}
