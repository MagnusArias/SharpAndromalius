﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Objects.Items
{
    class I_Dash
    {
        private int tick;
        private double b;

        private Player player;

        public ItemDash(TileMap tm, Player pl)
        {
            super(tm);
            player = pl;
            facingRight = true;

            width = 16;
            height = 16;

            cwidth = 16;
            cheight = 16;

            sprites = Content.Dash[0];

            animation.setFrames(sprites);
            animation.setDelay(-1);

            tick = 0;
            b = Math.random() * 0.06 + 0.07;
        }

        public void update()
        {
            tick++;

            y = Math.sin(b * tick) + y;

            // update animation
            animation.update();
        }

        public void draw(Graphics2D g)
        {
            super.draw(g);
            if (player.getSkill(1)) super.drawInHUD(g, 30, 35);
        }
    }
}